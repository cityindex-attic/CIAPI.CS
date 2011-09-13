namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections;
    using System.Text;

    internal class UpdateInfoImpl : IUpdateInfo
    {
        private FullTableManager.ItemInfo info;
        private bool isSnapshot;
        private string[] prevState;
        private string[] updEvent;

        internal UpdateInfoImpl(FullTableManager.ItemInfo info, string[] prevState, string[] updEvent, bool isSnapshot)
        {
            this.info = info;
            this.prevState = prevState;
            this.updEvent = updEvent;
            this.isSnapshot = isSnapshot;
        }

        private int GetIndex(int fieldPos)
        {
            if ((fieldPos <= 0) || (fieldPos > this.prevState.Length))
            {
                throw new ArgumentException();
            }
            return (fieldPos - 1);
        }

        private int GetIndex(string fieldName)
        {
            IDictionary indexMap = this.info.GetFieldIndexMap();
            if (indexMap == null)
            {
                throw new ArgumentException();
            }
            if (!indexMap.Contains(fieldName))
            {
                throw new ArgumentException();
            }
            return (int) indexMap[fieldName];
        }

        public string GetItemName()
        {
            return this.info.name;
        }

        public int GetItemPos()
        {
            return this.info.pos;
        }

        public string GetNewValue(int fieldPos)
        {
            int index = this.GetIndex(fieldPos);
            string val = this.updEvent[index];
            if (val != Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED)
            {
                return val;
            }
            return this.prevState[index];
        }

        public string GetNewValue(string fieldName)
        {
            int index = this.GetIndex(fieldName);
            string val = this.updEvent[index];
            if (val != Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED)
            {
                return val;
            }
            return this.prevState[index];
        }

        public int GetNumFields()
        {
            return this.prevState.Length;
        }

        public string GetOldValue(int fieldPos)
        {
            return this.prevState[this.GetIndex(fieldPos)];
        }

        public string GetOldValue(string fieldName)
        {
            return this.prevState[this.GetIndex(fieldName)];
        }

        public bool IsSnapshot()
        {
            return this.isSnapshot;
        }

        public bool IsValueChanged(int fieldPos)
        {
            return (this.updEvent[this.GetIndex(fieldPos)] != Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED);
        }

        public bool IsValueChanged(string fieldName)
        {
            return (this.updEvent[this.GetIndex(fieldName)] != Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED);
        }

        public override string ToString()
        {
            int last = this.updEvent.Length - 1;
            if (last < 0)
            {
                return "[]";
            }
            StringBuilder b = new StringBuilder();
            b.Append("[ ");
            int i = 0;
            while (true)
            {
                string val = this.updEvent[i];
                if (val == Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED)
                {
                    string old = this.prevState[i];
                    b.Append('(');
                    b.Append((old != null) ? old : "null");
                    b.Append(')');
                }
                else
                {
                    b.Append((val != null) ? val : "null");
                }
                if (i == last)
                {
                    b.Append(" ]");
                    return b.ToString();
                }
                b.Append(", ");
                i++;
            }
        }

        public string ItemName
        {
            get
            {
                return this.info.name;
            }
        }

        public int ItemPos
        {
            get
            {
                return this.info.pos;
            }
        }

        public int NumFields
        {
            get
            {
                return this.prevState.Length;
            }
        }

        public bool Snapshot
        {
            get
            {
                return this.isSnapshot;
            }
        }
    }
}

