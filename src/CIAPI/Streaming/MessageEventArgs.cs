using System;

namespace CIAPI.StreamingClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAdapter"></param>
        /// <param name="topic"></param>
        /// <param name="messageData"></param>
        /// <param name="phase"></param>
        public MessageEventArgs(string dataAdapter, string topic, T messageData,int phase)
        {
            DataAdapter = dataAdapter;
            Topic = topic;
            Data = messageData;
            Phase = phase;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataAdapter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Phase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
}