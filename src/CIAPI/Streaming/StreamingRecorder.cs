using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.StreamingClient;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamingMessageFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="adapter"></param>
        /// <param name="topic"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static MessageEventArgs<TDTO> FindExact<TDTO>(string adapter,string topic,IEnumerable<MessageEventArgs<TDTO>> messages) where TDTO:class,new()
        {
            foreach (var item in messages)
            {
                if(string.Compare(item.DataAdapter,adapter,StringComparison.InvariantCultureIgnoreCase)==0)
                {
                    return item;
                }
            }
            throw new Exception(string.Format("no matching message found for {0}.{1}",adapter,topic));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class StreamingRecorder : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        protected StreamingRecorder(IStreamingListener listener)
        {
            Paused = true;
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool Paused { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Paused = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Paused = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //noop             
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="listener"></param>
        /// <returns></returns>
        public static StreamingRecorder<TDTO> Create<TDTO>(IStreamingListener<TDTO> listener) where TDTO : class,new()
        {
            return new StreamingRecorder<TDTO>(listener);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    public class StreamingRecorder<TDTO> : StreamingRecorder where TDTO : class,new()
    {
        private readonly List<MessageEventArgs<TDTO>> _messages;
        private IStreamingListener<TDTO> _listener;
        private readonly object _lockTarget = new object();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public StreamingRecorder(IStreamingListener<TDTO> listener)
            : base(listener)
        {
            
            _listener = listener;
            _listener.MessageReceived += ListenerMessageReceived;
            _messages = new List<MessageEventArgs<TDTO>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MessageEventArgs<TDTO>> GetMessages()
        {
            lock (_lockTarget)
            {
                return _messages.ToList();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ClearMessages()
        {
            lock (_lockTarget)
            {
                _messages.Clear();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ListenerMessageReceived(object sender, MessageEventArgs<TDTO> e)
        {
            if (!Paused)
            {
                lock (_lockTarget)
                {
                    _messages.Add(e);
                }
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _listener.MessageReceived -= ListenerMessageReceived;
                _listener = null;

            }
            base.Dispose(disposing);
        }
    }
}
