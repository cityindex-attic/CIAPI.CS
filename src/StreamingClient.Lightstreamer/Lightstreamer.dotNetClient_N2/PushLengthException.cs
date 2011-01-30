namespace Lightstreamer.DotNet.Client
{
    using System;

    internal class PushLengthException : Exception
    {
        private long holdingMillis;

        internal PushLengthException(long holdingTime) : base("Connection consumed")
        {
            this.holdingMillis = holdingTime;
        }

        public virtual long HoldingMillis
        {
            get
            {
                return this.holdingMillis;
            }
        }
    }
}

