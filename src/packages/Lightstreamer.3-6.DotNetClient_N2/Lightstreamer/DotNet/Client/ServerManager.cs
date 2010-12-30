namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Threading;

    internal class ServerManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private static TimerSupport activityTimer = new TimerSupport();
        private BatchMonitor batchMonitor = new BatchMonitor();
        private PushServerProxy localPushServerProxy;
        private static TimerSupport notificationsSender = new TimerSupport();
        private long probeTimeoutMillis;
        private long probeWarningMillis;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private long reconnectionTimeoutMillis;
        private IServerListener serverListener;
        private static ILog sessionLogger = LogManager.GetLogger("com.lightstreamer.ls_client.session");
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");
        private Hashtable tables = new Hashtable();
        private AnonymousClassThread worker;

        internal ServerManager(ConnectionInfo info, IConnectionListener listener, IServerListener asyncListener)
        {
            Stream stream = null;
            this.probeTimeoutMillis = info.probeTimeoutMillis;
            this.probeWarningMillis = info.probeWarningMillis;
            this.reconnectionTimeoutMillis = info.reconnectionTimeoutMillis;
            this.localPushServerProxy = new PushServerProxy(info);
            try
            {
                stream = this.localPushServerProxy.ConnectForSession();
                try
                {
                    listener.OnConnectionEstablished();
                }
                catch (Exception)
                {
                }
                this.localPushServerProxy.StartSession(stream);
                try
                {
                    listener.OnSessionStarted();
                }
                catch (Exception)
                {
                }
            }
            catch (PhaseException)
            {
            }
            this.serverListener = asyncListener;
        }

        internal virtual void BatchRequests(int batchSize)
        {
            lock (this.batchMonitor)
            {
                if (this.batchMonitor.Filled)
                {
                    this.localPushServerProxy.StartBatch();
                    if (batchSize <= 0)
                    {
                        actionsLogger.Debug("Starting a new batch for unlimited requests in session " + this.localPushServerProxy.SessionId);
                    }
                    else
                    {
                        actionsLogger.Debug(string.Concat(new object[] { "Starting a new batch for ", batchSize, " requests in session ", this.localPushServerProxy.SessionId }));
                    }
                }
                else if (batchSize <= 0)
                {
                    actionsLogger.Debug("Extending the current batch with unlimited requests in session " + this.localPushServerProxy.SessionId);
                }
                else
                {
                    actionsLogger.Debug(string.Concat(new object[] { "Extending the current batch with ", batchSize, " requests in session ", this.localPushServerProxy.SessionId }));
                }
                this.batchMonitor.Expand(batchSize);
            }
        }

        internal virtual void ChangeConstraints(Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
        {
            this.localPushServerProxy.RequestNewConstraints(constraints);
        }

        internal virtual ITableManager[] Close()
        {
            ITableManager[] managerArray;
            lock (this.tables.SyncRoot)
            {
                managerArray = (ITableManager[]) CollectionsSupport.ToArray(this.tables.Values, new ITableManager[0]);
                this.tables.Clear();
            }
            sessionLogger.Info("Terminating session " + this.localPushServerProxy.SessionId);
            this.localPushServerProxy.Dispose();
            this.CloseBatch();
            if (actionsLogger.IsInfoEnabled)
            {
                for (int i = 0; i < managerArray.Length; i++)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Discarded ", managerArray[i], " from session ", this.localPushServerProxy.SessionId }));
                }
            }
            return managerArray;
        }

        internal virtual void CloseBatch()
        {
            lock (this.batchMonitor)
            {
                actionsLogger.Debug("Executing the current batch in session " + this.localPushServerProxy.SessionId);
                this.localPushServerProxy.CloseBatch();
                this.batchMonitor.Clear();
            }
        }

        internal virtual ITableManager[] DetachTables(Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys)
        {
            int num;
            ITableManager[] managerArray = new ITableManager[subscrKeys.Length];
            lock (this.tables.SyncRoot)
            {
                num = 0;
                while (num < subscrKeys.Length)
                {
                    object obj2 = this.tables[subscrKeys[num].KeyValue];
                    this.tables.Remove(subscrKeys[num].KeyValue);
                    managerArray[num] = (ITableManager) obj2;
                    num++;
                }
            }
            if (actionsLogger.IsInfoEnabled)
            {
                for (num = 0; num < subscrKeys.Length; num++)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Removed ", managerArray[num], " from session ", this.localPushServerProxy.SessionId }));
                }
            }
            return managerArray;
        }

        private ITableManager GetUpdatedTable(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            lock (this.tables.SyncRoot)
            {
                return (ITableManager) this.tables[values.WinCode];
            }
        }

        internal virtual bool Rebind(ActivityController activityController)
        {
            activityController.StartConnection();
            try
            {
                this.localPushServerProxy.ResyncSession();
                return true;
            }
            catch (PushServerException exception)
            {
                protLogger.Debug("Error in rebinding to the session", exception);
                sessionLogger.Error("Error while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(exception);
            }
            catch (PushConnException exception2)
            {
                streamLogger.Debug("Error in connection", exception2);
                sessionLogger.Error("Error while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(exception2);
            }
            catch (PhaseException)
            {
                sessionLogger.Info("Listening loop closed for session " + this.localPushServerProxy.SessionId);
            }
            finally
            {
                activityController.StopConnection();
            }
            return false;
        }

        internal virtual void SendMessage(string message)
        {
            this.localPushServerProxy.SendMessage(message);
        }

        internal virtual void Start()
        {
            this.worker = new AnonymousClassThread(this, "Lightstreamer listening thread");
            this.worker.Start();
        }

        internal virtual Lightstreamer.DotNet.Client.SubscribedTableKey[] SubscrItems(VirtualTableManager table, bool batchable)
        {
            int num;
            object obj2;
            Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[table.NumItems];
            actionsLogger.Info(string.Concat(new object[] { "Adding ", table, " to session ", this.localPushServerProxy.SessionId }));
            lock ((obj2 = this.tables.SyncRoot))
            {
                for (num = 0; num < table.NumItems; num++)
                {
                    subscrKeys[num] = this.localPushServerProxy.WindowCode;
                    this.tables[subscrKeys[num].KeyValue] = table.GetItemManager(num);
                }
            }
            bool flag = false;
            try
            {
                this.localPushServerProxy.RequestItemsSubscr(table, subscrKeys, batchable ? this.batchMonitor : null);
                flag = true;
            }
            finally
            {
                if (!flag)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Undoing add of ", table, " to session ", this.localPushServerProxy.SessionId }));
                    lock ((obj2 = this.tables.SyncRoot))
                    {
                        for (num = 0; num < subscrKeys.Length; num++)
                        {
                            this.tables.Remove(subscrKeys[num].KeyValue);
                        }
                    }
                }
            }
            return subscrKeys;
        }

        internal virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscrTable(ITableManager table, bool batchable)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey windowCode;
            object obj2;
            actionsLogger.Info(string.Concat(new object[] { "Adding ", table, " to session ", this.localPushServerProxy.SessionId }));
            lock ((obj2 = this.tables.SyncRoot))
            {
                windowCode = this.localPushServerProxy.WindowCode;
                this.tables[windowCode.KeyValue] = table;
            }
            bool flag = false;
            try
            {
                this.localPushServerProxy.RequestSubscr(table, windowCode, batchable ? this.batchMonitor : null);
                flag = true;
            }
            finally
            {
                if (!flag)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Undoing add of ", table, " to session ", this.localPushServerProxy.SessionId }));
                    lock ((obj2 = this.tables.SyncRoot))
                    {
                        this.tables.Remove(windowCode.KeyValue);
                    }
                }
            }
            return windowCode;
        }

        internal virtual void UnbatchRequest()
        {
            lock (this.batchMonitor)
            {
                if (!this.batchMonitor.Filled)
                {
                    this.batchMonitor.UseOne();
                    if (this.batchMonitor.Filled)
                    {
                        actionsLogger.Debug("Shrinking and executing the current batch in session " + this.localPushServerProxy.SessionId);
                        this.localPushServerProxy.CloseBatch();
                    }
                    else
                    {
                        actionsLogger.Debug("Shrinking the current batch in session " + this.localPushServerProxy.SessionId);
                    }
                }
            }
        }

        internal virtual void UnsubscrTables(Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys, bool batchable)
        {
            if (subscrKeys.Length != 0)
            {
                this.localPushServerProxy.DelSubscrs(subscrKeys, this.batchMonitor);
            }
        }

        internal virtual void WaitEvents()
        {
            long num = 0L;
            try
            {
                Lightstreamer.DotNet.Client.ServerUpdateEvent event2;
                bool flag;
                ActivityController activityController = new ActivityController(this);
                activityController.StartKeepalives();
                sessionLogger.Info("Listening for updates on session " + this.localPushServerProxy.SessionId);
                goto Label_0162;
            Label_0038:;
                try
                {
                    event2 = this.localPushServerProxy.WaitUpdate(activityController);
                }
                catch (PushServerException exception)
                {
                    protLogger.Debug("Error in received data", exception);
                    sessionLogger.Error("Error while listening for data in session " + this.localPushServerProxy.SessionId);
                    this.serverListener.OnDataError(exception);
                    goto Label_0162;
                }
                catch (PushLengthException exception2)
                {
                    long holdingMillis = exception2.HoldingMillis;
                    if (holdingMillis > 0L)
                    {
                        try
                        {
                            Thread.Sleep((int) holdingMillis);
                        }
                        catch (ThreadInterruptedException)
                        {
                        }
                    }
                    if (!this.Rebind(activityController))
                    {
                        return;
                    }
                    activityController.StartKeepalives();
                    goto Label_0162;
                }
                ITableManager updatedTable = this.GetUpdatedTable(event2);
                if (updatedTable == null)
                {
                    if (!this.localPushServerProxy.IsWindowCodeConsumed(event2.WinCode))
                    {
                        this.serverListener.OnDataError(new PushServerException(1));
                    }
                }
                else
                {
                    this.serverListener.OnUpdate(updatedTable, event2);
                }
                long totalBytes = this.localPushServerProxy.TotalBytes;
                this.serverListener.OnNewBytes(totalBytes - num);
                num = totalBytes;
            Label_0162:
                flag = true;
                goto Label_0038;
            }
            catch (PushConnException exception3)
            {
                streamLogger.Debug("Error in connection", exception3);
                sessionLogger.Error("Error while listening for data in session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(exception3);
            }
            catch (PhaseException)
            {
                sessionLogger.Info("Listening loop closed for session " + this.localPushServerProxy.SessionId);
            }
        }

        internal class ActivityController
        {
            private bool connectionCheck;
            private ServerManager enclosingInstance;
            private long lastActivity;
            private int phase = 1;
            private bool warningPending;

            public ActivityController(ServerManager enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }

            private void Launch(long millis, int currPhase)
            {
                ServerManager.activityTimer.Schedule(new AnonymousClassTimerTask(currPhase, this), millis);
            }

            public virtual void OnActivity()
            {
                lock (this)
                {
                    if (this.warningPending)
                    {
                        this.OnActivityWarning(false);
                        this.warningPending = false;
                        this.lastActivity = 0L;
                        this.phase++;
                        long millis = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.probeWarningMillis;
                        this.Launch(millis, this.phase);
                    }
                    else
                    {
                        this.lastActivity = (DateTime.Now.Ticks - 0x89f7ff5f7b58000L) / 0x2710L;
                    }
                }
            }

            private void OnActivityWarning(bool warningOn)
            {
                ServerManager.notificationsSender.Schedule(new AnonymousClassTimerTask3(warningOn, this), 0L);
            }

            private void OnNoActivity()
            {
                ServerManager.notificationsSender.Schedule(new AnonymousClassTimerTask1(this), 0L);
            }

            private void OnReconnectionTimeout()
            {
                ServerManager.notificationsSender.Schedule(new AnonymousClassTimerTask2(this), 0L);
            }

            public virtual void OnTimeout(int refPhase)
            {
                lock (this)
                {
                    if (refPhase == this.phase)
                    {
                        if (this.connectionCheck)
                        {
                            this.OnReconnectionTimeout();
                            this.phase++;
                        }
                        else if (this.warningPending)
                        {
                            this.OnNoActivity();
                            this.phase++;
                        }
                        else
                        {
                            long probeTimeoutMillis;
                            if (this.lastActivity == 0L)
                            {
                                this.OnActivityWarning(true);
                                this.warningPending = true;
                                probeTimeoutMillis = this.enclosingInstance.probeTimeoutMillis;
                                this.Launch(probeTimeoutMillis, this.phase);
                            }
                            else
                            {
                                probeTimeoutMillis = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.probeWarningMillis;
                                long num2 = this.lastActivity + probeTimeoutMillis;
                                long millis = num2 - ((DateTime.Now.Ticks - 0x89f7ff5f7b58000L) / 0x2710L);
                                this.lastActivity = 0L;
                                if (millis > 0L)
                                {
                                    this.Launch(millis, refPhase);
                                }
                                else
                                {
                                    this.OnTimeout(refPhase);
                                }
                            }
                        }
                    }
                }
            }

            public virtual void StartConnection()
            {
                lock (this)
                {
                    this.connectionCheck = true;
                    this.phase++;
                    long reconnectionTimeoutMillis = this.enclosingInstance.reconnectionTimeoutMillis;
                    this.Launch(reconnectionTimeoutMillis, this.phase);
                }
            }

            public virtual void StartKeepalives()
            {
                lock (this)
                {
                    this.warningPending = false;
                    this.connectionCheck = false;
                    this.lastActivity = 0L;
                    this.phase++;
                    long millis = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.probeWarningMillis;
                    this.Launch(millis, this.phase);
                }
            }

            public virtual void StopConnection()
            {
                lock (this)
                {
                    this.phase++;
                }
            }

            public virtual void StopKeepalives()
            {
                lock (this)
                {
                    this.OnActivity();
                    this.phase++;
                }
            }

            private class AnonymousClassTimerTask : IThreadRunnable
            {
                private int currPhase;
                private ServerManager.ActivityController enclosingInstance;

                public AnonymousClassTimerTask(int currPhase, ServerManager.ActivityController enclosingInstance)
                {
                    this.currPhase = currPhase;
                    this.enclosingInstance = enclosingInstance;
                }

                public void Run()
                {
                    this.enclosingInstance.OnTimeout(this.currPhase);
                }
            }

            private class AnonymousClassTimerTask1 : IThreadRunnable
            {
                private ServerManager.ActivityController enclosingInstance;

                public AnonymousClassTimerTask1(ServerManager.ActivityController enclosingInstance)
                {
                    this.enclosingInstance = enclosingInstance;
                }

                public void Run()
                {
                    PushServerException e = new PushServerException(10);
                    this.enclosingInstance.enclosingInstance.serverListener.OnFailure(e);
                    ServerManager.sessionLogger.Info("Terminating session " + this.enclosingInstance.enclosingInstance.localPushServerProxy.SessionId + " because of an activity timeout");
                    this.enclosingInstance.enclosingInstance.localPushServerProxy.Dispose();
                }
            }

            private class AnonymousClassTimerTask2 : IThreadRunnable
            {
                private ServerManager.ActivityController enclosingInstance;

                public AnonymousClassTimerTask2(ServerManager.ActivityController enclosingInstance)
                {
                    this.enclosingInstance = enclosingInstance;
                }

                public void Run()
                {
                    PushServerException e = new PushServerException(11);
                    this.enclosingInstance.enclosingInstance.serverListener.OnFailure(e);
                    ServerManager.sessionLogger.Info("Terminating session " + this.enclosingInstance.enclosingInstance.localPushServerProxy.SessionId + " because of a reconnection timeout");
                    this.enclosingInstance.enclosingInstance.localPushServerProxy.Dispose();
                }
            }

            private class AnonymousClassTimerTask3 : IThreadRunnable
            {
                private ServerManager.ActivityController enclosingInstance;
                private bool warningOn;

                public AnonymousClassTimerTask3(bool warningOn, ServerManager.ActivityController enclosingInstance)
                {
                    this.warningOn = warningOn;
                    this.enclosingInstance = enclosingInstance;
                }

                public void Run()
                {
                    this.enclosingInstance.enclosingInstance.serverListener.OnActivityWarning(this.warningOn);
                    if (this.warningOn)
                    {
                        ServerManager.sessionLogger.Info("Session " + this.enclosingInstance.enclosingInstance.localPushServerProxy.SessionId + " stalled");
                    }
                    else
                    {
                        ServerManager.sessionLogger.Info("Session " + this.enclosingInstance.enclosingInstance.localPushServerProxy.SessionId + " no longer stalled");
                    }
                }
            }
        }

        private class AnonymousClassThread : ThreadSupport
        {
            private ServerManager enclosingInstance;

            internal AnonymousClassThread(ServerManager enclosingInstance, string Param1) : base(Param1)
            {
                this.enclosingInstance = enclosingInstance;
            }

            public override void Run()
            {
                try
                {
                    this.enclosingInstance.WaitEvents();
                }
                catch (Exception exception)
                {
                    PushServerException e = new PushServerException(12, exception);
                    ServerManager.protLogger.Debug("Error in received data", exception);
                    ServerManager.sessionLogger.Error("Unrecoverable error while listening to data in session " + this.enclosingInstance.localPushServerProxy.SessionId);
                    this.enclosingInstance.serverListener.OnFailure(e);
                }
                finally
                {
                    this.enclosingInstance.serverListener.OnClose();
                }
            }
        }

        internal interface IServerListener
        {
            void OnActivityWarning(bool warningOn);
            void OnClose();
            void OnDataError(PushServerException e);
            void OnFailure(PushConnException e);
            void OnFailure(PushServerException e);
            void OnNewBytes(long bytes);
            void OnUpdate(ITableManager table, Lightstreamer.DotNet.Client.ServerUpdateEvent values);
        }
    }
}

