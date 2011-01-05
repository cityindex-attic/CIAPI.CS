namespace Lightstreamer.DotNet.Client
{
    using System;

    [Serializable]
    public class PushServerException : Exception
    {
        private static readonly string[] defaultMsgs = new string[] { "Unspecified error", "Wrong window number", "Wrong item number", "Wrong number of fields", "Answer was interrupted", "Incorrect answer", "No answer", "Unexpected answer", "Session not found", "Server refusal", "No data from server", "No more answer from server", "Unexpected error" };
        private int errorCode;
        public const int FIELDS_ERROR = 3;
        public const int ITEMS_ERROR = 2;
        public const int NO_ANSWER = 6;
        public const int PROTOCOL_ERROR = 7;
        public const int RECONNECTION_TIMEOUT = 11;
        public const int SERVER_REFUSAL = 9;
        public const int SERVER_TIMEOUT = 10;
        public const int SYNC_ERROR = 8;
        public const int SYNTAX_ERROR = 5;
        public const int TABLE_ERROR = 1;
        public const int UNEXPECTED_END = 4;
        public const int UNEXPECTED_ERROR = 12;

        internal PushServerException(int errorCode) : base(GetMsg(errorCode, null))
        {
            this.errorCode = errorCode;
        }

        internal PushServerException(int errorCode, Exception t) : base(GetMsg(errorCode, t.Message), t)
        {
            this.errorCode = errorCode;
        }

        internal PushServerException(int errorCode, string msg) : base(GetMsg(errorCode, msg))
        {
            this.errorCode = errorCode;
        }

        private static string GetMsg(int errorCode, string extraMsg)
        {
            string str = defaultMsgs[0];
            if ((errorCode > 0) && (errorCode < defaultMsgs.Length))
            {
                str = defaultMsgs[errorCode];
            }
            if (extraMsg != null)
            {
                str = str + ": " + extraMsg;
            }
            return str;
        }

        public virtual int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }
    }
}

