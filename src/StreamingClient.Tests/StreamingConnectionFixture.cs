using System;
using NUnit.Framework;

namespace StreamingClient.Tests
{
    [TestFixture]
    public class StreamingConnectionFixture
    {
        [Test]
        public void RaisesMessageEvents()
        {
            MessageEventArgs<string> recievedMessage = null;
            IStreamingConnection connection = new MockStreamingConnection();
           
            connection.MessageRecieved += (s, message) =>
                                     {
                                         recievedMessage = message;
                                     };

            ((MockStreamingConnection)connection).RaiseMessageRecieved("topic1","message data");

            Assert.IsNotNull(recievedMessage);
            Assert.AreEqual("topic1", recievedMessage.Topic);
            Assert.AreEqual("message data", recievedMessage.Data);
        }
    }

    public interface IStreamingConnection
    {
        event EventHandler<MessageEventArgs<string>> MessageRecieved;
    }

    public class MessageEventArgs<T> : EventArgs 
    {
        public MessageEventArgs(string topic, T messageData)
        {
            Topic = topic;
            Data = messageData;
        }

        public string Topic { get; set; }
        public T Data { get; set; }
    }


    public class MockStreamingConnection : IStreamingConnection
    {
        public event EventHandler<MessageEventArgs<string>> MessageRecieved;
        public void RaiseMessageRecieved(string topic, string messageData)
        {
            if (MessageRecieved != null) MessageRecieved(this,new MessageEventArgs<string>(topic, messageData));
        }
    }


}