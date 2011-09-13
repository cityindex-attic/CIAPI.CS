namespace Lightstreamer.DotNet.Client
{
    using System;

    internal class PushEndException : Exception
    {
        private int endCause;

        internal PushEndException() : base("Connection consumed")
        {
            this.endCause = 0;
        }

        internal PushEndException(int endCause) : base("Connection consumed")
        {
            if (endCause < 0)
            {
                this.endCause = 0;
            }
            else
            {
                this.endCause = endCause;
            }
        }

        public virtual int EndCause
        {
            get
            {
                return this.endCause;
            }
        }
    }
}

