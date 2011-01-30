namespace Lightstreamer.DotNet.Client
{
    using Common.Logging;
    using System;
    using System.Threading;

    public class LSClient
    {
        private static ILog actionsLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.actions));
        private MyServerListener asyncListener;
        private IConnectionListener connListener;
        private ServerManager connManager;
        private object connMutex = new object();
        private int phase = 0;
        private object stateMutex = new object();
        private LSClient subClient = null;

        public virtual void BatchRequests(int batchSize)
        {
            ServerManager connManager = this.ConnManager;
            try
            {
                connManager.BatchRequests(batchSize);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
        }

        public virtual void ChangeConstraints(ConnectionConstraints constraints)
        {
            ServerManager connManager;
            try
            {
                connManager = this.ConnManager;
            }
            catch (SubscrException)
            {
                return;
            }
            try
            {
                connManager.ChangeConstraints(constraints);
            }
            catch (PhaseException)
            {
            }
        }

        public virtual void CloseBatch()
        {
            try
            {
                this.ConnManager.CloseBatch();
            }
            catch (SubscrException)
            {
                actionsLogger.Debug("Unbatch request received with no open session");
            }
        }

        public virtual void CloseConnection()
        {
            IConnectionListener closedListener = null;
            ServerManager connManager = null;
            MyServerListener asyncListener = null;
            LSClient subClient = null;
            lock (this.connMutex)
            {
                lock (this.stateMutex)
                {
                    if (this.subClient != null)
                    {
                        subClient = this.subClient;
                        this.subClient = null;
                    }
                    else if (this.connManager != null)
                    {
                        this.phase++;
                        connManager = this.connManager;
                        closedListener = this.connListener;
                        asyncListener = this.asyncListener;
                        this.connManager = null;
                        this.connListener = null;
                        this.asyncListener = null;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (subClient != null)
            {
                subClient.CloseConnection();
            }
            else
            {
                foreach (ITableManager manager2 in connManager.Close())
                {
                    manager2.NotifyUnsub();
                }
                asyncListener.OnClosed(closedListener);
            }
        }

        public virtual void ForceUnsubscribeTable(SubscribedTableKey tableKey)
        {
            ServerManager connManager = this.ConnManager;
            if (tableKey.KeyValue != -1)
            {
                SubscribedTableKey[] subscrKeys = new SubscribedTableKey[] { tableKey };
                try
                {
                    connManager.UnsubscrTables(subscrKeys, false);
                }
                catch (PhaseException)
                {
                    throw new SubscrException("Connection closed");
                }
            }
        }

        internal IConnectionListener GetActiveListener(int currPhase)
        {
            lock (this.stateMutex)
            {
                if (this.subClient != null)
                {
                    return this.subClient.GetActiveListener(currPhase);
                }
                if (currPhase == this.phase)
                {
                    return this.connListener;
                }
                return null;
            }
        }

        public virtual void OpenConnection(ConnectionInfo info, IConnectionListener listener)
        {
            ConnectionInfo info2 = (ConnectionInfo) info.Clone();
            info2.useGetForStreaming = true;
            lock (this.connMutex)
            {
                object obj3;
                this.CloseConnection();
                if (info2.enableStreamSense && !info2.isPolling)
                {
                    ThreadStart start = null;
                    LSClient testClient = new LSClient();
                    ExtConnectionListener myListener = new ExtConnectionListener(listener);
                    ConnectionInfo mySubInfo = (ConnectionInfo) info.Clone();
                    mySubInfo.enableStreamSense = false;
                    new Thread(() => {
                        try
                        {
                            testClient.OpenConnection(mySubInfo, myListener);
                        }
                        catch (Exception)
                        {
                        }
                    }).Start();
                    if (!myListener.WaitStreamingTimeoutAnswer())
                    {
                        lock ((obj3 = this.stateMutex))
                        {
                            this.subClient = testClient;
                        }
                        myListener.FlushAndStart();
                    }
                    else
                    {
                        if (start == null)
                        {
                            start = delegate {
                                testClient.CloseConnection();
                            };
                        }
                        new Thread(start).Start();
                        LSClient client = new LSClient();
                        info2.isPolling = true;
                        client.OpenConnection(info2, listener);
                        lock ((obj3 = this.stateMutex))
                        {
                            this.subClient = client;
                        }
                    }
                }
                else
                {
                    int num;
                    lock ((obj3 = this.stateMutex))
                    {
                        num = this.phase + 1;
                    }
                    MyServerListener asyncListener = new MyServerListener(this, listener, num);
                    bool flag2 = false;
                    try
                    {
                        ServerManager manager = new ServerManager(info2, asyncListener);
                        manager.Connect();
                        lock ((obj3 = this.stateMutex))
                        {
                            this.connListener = listener;
                            this.asyncListener = asyncListener;
                            this.phase = num;
                            this.connManager = manager;
                        }
                        flag2 = true;
                        manager.Start();
                    }
                    finally
                    {
                        if (!flag2)
                        {
                            asyncListener.OnClosed(null);
                        }
                    }
                }
            }
        }

        public virtual void SendMessage(string message)
        {
            ServerManager connManager;
            try
            {
                connManager = this.ConnManager;
            }
            catch (SubscrException)
            {
                return;
            }
            try
            {
                connManager.SendMessage(message);
            }
            catch (PhaseException)
            {
            }
        }

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener.")]
        public virtual SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IExtendedTableListener listener)
        {
            SubscribedTableKey[] keyArray;
            ServerManager connManager = this.ConnManager;
            VirtualTableManager table = new VirtualTableManager(items, listener);
            try
            {
                keyArray = connManager.SubscrItems(table, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return keyArray;
        }

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener.")]
        public virtual SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IFastItemsListener listener)
        {
            SubscribedTableKey[] keyArray;
            ServerManager connManager = this.ConnManager;
            VirtualTableManager table = new VirtualTableManager(items, listener);
            try
            {
                keyArray = connManager.SubscrItems(table, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return keyArray;
        }

        public virtual SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IHandyTableListener listener)
        {
            SubscribedTableKey[] keyArray;
            ServerManager connManager = this.ConnManager;
            VirtualTableManager table = new VirtualTableManager(items, listener);
            try
            {
                keyArray = connManager.SubscrItems(table, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return keyArray;
        }

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener.")]
        public virtual SubscribedTableKey SubscribeTable(ExtendedTableInfo table, IExtendedTableListener listener)
        {
            SubscribedTableKey key;
            ServerManager connManager = this.ConnManager;
            ITableManager manager2 = new ExtendedTableManager(table, listener);
            try
            {
                key = connManager.SubscrTable(manager2, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return key;
        }

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener.")]
        public virtual SubscribedTableKey SubscribeTable(SimpleTableInfo table, ISimpleTableListener listener)
        {
            SubscribedTableKey key;
            ServerManager connManager = this.ConnManager;
            ITableManager manager2 = new SimpleTableManager(table, listener);
            try
            {
                key = connManager.SubscrTable(manager2, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return key;
        }

        public virtual SubscribedTableKey SubscribeTable(ExtendedTableInfo table, IHandyTableListener listener, bool commandLogic)
        {
            SubscribedTableKey key;
            ServerManager connManager = this.ConnManager;
            ITableManager manager2 = new FullTableManager(table, listener, commandLogic);
            try
            {
                key = connManager.SubscrTable(manager2, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return key;
        }

        public virtual SubscribedTableKey SubscribeTable(SimpleTableInfo table, IHandyTableListener listener, bool commandLogic)
        {
            SubscribedTableKey key;
            ServerManager connManager = this.ConnManager;
            ITableManager manager2 = new FullTableManager(table, listener, commandLogic);
            try
            {
                key = connManager.SubscrTable(manager2, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return key;
        }

        public virtual void UnbatchRequest()
        {
            try
            {
                this.ConnManager.UnbatchRequest();
            }
            catch (SubscrException)
            {
                actionsLogger.Debug("Unbatch request received with no open session");
            }
        }

        public virtual void UnsubscribeTable(SubscribedTableKey tableKey)
        {
            ServerManager connManager = this.ConnManager;
            if (tableKey.KeyValue != -1)
            {
                SubscribedTableKey[] subscrKeys = new SubscribedTableKey[] { tableKey };
                ITableManager[] managerArray = connManager.DetachTables(subscrKeys);
                if (managerArray[0] == null)
                {
                    try
                    {
                        connManager.UnsubscrTables(new SubscribedTableKey[0], true);
                    }
                    catch (PhaseException)
                    {
                    }
                    throw new SubscrException("Table not found");
                }
                managerArray[0].NotifyUnsub();
                try
                {
                    connManager.UnsubscrTables(subscrKeys, true);
                }
                catch (PhaseException)
                {
                    throw new SubscrException("Connection closed");
                }
            }
        }

        public virtual void UnsubscribeTables(SubscribedTableKey[] tableKeys)
        {
            int num2;
            ServerManager connManager = this.ConnManager;
            ITableManager[] managerArray = connManager.DetachTables(tableKeys);
            int num = 0;
            for (num2 = 0; num2 < managerArray.Length; num2++)
            {
                if (managerArray[num2] != null)
                {
                    managerArray[num2].NotifyUnsub();
                    num++;
                }
            }
            if (num == 0)
            {
            }
            SubscribedTableKey[] subscrKeys = new SubscribedTableKey[num];
            int index = 0;
            for (num2 = 0; num2 < managerArray.Length; num2++)
            {
                if (managerArray[num2] != null)
                {
                    subscrKeys[index] = tableKeys[num2];
                    index++;
                }
            }
            try
            {
                connManager.UnsubscrTables(subscrKeys, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
        }

        private ServerManager ConnManager
        {
            get
            {
                lock (this.stateMutex)
                {
                    if (this.subClient != null)
                    {
                        return this.subClient.ConnManager;
                    }
                    if (this.connManager == null)
                    {
                        throw new SubscrException("Connection closed");
                    }
                    return this.connManager;
                }
            }
        }
    }
}

