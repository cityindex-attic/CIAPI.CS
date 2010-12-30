using System.Text;

namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using System;
    using System.Collections;

    internal class ServerUpdateEvent
    {
        private bool eos;
        private string itemCode;
        private int overflow;
        internal static readonly string UNCHANGED = new StringBuilder("UNCHANGED").ToString();
        private ArrayList values;
        private string winCode;

        internal ServerUpdateEvent(string winCode, string itemCode)
        {
            this.winCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.winCode = winCode;
            this.itemCode = itemCode;
            this.values = new ArrayList();
        }

        internal ServerUpdateEvent(string winCode, string itemCode, bool eos)
        {
            this.winCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.winCode = winCode;
            this.itemCode = itemCode;
            this.eos = eos;
            if (!eos)
            {
                this.values = new ArrayList();
            }
        }

        internal ServerUpdateEvent(string winCode, string itemCode, int overflow)
        {
            this.winCode = null;
            this.itemCode = null;
            this.values = null;
            this.eos = false;
            this.overflow = 0;
            this.winCode = winCode;
            this.itemCode = itemCode;
            this.overflow = overflow;
        }

        internal virtual void AddValue(string val)
        {
            this.values.Add(val);
        }

        internal virtual IDictionary GetMap(string[] fieldNames)
        {
            IDictionary dictionary = new Hashtable();
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string str = (string) this.values[i];
                if (!(str == UNCHANGED))
                {
                    dictionary[fieldNames[i]] = str;
                }
            }
            return dictionary;
        }

        internal virtual string GetValue(int pos)
        {
            return (string) this.values[pos];
        }

        public override string ToString()
        {
            return ("event for item n\x00b0" + this.itemCode + " in window n\x00b0" + this.winCode + " with values " + CollectionsSupport.ToString(this.values));
        }

        internal virtual string[] Array
        {
            get
            {
                string[] strArray = new string[this.values.Count];
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = (string) this.values[i];
                }
                return strArray;
            }
        }

        internal virtual bool EOS
        {
            get
            {
                return this.eos;
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

        internal virtual int WinCode
        {
            get
            {
                if (this.winCode == null)
                {
                    return -1;
                }
                return int.Parse(this.winCode);
            }
        }
    }
}

