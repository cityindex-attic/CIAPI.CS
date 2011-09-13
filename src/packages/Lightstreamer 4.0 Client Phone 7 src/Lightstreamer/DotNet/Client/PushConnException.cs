namespace Lightstreamer.DotNet.Client
{
    using System;

    public class PushConnException : ClientException
    {
        public PushConnException(Exception cause) : base(cause.Message, cause)
        {
        }

        public PushConnException(string msg) : base(msg)
        {
        }
    }
}

