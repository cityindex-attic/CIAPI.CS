namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Threading;

    internal class ExtConnectionListener : IConnectionListener
    {
        private Exception connFailure = null;
        private NotificationQueue queue = new NotificationQueue("Stream-sense queue", false);
        private bool streamingAppended = false;
        private ManualResetEvent streamingTested = new ManualResetEvent(false);
        private IConnectionListener target;

        public ExtConnectionListener(IConnectionListener target)
        {
            this.target = target;
        }

        public void FlushAndStart()
        {
            this.queue.Start();
        }

        public void OnActivityWarning(bool warningOn)
        {
            this.queue.Add(delegate {
                this.target.OnActivityWarning(warningOn);
            });
        }

        public void OnClose()
        {
            this.queue.Add(delegate {
                this.target.OnClose();
            });
            this.queue.End();
        }

        public void OnConnectException(Exception e)
        {
            this.connFailure = e;
            this.streamingTested.Set();
        }

        public void OnConnectionEstablished()
        {
            this.queue.Add(delegate {
                this.target.OnConnectionEstablished();
            });
        }

        public void OnConnectTimeout(PushServerException e)
        {
            this.streamingAppended = true;
            this.streamingTested.Set();
            this.queue.Add(delegate {
                this.target.OnFailure(e);
            });
        }

        public void OnDataError(PushServerException e)
        {
            this.queue.Add(delegate {
                this.target.OnDataError(e);
            });
        }

        public void OnEnd(int cause)
        {
            this.queue.Add(delegate {
                this.target.OnEnd(cause);
            });
            this.streamingTested.Set();
        }

        public void OnFailure(PushConnException e)
        {
            this.queue.Add(delegate {
                this.target.OnFailure(e);
            });
            this.streamingTested.Set();
        }

        public void OnFailure(PushServerException e)
        {
            this.queue.Add(delegate {
                this.target.OnFailure(e);
            });
            this.streamingTested.Set();
        }

        public void OnNewBytes(long bytes)
        {
            this.queue.Add(delegate {
                this.target.OnNewBytes(bytes);
            });
        }

        public void OnSessionStarted(bool isPolling)
        {
            this.queue.Add(delegate {
                this.target.OnSessionStarted(isPolling);
            });
        }

        public void OnStreamingReturned()
        {
            this.streamingTested.Set();
        }

        public bool WaitStreamingTimeoutAnswer()
        {
            this.streamingTested.WaitOne();
            if (this.connFailure != null)
            {
                throw this.connFailure;
            }
            return this.streamingAppended;
        }
    }
}

