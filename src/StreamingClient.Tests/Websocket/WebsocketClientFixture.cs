using System;
using System.IO;
using System.Text;
using CIAPI.Streaming;
using Common.Logging;
using NUnit.Framework;
using StreamingClient.Websocket;

namespace StreamingClient.Tests.Websocket
{
    [TestFixture]
    public class WebSocketFixture
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(WebSocketFixture));

        private Stream _mockIncomingStream;
        private Stream _mockOutgoingStream;
        [SetUp]
        public void SetUp()
        {
            _mockIncomingStream = new MemoryStream();
            _mockOutgoingStream = new MemoryStream(); 
        }

        [Test]
        public void ThrowsExceptionIfInvalidScheme()
        {
            Assert.Throws<ArgumentException>(() => CreateWebsocketClient(new Uri("http://invalidscheme.com")));
            Assert.DoesNotThrow(() => CreateWebsocketClient(new Uri("ws://validscheme.com")));
            Assert.DoesNotThrow(() => CreateWebsocketClient(new Uri("wss://validscheme.com")));
        }

       
        private WebsocketClient CreateWebsocketClient(Uri uri)
        {
            return new WebsocketClient(uri, new MockTcpClient(_mockOutgoingStream, _mockIncomingStream));
        }

        [Test]
        public void ConnectSendsCorrectData()
        {
            PrepareStreamWithWebSocketHandshake(_mockIncomingStream);

            var client = CreateWebsocketClient(new Uri("ws://validscheme.com"));
            client.Connect();

            _mockOutgoingStream.Position = 0;
            var streamReader = new StreamReader(_mockOutgoingStream);

            Assert.AreEqual("GET / HTTP/1.1", streamReader.ReadLine());
            Assert.AreEqual("Upgrade: WebSocket", streamReader.ReadLine());
            Assert.AreEqual("Connection: Upgrade", streamReader.ReadLine());
            Assert.AreEqual("Host: validscheme.com", streamReader.ReadLine());
            Assert.AreEqual("Origin: http://validscheme.com", streamReader.ReadLine());

            client.Close();
        }

        private static void PrepareStreamWithWebSocketHandshake(Stream stream)
        {
            var streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
            streamWriter.WriteLine("Upgrade: WebSocket");
            streamWriter.WriteLine("Connection: Upgrade");
            streamWriter.Flush();
            stream.Position = 0;
        }

        [Test, Category("DependsOnExternalResource")]
        public void CanConnectToExternal()
        {
            StompMessage stompMessage;
            using (var stomp = new StompOverWebsocketConnection(
                new Uri("ws://ec2-50-16-152-101.compute-1.amazonaws.com:80"), "", ""))
               {
                    stomp.Subscribe("/topic/mock.news");
                    stompMessage = stomp.WaitForMessage();
               }

            _logger.InfoFormat("Message body is: {0}", stompMessage.Body);
            Assert.IsNotNull(stompMessage.Body);
        }
    }

    internal class MockTcpClient : ITcpClient
    {
        private readonly Stream _outgoingStream;
        private readonly Stream _incomingStream;

        public MockTcpClient(Stream outgoingStream, Stream incomingStream)
        {
            _outgoingStream = outgoingStream;
            _incomingStream = incomingStream;
        }

        public Stream Open(string host, int port)
        {
            Port = port;
            return _incomingStream;
        }

        public void Close()
        {
            _incomingStream.Close();
            _incomingStream.Dispose();

            _outgoingStream.Close();
            _outgoingStream.Dispose();
        }

        public int Port { get; private set; }
        public void Write(string data)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(data);
            _outgoingStream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        public TextReader GetTextReaderForResponseStream()
        {
            return new StreamReader(_incomingStream);
        }

        public void WriteByte(byte b)
        {
            _outgoingStream.WriteByte(b);
        }

        public void Flush()
        {
            _outgoingStream.Flush();
        }

        public BinaryReader GetBinaryReaderForResponseStream()
        {
            return new BinaryReader(_incomingStream);
        }
    }
}