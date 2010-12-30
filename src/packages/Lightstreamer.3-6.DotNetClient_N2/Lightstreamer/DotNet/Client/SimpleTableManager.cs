namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;

    internal class SimpleTableManager : ITableManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private ISimpleTableListener listener;
        private SimpleTableInfo table;

        internal SimpleTableManager(SimpleTableInfo table, ISimpleTableListener listener)
        {
            this.table = table;
            this.listener = listener;
        }

        public virtual void DoUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            int itemCode = values.ItemCode;
            if (values.EOS)
            {
                try
                {
                    this.listener.OnSnapshotEnd(itemCode);
                }
                catch (Exception)
                {
                }
            }
            else if (values.Overflow > 0)
            {
                if (!this.table.hasUnfilteredData())
                {
                    throw new PushServerException(7);
                }
                try
                {
                    this.listener.OnRawUpdatesLost(itemCode, values.Overflow);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                string[] array = values.Array;
                if (actionsLogger.IsDebugEnabled)
                {
                    actionsLogger.Debug(string.Concat(new object[] { "Got event for item n\x00b0", itemCode, " in group ", this.table.group, " with values ", CollectionsSupport.ToString(array) }));
                }
                try
                {
                    this.listener.OnUpdate(itemCode, array);
                }
                catch (Exception)
                {
                }
            }
        }

        public virtual void NotifyUnsub()
        {
            try
            {
                this.listener.OnUnsubscrAll();
            }
            catch (Exception)
            {
            }
        }

        public override string ToString()
        {
            return (this.Mode + " table [" + this.Group + " ; " + this.Schema + "]");
        }

        public virtual string DataAdapter
        {
            get
            {
                return this.table.dataAdapter;
            }
        }

        public virtual int DistinctSnapshotLength
        {
            get
            {
                return this.table.distinctSnapshotLength;
            }
        }

        public virtual int End
        {
            get
            {
                return this.table.end;
            }
        }

        public virtual string Group
        {
            get
            {
                return this.table.group;
            }
        }

        public virtual int MaxBufferSize
        {
            get
            {
                return this.table.bufferSize;
            }
        }

        public virtual double MaxFrequency
        {
            get
            {
                return this.table.maxFrequency;
            }
        }

        public virtual string Mode
        {
            get
            {
                return this.table.mode;
            }
        }

        public virtual string Schema
        {
            get
            {
                return this.table.schema;
            }
        }

        public virtual string Selector
        {
            get
            {
                return this.table.selector;
            }
        }

        public virtual bool Snapshot
        {
            get
            {
                return this.table.snapshot;
            }
        }

        public virtual int Start
        {
            get
            {
                return this.table.start;
            }
        }

        public virtual bool Unfiltered
        {
            get
            {
                return this.table.unfiltered;
            }
        }
    }
}

