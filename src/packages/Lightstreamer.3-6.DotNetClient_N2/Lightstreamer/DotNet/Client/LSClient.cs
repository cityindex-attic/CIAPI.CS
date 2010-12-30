namespace Lightstreamer.DotNet.Client
{
    using log4net;
    using System;

    public class LSClient
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private IConnectionListener connListener;
        private ServerManager connManager;
        private object connMutex = new object();
        private int phase = 0;
        private object stateMutex = new object();

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

        public virtual void ChangeConstraints(Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
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
            IConnectionListener connListener;
            ServerManager connManager;
            lock (this.connMutex)
            {
                lock (this.stateMutex)
                {
                    if (this.connManager == null)
                    {
                        return;
                    }
                    this.phase++;
                    connManager = this.connManager;
                    connListener = this.connListener;
                }
                this.connManager = null;
                this.connListener = null;
            }
            foreach (ITableManager manager2 in connManager.Close())
            {
                manager2.NotifyUnsub();
            }
            try
            {
                connListener.OnClose();
            }
            catch (Exception)
            {
            }
        }

        public virtual void ForceUnsubscribeTable(Lightstreamer.DotNet.Client.SubscribedTableKey tableKey)
        {
            ServerManager connManager = this.ConnManager;
            Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[] { tableKey };
            try
            {
                connManager.UnsubscrTables(subscrKeys, false);
            }
            catch (PhaseException)
            {
                throw new SubscrException("Connection closed");
            }
        }

        private IConnectionListener GetActiveListener(int currPhase)
        {
            lock (this.stateMutex)
            {
                if (currPhase == this.phase)
                {
                    return this.connListener;
                }
                return null;
            }
        }

        public virtual void OpenConnection(ConnectionInfo info, IConnectionListener listener)
        {
            lock (this.connMutex)
            {
                int num;
                object obj3;
                this.CloseConnection();
                lock ((obj3 = this.stateMutex))
                {
                    num = this.phase + 1;
                }
                ServerManager.IServerListener asyncListener = new MyServerListener(this, num);
                ServerManager manager = new ServerManager(info, listener, asyncListener);
                lock ((obj3 = this.stateMutex))
                {
                    this.connListener = listener;
                    this.phase = num;
                    this.connManager = manager;
                }
                manager.Start();
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

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener")]
        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IExtendedTableListener listener)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey[] keyArray;
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

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener")]
        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IFastItemsListener listener)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey[] keyArray;
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

        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey[] SubscribeItems(ExtendedTableInfo items, IHandyTableListener listener)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey[] keyArray;
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

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener")]
        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscribeTable(ExtendedTableInfo table, IExtendedTableListener listener)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey key;
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

        [Obsolete("The use of this overload is deprecated in favor of the new subscription methods based on the IHandyTableListener")]
        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscribeTable(SimpleTableInfo table, ISimpleTableListener listener)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey key;
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

        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscribeTable(ExtendedTableInfo table, IHandyTableListener listener, bool commandLogic)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey key;
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

        public virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscribeTable(SimpleTableInfo table, IHandyTableListener listener, bool commandLogic)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey key;
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

        public virtual void UnsubscribeTable(Lightstreamer.DotNet.Client.SubscribedTableKey tableKey)
        {
            ServerManager connManager = this.ConnManager;
            Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[] { tableKey };
            ITableManager[] managerArray = connManager.DetachTables(subscrKeys);
            if (managerArray[0] == null)
            {
                try
                {
                    connManager.UnsubscrTables(new Lightstreamer.DotNet.Client.SubscribedTableKey[0], true);
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

        public virtual void UnsubscribeTables(Lightstreamer.DotNet.Client.SubscribedTableKey[] tableKeys)
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
            Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[num];
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
                    if (this.connManager == null)
                    {
                        throw new SubscrException("Connection closed");
                    }
                    return this.connManager;
                }
            }
        }

        private class MyServerListener : ServerManager.IServerListener
        {
            private int currPhase;
            private LSClient enclosingInstance;

            internal MyServerListener(LSClient enclosingInstance, int currPhase)
            {
                this.enclosingInstance = enclosingInstance;
                this.currPhase = currPhase;
            }

            public virtual void OnActivityWarning(bool warningOn)
            {
                IConnectionListener activeListener = this.enclosingInstance.GetActiveListener(this.currPhase);
                if (activeListener != null)
                {
                    try
                    {
                        activeListener.OnActivityWarning(warningOn);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public virtual void OnClose()
            {
                if (this.enclosingInstance.GetActiveListener(this.currPhase) != null)
                {
                    this.enclosingInstance.CloseConnection();
                }
            }

            public virtual void OnDataError(PushServerException e)
            {
                IConnectionListener activeListener = this.enclosingInstance.GetActiveListener(this.currPhase);
                if (activeListener != null)
                {
                    try
                    {
                        activeListener.OnDataError(e);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public virtual void OnFailure(PushConnException e)
            {
                IConnectionListener activeListener = this.enclosingInstance.GetActiveListener(this.currPhase);
                if (activeListener != null)
                {
                    try
                    {
                        activeListener.OnFailure(e);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public virtual void OnFailure(PushServerException e)
            {
                IConnectionListener activeListener = this.enclosingInstance.GetActiveListener(this.currPhase);
                if (activeListener != null)
                {
                    try
                    {
                        activeListener.OnFailure(e);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public virtual void OnNewBytes(long bytes)
            {
                IConnectionListener activeListener = this.enclosingInstance.GetActiveListener(this.currPhase);
                if (activeListener != null)
                {
                    try
                    {
                        activeListener.OnNewBytes(bytes);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public virtual void OnUpdate(ITableManager table, Lightstreamer.DotNet.Client.ServerUpdateEvent values)
            {
                IConnectionListener activeListener = this.enclosingInstance.GetActiveListener(this.currPhase);
                if (activeListener != null)
                {
                    try
                    {
                        table.DoUpdate(values);
                    }
                    catch (PushServerException exception)
                    {
                        LSClient.actionsLogger.Debug("Error in received values", exception);
                        try
                        {
                            activeListener.OnDataError(exception);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}

