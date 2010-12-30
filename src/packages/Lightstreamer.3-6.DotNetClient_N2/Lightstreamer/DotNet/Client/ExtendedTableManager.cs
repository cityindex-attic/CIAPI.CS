namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;
    using System.Collections;

    internal class ExtendedTableManager : ITableManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private IExtendedTableListener listener;
        private ExtendedTableInfo table;

        internal ExtendedTableManager(ExtendedTableInfo table, IExtendedTableListener listener)
        {
            this.table = table;
            this.listener = listener;
        }

        public virtual void DoUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            int itemCode = values.ItemCode;
            if ((itemCode <= 0) || (itemCode > this.table.items.Length))
            {
                throw new PushServerException(2);
            }
            ProcessUpdate(values, itemCode - 1, this.table, this.listener);
        }

        public virtual void NotifyUnsub()
        {
            for (int i = 0; i < this.table.items.Length; i++)
            {
                try
                {
                    this.listener.OnUnsubscr(this.table.items[i]);
                }
                catch (Exception)
                {
                }
            }
            try
            {
                this.listener.OnUnsubscrAll();
            }
            catch (Exception)
            {
            }
        }

        internal static void ProcessUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values, int itemIndex, ExtendedTableInfo table, IExtendedTableListener listener)
        {
            string item = table.items[itemIndex];
            if (values.EOS)
            {
                try
                {
                    listener.OnSnapshotEnd(item);
                }
                catch (Exception)
                {
                }
            }
            else if (values.Overflow > 0)
            {
                if (!table.hasUnfilteredData())
                {
                    throw new PushServerException(7);
                }
                actionsLogger.Warn("Got notification of updates lost for item " + item);
                try
                {
                    listener.OnRawUpdatesLost(item, values.Overflow);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                if (values.Size != table.fields.Length)
                {
                    throw new PushServerException(3);
                }
                IDictionary map = values.GetMap(table.fields);
                if (actionsLogger.IsDebugEnabled)
                {
                    actionsLogger.Debug("Got event for item " + item + " with values " + CollectionsSupport.ToString(map));
                }
                try
                {
                    listener.OnUpdate(item, map);
                }
                catch (Exception)
                {
                }
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

