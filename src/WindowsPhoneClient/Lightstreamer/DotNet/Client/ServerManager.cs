namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    internal class ServerManager
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private ActivityController activityController;
        private static TimerSupport activityTimer = new TimerSupport();
        private BatchMonitor batchMonitor = new BatchMonitor();
        private ConnectionInfo connInfo;
        private PushServerProxy localPushServerProxy;
        private BatchMonitor mexBatchMonitor = new BatchMonitor();
        private MessageParallelizer mexParallelizer;
        private static NotificationQueue notificationsSender = new NotificationQueue("Session events queue", true);
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private SequencesHandler sequencesHandler = new SequencesHandler();
        private IServerListener serverListener;
        private static ILog sessionLogger = LogManager.GetLogger("com.lightstreamer.ls_client.session");
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");
        private IDictionary tables = new Dictionary<int, ITableManager>();
        private SessionActivityManager worker;

        internal ServerManager(ConnectionInfo info, IServerListener asyncListener)
        {
            this.connInfo = info;
            this.localPushServerProxy = new PushServerProxy(info);
            this.activityController = new ActivityController(this);
            this.mexParallelizer = new MessageParallelizer(this.mexBatchMonitor, this);
            this.serverListener = asyncListener;
        }

        private void AbortPendingMessages()
        {
            lock (this.sequencesHandler)
            {
                IEnumerator<KeyValuePair<string, SequenceHandler>> sequences = this.sequencesHandler.Reset();
                try
                {
                    sequences.Reset();
                }
                catch (NotSupportedException)
                {
                }
                while (sequences.MoveNext())
                {
                    KeyValuePair<string, SequenceHandler> dicVAl = sequences.Current;
                    SequenceHandler sequence = dicVAl.Value;
                    MessageManager[] toAbort = sequence.Iterator();
                    for (int i = 0; i < toAbort.Length; i++)
                    {
                        if (!toAbort[i].HasOutcome())
                        {
                            this.serverListener.OnMessageOutcome(toAbort[i], sequence, null, null);
                        }
                    }
                }
                this.serverListener.OnEndMessages();
            }
        }

        internal virtual void BatchMessageRequests(int batchSize)
        {
            this.BatchRequests(batchSize, this.mexBatchMonitor, true);
        }

        internal virtual void BatchRequests(int batchSize)
        {
            this.BatchRequests(batchSize, this.batchMonitor, false);
        }

        private void BatchRequests(int batchSize, BatchMonitor monitor, bool messageBatch)
        {
            lock (monitor)
            {
                if (monitor.Filled)
                {
                    if (messageBatch)
                    {
                        this.localPushServerProxy.StartMessageBatch();
                    }
                    else
                    {
                        this.localPushServerProxy.StartBatch();
                    }
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
                monitor.Expand(batchSize);
            }
        }

        internal virtual void ChangeConstraints(Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
        {
            this.localPushServerProxy.RequestNewConstraints(constraints);
        }

        internal virtual ITableManager[] Close()
        {
            ITableManager[] zombieTables;
            this.activityController.OnCloseRequested();
            lock (this.tables.SyncRoot)
            {
                zombieTables = (ITableManager[]) CollectionsSupport.ToArray(this.tables.Values, new ITableManager[0]);
                this.tables.Clear();
            }
            this.AbortPendingMessages();
            sessionLogger.Info("Terminating session " + this.localPushServerProxy.SessionId);
            this.localPushServerProxy.Dispose(true);
            this.CloseBatch();
            this.CloseMessageBatch();
            if (actionsLogger.IsInfoEnabled)
            {
                for (int i = 0; i < zombieTables.Length; i++)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Discarded ", zombieTables[i], " from session ", this.localPushServerProxy.SessionId }));
                }
            }
            return zombieTables;
        }

        internal virtual void CloseBatch()
        {
            this.CloseBatch(this.batchMonitor, false);
        }

        internal virtual void CloseBatch(BatchMonitor monitor, bool messageBatch)
        {
            lock (monitor)
            {
                actionsLogger.Debug("Executing the current batch in session " + this.localPushServerProxy.SessionId);
                if (messageBatch)
                {
                    this.localPushServerProxy.CloseMessageBatch();
                }
                else
                {
                    this.localPushServerProxy.CloseBatch();
                }
                monitor.Clear();
            }
        }

        internal virtual void CloseMessageBatch()
        {
            this.CloseBatch(this.mexBatchMonitor, true);
        }

        internal virtual void Connect()
        {
            Stream stream = null;
            bool badEnd = true;
            this.activityController.StartConnection(true);
            try
            {
                stream = this.localPushServerProxy.ConnectForSession();
                this.serverListener.OnConnectionEstablished();
                this.localPushServerProxy.StartSession(stream);
                this.serverListener.OnSessionStarted(this.connInfo.Polling);
                badEnd = false;
            }
            catch (PhaseException)
            {
            }
            catch (Exception e)
            {
                actionsLogger.Debug("Notifying an exception on the current connection");
                this.serverListener.OnConnectException(e);
                throw e;
            }
            finally
            {
                this.activityController.StopConnection();
                if (badEnd)
                {
                    streamLogger.Debug("Closing create connection");
                    try
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                    catch (IOException e)
                    {
                        streamLogger.Debug("Error closing create connection", e);
                    }
                }
            }
        }

        internal virtual ITableManager[] DetachTables(Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys)
        {
            int i;
            ITableManager[] infos = new ITableManager[subscrKeys.Length];
            lock (this.tables.SyncRoot)
            {
                i = 0;
                while (i < subscrKeys.Length)
                {
                    if (subscrKeys[i].KeyValue != -1)
                    {
                        object tempObject = this.tables[subscrKeys[i].KeyValue];
                        this.tables.Remove(subscrKeys[i].KeyValue);
                        infos[i] = (ITableManager) tempObject;
                    }
                    else
                    {
                        infos[i] = null;
                    }
                    i++;
                }
            }
            if (actionsLogger.IsInfoEnabled)
            {
                for (i = 0; i < subscrKeys.Length; i++)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Removed ", infos[i], " from session ", this.localPushServerProxy.SessionId }));
                }
            }
            return infos;
        }

        private void ExpandMultipleMessageUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            int losts = 0;
            bool ok = false;
            try
            {
                losts = Convert.ToInt32(values.ErrorMessage);
                ok = true;
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            finally
            {
                if (!ok)
                {
                    this.serverListener.OnDataError(new PushServerException(7));
                }
            }
            if (ok && (losts > 0))
            {
                for (int next = (values.MessageProg - losts) + 1; next <= values.MessageProg; next++)
                {
                    this.MessageUpdate(new Lightstreamer.DotNet.Client.ServerUpdateEvent(values.MessageSequence, next, 0x26, "Message discarded"));
                }
            }
        }

        private ITableManager GetUpdatedTable(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            lock (this.tables.SyncRoot)
            {
                return (ITableManager) this.tables[values.TableCode];
            }
        }

        private void MessageUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            if (values.ErrorCode == 0x27)
            {
                this.ExpandMultipleMessageUpdate(values);
            }
            else
            {
                lock (this.sequencesHandler)
                {
                    SequenceHandler seq = this.sequencesHandler.GetSequence(values.MessageSequence);
                    MessageManager message = seq.GetMessage(values.MessageProg);
                    if (message == null)
                    {
                        this.serverListener.OnDataError(new PushServerException(13));
                    }
                    else
                    {
                        this.serverListener.OnMessageOutcome(message, seq, values, null);
                    }
                }
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
            catch (PushEndException e)
            {
                streamLogger.Debug("Forced connection end", e);
                sessionLogger.Error("Connection forcibly closed by the Server while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnEnd(e.EndCause);
            }
            catch (PushServerException e)
            {
                protLogger.Debug("Error in rebinding to the session", e);
                sessionLogger.Error("Error while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(e);
            }
            catch (PushConnException e)
            {
                streamLogger.Debug("Error in connection", e);
                sessionLogger.Error("Error while trying to rebind to session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(e);
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

        internal int SendMessage(MessageManager message, bool sendAsynchronously)
        {
            int prog = 0;
            lock (this.sequencesHandler)
            {
                prog = this.sequencesHandler.GetSequence(message.Sequence).Enqueue(message);
            }
            if (sendAsynchronously)
            {
                if (!this.mexBatchMonitor.Unlimited)
                {
                    this.BatchMessageRequests(0);
                }
                this.mexParallelizer.EnqueueMessage(message, prog);
                return prog;
            }
            this.SendMessage(message, prog);
            return prog;
        }

        internal void SendMessage(MessageManager message, int prog)
        {
            bool ok = false;
            Exception problem = null;
            try
            {
                this.localPushServerProxy.RequestSendMessage(message, prog, this.mexBatchMonitor);
                ok = true;
            }
            catch (PhaseException e)
            {
                problem = e;
                throw e;
            }
            catch (PushConnException e)
            {
                problem = e;
                throw e;
            }
            catch (PushServerException e)
            {
                problem = e;
                throw e;
            }
            catch (PushUserException e)
            {
                problem = e;
                throw e;
            }
            catch (SubscrException e)
            {
                problem = e;
                throw e;
            }
            finally
            {
                if (!ok)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Undoing sending of ", message, " to session ", this.localPushServerProxy.SessionId }));
                    lock (this.sequencesHandler)
                    {
                        SequenceHandler seq = this.sequencesHandler.GetSequence(message.Sequence);
                        if (message != null)
                        {
                            this.serverListener.OnMessageOutcome(message, seq, null, problem);
                        }
                    }
                }
            }
        }

        internal virtual void Start()
        {
            this.worker = new SessionActivityManager(this, "Lightstreamer listening thread");
            this.worker.Start(false);
        }

        internal virtual Lightstreamer.DotNet.Client.SubscribedTableKey[] SubscrItems(VirtualTableManager table, bool batchable)
        {
            int i;
            object lockObj;
            if (table.NumItems == 0)
            {
                if (batchable)
                {
                    this.UnbatchRequest();
                }
                return new Lightstreamer.DotNet.Client.SubscribedTableKey[0];
            }
            Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys = new Lightstreamer.DotNet.Client.SubscribedTableKey[table.NumItems];
            actionsLogger.Info(string.Concat(new object[] { "Adding ", table, " to session ", this.localPushServerProxy.SessionId }));
            lock ((lockObj = this.tables.SyncRoot))
            {
                for (i = 0; i < table.NumItems; i++)
                {
                    subscrKeys[i] = this.localPushServerProxy.TableCode;
                    this.tables[subscrKeys[i].KeyValue] = table.GetItemManager(i);
                }
            }
            bool ok = false;
            try
            {
                this.localPushServerProxy.RequestItemsSubscr(table, subscrKeys, batchable ? this.batchMonitor : null);
                ok = true;
            }
            finally
            {
                if (!ok)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Undoing add of ", table, " to session ", this.localPushServerProxy.SessionId }));
                    lock ((lockObj = this.tables.SyncRoot))
                    {
                        for (i = 0; i < subscrKeys.Length; i++)
                        {
                            this.tables.Remove(subscrKeys[i].KeyValue);
                        }
                    }
                }
            }
            return subscrKeys;
        }

        internal virtual Lightstreamer.DotNet.Client.SubscribedTableKey SubscrTable(ITableManager table, bool batchable)
        {
            Lightstreamer.DotNet.Client.SubscribedTableKey subscrKey;
            object lockObj;
            actionsLogger.Info(string.Concat(new object[] { "Adding ", table, " to session ", this.localPushServerProxy.SessionId }));
            lock ((lockObj = this.tables.SyncRoot))
            {
                subscrKey = this.localPushServerProxy.TableCode;
                this.tables[subscrKey.KeyValue] = table;
            }
            bool ok = false;
            try
            {
                this.localPushServerProxy.RequestSubscr(table, subscrKey, batchable ? this.batchMonitor : null);
                ok = true;
            }
            finally
            {
                if (!ok)
                {
                    actionsLogger.Info(string.Concat(new object[] { "Undoing add of ", table, " to session ", this.localPushServerProxy.SessionId }));
                    lock ((lockObj = this.tables.SyncRoot))
                    {
                        this.tables.Remove(subscrKey.KeyValue);
                    }
                }
            }
            return subscrKey;
        }

        private void TableUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            ITableManager table = this.GetUpdatedTable(values);
            if (table == null)
            {
                if (!this.localPushServerProxy.IsTableCodeConsumed(values.TableCode))
                {
                    this.serverListener.OnDataError(new PushServerException(1));
                }
            }
            else
            {
                this.serverListener.OnUpdate(table, values);
            }
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
            long giaLetti = 0L;
            try
            {
                Lightstreamer.DotNet.Client.ServerUpdateEvent values;
                bool boolVal;
                this.activityController.OnConnectionReturned();
                sessionLogger.Info("Listening for updates on session " + this.localPushServerProxy.SessionId);
                goto Label_0145;
            Label_0036:;
                try
                {
                    values = this.localPushServerProxy.WaitUpdate(this.activityController);
                }
                catch (PushServerException e)
                {
                    protLogger.Debug("Error in received data", e);
                    sessionLogger.Error("Error while listening for data in session " + this.localPushServerProxy.SessionId);
                    this.serverListener.OnDataError(e);
                    goto Label_0145;
                }
                if (values != null)
                {
                    if (values.TableUpdate)
                    {
                        this.TableUpdate(values);
                    }
                    else
                    {
                        if (values.Loop)
                        {
                            long holdingMillis = values.HoldingMillis;
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
                            this.activityController.OnConnectionReturned();
                            goto Label_0145;
                        }
                        this.MessageUpdate(values);
                    }
                    long letti = this.localPushServerProxy.TotalBytes;
                    this.serverListener.OnNewBytes(letti - giaLetti);
                    giaLetti = letti;
                }
            Label_0145:
                boolVal = true;
                goto Label_0036;
            }
            catch (PushConnException e)
            {
                streamLogger.Debug("Error in connection", e);
                sessionLogger.Error("Error while listening for data in session " + this.localPushServerProxy.SessionId);
                this.serverListener.OnFailure(e);
            }
            catch (PushEndException e)
            {
                streamLogger.Debug("Forced connection end", e);
                if (this.activityController.IsCloseUnexpected())
                {
                    sessionLogger.Error("Connection forcibly closed by the Server in session " + this.localPushServerProxy.SessionId);
                }
                this.serverListener.OnEnd(e.EndCause);
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
            private bool expectingInterruptedConnection = false;
            private bool isFirstConn;
            private long lastActivity;
            private int phase = 1;
            internal bool streamingConfirmed;
            internal bool streamingNotified = false;
            private bool warningPending;

            public ActivityController(ServerManager enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
                this.streamingConfirmed = enclosingInstance.connInfo.Polling;
            }

            public bool IsCloseUnexpected()
            {
                lock (this)
                {
                    return !this.expectingInterruptedConnection;
                }
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
                        long checkTime = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.connInfo.ProbeWarningMillis;
                        this.Launch(checkTime, this.phase);
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

            public void OnCloseRequested()
            {
                lock (this)
                {
                    this.expectingInterruptedConnection = true;
                }
            }

            public void OnConnectionReturned()
            {
                lock (this)
                {
                    if (!((this.enclosingInstance.connInfo.Polling || !this.streamingConfirmed) || this.streamingNotified))
                    {
                        this.OnStreamingResponse();
                        this.streamingNotified = true;
                    }
                    this.StartKeepalives();
                }
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
                    PushServerException exc = new PushServerException(10);
                    if (this.enclosingInstance.serverListener.OnFailure(exc))
                    {
                        ServerManager.sessionLogger.Info("Terminating session " + this.enclosingInstance.localPushServerProxy.SessionId + " because of an activity timeout");
                        this.enclosingInstance.localPushServerProxy.Dispose(true);
                    }
                });
            }

            internal void OnStreamingResponse()
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
                            long checkTime;
                            if (this.lastActivity == 0L)
                            {
                                this.OnActivityWarning(true);
                                this.warningPending = true;
                                checkTime = this.enclosingInstance.connInfo.ProbeTimeoutMillis;
                                this.Launch(checkTime, this.phase);
                            }
                            else
                            {
                                checkTime = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.connInfo.ProbeWarningMillis;
                                long limit = this.lastActivity + checkTime;
                                long left = limit - ((DateTime.Now.Ticks - 0x89f7ff5f7b58000L) / 0x2710L);
                                this.lastActivity = 0L;
                                if (left > 0L)
                                {
                                    this.Launch(left, refPhase);
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
                    long checkTime;
                    this.connectionCheck = true;
                    this.isFirstConn = isFirstConnect;
                    this.phase++;
                    if (!isFirstConnect)
                    {
                        checkTime = this.enclosingInstance.connInfo.ReconnectionTimeoutMillis;
                        if (this.enclosingInstance.connInfo.Polling)
                        {
                            checkTime += this.enclosingInstance.connInfo.PollingIdleMillis;
                        }
                        else if (!this.streamingConfirmed)
                        {
                            checkTime = this.enclosingInstance.connInfo.StreamingTimeoutMillis;
                        }
                    }
                    else if (!this.enclosingInstance.connInfo.StartsWithPoll() && !this.streamingConfirmed)
                    {
                        checkTime = this.enclosingInstance.connInfo.StreamingTimeoutMillis;
                    }
                    else
                    {
                        goto Label_00E4;
                    }
                    this.Launch(checkTime, this.phase);
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
                    long checkTime = this.enclosingInstance.localPushServerProxy.KeepaliveMillis + this.enclosingInstance.connInfo.ProbeWarningMillis;
                    this.Launch(checkTime, this.phase);
                }
            }

            public virtual void StopConnection()
            {
                lock (this)
                {
                    if (!this.isFirstConn)
                    {
                        if (!this.enclosingInstance.connInfo.Polling && !this.streamingConfirmed)
                        {
                            this.streamingConfirmed = true;
                        }
                    }
                    else if (!this.enclosingInstance.connInfo.StartsWithPoll() && !this.streamingConfirmed)
                    {
                        this.streamingConfirmed = true;
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
            void OnEndMessages();
            bool OnFailure(PushConnException e);
            bool OnFailure(PushServerException e);
            bool OnMessageOutcome(MessageManager message, SequenceHandler sequence, Lightstreamer.DotNet.Client.ServerUpdateEvent values, Exception problem);
            bool OnNewBytes(long bytes);
            bool OnReconnectTimeout();
            void OnSessionStarted(bool isPolling);
            void OnStreamingReturned();
            bool OnUpdate(ITableManager table, Lightstreamer.DotNet.Client.ServerUpdateEvent values);
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
                catch (Exception e)
                {
                    PushServerException exc = new PushServerException(12, e);
                    ServerManager.protLogger.Debug("Error in received data", e);
                    ServerManager.sessionLogger.Error("Unrecoverable error while listening to data in session " + this.enclosingInstance.localPushServerProxy.SessionId);
                    this.enclosingInstance.serverListener.OnFailure(exc);
                }
                finally
                {
                    this.enclosingInstance.serverListener.OnClose();
                }
            }
        }
    }
}

