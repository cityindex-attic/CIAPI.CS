namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;
    using System.Collections;

    internal class FullTableManager : ITableManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private SimpleTableInfo baseInfo;
        private ExtendedTableInfo extInfo;
        private IDictionary fieldIndexMap = new Hashtable();
        private bool isCommandLogic;
        private IList itemInfos = new ArrayList();
        private IHandyTableListener listener;
        private static object nullPlaceholder = new object();
        private bool unsubscrDone = false;

        internal FullTableManager(SimpleTableInfo table, IHandyTableListener listener, bool doCommandLogic)
        {
            this.baseInfo = table;
            this.isCommandLogic = doCommandLogic;
            if (table is ExtendedTableInfo)
            {
                this.extInfo = (ExtendedTableInfo) table;
                this.fieldIndexMap = new Hashtable();
                for (int i = 0; i < this.extInfo.fields.Length; i++)
                {
                    this.fieldIndexMap[this.extInfo.fields[i]] = i;
                }
            }
            else
            {
                this.extInfo = null;
                this.fieldIndexMap = null;
            }
            this.listener = listener;
        }

        public virtual void DoUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            int itemCode = values.ItemCode;
            int itemIndex = itemCode - 1;
            this.ProcessUpdate(values, itemCode, itemIndex);
        }

        public virtual void NotifyUnsub()
        {
            ItemInfo[] objects = new ItemInfo[this.itemInfos.Count];
            lock (this.itemInfos)
            {
                objects = (ItemInfo[]) CollectionsSupport.ToArray(this.itemInfos, objects);
                this.unsubscrDone = true;
            }
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != null)
                {
                    this.NotifyUnsubForItem(objects[i].pos, objects[i].name);
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

        internal void NotifyUnsubForItem(int itemPos, string itemName)
        {
            try
            {
                this.listener.OnUnsubscr(itemPos, itemName);
            }
            catch (Exception)
            {
            }
        }

        internal void ProcessUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values, int itemPos, int itemIndex)
        {
            string name = null;
            ItemInfo info;
            if (this.extInfo != null)
            {
                if ((itemIndex < 0) || (itemIndex >= this.extInfo.items.Length))
                {
                    throw new PushServerException(2);
                }
                name = this.extInfo.items[itemIndex];
            }
            lock (this.itemInfos)
            {
                if (this.unsubscrDone)
                {
                    return;
                }
                while (this.itemInfos.Count <= itemIndex)
                {
                    this.itemInfos.Add(null);
                }
                info = (ItemInfo) this.itemInfos[itemIndex];
                if (info == null)
                {
                    if (this.isCommandLogic)
                    {
                        info = new CommandLogicItemInfo(this, itemPos, name);
                    }
                    else
                    {
                        info = new ItemInfo(this, itemPos, name);
                    }
                    this.itemInfos[itemIndex] = info;
                }
            }
            if (values.EOS)
            {
                info.snapshotPending = false;
                try
                {
                    this.listener.OnSnapshotEnd(itemPos, name);
                }
                catch (Exception)
                {
                }
            }
            else if (values.Overflow > 0)
            {
                if (!this.baseInfo.hasUnfilteredData())
                {
                    throw new PushServerException(7);
                }
                actionsLogger.Warn("Got notification of updates lost for item " + info);
                try
                {
                    this.listener.OnRawUpdatesLost(itemPos, name, values.Overflow);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                if ((this.extInfo != null) && (values.Size != this.extInfo.fields.Length))
                {
                    throw new PushServerException(3);
                }
                string[] array = values.Array;
                if (actionsLogger.IsDebugEnabled)
                {
                    actionsLogger.Debug(string.Concat(new object[] { "Got event for item ", info, " with values ", CollectionsSupport.ToString(array) }));
                }
                bool snapshotPending = info.snapshotPending;
                string[] prevState = info.Update(array);
                if (prevState != null)
                {
                    UpdateInfo update = new UpdateInfo(info, prevState, array, snapshotPending);
                    if (actionsLogger.IsDebugEnabled)
                    {
                        actionsLogger.Debug(string.Concat(new object[] { "Notifying event for item ", info, " with values ", update }));
                    }
                    try
                    {
                        this.listener.OnUpdate(itemPos, name, update);
                    }
                    catch (Exception)
                    {
                    }
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
                return this.baseInfo.dataAdapter;
            }
        }

        public virtual int DistinctSnapshotLength
        {
            get
            {
                return this.baseInfo.distinctSnapshotLength;
            }
        }

        public virtual int End
        {
            get
            {
                return this.baseInfo.end;
            }
        }

        public virtual string Group
        {
            get
            {
                return this.baseInfo.group;
            }
        }

        public virtual int MaxBufferSize
        {
            get
            {
                return this.baseInfo.bufferSize;
            }
        }

        public virtual double MaxFrequency
        {
            get
            {
                return this.baseInfo.maxFrequency;
            }
        }

        public virtual string Mode
        {
            get
            {
                return this.baseInfo.mode;
            }
        }

        public virtual string Schema
        {
            get
            {
                return this.baseInfo.schema;
            }
        }

        public virtual string Selector
        {
            get
            {
                return this.baseInfo.selector;
            }
        }

        public virtual bool Snapshot
        {
            get
            {
                return this.baseInfo.snapshot;
            }
        }

        public virtual int Start
        {
            get
            {
                return this.baseInfo.start;
            }
        }

        public virtual bool Unfiltered
        {
            get
            {
                return this.baseInfo.unfiltered;
            }
        }

        private class CommandLogicItemInfo : FullTableManager.ItemInfo
        {
            private int commandIndex;
            private int keyIndex;
            private IDictionary keyStates;

            public CommandLogicItemInfo(FullTableManager enclosingInstance, int pos, string name) : base(enclosingInstance, pos, name)
            {
                this.keyStates = new Hashtable();
                if (enclosingInstance.extInfo != null)
                {
                    if (enclosingInstance.fieldIndexMap.Contains("key"))
                    {
                        this.keyIndex = (int) enclosingInstance.fieldIndexMap["key"];
                    }
                    else
                    {
                        this.keyIndex = -1;
                    }
                    if (enclosingInstance.fieldIndexMap.Contains("command"))
                    {
                        this.commandIndex = (int) enclosingInstance.fieldIndexMap["command"];
                    }
                    else
                    {
                        this.commandIndex = -1;
                    }
                }
                else
                {
                    this.keyIndex = 0;
                    this.commandIndex = 1;
                }
            }

            public override string[] Update(string[] updEvent)
            {
                int num;
                base.Update(updEvent);
                string str = "ADD";
                string str2 = "UPDATE";
                string str3 = "DELETE";
                object nullPlaceholder = FullTableManager.nullPlaceholder;
                if ((this.keyIndex >= 0) && (this.keyIndex < base.currState.Length))
                {
                    nullPlaceholder = base.currState[this.keyIndex];
                }
                else
                {
                    FullTableManager.actionsLogger.Warn("key field not subscribed for item " + this + " - null key forced for command logic");
                }
                string str4 = null;
                if ((this.commandIndex >= 0) && (this.commandIndex < base.currState.Length))
                {
                    str4 = base.currState[this.commandIndex];
                    if (str4 == null)
                    {
                        FullTableManager.actionsLogger.Warn("No value found for command field for item " + this + " - trying to add/update for command logic");
                    }
                    else if (str4.Equals(str3))
                    {
                        str4 = str3;
                    }
                    else if (str4.Equals(str))
                    {
                        str4 = str;
                    }
                    else if (str4.Equals(str2))
                    {
                        str4 = str2;
                    }
                    else
                    {
                        FullTableManager.actionsLogger.Warn("Invalid value for command field for item " + this + " - trying to add/update for command logic");
                    }
                }
                else
                {
                    FullTableManager.actionsLogger.Warn("command field not subscribed for item " + this + " - trying to add/update for command logic");
                }
                string[] strArray = (string[]) this.keyStates[nullPlaceholder];
                if (str4 == str3)
                {
                    if (FullTableManager.actionsLogger.IsDebugEnabled)
                    {
                        FullTableManager.actionsLogger.Debug(string.Concat(new object[] { "Processing DELETE event in COMMAND logic for item ", this, " and key ", (nullPlaceholder != FullTableManager.nullPlaceholder) ? nullPlaceholder : "null" }));
                    }
                    if (strArray == null)
                    {
                        FullTableManager.actionsLogger.Warn("Unexpected DELETE command for item " + this + " - discarding the command");
                        return null;
                    }
                    this.keyStates.Remove(nullPlaceholder);
                    for (num = 0; num < base.currState.Length; num++)
                    {
                        if (num == this.keyIndex)
                        {
                            updEvent[num] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                        }
                        else if (num == this.commandIndex)
                        {
                            updEvent[num] = str3;
                        }
                        else
                        {
                            updEvent[num] = null;
                        }
                    }
                    return strArray;
                }
                if (FullTableManager.actionsLogger.IsDebugEnabled)
                {
                    FullTableManager.actionsLogger.Debug(string.Concat(new object[] { "Processing ADD/UPDATE event in COMMAND logic for item ", this, " and key ", (nullPlaceholder != FullTableManager.nullPlaceholder) ? nullPlaceholder : "null" }));
                }
                if (strArray == null)
                {
                    if (str4 == str2)
                    {
                        FullTableManager.actionsLogger.Warn("Unexpected UPDATE command for item " + this + " - command changed into ADD");
                    }
                    for (num = 0; num < base.currState.Length; num++)
                    {
                        if (num == this.commandIndex)
                        {
                            updEvent[num] = str;
                        }
                        else
                        {
                            updEvent[num] = base.currState[num];
                        }
                    }
                    this.keyStates[nullPlaceholder] = updEvent;
                    return new string[base.currState.Length];
                }
                if (str4 == str)
                {
                    FullTableManager.actionsLogger.Warn("Unexpected ADD command for item " + this + " - command changed into UPDATE");
                }
                for (num = 0; num < base.currState.Length; num++)
                {
                    if (num == this.keyIndex)
                    {
                        updEvent[num] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else if (num == this.commandIndex)
                    {
                        updEvent[num] = (strArray[num] == str) ? str2 : Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else if ((base.currState[num] == null) && (strArray[num] == null))
                    {
                        updEvent[num] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else if ((base.currState[num] == null) || (strArray[num] == null))
                    {
                        updEvent[num] = base.currState[num];
                    }
                    else if (base.currState[num].Equals(strArray[num]))
                    {
                        updEvent[num] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else
                    {
                        updEvent[num] = base.currState[num];
                    }
                }
                if (str4 == str2)
                {
                    this.keyStates[nullPlaceholder] = base.currState;
                }
                else
                {
                    string[] strArray2 = new string[base.currState.Length];
                    for (num = 0; num < base.currState.Length; num++)
                    {
                        if (num == this.commandIndex)
                        {
                            strArray2[num] = str2;
                        }
                        else
                        {
                            strArray2[num] = base.currState[num];
                        }
                    }
                    this.keyStates[nullPlaceholder] = strArray2;
                }
                return strArray;
            }
        }

        internal class ItemInfo
        {
            protected string[] currState;
            protected FullTableManager enclosingInstance;
            public string name;
            public int pos;
            public bool snapshotPending;

            public ItemInfo(FullTableManager enclosingInstance, int pos, string name)
            {
                this.enclosingInstance = enclosingInstance;
                this.pos = pos;
                this.name = name;
                this.snapshotPending = enclosingInstance.baseInfo.snapshot;
            }

            public IDictionary GetFieldIndexMap()
            {
                return this.enclosingInstance.fieldIndexMap;
            }

            public override string ToString()
            {
                if (this.name != null)
                {
                    return this.name;
                }
                return this.pos.ToString();
            }

            public virtual string[] Update(string[] updEvent)
            {
                if (this.currState == null)
                {
                    this.currState = new string[updEvent.Length];
                }
                string[] currState = this.currState;
                this.currState = new string[this.currState.Length];
                for (int i = 0; i < this.currState.Length; i++)
                {
                    if (updEvent[i] != Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED)
                    {
                        this.currState[i] = updEvent[i];
                    }
                    else
                    {
                        this.currState[i] = currState[i];
                    }
                }
                if (this.snapshotPending && this.enclosingInstance.baseInfo.mode.Equals("MERGE"))
                {
                    this.snapshotPending = false;
                }
                return currState;
            }
        }
    }
}

