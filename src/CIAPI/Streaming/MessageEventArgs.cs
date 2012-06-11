using System;

namespace CIAPI.StreamingClient
{
    public class MessageEventArgs<T> : EventArgs
    {
        public MessageEventArgs(string dataAdapter, string topic, T messageData,int phase)
        {
            DataAdapter = dataAdapter;
            Topic = topic;
            Data = messageData;
            Phase = phase;
        }
        public string DataAdapter { get; set; }
        public int Phase { get; set; }
        public string Topic { get; set; }
        public T Data { get; set; }
    }
}