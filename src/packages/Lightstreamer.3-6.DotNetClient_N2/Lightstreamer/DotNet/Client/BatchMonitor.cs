namespace Lightstreamer.DotNet.Client
{
    using System;

    internal class BatchMonitor
    {
        private int pendingCalls = 0;
        private bool unlimitedBatch = false;

        internal virtual void Clear()
        {
            lock (this)
            {
                this.unlimitedBatch = false;
                this.pendingCalls = 0;
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

        internal virtual void UseOne()
        {
            lock (this)
            {
                if (!(this.unlimitedBatch || (this.pendingCalls <= 0)))
                {
                    this.pendingCalls--;
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
    }
}

