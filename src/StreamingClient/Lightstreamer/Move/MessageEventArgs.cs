using System;

namespace StreamingClient
{
    public class MessageEventArgs<T> : EventArgs
    {
        public MessageEventArgs(string topic, T messageData,int phase)
        {
            Topic = topic;
            Data = messageData;
            Phase = phase;

        }

        public int Phase { get; set; }
        public string Topic { get; set; }
        public T Data { get; set; }
    }
}