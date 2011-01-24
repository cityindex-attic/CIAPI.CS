using System.Collections.Generic;

namespace Lightstreamer.DotNet.Client
{
    using Common.Logging;
    using System;
    using System.Collections;
    using System.IO;
    using System.Threading;

    internal class ServerManager
    {
        private static ILog actionsLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.actions));
        private ActivityController activityController;
        private static TimerSupport activityTimer = new TimerSupport();
        private BatchMonitor batchMonitor = new BatchMonitor();
        private ConnectionInfo connInfo;
        private PushServerProxy localPushServerProxy;
        private static NotificationQueue notificationsSender = new NotificationQueue("Session events queue", true);
        private static ILog protLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.protocol));
        private IServerListener serverListener;
        private static ILog sessionLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.session));
        private static ILog streamLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.stream));
        private IDictionary tables = new Dictionary<int, ITableManager>();
        private SessionActivityManager worker;

        internal ServerManager(ConnectionInfo info, IServerListener asyncListener)
        {
            this.connInfo = info;
            this.localPushServerProxy = new PushServerProxy(info);
            this.activityController = new ActivityController(this);
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

        internal virtual void ChangeConstraints(ConnectionConstraints constraints)
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
            this.localPushServerProxy.Dispose(true);
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

        internal virtual void Connect()
        {
            Stream stream = null;
            this.activityController.StartConnection(true);
            try
            {
                stream = this.localPushServerProxy.ConnectForSession();
                this.serverListener.OnConnectionEstablished();
                this.localPushServerProxy.StartSession(stream);
                this.serverListener.OnSessionStarted(this.connInfo.isPolling);
            }
            catch (PhaseException)
            {
            }
            catch (Exception exception)
            {
                actionsLogger.Debug("Notifying an exception on the current connection");
                this.serverListener.OnConnectException(exception);
                throw exception;
            }
            finally
            {
                this.activityController.StopConnection();
            }
        }

        internal virtual ITableManager[] DetachTables(SubscribedTableKey[] subscrKeys)
        {
            int num;
            ITableManager[] managerArray = new ITableManager[subscrKeys.Length];
            lock (this.tables.SyncRoot)
            {
                num = 0;
                while (num < subscrKeys.Length)
                {
                    if (subscrKeys[num].KeyValue != -1)
                    {
                        object obj2 = this.tables[subscrKeys[num].KeyValue];
                        this.tables.Remove(subscrKeys[num].KeyValue);
                        managerArray[num] = (ITableManager) obj2;
                    }
                    else
                    {
                        managerArray[num] = null;
                    }
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

        private ITableManager GetUpdatedTable(ServerUpdateEvent values)
        {
            lock (this.tables.SyncRoot)
            {
                return (ITableManager) this.tables[values.TableCode];
            }
        }

        internal virtual bool Rebind(ActivityController activityController)
        {
            activityController.StartConnection(false);
            try
            {
                this.localPushServerProxy.ResyncSession();
                return true;
            }
            catch (PushEndException exception)
            {
                streamLogger.Debug("Forced connection end", exception);
                sessionLogger.Error("Connection forcibly closed by the Server while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnEnd(exception.EndCause);
            }
            catch (PushServerException exception2)
            {
                protLogger.Debug("Error in rebinding to the session", exception2);
                sessionLogger.Error("Error while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(exception2);
            }
            catch (PushConnException exception3)
            {
                streamLogger.Debug("Error in connection", exception3);
                sessionLogger.Error("Error while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(exception3);
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
            this.worker = new SessionActivityManager(this, "Lightstreamer listening thread");
            this.worker.Start();
        }

        internal virtual SubscribedTableKey[] SubscrItems(VirtualTableManager table, bool batchable)
        {
            int num;
            object obj2;
            if (table.NumItems == 0)
            {
                if (batchable)
                {
                    this.UnbatchRequest();
                }
                return new SubscribedTableKey[0];
            }
            SubscribedTableKey[] subscrKeys = new SubscribedTableKey[table.NumItems];
            actionsLogger.Info(string.Concat(new object[] { "Adding ", table, " to session ", this.localPushServerProxy.SessionId }));
            lock ((obj2 = this.tables.SyncRoot))
            {
                for (num = 0; num < table.NumItems; num++)
                {
                    subscrKeys[num] = this.localPushServerProxy.TableCode;
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

        internal virtual SubscribedTableKey SubscrTable(ITableManager table, bool batchable)
        {
            SubscribedTableKey tableCode;
            object obj2;
            actionsLogger.Info(string.Concat(new object[] { "Adding ", table, " to session ", this.localPushServerProxy.SessionId }));
            lock ((obj2 = this.tables.SyncRoot))
            {
                tableCode = this.localPushServerProxy.TableCode;
                this.tables[tableCode.KeyValue] = table;
            }
            bool flag = false;
            try
            {
                this.localPushServerProxy.RequestSubscr(table, tableCode, batchable ? this.batchMonitor : null);
                flag = true;
            }
            finally
            {
                if (!flag)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Undoing add of ", table, " to session ", this.localPushServerProxy.SessionId }));
                    lock ((obj2 = this.tables.SyncRoot))
                    {
                        this.tables.Remove(tableCode.KeyValue);
                    }
                }
            }
            return tableCode;
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

        internal virtual void UnsubscrTables(SubscribedTableKey[] subscrKeys, bool batchable)
        {
            if (subscrKeys.Length == 0)
            {
                if (batchable)
                {
                    this.UnbatchRequest();
                }
            }
            else
            {
                this.localPushServerProxy.DelSubscrs(subscrKeys, batchable ? this.batchMonitor : null);
            }
        }

        internal virtual void WaitEvents()
        {
            long num = 0L;
            try
            {
                ServerUpdateEvent event2;
                bool flag;
                this.activityController.StartKeepalives();
                sessionLogger.Info("Listening for updates on session " + this.localPushServerProxy.SessionId);
                goto Label_016D;
            Label_0036:;
                try
                {
                    event2 = this.localPushServerProxy.WaitUpdate(this.activityController);
                }
                catch (PushServerException exception)
                {
                    protLogger.Debug("Error in received data", exception);
                    sessionLogger.Error("Error while listening for data in session " + this.localPushServerProxy.SessionId);
                    this.serverListener.OnDataError(exception);
                    goto Label_016D;
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
                        catch (Exception)
                        {
                        }
                    }
                    if (!this.Rebind(this.activityController))
                    {
                        return;
                    }
                    this.activityController.StartKeepalives();
                    goto Label_016D;
                }
                ITableManager updatedTable = this.GetUpdatedTable(event2);
                if (updatedTable == null)
                {
                    if (!this.localPushServerProxy.IsTableCodeConsumed(event2.TableCode))
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
            Label_016D:
                flag = true;
                goto Label_0036;
            }
            catch (PushConnException exception3)
            {
                streamLogger.Debug("Error in connection", exception3);
                sessionLogger.Error("Error while listening for data in session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(exception3);
            }
            catch (PushEndException exception4)
            {
                streamLogger.Debug("Forced connection end", exception4);
                sessionLogger.Error("Connection forcibly closed by the Server in session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnEnd(exception4.EndCause);
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
            private bool isFirstConn;
            private long lastActivity;
            private int phase = 1;
            private bool streamingConfirmed;
            private bool warningPending;

            public ActivityController(ServerManager enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
                this.streamingConfirmed = enclosingInstance.connInfo.isPolling;
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
                        long millis = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.connInfo.probeWarningMillis;
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
                ServerManager.notificationsSender.Add(delegate {
                    if (this.enclosingInstance.serverListener.OnActivityWarning(warningOn))
                    {
                        if (warningOn)
                        {
                            ServerManager.sessionLogger.Info("Session " + this.enclosingInstance.localPushServerProxy.SessionId + " stalled");
                        }
                        else
                        {
                            ServerManager.sessionLogger.Info("Session " + this.enclosingInstance.localPushServerProxy.SessionId + " no longer stalled");
                        }
                    }
                });
            }

            private void OnConnectionTimeout(bool isFirstConn)
            {
                ServerManager.notificationsSender.Add(delegate {
                    if (isFirstConn)
                    {
                        ServerManager.actionsLogger.Debug("Notifying a timeout check on the current connection");
                        this.enclosingInstance.serverListener.OnConnectTimeout();
                    }
                    else if (this.enclosingInstance.serverListener.OnReconnectTimeout())
                    {
                        ServerManager.sessionLogger.Info("Terminating session " + this.enclosingInstance.localPushServerProxy.SessionId + " because of a reconnection timeout");
                        this.enclosingInstance.localPushServerProxy.Dispose(true);
                    }
                });
            }

            private void OnNoActivity()
            {
                ServerManager.notificationsSender.Add(delegate {
                    PushServerException e = new PushServerException(10);
                    if (this.enclosingInstance.serverListener.OnFailure(e))
                    {
                        ServerManager.sessionLogger.Info("Terminating session " + this.enclosingInstance.localPushServerProxy.SessionId + " because of an activity timeout");
                        this.enclosingInstance.localPushServerProxy.Dispose(true);
                    }
                });
            }

            private void OnStreamingResponse()
            {
                ServerManager.notificationsSender.Add(delegate {
                    ServerManager.actionsLogger.Debug("Notifying return on the current connection");
                    this.enclosingInstance.serverListener.OnStreamingReturned();
                });
            }

            public virtual void OnTimeout(int refPhase)
            {
                lock (this)
                {
                    if (refPhase == this.phase)
                    {
                        if (this.connectionCheck)
                        {
                            this.OnConnectionTimeout(this.isFirstConn);
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
                                probeTimeoutMillis = this.enclosingInstance.connInfo.probeTimeoutMillis;
                                this.Launch(probeTimeoutMillis, this.phase);
                            }
                            else
                            {
                                probeTimeoutMillis = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.connInfo.probeWarningMillis;
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

            public virtual void StartConnection(bool isFirstConnect)
            {
                lock (this)
                {
                    long reconnectionTimeoutMillis;
                    this.connectionCheck = true;
                    this.isFirstConn = isFirstConnect;
                    this.phase++;
                    if (!isFirstConnect)
                    {
                        reconnectionTimeoutMillis = this.enclosingInstance.connInfo.reconnectionTimeoutMillis;
                        if (this.enclosingInstance.connInfo.isPolling)
                        {
                            reconnectionTimeoutMillis += this.enclosingInstance.connInfo.pollingIdleMillis;
                        }
                        else if (!this.streamingConfirmed)
                        {
                            reconnectionTimeoutMillis = this.enclosingInstance.connInfo.streamingTimeoutMillis;
                        }
                    }
                    else if (!this.enclosingInstance.connInfo.StartsWithPoll() && !this.streamingConfirmed)
                    {
                        reconnectionTimeoutMillis = this.enclosingInstance.connInfo.streamingTimeoutMillis;
                    }
                    else
                    {
                        goto Label_00E4;
                    }
                    this.Launch(reconnectionTimeoutMillis, this.phase);
                Label_00E4:;
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
                    long millis = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.connInfo.probeWarningMillis;
                    this.Launch(millis, this.phase);
                }
            }

            public virtual void StopConnection()
            {
                lock (this)
                {
                    if (!this.isFirstConn)
                    {
                        if (!this.enclosingInstance.connInfo.isPolling && !this.streamingConfirmed)
                        {
                            this.streamingConfirmed = true;
                            this.OnStreamingResponse();
                        }
                    }
                    else if (!this.enclosingInstance.connInfo.StartsWithPoll() && !this.streamingConfirmed)
                    {
                        this.streamingConfirmed = true;
                        this.OnStreamingResponse();
                    }
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
        }

        internal interface IServerListener
        {
            bool OnActivityWarning(bool warningOn);
            bool OnClose();
            void OnConnectException(Exception e);
            void OnConnectionEstablished();
            void OnConnectTimeout();
            bool OnDataError(PushServerException e);
            bool OnEnd(int endCause);
            bool OnFailure(PushConnException e);
            bool OnFailure(PushServerException e);
            bool OnNewBytes(long bytes);
            bool OnReconnectTimeout();
            void OnSessionStarted(bool isPolling);
            void OnStreamingReturned();
            bool OnUpdate(ITableManager table, ServerUpdateEvent values);
        }

        private class SessionActivityManager : ThreadSupport
        {
            private ServerManager enclosingInstance;

            internal SessionActivityManager(ServerManager enclosingInstance, string Param1) : base(Param1)
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
    }
}

