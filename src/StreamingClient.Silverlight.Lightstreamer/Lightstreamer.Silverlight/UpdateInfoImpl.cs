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
            IDictionary fieldIndexMap = this.info.GetFieldIndexMap();
            if (fieldIndexMap == null)
            {
                throw new ArgumentException();
            }
            if (!fieldIndexMap.Contains(fieldName))
            {
                throw new ArgumentException();
            }
            return (int) fieldIndexMap[fieldName];
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
            string str = this.updEvent[index];
            if (str != ServerUpdateEvent.UNCHANGED)
            {
                return str;
            }
            return this.prevState[index];
        }

        public string GetNewValue(string fieldName)
        {
            int index = this.GetIndex(fieldName);
            string str = this.updEvent[index];
            if (str != ServerUpdateEvent.UNCHANGED)
            {
                return str;
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
            return (this.updEvent[this.GetIndex(fieldPos)] != ServerUpdateEvent.UNCHANGED);
        }

        public bool IsValueChanged(string fieldName)
        {
            return (this.updEvent[this.GetIndex(fieldName)] != ServerUpdateEvent.UNCHANGED);
        }

        public override string ToString()
        {
            int num = this.updEvent.Length - 1;
            if (num < 0)
            {
                return "[]";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[ ");
            int index = 0;
            while (true)
            {
                string str = this.updEvent[index];
                if (str == ServerUpdateEvent.UNCHANGED)
                {
                    string str2 = this.prevState[index];
                    builder.Append('(');
                    builder.Append((str2 != null) ? str2 : "null");
                    builder.Append(')');
                }
                else
                {
                    builder.Append((str != null) ? str : "null");
                }
                if (index == num)
                {
                    builder.Append(" ]");
                    return builder.ToString();
                }
                builder.Append(", ");
                index++;
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

