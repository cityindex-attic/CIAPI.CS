namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.Threading;

    public class LSClient
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private MyServerListener asyncListener;
        private IConnectionListener connListener;
        private ServerManager connManager;
        private int phase = 0;
        private bool sendMessageAutoBatchingEnabled = true;
        private object stateMutex = new object();
        private LSClient subClient = null;

        private static void AsynchCloseConnection(LSClient testClient)
        {
            var t = new Thread(() => testClient.CloseConnection()) {IsBackground = true};
            t.Start();

        }

        public virtual void BatchRequests(int batchSize)
        {
            ServerManager currConnManager = this.ConnManager;
            try
            {
                currConnManager.BatchRequests(batchSize);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
        }

        public virtual void ChangeConstraints(Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
        {
            ServerManager currConnManager;
            try
            {
                currConnManager = this.ConnManager;
            }
            catch (SubscrException)
            {
                return;
            }
            try
            {
                currConnManager.ChangeConstraints(constraints);
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
            IConnectionListener activeListener = null;
            ServerManager closingManager = null;
            MyServerListener closeListener = null;
            LSClient closingSubClient = null;
            lock (this.stateMutex)
            {
                this.phase++;
                if (this.subClient != null)
                {
                    closingSubClient = this.subClient;
                    this.subClient = null;
                }
                else if (this.connManager != null)
                {
                    closingManager = this.connManager;
                    activeListener = this.connListener;
                    closeListener = this.asyncListener;
                    this.connManager = null;
                    this.connListener = null;
                    this.asyncListener = null;
                }
                else
                {
                    return;
                }
            }
            if (closingSubClient != null)
            {
                closingSubClient.CloseConnection();
            }
            else
            {
                CloseFlushing(closingManager, closeListener, activeListener);
            }
        }

        private static void CloseFlushing(ServerManager closingManager, MyServerListener closeListener, IConnectionListener activeListener)
        {
            foreach (ITableManager info in closingManager.Close())
            {
                info.NotifyUnsub();
            }
            closeListener.OnClosed(activeListener);
        }

        public virtual void ForceUnsubscribeTable(Lightstreamer.DotNet.Client.SubscribedTableKey tableKey)
        {
            ServerManager currConnManager = this.ConnManager;
            if (tableKey.KeyValue != -1)
            {
                Lightstreamer.DotNet.Client.SubscribedTableKey[] tableKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[] { tableKey };
                try
                {
                    currConnManager.UnsubscrTables(tableKeys, false);
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
            int currPhase;
            object lockObj;
            lock ((lockObj = this.stateMutex))
            {
                this.CloseConnection();
                currPhase = ++this.phase;
            }
            ConnectionInfo myInfo = (ConnectionInfo) info.Clone();
            if (myInfo.EnableStreamSense && !myInfo.Polling)
            {
                LSClient testClient = new LSClient();
                ExtConnectionListener myListener = new ExtConnectionListener(listener);
                ConnectionInfo mySubInfo = (ConnectionInfo) info.Clone();
                mySubInfo.EnableStreamSense = false;
                var t = new Thread(()=>
                                       {
                                           try
                                           {
                                               testClient.OpenConnection(mySubInfo, myListener);
                                           }
                                           catch (Exception)
                                           {
                                           }

                                       });
                t.IsBackground = true;
                t.Start();
                
                if (!myListener.WaitStreamingTimeoutAnswer())
                {
                    lock ((lockObj = this.stateMutex))
                    {
                        if (currPhase == this.phase)
                        {
                            this.subClient = testClient;
                        }
                        else
                        {
                            AsynchCloseConnection(testClient);
                            return;
                        }
                    }
                    myListener.FlushAndStart();
                }
                else
                {
                    AsynchCloseConnection(testClient);
                    lock ((lockObj = this.stateMutex))
                    {
                        if (currPhase != this.phase)
                        {
                            return;
                        }
                    }
                    LSClient pollClient = new LSClient();
                    myInfo.Polling = true;
                    pollClient.OpenConnection(myInfo, listener);
                    lock ((lockObj = this.stateMutex))
                    {
                        if (currPhase == this.phase)
                        {
                            this.subClient = pollClient;
                        }
                        else
                        {
                            AsynchCloseConnection(pollClient);
                        }
                    }
                }
            }
            else
            {
                MyServerListener serverListener = new MyServerListener(this, listener, currPhase);
                bool ok = false;
                try
                {
                    ServerManager newManager = new ServerManager(myInfo, serverListener);
                    newManager.Connect();
                    ok = true;
                    lock ((lockObj = this.stateMutex))
                    {
                        if (currPhase == this.phase)
                        {
                            this.connListener = listener;
                            this.asyncListener = serverListener;
                            this.connManager = newManager;
                        }
                        else
                        {
                            CloseFlushing(newManager, serverListener, listener);
                            return;
                        }
                    }
                    newManager.Start();
                }
                finally
                {
                    if (!ok)
                    {
                        serverListener.OnClosed(null);
                    }
                }
            }
        }

        public virtual void SendMessage(string message)
        {
            ServerManager currConnManager;
            try
            {
                currConnManager = this.ConnManager;
            }
            catch (SubscrException)
            {
                return;
            }
            try
            {
                currConnManager.SendMessage(message);
            }
            catch (PhaseException)
            {
            }
        }

        public virtual int SendMessage(MessageInfo message, ISendMessageListener listener)
        {
            ServerManager currConnManager;
            int prog;
            try
            {
                currConnManager = this.ConnManager;
            }
            catch (SubscrException)
            {
                return 0;
            }
            try
            {
                prog = currConnManager.SendMessage(new MessageManager(message, listener), this.sendMessageAutoBatchingEnabled);
            }
            catch (PhaseException)
            {
                return 0;
            }
            catch (SubscrException)
            {
                return 0;
            }
            return prog;
        }

        public static void SetLoggerProvider(ILoggerProvider loggerProvider)
        {
            LogManager.SetLoggerProvider(loggerProvider);
        }

        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IHandyTableListener listener)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey[] keys;
            ServerManager currConnManager = this.ConnManager;
            VirtualTableManager tableInfo = new VirtualTableManager(items, listener);
            try
            {
                keys = currConnManager.SubscrItems(tableInfo, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return keys;
        }

        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscribeTable(ExtendedTableInfo table, IHandyTableListener listener, bool commandLogic)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey keys;
            ServerManager currConnManager = this.ConnManager;
            ITableManager tableInfo = new FullTableManager(table, listener, commandLogic);
            try
            {
                keys = currConnManager.SubscrTable(tableInfo, true);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
            return keys;
        }

        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscribeTable(SimpleTableInfo table, IHandyTableListener listener, bool commandLogic)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey key;
            ServerManager currConnManager = this.ConnManager;
            ITableManager tableInfo = new FullTableManager(table, listener, commandLogic);
            try
            {
                key = currConnManager.SubscrTable(tableInfo, true);
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

        public virtual void UnsubscribeTable(Lightstreamer.DotNet.Client.SubscribedTableKey tableKey)
        {
            ServerManager currConnManager = this.ConnManager;
            if (tableKey.KeyValue != -1)
            {
                Lightstreamer.DotNet.Client.SubscribedTableKey[] tableKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[] { tableKey };
                ITableManager[] infos = currConnManager.DetachTables(tableKeys);
                if (infos[0] == null)
                {
                    try
                    {
                        currConnManager.UnsubscrTables(new Lightstreamer.DotNet.Client.SubscribedTableKey[0], true);
                    }
                    catch (PhaseException)
                    {
                    }
                    throw new SubscrException("Table not found");
                }
                infos[0].NotifyUnsub();
                try
                {
                    currConnManager.UnsubscrTables(tableKeys, true);
                }
                catch (PhaseException)
                {
                    throw new SubscrException("Connection closed");
                }
            }
        }

        public virtual void UnsubscribeTables(Lightstreamer.DotNet.Client.SubscribedTableKey[] tableKeys)
        {
            int i;
            ServerManager currConnManager = this.ConnManager;
            ITableManager[] infos = currConnManager.DetachTables(tableKeys);
            int found = 0;
            for (i = 0; i < infos.Length; i++)
            {
                if (infos[i] != null)
                {
                    infos[i].NotifyUnsub();
                    found++;
                }
            }
            if (found == 0)
            {
            }
            Lightstreamer.DotNet.Client.SubscribedTableKey[] foundTableKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[found];
            int curr = 0;
            for (i = 0; i < infos.Length; i++)
            {
                if (infos[i] != null)
                {
                    foundTableKeys[curr] = tableKeys[i];
                    curr++;
                }
            }
            try
            {
                currConnManager.UnsubscrTables(foundTableKeys, true);
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

