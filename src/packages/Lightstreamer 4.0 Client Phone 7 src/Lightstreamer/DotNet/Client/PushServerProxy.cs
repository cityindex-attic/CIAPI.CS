namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    internal class PushServerProxy
    {
        private bool closed = true;
        private static object codes = new object();
        private static int currCode = 0;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private Stream pushLowLevelStream = null;
        private StreamReader pushStream = null;
        private PushServerProxyInfo serverInfo = null;
        private PushServerTranslator serverTranslator;
        private static ILog sessionLogger = LogManager.GetLogger("com.lightstreamer.ls_client.session");
        private bool streamCompleted = false;
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");
        private long totalBytes = 0L;
        private string userId = null;

        internal PushServerProxy(ConnectionInfo info)
        {
            this.serverTranslator = new PushServerTranslator(info);
        }

        private void Check()
        {
            lock (this)
            {
                if (this.closed)
                {
                    throw new PhaseException();
                }
            }
        }

        internal virtual void CloseBatch()
        {
            this.serverTranslator.CloseControlBatch();
        }

        internal virtual void CloseMessageBatch()
        {
            this.serverTranslator.CloseMessageBatch();
        }

        internal virtual Stream ConnectForSession()
        {
            Stream stream;
            sessionLogger.Info("Connecting for a new session");
            try
            {
                stream = this.serverTranslator.CallSession();
            }
            catch (UriFormatException e)
            {
                sessionLogger.Info("Unsuccessful connection for new session");
                sessionLogger.Debug("Unsuccessful connection for new session", e);
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                sessionLogger.Info("Unsuccessful connection for new session");
                sessionLogger.Debug("Unsuccessful connection for new session", e);
                throw new PushConnException(e);
            }
            catch (IOException e)
            {
                sessionLogger.Info("Unsuccessful connection for new session");
                sessionLogger.Debug("Unsuccessful connection for new session", e);
                throw new PushConnException(e);
            }
            bool late = false;
            lock (this)
            {
                if (!this.closed)
                {
                    late = true;
                }
            }
            if (!late)
            {
                return stream;
            }
            sessionLogger.Info("Connection started but no longer requested");
            try
            {
                streamLogger.Debug("Closing stream connection");
                stream.Close();
            }
            catch (IOException e1)
            {
                streamLogger.Debug("Error closing the stream connection", e1);
            }
            throw new PhaseException();
        }

        internal virtual void DelSubscrs(Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys, BatchMonitor batch)
        {
            string[] tableCodes = new string[subscrKeys.Length];
            for (int i = 0; i < subscrKeys.Length; i++)
            {
                tableCodes[i] = subscrKeys[i].KeyValue.ToString();
            }
            this.Check();
            try
            {
                this.serverTranslator.CallDelete(this.userId, this.serverInfo, tableCodes, batch);
            }
            catch (PushUserException e)
            {
                protLogger.Debug("Refused delete request", e);
                throw new PushServerException(9);
            }
            catch (IOException e)
            {
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                throw new PushConnException(e);
            }
            this.Check();
        }

        internal virtual void Dispose(bool alsoCloseSession)
        {
            Stream oldLowLevelStream = null;
            StreamReader oldStream = null;
            bool oldStreamCompleted = false;
            PushServerProxyInfo currServerInfo = null;
            bool late = false;
            lock (this)
            {
                if (!this.closed)
                {
                    oldLowLevelStream = this.pushLowLevelStream;
                    oldStream = this.pushStream;
                    oldStreamCompleted = this.streamCompleted;
                    this.pushLowLevelStream = null;
                    this.pushStream = null;
                    this.streamCompleted = false;
                    currServerInfo = this.serverInfo;
                    if (alsoCloseSession)
                    {
                        this.closed = true;
                        this.serverTranslator.AbortBatches();
                    }
                }
                else
                {
                    late = true;
                }
            }
            if (!late)
            {
                if (alsoCloseSession && !oldStreamCompleted)
                {
                    this.DisposeStreams(oldLowLevelStream, oldStream, currServerInfo);
                }
                else
                {
                    this.DisposeStreams(oldLowLevelStream, oldStream, null);
                }
            }
            else
            {
                sessionLogger.Info("Session " + this.SessionId + " already terminated");
            }
        }

        internal virtual void DisposeStreams(Stream closingLowLevelStream, StreamReader closingStream, PushServerProxyInfo closingInfo)
        {
            new AnonymousClassThread1(closingLowLevelStream, closingStream, this).Start(true);
            if (closingInfo != null)
            {
                new AnonymousClassThread2(closingInfo, this).Start(true);
            }
        }

        internal virtual bool IsTableCodeConsumed(int tableCode)
        {
            lock (codes)
            {
                int code = tableCode;
                return ((code > 0) && (code <= currCode));
            }
        }

        internal virtual void RequestItemsSubscr(VirtualTableManager table, Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys, BatchMonitor batch)
        {
            string[] tableCodes = new string[subscrKeys.Length];
            for (int i = 0; i < subscrKeys.Length; i++)
            {
                tableCodes[i] = subscrKeys[i].KeyValue.ToString();
            }
            this.Check();
            try
            {
                this.serverTranslator.CallItemsRequest(this.serverInfo, tableCodes, table, batch);
            }
            catch (IOException e)
            {
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                throw new PushConnException(e);
            }
            this.Check();
        }

        internal virtual void RequestNewConstraints(Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
        {
            this.Check();
            try
            {
                this.serverTranslator.CallConstrainRequest(this.serverInfo, constraints);
            }
            catch (PushUserException e)
            {
                protLogger.Debug("Refused constraints request", e);
                throw new PushServerException(9);
            }
            catch (IOException e)
            {
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                throw new PushConnException(e);
            }
            this.Check();
        }

        internal virtual void RequestSendMessage(MessageManager message, int prog, BatchMonitor batch)
        {
            this.Check();
            try
            {
                this.serverTranslator.CallGuaranteedSendMessageRequest(this.serverInfo, Convert.ToString(prog), message, batch);
            }
            catch (IOException e)
            {
                throw new PushConnException(e);
            }
            this.Check();
        }

        internal virtual void RequestSubscr(ITableManager table, Lightstreamer.DotNet.Client.SubscribedTableKey subscrKey, BatchMonitor batch)
        {
            string tableCode = subscrKey.KeyValue.ToString();
            this.Check();
            try
            {
                this.serverTranslator.CallTableRequest(this.serverInfo, tableCode, table, batch);
            }
            catch (IOException e)
            {
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                throw new PushConnException(e);
            }
            this.Check();
        }

        internal virtual void ResyncSession()
        {
            Stream stream = null;
            StreamReader answer = null;
            PushServerProxyInfo info = null;
            sessionLogger.Info("Rebinding session " + this.serverInfo.sessionId);
            this.Check();
            try
            {
                stream = this.serverTranslator.CallResync(this.serverInfo, null);
                answer = new StreamReader(stream, Encoding.UTF8);
                this.serverTranslator.CheckAnswer(answer);
                info = this.serverTranslator.ReadSessionId(answer);
            }
            catch (PushUserException e1)
            {
                sessionLogger.Info("Refused resync request " + this.serverInfo.sessionId);
                protLogger.Debug("Refused resync request", e1);
                throw new PushServerException(9);
            }
            catch (IOException e)
            {
                sessionLogger.Info("Unsuccessful rebinding of session " + this.serverInfo.sessionId);
                sessionLogger.Debug("Unsuccessful rebinding of session " + this.serverInfo.sessionId, e);
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                sessionLogger.Info("Unsuccessful rebinding of session " + this.serverInfo.sessionId);
                sessionLogger.Debug("Unsuccessful rebinding of session " + this.serverInfo.sessionId, e);
                throw new PushConnException(e);
            }
            bool late = false;
            lock (this)
            {
                if (!this.closed)
                {
                    this.Dispose(false);
                    this.pushLowLevelStream = stream;
                    this.pushStream = answer;
                    this.streamCompleted = false;
                    this.serverInfo = info;
                }
                else
                {
                    late = true;
                }
            }
            if (!late)
            {
                sessionLogger.Info("Rebind successful on session " + this.serverInfo.sessionId);
            }
            else
            {
                sessionLogger.Info("Rebind successful but no longer requested");
                this.DisposeStreams(stream, answer, null);
                throw new PhaseException();
            }
        }

        internal virtual void SendMessage(string message)
        {
            this.Check();
            try
            {
                this.serverTranslator.CallSendMessageRequest(this.serverInfo, message);
            }
            catch (IOException e)
            {
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                throw new PushConnException(e);
            }
            this.Check();
        }

        internal virtual void StartBatch()
        {
            lock (this)
            {
                this.Check();
                this.serverTranslator.StartControlBatch(this.serverInfo);
            }
        }

        internal virtual void StartMessageBatch()
        {
            lock (this)
            {
                this.Check();
                this.serverTranslator.StartMessageBatch(this.serverInfo);
            }
        }

        internal virtual void StartSession(Stream stream)
        {
            StreamReader answer = null;
            PushServerProxyInfo info = null;
            sessionLogger.Info("Starting new session");
            try
            {
                answer = new StreamReader(stream, Encoding.UTF8);
                this.serverTranslator.CheckAnswer(answer);
                info = this.serverTranslator.ReadSessionId(answer);
            }
            catch (PushEndException)
            {
                throw new PushServerException(7);
            }
            catch (IOException e)
            {
                sessionLogger.Info("Unsuccessful start of new session");
                sessionLogger.Debug("Unsuccessful start of new session", e);
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                sessionLogger.Info("Unsuccessful start of new session");
                sessionLogger.Debug("Unsuccessful start of new session", e);
                throw new PushConnException(e);
            }
            bool late = false;
            lock (this)
            {
                if (!this.closed)
                {
                    late = true;
                }
                else
                {
                    this.pushLowLevelStream = stream;
                    this.pushStream = answer;
                    this.streamCompleted = false;
                    this.serverInfo = info;
                    this.closed = false;
                }
            }
            if (!late)
            {
                sessionLogger.Info("Started session " + this.serverInfo.sessionId);
            }
            else
            {
                sessionLogger.Info("Session started but no longer requested");
                this.DisposeStreams(stream, answer, info);
                throw new PhaseException();
            }
        }

        private InfoString WaitCommand(ServerManager.ActivityController activityController)
        {
            StreamReader currStream;
            InfoString CS$1$0000;
            PushServerProxy CS$2$0001;
            lock ((CS$2$0001 = this))
            {
                this.Check();
                currStream = this.pushStream;
            }
            try
            {
                InfoString pushData = this.serverTranslator.WaitCommand(currStream);
                if ((pushData != null) && (pushData.value == null))
                {
                    this.Check();
                    activityController.StopKeepalives();
                }
                else
                {
                    activityController.OnActivity();
                }
                CS$1$0000 = pushData;
            }
            catch (PushEndException e)
            {
                lock ((CS$2$0001 = this))
                {
                    this.Check();
                    this.streamCompleted = true;
                }
                throw e;
            }
            catch (IOException e)
            {
                lock ((CS$2$0001 = this))
                {
                    this.Check();
                    this.streamCompleted = true;
                }
                throw new PushConnException(e);
            }
            catch (WebException e)
            {
                lock ((CS$2$0001 = this))
                {
                    this.Check();
                    this.streamCompleted = true;
                }
                throw new PushConnException(e);
            }
            return CS$1$0000;
        }

        internal virtual Lightstreamer.DotNet.Client.ServerUpdateEvent WaitUpdate(ServerManager.ActivityController activityController)
        {
            while (true)
            {
                this.Check();
                InfoString pushData = this.WaitCommand(activityController);
                if (pushData != null)
                {
                    Lightstreamer.DotNet.Client.ServerUpdateEvent evnt;
                    if (pushData.value == null)
                    {
                        return new Lightstreamer.DotNet.Client.ServerUpdateEvent(pushData.holdingMillis);
                    }
                    try
                    {
                        evnt = this.serverTranslator.ParsePushData(pushData.value);
                    }
                    catch (PushServerException e)
                    {
                        throw e;
                    }
                    catch (Exception e)
                    {
                        throw new PushServerException(12, e);
                    }
                    lock (this)
                    {
                        this.totalBytes += pushData.value.Length + 2;
                    }
                    this.Check();
                    return evnt;
                }
            }
        }

        internal virtual long KeepaliveMillis
        {
            get
            {
                return this.serverInfo.keepaliveMillis;
            }
        }

        internal virtual string SessionId
        {
            get
            {
                return this.serverInfo.sessionId;
            }
        }

        internal virtual Lightstreamer.DotNet.Client.SubscribedTableKey TableCode
        {
            get
            {
                lock (codes)
                {
                    currCode++;
                    return new Lightstreamer.DotNet.Client.SubscribedTableKey(currCode);
                }
            }
        }

        internal virtual long TotalBytes
        {
            get
            {
                lock (this)
                {
                    return this.totalBytes;
                }
            }
        }

        private class AnonymousClassThread1 : ThreadSupport
        {
            private Stream closingLowLevelStream;
            private StreamReader closingStream;
            private PushServerProxy enclosingInstance;

            public AnonymousClassThread1(Stream closingLowLevelStream, StreamReader closingStream, PushServerProxy enclosingInstance)
            {
                this.closingLowLevelStream = closingLowLevelStream;
                this.closingStream = closingStream;
                this.enclosingInstance = enclosingInstance;
            }

            public override void Run()
            {
                IOException e;
                try
                {
                    this.closingLowLevelStream.Close();
                }
                catch (IOException exception1)
                {
                    e = exception1;
                    PushServerProxy.streamLogger.Debug("Error closing the stream connection", e);
                }
                try
                {
                    PushServerProxy.streamLogger.Debug("Closing stream connection");
                    this.closingStream.Close();
                }
                catch (IOException exception2)
                {
                    e = exception2;
                    PushServerProxy.streamLogger.Debug("Error closing the stream connection", e);
                }
            }
        }

        private class AnonymousClassThread2 : ThreadSupport
        {
            private PushServerProxy.PushServerProxyInfo closingServerInfo;
            private PushServerProxy enclosingInstance;

            public AnonymousClassThread2(PushServerProxy.PushServerProxyInfo closingServerInfo, PushServerProxy enclosingInstance)
            {
                this.closingServerInfo = closingServerInfo;
                this.enclosingInstance = enclosingInstance;
            }

            public override void Run()
            {
                try
                {
                    this.enclosingInstance.serverTranslator.CallDestroyRequest(this.closingServerInfo);
                }
                catch (Exception)
                {
                }
            }
        }

        internal class PushServerProxyInfo
        {
            public string controlAddress;
            public long keepaliveMillis;
            public string rebindAddress;
            public string sessionId;

            public PushServerProxyInfo(string sessionId, string controlAddress, string rebindAddress, long keepaliveMillis)
            {
                this.sessionId = sessionId;
                this.controlAddress = controlAddress;
                this.rebindAddress = rebindAddress;
                this.keepaliveMillis = keepaliveMillis;
            }

            public override string ToString()
            {
                return string.Concat(new object[] { "[ Session ID: ", this.sessionId, " - Control Address to be used: ", this.controlAddress, " - Rebind Address to be used: ", this.rebindAddress, " - Keepalive millis: ", this.keepaliveMillis, "]" });
            }
        }
    }
}

