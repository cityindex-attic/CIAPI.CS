using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            client.Connect(new Dictionary<string, string>{{"extraHeader1", "value1"}});

            _mockOutgoingStream.Position = 0;
            var streamReader = new StreamReader(_mockOutgoingStream);

            Assert.AreEqual("GET / HTTP/1.1", streamReader.ReadLine());
            Assert.AreEqual("Upgrade: WebSocket", streamReader.ReadLine());
            Assert.AreEqual("Connection: Upgrade", streamReader.ReadLine());
            Assert.AreEqual("Host: validscheme.com", streamReader.ReadLine());
            Assert.AreEqual("Origin: http://validscheme.com", streamReader.ReadLine());

            client.Close();
        }

        [Test]
        public void ThrowsExceptionIfConnectionResponseIsInvalid()
        {
            PrepareStream(_mockIncomingStream, new List<string>
                                      {
                                          "HTTP/1.1 403 Invalid connection",
                                          "Upgrade: WebSocket",
                                          "Connection: Upgrade"
                                      });
            var client = CreateWebsocketClient(new Uri("ws://validscheme.com"));

            Assert.Throws<IOException>(client.Connect);
        }

        [Test]
        public void ThrowsExceptionIfConnectionResponseIsNull()
        {
            PrepareStream(_mockIncomingStream, new List<string>());
            var client = CreateWebsocketClient(new Uri("ws://validscheme.com"));

            Assert.Throws<IOException>(client.Connect);
        }

        private static void PrepareStreamWithWebSocketHandshake(Stream stream)
        {
            PrepareStream(stream, new List<string>
                                      {
                                          "HTTP/1.1 101 Web Socket Protocol Handshake",
                                          "Upgrade: WebSocket",
                                          "Connection: Upgrade"
                                      });
        }

        private static void PrepareStream(Stream stream, IEnumerable<string> lines)
        {
            var streamWriter = new StreamWriter(stream);
            foreach (var line in lines)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Flush();
            stream.Position = 0;
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