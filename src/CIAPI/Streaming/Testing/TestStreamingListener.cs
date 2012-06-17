using System;
using System.Threading;
using CIAPI.StreamingClient;

namespace CIAPI.Streaming.Testing
{
    public abstract class TestStreamingListener : IStreamingListener
    {

        internal Action TearMeDown;
        private readonly string _topic;
        private Timer _timer;
        private readonly string _adapter;
        public string Adapter
        {
            get
            {
                return _adapter;
            }

        }

        protected TestStreamingListener(string adapter, string topic)
        {
            _adapter = adapter;
            
            _adapter = adapter;
            _topic = topic;
        }

        public int Phase { get; set; }

        #region IStreamingListener Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start(int phase)
        {
            Phase = phase;
            if (_timer != null)
            {
                Stop();
            }
            _timer = new Timer(TimerCallback, null, 0, 250);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        public string Topic
        {
            get { return _topic; }
        }

     



        #endregion

        protected abstract void TimerCallback(object ignored);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }
    }

    public class TestStreamingListener<TDTO> : TestStreamingListener, IStreamingListener<TDTO> where TDTO : class, new()
    {
        public TestStreamingListener(string adapter, string topic)
            : base(adapter, topic)
        {
        }

        #region IStreamingListener<TDTO> Members

        public event EventHandler<MessageEventArgs<TDTO>> MessageReceived;

        #endregion

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

        public Action<MessageEventArgs<TDTO>> CreateMessage;
    }
}