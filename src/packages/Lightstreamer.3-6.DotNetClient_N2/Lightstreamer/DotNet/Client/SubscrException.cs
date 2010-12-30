namespace Lightstreamer.DotNet.Client
{
    using System;

    [Serializable]
    public class SubscrException : Exception
    {
        internal SubscrException(string msg) : base(msg)
        {
        }
    }
}

