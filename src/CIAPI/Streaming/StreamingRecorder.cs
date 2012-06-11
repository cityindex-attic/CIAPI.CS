using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.StreamingClient;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming
{
    public class StreamingMessageFinder
    {
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
    public abstract class StreamingRecorder : IDisposable
    {
        protected StreamingRecorder(IStreamingListener listener)
        {
            Paused = true;
            
        }
        
        public bool Paused { get; private set; }

        public void Start()
        {
            Paused = false;
        }
        public void Stop()
        {
            Paused = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //noop             
            }
        }

        public static StreamingRecorder<TDTO> Create<TDTO>(IStreamingListener<TDTO> listener) where TDTO : class,new()
        {
            return new StreamingRecorder<TDTO>(listener);
        }
    }
    public class StreamingRecorder<TDTO> : StreamingRecorder where TDTO : class,new()
    {
        private readonly List<MessageEventArgs<TDTO>> _messages;
        private IStreamingListener<TDTO> _listener;
        private readonly object _lockTarget = new object();
        
        public StreamingRecorder(IStreamingListener<TDTO> listener)
            : base(listener)
        {
            
            _listener = listener;
            _listener.MessageReceived += ListenerMessageReceived;
            _messages = new List<MessageEventArgs<TDTO>>();
        }

        public List<MessageEventArgs<TDTO>> GetMessages()
        {
            lock (_lockTarget)
            {
                return _messages.ToList();
            }
        }
        public void ClearMessages()
        {
            lock (_lockTarget)
            {
                _messages.Clear();
            }

        }
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
