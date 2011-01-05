namespace Lightstreamer.DotNet.Client
{
    using System;

    public class SubscribedTableKey
    {
        private int val = -1;

        internal SubscribedTableKey(int val)
        {
            this.val = val;
        }

        internal virtual int KeyValue
        {
            get
            {
                return this.val;
            }
        }
    }
}

