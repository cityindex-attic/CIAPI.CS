using System;
using CIAPI.Streaming;
using NUnit.Framework;
using StreamingClient.Websocket;

namespace StreamingClient.Tests.Websocket
{
    [TestFixture]
    public class WebSocketFixture
    {
       [Test, Category("DependsOnExternalResource")]
       public void CanConnect()
       {
           var websocket = new WebsocketClient(new Uri("ws://ec2-50-16-152-101.compute-1.amazonaws.com:61614"));    
           websocket.Connect();
           var value = websocket.Recv();
           websocket.Close();
           Assert.IsNotNull(value);
       }
    }
}