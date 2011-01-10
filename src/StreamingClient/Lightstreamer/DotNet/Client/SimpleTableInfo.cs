namespace Lightstreamer.DotNet.Client
{
    using System;

    public class SimpleTableInfo
    {
        internal int bufferSize = -1;
        public const string COMMAND = "COMMAND";
        internal string dataAdapter = null;
        public const string DISTINCT = "DISTINCT";
        internal int distinctSnapshotLength = -1;
        internal int end = -1;
        internal string group;
        internal double maxFrequency = -1.0;
        public const string MERGE = "MERGE";
        internal string mode;
        public const string RAW = "RAW";
        internal string schema;
        internal string selector = null;
        internal bool snapshot;
        internal int start = -1;
        internal bool unfiltered = false;

        public SimpleTableInfo(string group, string mode, string schema, bool snap)
        {
            this.group = group;
            this.mode = mode;
            this.schema = schema;
            if (snap)
            {
                if ((!mode.Equals("MERGE") && !mode.Equals("DISTINCT")) && !mode.Equals("COMMAND"))
                {
                    throw new SubscrException("Snapshot ineffective for mode " + mode);
                }
                this.snapshot = true;
            }
            else
            {
                this.snapshot = false;
            }
        }

        internal bool hasUnfilteredData()
        {
            return ((this.mode.Equals("RAW") || this.unfiltered) || this.mode.Equals("COMMAND"));
        }

        public virtual void RequestUnfilteredDispatching()
        {
            if ((!this.mode.Equals("MERGE") && !this.mode.Equals("DISTINCT")) && !this.mode.Equals("COMMAND"))
            {
                throw new SubscrException("Unfiltered dispatching cannot be specified for mode " + this.mode);
            }
            this.unfiltered = true;
        }

        public virtual void SetRange(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public virtual string DataAdapter
        {
            set
            {
                this.dataAdapter = value;
            }
        }

        public virtual int RequestedBufferSize
        {
            set
            {
                if (!this.mode.Equals("MERGE") && !this.mode.Equals("DISTINCT"))
                {
                    throw new SubscrException("Buffer size ineffective for mode " + this.mode);
                }
                this.bufferSize = value;
            }
        }

        public virtual int RequestedDistinctSnapshotLength
        {
            set
            {
                if (!this.mode.Equals("DISTINCT"))
                {
                    throw new SubscrException("Snapshot length ineffective for mode " + this.mode);
                }
                if (!this.snapshot)
                {
                    throw new SubscrException("Snapshot not requested for the item");
                }
                this.distinctSnapshotLength = value;
            }
        }

        public virtual double RequestedMaxFrequency
        {
            set
            {
                if ((!this.mode.Equals("MERGE") && !this.mode.Equals("DISTINCT")) && !this.mode.Equals("COMMAND"))
                {
                    throw new SubscrException("Max frequency ineffective for mode " + this.mode);
                }
                this.maxFrequency = value;
            }
        }

        public virtual string Selector
        {
            set
            {
                this.selector = value;
            }
        }
    }
}

