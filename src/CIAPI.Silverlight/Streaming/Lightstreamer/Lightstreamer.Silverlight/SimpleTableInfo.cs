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

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        [Obsolete("Use the RangeEnd property instead of the GetEnd method.")]
        public virtual int GetEnd()
        {
            return ((this.end != -1) ? this.end : 0);
        }

        [Obsolete("Use the Group property instead of the GetGroup method.")]
        public virtual string GetGroup()
        {
            return this.group;
        }

        [Obsolete("Use the Mode property instead of the GetMode method.")]
        public virtual string GetMode()
        {
            return this.mode;
        }

        [Obsolete("Use the Schema property instead of the GetSchema method.")]
        public virtual string GetSchema()
        {
            return this.schema;
        }

        [Obsolete("Use the RangeStart property instead of the GetStart method.")]
        public virtual int GetStart()
        {
            return ((this.start != -1) ? this.start : 0);
        }

        internal bool hasUnfilteredData()
        {
            return ((this.mode.Equals("RAW") || this.unfiltered) || this.mode.Equals("COMMAND"));
        }

        [Obsolete("Use the Snapshot property instead of the IsSnapshot method.")]
        public virtual bool IsSnapshot()
        {
            return this.snapshot;
        }

        [Obsolete("Use the RequestedUnfilteredDispatching property instead of the IsUnfilteredDispatching method.")]
        public virtual bool IsUnfilteredDispatching()
        {
            return this.unfiltered;
        }

        [Obsolete("Use the RequestedUnfilteredDispatching property instead of the RequestUnfilteredDispatching method.")]
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
            get
            {
                return this.dataAdapter;
            }
            set
            {
                this.dataAdapter = value;
            }
        }

        public virtual string Group
        {
            get
            {
                return this.group;
            }
        }

        public virtual string Mode
        {
            get
            {
                return this.mode;
            }
        }

        public virtual int RangeEnd
        {
            get
            {
                return this.GetEnd();
            }
        }

        public virtual int RangeStart
        {
            get
            {
                return this.GetStart();
            }
        }

        public virtual int RequestedBufferSize
        {
            get
            {
                return ((this.bufferSize != -1) ? this.bufferSize : -1);
            }
            set
            {
                if (!this.mode.Equals("MERGE") && !this.mode.Equals("DISTINCT"))
                {
                    throw new SubscrException("Buffer size ineffective for mode " + this.mode);
                }
                this.bufferSize = (value >= 0) ? value : -1;
            }
        }

        public virtual int RequestedDistinctSnapshotLength
        {
            get
            {
                return ((this.distinctSnapshotLength != -1) ? this.distinctSnapshotLength : 0);
            }
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
                this.distinctSnapshotLength = (value > 0) ? value : -1;
            }
        }

        public virtual double RequestedMaxFrequency
        {
            get
            {
                return ((this.maxFrequency != -1.0) ? this.maxFrequency : 0.0);
            }
            set
            {
                if ((!this.mode.Equals("MERGE") && !this.mode.Equals("DISTINCT")) && !this.mode.Equals("COMMAND"))
                {
                    throw new SubscrException("Max frequency ineffective for mode " + this.mode);
                }
                this.maxFrequency = (value > 0.0) ? value : -1.0;
            }
        }

        public virtual bool RequestedUnfilteredDispatching
        {
            get
            {
                return this.unfiltered;
            }
            set
            {
                if (value)
                {
                    this.RequestUnfilteredDispatching();
                }
                else
                {
                    this.unfiltered = value;
                }
            }
        }

        public virtual string Schema
        {
            get
            {
                return this.schema;
            }
        }

        public virtual string Selector
        {
            get
            {
                return this.selector;
            }
            set
            {
                this.selector = value;
            }
        }

        public virtual bool Snapshot
        {
            get
            {
                return this.snapshot;
            }
        }
    }
}

