namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class ServerUpdateEvent
    {
        private bool eos;
        private int errorCode;
        private string errorMessage;
        private long holdingMillis;
        private bool isLoop;
        private string itemCode;
        private int messageProg;
        private string messageSequence;
        private int overflow;
        private string tableCode;
        internal static readonly string UNCHANGED = new StringBuilder("UNCHANGED").ToString();
        private IList values;

        public ServerUpdateEvent(long HoldingMillis)
        {
            this.tableCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.messageProg = 0;
            this.messageSequence = null;
            this.errorCode = 0;
            this.errorMessage = null;
            this.isLoop = false;
            this.holdingMillis = HoldingMillis;
            this.isLoop = true;
        }

        internal ServerUpdateEvent(string messageSequence, int messageProg)
        {
            this.tableCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.messageProg = 0;
            this.messageSequence = null;
            this.errorCode = 0;
            this.errorMessage = null;
            this.isLoop = false;
            this.messageSequence = messageSequence;
            this.messageProg = messageProg;
        }

        internal ServerUpdateEvent(string tableCode, string itemCode)
        {
            this.tableCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.messageProg = 0;
            this.messageSequence = null;
            this.errorCode = 0;
            this.errorMessage = null;
            this.isLoop = false;
            this.tableCode = tableCode;
            this.itemCode = itemCode;
            this.values = new List<string>();
        }

        internal ServerUpdateEvent(string tableCode, string itemCode, bool eos)
        {
            this.tableCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.messageProg = 0;
            this.messageSequence = null;
            this.errorCode = 0;
            this.errorMessage = null;
            this.isLoop = false;
            this.tableCode = tableCode;
            this.itemCode = itemCode;
            this.eos = eos;
            if (!eos)
            {
                this.values = new List<string>();
            }
        }

        internal ServerUpdateEvent(string tableCode, string itemCode, int overflow)
        {
            this.tableCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.messageProg = 0;
            this.messageSequence = null;
            this.errorCode = 0;
            this.errorMessage = null;
            this.isLoop = false;
            this.tableCode = tableCode;
            this.itemCode = itemCode;
            this.overflow = overflow;
        }

        internal ServerUpdateEvent(string messageSequence, int messageProg, int errorCode, string errorMessage)
        {
            this.tableCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.messageProg = 0;
            this.messageSequence = null;
            this.errorCode = 0;
            this.errorMessage = null;
            this.isLoop = false;
            this.messageSequence = messageSequence;
            this.messageProg = messageProg;
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }

        internal virtual void AddValue(string val)
        {
            this.values.Add(val);
        }

        internal virtual IDictionary GetMap(string[] fieldNames)
        {
            IDictionary map = new Dictionary<string, string>();
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string val = (string) this.values[i];
                if (!(val == UNCHANGED))
                {
                    map[fieldNames[i]] = val;
                }
            }
            return map;
        }

        internal virtual string GetValue(int pos)
        {
            return (string) this.values[pos];
        }

        public override string ToString()
        {
            if (this.TableUpdate)
            {
                return ("event for item n\x00b0" + this.itemCode + " in table n\x00b0" + this.tableCode + " with values " + CollectionsSupport.ToString(this.values));
            }
            return string.Concat(new object[] { "event for message n\x00b0", this.messageProg, " in sequence ", this.messageSequence, " with error message (if any): ", this.errorMessage });
        }

        internal virtual string[] Array
        {
            get
            {
                string[] array = new string[this.values.Count];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (string) this.values[i];
                }
                return array;
            }
        }

        internal virtual bool EOS
        {
            get
            {
                return this.eos;
            }
        }

        internal int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        internal string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        public long HoldingMillis
        {
            get
            {
                return this.holdingMillis;
            }
        }

        internal virtual int ItemCode
        {
            get
            {
                if (this.itemCode == null)
                {
                    return -1;
                }
                return int.Parse(this.itemCode);
            }
        }

        public bool Loop
        {
            get
            {
                return this.isLoop;
            }
        }

        internal int MessageProg
        {
            get
            {
                return this.messageProg;
            }
        }

        internal string MessageSequence
        {
            get
            {
                return this.messageSequence;
            }
        }

        internal virtual int Overflow
        {
            get
            {
                return this.overflow;
            }
        }

        internal virtual int Size
        {
            get
            {
                return this.values.Count;
            }
        }

        internal virtual int TableCode
        {
            get
            {
                if (this.tableCode == null)
                {
                    return -1;
                }
                return int.Parse(this.tableCode);
            }
        }

        internal bool TableUpdate
        {
            get
            {
                return (this.tableCode != null);
            }
        }
    }
}

