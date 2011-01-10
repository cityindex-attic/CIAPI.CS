namespace Lightstreamer.DotNet.Client
{
    using System;

    [Serializable]
    public class PushConnException : Exception
    {
        internal PushConnException(Exception e) : base(e.Message, e)
        {
        }

        internal PushConnException(string msg) : base(msg)
        {
        }
    }
}

