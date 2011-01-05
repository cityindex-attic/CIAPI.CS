namespace Lightstreamer.DotNet.Client
{
    using System;

    [Serializable]
    public class PushUserException : Exception
    {
        private int code;

        internal PushUserException(int code, string msg) : base(msg)
        {
            this.code = code;
        }

        public virtual int ErrorCode
        {
            get
            {
                return this.code;
            }
        }
    }
}

