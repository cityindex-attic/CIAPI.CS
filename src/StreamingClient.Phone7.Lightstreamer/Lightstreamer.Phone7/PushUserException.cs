namespace Lightstreamer.DotNet.Client
{
    using System;

    public class PushUserException : ClientException
    {
        private int code;

        public PushUserException(int serverCode, string serverMsg) : base(serverMsg)
        {
            this.code = serverCode;
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

