using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StreamingClient.Websocket
{
    public class TcpClientFacade: ITcpClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;

        public Stream Open(string host, int port)
        {
            _tcpClient = new TcpClient(host, port);
            _stream = _tcpClient.GetStream();
            return _stream;
        }

        public void Close()
        {
            _stream.Close();
            _stream.Dispose();
            _tcpClient.Close();
            _tcpClient = null;
            _stream = null;
        }

        public int Port
        {
            get { return ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Port; }
        }

        public void Write(string data)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(data);
            _stream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        public TextReader GetTextReaderForResponseStream()
        {
            return new StreamReader(_stream);
        }

        public void WriteByte(byte b)
        {
            _stream.WriteByte(b);
        }

        public void Flush()
        {
            _stream.Flush();
        }

        public BinaryReader GetBinaryReaderForResponseStream()
        {
            return new BinaryReader(_stream);
        }
    }
}