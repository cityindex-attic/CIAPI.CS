using System;

namespace CIAPI.Streaming
{
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
}