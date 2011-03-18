using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Logging;

namespace StreamingClient.Websocket
{
    public interface IWebsocketClient
    {
        void Connect();
        void Connect(Dictionary<string, string> headers);
        void SendFrame(string frameData);
        string ReceiveFrame();
        void Close();
    }

    public class WebsocketClient : IWebsocketClient
    {
        private readonly Uri _url;
        private ITcpClient _tcpClient;
        private bool _handshakeComplete;
        private readonly ILog _logger = LogManager.GetLogger(typeof(WebsocketClient));

        public WebsocketClient(Uri url) : this(url, new TcpClientFacade())
        {
        }

        public WebsocketClient(Uri url, ITcpClient tcpClient)
        {
            _url = url;
            _tcpClient = tcpClient;

            if (!_url.Scheme.Equals("ws") && !_url.Scheme.Equals("wss"))
                throw new ArgumentException("Unsupported scheme: " + _url.Scheme);
        }

        public void Connect()
        {
            Connect(null);
        }
        public void Connect(Dictionary<string, string> headers)
        {
            var host = _url.DnsSafeHost;
            var path = _url.PathAndQuery;

            OpenStream(_url);

            var extraHeaders = new StringBuilder();
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                    extraHeaders.Append(header.Key + ": " + header.Value + "\r\n");
            }

            var request = "GET " + path + " HTTP/1.1\r\n" +
                             "Upgrade: WebSocket\r\n" +
                             "Connection: Upgrade\r\n" +
                             "Host: " + GetFullHostName(host, _tcpClient.Port) + "\r\n" +
                             "Origin: " + GetOrigin(host) + "\r\n" +
                             extraHeaders + "\r\n";
            
            _tcpClient.Write(request);

            var reader = _tcpClient.GetTextReaderForResponseStream();
            EnsureNextLineIs(reader, "HTTP/1.1 101 Web Socket Protocol Handshake");
            EnsureNextLineIs(reader, "Upgrade: WebSocket");
            EnsureNextLineIs(reader, "Connection: Upgrade");

            _handshakeComplete = true;
        }

        private static void EnsureNextLineIs(TextReader reader, string expectedLineValue)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
                throw new IOException("Invalid handshake response: line was empty");
            if (!line.Equals(expectedLineValue))
                throw new IOException(string.Format("Invalid handshake response: expected: {0} actual: {1}", expectedLineValue, line));
        }

        private static string GetOrigin(string host)
        {
            return "http://" + host;
        }

        private static string GetFullHostName(string host, int port)
        {
            var fullHostName = host;
            if (port != 80)
                fullHostName = host + ":" + port;
            return fullHostName;
        }

        public void SendFrame(string frameData)
        {
            if (!_handshakeComplete)
                throw new InvalidOperationException("Handshake not complete");

            _logger.DebugFormat("Sending frame data: \n{0}", PrettyLog(frameData));

            _tcpClient.WriteByte(0x00);
            _tcpClient.Write(frameData);
            _tcpClient.WriteByte(0xff);
            _tcpClient.Flush();
        }

        private string PrettyLog(string frameData)
        {
            return frameData.Replace("\0", "").TrimEnd('\r', '\n');
        }

        public string ReceiveFrame()
        {
            EnsureConnected();

            var recvBuffer = new List<byte>();
            var reader = _tcpClient.GetBinaryReaderForResponseStream();
            
            SkipDataFrame(reader);

            for (var i = 0; i < int.MaxValue; i++)
            {
                var b = reader.ReadByte();
                if (b == 0xff)
                    break;

                recvBuffer.Add(b);
            } 

            var receivedFrame = Encoding.UTF8.GetString(recvBuffer.ToArray());
            _logger.DebugFormat("Received frame data: \n{0}", PrettyLog(receivedFrame));
            
            return receivedFrame;
        }

        private void EnsureConnected()
        {
            if (!_handshakeComplete)
                throw new InvalidOperationException("Handshake not complete");
        }

        private void SkipDataFrame(BinaryReader reader)
        {
            var b = reader.ReadByte();
            if ((b & 0x80) == 0x80)
            {
                // Skip data frame
                int len = 0;
                do
                {
                    b = (byte)(reader.ReadByte() & 0x7f);
                    len += b * 128;
                } while ((b & 0x80) != 0x80);

                for (int i = 0; i < len; i++)
                    reader.ReadByte();
            }
        }

        public void Close()
        {
            _tcpClient.Close();
            _tcpClient = null;
        }

        private void OpenStream(Uri url)
        {
            var scheme = url.Scheme;
            var host = url.DnsSafeHost;

            var port = url.Port;
            if (port <= 0)
            {
                if (scheme.Equals("wss"))
                    port = 443;
                else if (scheme.Equals("ws"))
                    port = 80;
                else
                    throw new ArgumentException("Unsupported scheme");
            }

            if (scheme.Equals("wss"))
                throw new NotImplementedException("SSL support not implemented yet");
            
            _tcpClient.Open(host, port);
        }
    }
}
