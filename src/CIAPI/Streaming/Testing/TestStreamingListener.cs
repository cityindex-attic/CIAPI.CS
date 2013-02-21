using System;
using System.Threading;
using CIAPI.StreamingClient;

namespace CIAPI.Streaming.Testing
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TestStreamingListener : IStreamingListener
    {

        internal Action TearMeDown;
        private readonly string _topic;
        private Timer _timer;
        private readonly string _adapter;
        /// <summary>
        /// 
        /// </summary>
        public string Adapter
        {
            get
            {
                return _adapter;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="topic"></param>
        protected TestStreamingListener(string adapter, string topic)
        {
            _adapter = adapter;
            
            _adapter = adapter;
            _topic = topic;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Phase { get; set; }

        #region IStreamingListener Members
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
        /// <param name="phase"></param>
        public void Start(int phase)
        {
            Phase = phase;
            if (_timer != null)
            {
                Stop();
            }
            _timer = new Timer(TimerCallback, null, 0, 250);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Topic
        {
            get { return _topic; }
        }

     



        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignored"></param>
        protected abstract void TimerCallback(object ignored);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    public class TestStreamingListener<TDTO> : TestStreamingListener, IStreamingListener<TDTO> where TDTO : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="topic"></param>
        public TestStreamingListener(string adapter, string topic)
            : base(adapter, topic)
        {
        }

        #region IStreamingListener<TDTO> Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MessageEventArgs<TDTO>> MessageReceived;

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignored"></param>
        protected override void TimerCallback(object ignored)
        {
            OnCreateMessage(this, new MessageEventArgs<TDTO>(Adapter,  Topic, new TDTO(), Phase));
        }


        private void OnCreateMessage(object sender, MessageEventArgs<TDTO> e)
        {
            Action<MessageEventArgs<TDTO>> h1 = CreateMessage;
            if (h1 != null)
            {
                h1(e);
            }

            var l = (TestStreamingListener<TDTO>)sender;
            EventHandler<MessageEventArgs<TDTO>> handler = l.MessageReceived;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<TDTO>> CreateMessage;
    }
}