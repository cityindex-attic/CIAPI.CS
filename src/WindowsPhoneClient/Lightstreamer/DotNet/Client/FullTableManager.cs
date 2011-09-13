namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class FullTableManager : ITableManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private SimpleTableInfo baseInfo;
        private ExtendedTableInfo extInfo;
        private IDictionary fieldIndexMap = new Dictionary<string, int>();
        private bool isCommandLogic;
        private IList itemInfos = new List<ItemInfo>();
        private IHandyTableListener listener;
        private static object nullPlaceholder = new object();
        private bool unsubscrDone = false;

        internal FullTableManager(SimpleTableInfo table, IHandyTableListener listener, bool doCommandLogic)
        {
            this.baseInfo = (SimpleTableInfo) table.Clone();
            this.isCommandLogic = doCommandLogic;
            if (table is ExtendedTableInfo)
            {
                this.extInfo = (ExtendedTableInfo) this.baseInfo;
                this.fieldIndexMap = new Dictionary<string, int>();
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
            int itemPos = values.ItemCode;
            int itemIndex = itemPos - 1;
            this.ProcessUpdate(values, itemPos, itemIndex);
        }

        public virtual void NotifyUnsub()
        {
            ItemInfo[] infos = new ItemInfo[this.itemInfos.Count];
            lock (this.itemInfos)
            {
                infos = (ItemInfo[]) CollectionsSupport.ToArray(this.itemInfos, infos);
                this.unsubscrDone = true;
            }
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i] != null)
                {
                    this.NotifyUnsubForItem(infos[i].pos, infos[i].name);
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
            string itemName = null;
            ItemInfo info;
            if (this.extInfo != null)
            {
                if ((itemIndex < 0) || (itemIndex >= this.extInfo.items.Length))
                {
                    throw new PushServerException(2);
                }
                itemName = this.extInfo.items[itemIndex];
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
                        info = new CommandLogicItemInfo(this, itemPos, itemName);
                    }
                    else
                    {
                        info = new ItemInfo(this, itemPos, itemName);
                    }
                    this.itemInfos[itemIndex] = info;
                }
            }
            if (values.EOS)
            {
                info.snapshotPending = false;
                try
                {
                    this.listener.OnSnapshotEnd(itemPos, itemName);
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
                    this.listener.OnRawUpdatesLost(itemPos, itemName, values.Overflow);
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
                string[] update = values.Array;
                if (actionsLogger.IsDebugEnabled)
                {
                    actionsLogger.Debug(string.Concat(new object[] { "Got event for item ", info, " with values ", CollectionsSupport.ToString(update) }));
                }
                bool isSnapshot = info.snapshotPending;
                string[] prevState = info.Update(update);
                if (prevState != null)
                {
                    IUpdateInfo updInfo = new UpdateInfoImpl(info, prevState, update, isSnapshot);
                    if (actionsLogger.IsDebugEnabled)
                    {
                        actionsLogger.Debug(string.Concat(new object[] { "Notifying event for item ", info, " with values ", updInfo }));
                    }
                    try
                    {
                        this.listener.OnUpdate(itemPos, itemName, updInfo);
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
                this.keyStates = new Dictionary<object, string[]>();
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
                int i;
                base.Update(updEvent);
                string ADD = "ADD";
                string UPDATE = "UPDATE";
                string DELETE = "DELETE";
                object key = FullTableManager.nullPlaceholder;
                if ((this.keyIndex >= 0) && (this.keyIndex < base.currState.Length))
                {
                    key = base.currState[this.keyIndex];
                }
                else
                {
                    FullTableManager.actionsLogger.Warn("key field not subscribed for item " + this + " - null key forced for command logic");
                }
                string command = null;
                if ((this.commandIndex >= 0) && (this.commandIndex < base.currState.Length))
                {
                    command = base.currState[this.commandIndex];
                    if (command == null)
                    {
                        FullTableManager.actionsLogger.Warn("No value found for command field for item " + this + " - trying to add/update for command logic");
                    }
                    else if (command.Equals(DELETE))
                    {
                        command = DELETE;
                    }
                    else if (command.Equals(ADD))
                    {
                        command = ADD;
                    }
                    else if (command.Equals(UPDATE))
                    {
                        command = UPDATE;
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
                string[] keyState = (string[]) this.keyStates[key];
                if (command == DELETE)
                {
                    if (FullTableManager.actionsLogger.IsDebugEnabled)
                    {
                        FullTableManager.actionsLogger.Debug(string.Concat(new object[] { "Processing DELETE event in COMMAND logic for item ", this, " and key ", (key != FullTableManager.nullPlaceholder) ? key : "null" }));
                    }
                    if (keyState == null)
                    {
                        FullTableManager.actionsLogger.Warn("Unexpected DELETE command for item " + this + " - discarding the command");
                        return null;
                    }
                    this.keyStates.Remove(key);
                    for (i = 0; i < base.currState.Length; i++)
                    {
                        if (i == this.keyIndex)
                        {
                            updEvent[i] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                        }
                        else if (i == this.commandIndex)
                        {
                            updEvent[i] = DELETE;
                        }
                        else
                        {
                            updEvent[i] = null;
                        }
                    }
                    return keyState;
                }
                if (FullTableManager.actionsLogger.IsDebugEnabled)
                {
                    FullTableManager.actionsLogger.Debug(string.Concat(new object[] { "Processing ADD/UPDATE event in COMMAND logic for item ", this, " and key ", (key != FullTableManager.nullPlaceholder) ? key : "null" }));
                }
                if (keyState == null)
                {
                    if (command == UPDATE)
                    {
                        FullTableManager.actionsLogger.Warn("Unexpected UPDATE command for item " + this + " - command changed into ADD");
                    }
                    for (i = 0; i < base.currState.Length; i++)
                    {
                        if (i == this.commandIndex)
                        {
                            updEvent[i] = ADD;
                        }
                        else
                        {
                            updEvent[i] = base.currState[i];
                        }
                    }
                    this.keyStates[key] = updEvent;
                    return new string[base.currState.Length];
                }
                if (command == ADD)
                {
                    FullTableManager.actionsLogger.Warn("Unexpected ADD command for item " + this + " - command changed into UPDATE");
                }
                for (i = 0; i < base.currState.Length; i++)
                {
                    if (i == this.keyIndex)
                    {
                        updEvent[i] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else if (i == this.commandIndex)
                    {
                        updEvent[i] = (keyState[i] == ADD) ? UPDATE : Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else if ((base.currState[i] == null) && (keyState[i] == null))
                    {
                        updEvent[i] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else if ((base.currState[i] == null) || (keyState[i] == null))
                    {
                        updEvent[i] = base.currState[i];
                    }
                    else if (base.currState[i].Equals(keyState[i]))
                    {
                        updEvent[i] = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
                    }
                    else
                    {
                        updEvent[i] = base.currState[i];
                    }
                }
                if (command == UPDATE)
                {
                    this.keyStates[key] = base.currState;
                }
                else
                {
                    string[] newState = new string[base.currState.Length];
                    for (i = 0; i < base.currState.Length; i++)
                    {
                        if (i == this.commandIndex)
                        {
                            newState[i] = UPDATE;
                        }
                        else
                        {
                            newState[i] = base.currState[i];
                        }
                    }
                    this.keyStates[key] = newState;
                }
                return keyState;
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
                string[] prevState = this.currState;
                this.currState = new string[this.currState.Length];
                for (int i = 0; i < this.currState.Length; i++)
                {
                    if (updEvent[i] != Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED)
                    {
                        this.currState[i] = updEvent[i];
                    }
                    else
                    {
                        this.currState[i] = prevState[i];
                    }
                }
                if (this.snapshotPending && this.enclosingInstance.baseInfo.mode.Equals("MERGE"))
                {
                    this.snapshotPending = false;
                }
                return prevState;
            }
        }
    }
}

