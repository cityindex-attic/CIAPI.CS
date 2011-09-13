namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;

    internal class VirtualTableManager : ITableManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private FullTableManager managerWithListener = null;
        private ExtendedTableInfo table;

        internal VirtualTableManager(ExtendedTableInfo table, IHandyTableListener listener)
        {
            this.table = (ExtendedTableInfo) table.Clone();
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
                this.enclosingInstance.managerWithListener.ProcessUpdate(values, this.itemIndex + 1, this.itemIndex);
            }

            public virtual void NotifyUnsub()
            {
                int itemPos = this.itemIndex + 1;
                string itemName = this.enclosingInstance.table.items[this.itemIndex];
                this.enclosingInstance.managerWithListener.NotifyUnsubForItem(itemPos, itemName);
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

