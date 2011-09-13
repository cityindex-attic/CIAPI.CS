namespace Lightstreamer.DotNet.Client
{
    using System;

    internal class BatchMonitor
    {
        private bool empty = true;
        private IBatchListener listener = null;
        private int pendingCalls = 0;
        private bool unlimitedBatch = false;

        internal virtual void BatchedOne()
        {
            lock (this)
            {
                if (this.listener != null)
                {
                    this.listener.OnMessageBatched();
                }
            }
        }

        internal virtual void Clear()
        {
            lock (this)
            {
                this.unlimitedBatch = false;
                this.pendingCalls = 0;
                this.empty = true;
            }
        }

        internal virtual void Expand(int batchSize)
        {
            lock (this)
            {
                if (batchSize <= 0)
                {
                    this.unlimitedBatch = true;
                }
                else if (!this.unlimitedBatch)
                {
                    this.pendingCalls += batchSize;
                }
            }
        }

        public bool HasListener()
        {
            lock (this)
            {
                return (this.listener != null);
            }
        }

        internal virtual void UseOne()
        {
            lock (this)
            {
                this.empty = false;
                if (!(this.unlimitedBatch || (this.pendingCalls <= 0)))
                {
                    this.pendingCalls--;
                }
            }
        }

        internal virtual bool Empty
        {
            get
            {
                lock (this)
                {
                    return this.empty;
                }
            }
        }

        internal virtual bool Filled
        {
            get
            {
                lock (this)
                {
                    if (this.unlimitedBatch)
                    {
                        return false;
                    }
                    return (this.pendingCalls == 0);
                }
            }
        }

        public virtual IBatchListener Listener
        {
            set
            {
                lock (this)
                {
                    this.listener = value;
                }
            }
        }

        internal virtual bool Unlimited
        {
            get
            {
                lock (this)
                {
                    return this.unlimitedBatch;
                }
            }
        }
    }
}

