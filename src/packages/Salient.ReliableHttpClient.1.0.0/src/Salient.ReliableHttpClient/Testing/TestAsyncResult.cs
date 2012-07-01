using System;
using System.Threading;

namespace Salient.ReliableHttpClient.Testing
{
    public class TestAsyncResult : IAsyncResult,IDisposable
    {
        private const int DefaultLatencyInMs = 100;
        private readonly AsyncCallback _callback;
        private readonly object _state;
        private readonly ManualResetEvent _waitHandle;
        private readonly Timer _timer;
        public bool IsCompleted { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public bool CompletedSynchronously
        {
            get { return IsCompleted; }
        }

        public TestAsyncResult(AsyncCallback callback, object state)
            : this(callback, state, TimeSpan.FromMilliseconds(DefaultLatencyInMs))
        {
        }

        public TestAsyncResult(AsyncCallback callback, object state, TimeSpan latency)
        {
            IsCompleted = false;
            _callback = callback;
            _state = state;
            _waitHandle = new ManualResetEvent(false);
            _timer = new Timer(onTimer => NotifyComplete(), null, latency, TimeSpan.FromMilliseconds(-1));
        }

        public void Abort()
        {
            _timer.Dispose();
            NotifyComplete();
        }

        private void NotifyComplete()
        {
            IsCompleted = true;
            _waitHandle.Set();
            if (_callback != null)
                _callback(this);
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
                if (_waitHandle != null)
                {
                    ((IDisposable)_waitHandle).Dispose();
                }
                if (_timer != null)
                {
                    _timer.Dispose();
                }
            }

        }
    }
}