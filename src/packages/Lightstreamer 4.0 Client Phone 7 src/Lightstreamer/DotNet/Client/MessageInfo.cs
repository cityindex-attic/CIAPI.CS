namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Text.RegularExpressions;

    public class MessageInfo
    {
        private int delayTimeout;
        private static Regex ext_alpha_numeric = new Regex("^[a-zA-Z0-9_]*$");
        private string message;
        private const string noMessage = "Message cannot be null";
        private string sequence;
        public const string UNORDERED_MESSAGES = "UNORDERED_MESSAGES";
        private const string wrongSeqName = "Sequence name can only contain alphanumeric characters and/or underscores and can't be null nor an empry string";

        public MessageInfo(string message, string sequence) : this(message, sequence, -1)
        {
        }

        public MessageInfo(string message, string sequence, int delayTimeout)
        {
            this.delayTimeout = -1;
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null");
            }
            this.message = message;
            this.sequence = sequence;
            this.delayTimeout = delayTimeout;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { this.sequence, "/", this.delayTimeout, "/", this.message });
        }

        public int DelayTimeout
        {
            get
            {
                return this.delayTimeout;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public string Sequence
        {
            get
            {
                return this.sequence;
            }
            private set
            {
                if ((value == null) || (value == ""))
                {
                    throw new ArgumentException("Sequence name can only contain alphanumeric characters and/or underscores and can't be null nor an empry string");
                }
                if (!ext_alpha_numeric.IsMatch(this.sequence))
                {
                    throw new ArgumentException("Sequence name can only contain alphanumeric characters and/or underscores and can't be null nor an empry string");
                }
            }
        }
    }
}

