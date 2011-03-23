using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Logging;
using NUnit.Framework;
using Rhino.Mocks;
using StreamingClient.Websocket;

namespace StreamingClient.Tests.Websocket
{
    [TestFixture]
    public class StompOverWebsocketConnectionFixture
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(WebSocketFixture));

        private IWebsocketClient _mockWebSocket;
        
        [SetUp]
        public void SetUp()
        {
            _mockWebSocket = MockRepository.GenerateMock<IWebsocketClient>();
            
        }

        private StompOverWebsocketConnection CreateStompOverWebsocketConnection()
        {
            return new StompOverWebsocketConnection(_mockWebSocket);
        }

        [Test]
        public void ConnectingSendsCorrectStompLoginMessage()
        {
            const string expectedConnectMessage = "CONNECT\r\nlogin:username\r\npasscode:password\r\n\r\n\r\n\0";
            var actualConnectMessageSent = "";
           
            using (var connection = CreateStompOverWebsocketConnection())
            {
                SetupWebsocketToAllowConnect();
                _mockWebSocket.Expect(x => x.SendFrame(Arg<string>.Is.Anything))
                    .WhenCalled(i => actualConnectMessageSent = i.Arguments[0].ToString());

                connection.Connect("username", "password");
            }

            _mockWebSocket.AssertWasCalled(x => x.Connect());
            Assert.AreEqual(expectedConnectMessage, actualConnectMessageSent);
        }

        private void SetupWebsocketToAllowConnect()
        {
            _mockWebSocket.Expect(x => x.ReceiveFrame()).Return("CONNECTED\nsession:ID:ip-10-245-134-150-39319-1297138182156-4:20\n\n");
        }

        [Test]
        public void SubscribeSendsCorrectStompMessage()
        {
            AssertMessageSentIs("SUBSCRIBE\r\ndestination:/topic/mytopic\r\n\r\n\r\n\0",
               "Subscribe", "/topic/mytopic");
        }

        [Test]
        public void UnSubscribeSendsCorrectStompMessage()
        {
            AssertMessageSentIs("UNSUBSCRIBE\r\ndestination:/topic/mytopic\r\n\r\n\r\n\0",
                "Unsubscribe", "/topic/mytopic");
        }

        private void AssertMessageSentIs(string expectedMessage, string methodOnConnection, string methodArg1)
        {
            var actualMessageSent = "";

            using (var connection = CreateStompOverWebsocketConnection())
            {
                SetupWebsocketToAllowConnect();
                connection.Connect("username", "password");

                _mockWebSocket.Expect(x => x.SendFrame(Arg<string>.Is.Anything))
                    .WhenCalled(i => actualMessageSent = i.Arguments[0].ToString());
                connection.GetType().GetMethod(methodOnConnection, new[]{typeof(string)}).Invoke(connection, new object[] {methodArg1});
                connection.Unsubscribe("/topic/mytopic");

            }


            Assert.AreEqual(expectedMessage, actualMessageSent);
        }

        [Test, Category("DependsOnExternalResource")]
        [Ignore("Required external Websockets server.  Run manually")]
        public void CanConnectToExternal()
        {
            _logger.InfoFormat("Ready to subscribe");
            var stompMessages = new List<StompMessage>();
            using (var stomp = new StompOverWebsocketConnection(
                new Uri("ws://ec2-50-16-152-101.compute-1.amazonaws.com:80")))
            {
                stomp.Connect("", "");
                stomp.Subscribe("/topic/mock.news");
                for (var i = 0; i < 3; i++)
                {
                    stompMessages.Add(stomp.WaitForMessage());
                }
                stomp.Unsubscribe("/topic/mock.news");
            }

            Assert.AreEqual(3, stompMessages.Count);
        }
    }

}