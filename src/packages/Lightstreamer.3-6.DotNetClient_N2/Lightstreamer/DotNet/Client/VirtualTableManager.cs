namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;

    internal class VirtualTableManager : ITableManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private IExtendedTableListener extListener;
        private IFastItemsListener fastListener;
        private FullTableManager managerWithListener;
        private ExtendedTableInfo table;

        internal VirtualTableManager(ExtendedTableInfo table, IExtendedTableListener listener)
        {
            this.managerWithListener = null;
            this.extListener = null;
            this.fastListener = null;
            this.table = table;
            this.extListener = listener;
        }

        internal VirtualTableManager(ExtendedTableInfo table, IFastItemsListener listener)
        {
            this.managerWithListener = null;
            this.extListener = null;
            this.fastListener = null;
            this.table = table;
            this.fastListener = listener;
        }

        internal VirtualTableManager(ExtendedTableInfo table, IHandyTableListener listener)
        {
            this.managerWithListener = null;
            this.extListener = null;
            this.fastListener = null;
            this.table = table;
            this.managerWithListener = new FullTableManager(table, listener, false);
        }

        public virtual void DoUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            throw new PushServerException(12);
        }

        public virtual object GetItemManager(int i)
        {
            return new MonoTableManager(this, i);
        }

        public virtual object GetItemName(int i)
        {
            return this.table.items[i];
        }

        public virtual void NotifyUnsub()
        {
        }

        private static void ProcessFastUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values, int itemIndex, ExtendedTableInfo table, IFastItemsListener listener)
        {
            if (values.EOS)
            {
                try
                {
                    listener.OnSnapshotEnd(itemIndex + 1);
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
                try
                {
                    listener.OnRawUpdatesLost(itemIndex + 1, values.Overflow);
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
                string[] array = values.Array;
                if (actionsLogger.IsDebugEnabled)
                {
                    actionsLogger.Debug("Got event for item " + table.items[itemIndex] + " with values " + CollectionsSupport.ToString(array) + " for fields " + CollectionsSupport.ToString(table.fields));
                }
                try
                {
                    listener.OnUpdate(itemIndex + 1, array);
                }
                catch (Exception)
                {
                }
            }
        }

        public override string ToString()
        {
            return (this.Mode + " items [" + this.Group + "] with fields [" + this.Schema + "]");
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

        public virtual int NumItems
        {
            get
            {
                return this.table.items.Length;
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

        private class MonoTableManager : ITableManager
        {
            private VirtualTableManager enclosingInstance;
            private int itemIndex;

            public MonoTableManager(VirtualTableManager enclosingInstance, int itemIndex)
            {
                this.enclosingInstance = enclosingInstance;
                this.itemIndex = itemIndex;
            }

            public virtual void DoUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
            {
                if (values.ItemCode != 1)
                {
                    throw new PushServerException(2);
                }
                if (this.enclosingInstance.managerWithListener != null)
                {
                    this.enclosingInstance.managerWithListener.ProcessUpdate(values, this.itemIndex + 1, this.itemIndex);
                }
                else if (this.enclosingInstance.extListener != null)
                {
                    ExtendedTableManager.ProcessUpdate(values, this.itemIndex, this.enclosingInstance.table, this.enclosingInstance.extListener);
                }
                else
                {
                    VirtualTableManager.ProcessFastUpdate(values, this.itemIndex, this.enclosingInstance.table, this.enclosingInstance.fastListener);
                }
            }

            public virtual void NotifyUnsub()
            {
                if (this.enclosingInstance.managerWithListener != null)
                {
                    int itemPos = this.itemIndex + 1;
                    string itemName = this.enclosingInstance.table.items[this.itemIndex];
                    this.enclosingInstance.managerWithListener.NotifyUnsubForItem(itemPos, itemName);
                }
                else if (this.enclosingInstance.extListener != null)
                {
                    try
                    {
                        this.enclosingInstance.extListener.OnUnsubscr(this.enclosingInstance.table.items[this.itemIndex]);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    try
                    {
                        this.enclosingInstance.fastListener.OnUnsubscr(this.itemIndex + 1);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public override string ToString()
            {
                return (this.Mode + " item " + this.Group + " with fields [" + this.Schema + "]");
            }

            public virtual string DataAdapter
            {
                get
                {
                    return this.enclosingInstance.table.dataAdapter;
                }
            }

            public virtual int DistinctSnapshotLength
            {
                get
                {
                    return this.enclosingInstance.DistinctSnapshotLength;
                }
            }

            public virtual int End
            {
                get
                {
                    return -1;
                }
            }

            public virtual string Group
            {
                get
                {
                    return this.enclosingInstance.table.items[this.itemIndex];
                }
            }

            public virtual int MaxBufferSize
            {
                get
                {
                    return this.enclosingInstance.MaxBufferSize;
                }
            }

            public virtual double MaxFrequency
            {
                get
                {
                    return this.enclosingInstance.MaxFrequency;
                }
            }

            public virtual string Mode
            {
                get
                {
                    return this.enclosingInstance.Mode;
                }
            }

            public virtual string Schema
            {
                get
                {
                    return this.enclosingInstance.Schema;
                }
            }

            public virtual string Selector
            {
                get
                {
                    return this.enclosingInstance.table.selector;
                }
            }

            public virtual bool Snapshot
            {
                get
                {
                    return this.enclosingInstance.Snapshot;
                }
            }

            public virtual int Start
            {
                get
                {
                    return -1;
                }
            }

            public virtual bool Unfiltered
            {
                get
                {
                    return this.enclosingInstance.Unfiltered;
                }
            }
        }
    }
}

