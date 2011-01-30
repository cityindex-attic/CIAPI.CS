namespace Lightstreamer.DotNet.Client
{
    using Common.Logging;
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

        internal virtual Stream ConnectForSession()
        {
            Stream stream;
            sessionLogger.Info("Connecting for a new session");
            try
            {
                stream = this.serverTranslator.CallSession();
            }
            catch (UriFormatException exception)
            {
                sessionLogger.Info("Unsuccessful connection for new session");
                sessionLogger.Debug("Unsuccessful connection for new session", exception);
                throw new PushConnException(exception);
            }
            catch (WebException exception2)
            {
                sessionLogger.Info("Unsuccessful connection for new session");
                sessionLogger.Debug("Unsuccessful connection for new session", exception2);
                throw new PushConnException(exception2);
            }
            catch (IOException exception3)
            {
                sessionLogger.Info("Unsuccessful connection for new session");
                sessionLogger.Debug("Unsuccessful connection for new session", exception3);
                throw new PushConnException(exception3);
            }
            bool flag = false;
            lock (this)
            {
                if (!this.closed)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return stream;
            }
            sessionLogger.Info("Connection started but no longer requested");
            try
            {
                streamLogger.Debug("Closing stream connection");
                stream.Close();
            }
            catch (IOException exception4)
            {
                streamLogger.Debug("Error closing the stream connection", exception4);
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
            catch (PushUserException exception)
            {
                protLogger.Debug("Refused delete request", exception);
                throw new PushServerException(9);
            }
            catch (IOException exception2)
            {
                throw new PushConnException(exception2);
            }
            catch (WebException exception3)
            {
                throw new PushConnException(exception3);
            }
            this.Check();
        }

        internal virtual void Dispose(bool alsoCloseSession)
        {
            Stream closingLowLevelStream = null;
            StreamReader closingStream = null;
            bool streamCompleted = false;
            bool flag2 = false;
            lock (this)
            {
                if (!this.closed)
                {
                    closingLowLevelStream = this.pushLowLevelStream;
                    closingStream = this.pushStream;
                    streamCompleted = this.streamCompleted;
                    this.pushLowLevelStream = null;
                    this.pushStream = null;
                    this.streamCompleted = false;
                    if (alsoCloseSession)
                    {
                        this.closed = true;
                        this.serverTranslator.AbortControlBatch();
                    }
                }
                else
                {
                    flag2 = true;
                }
            }
            if (!flag2)
            {
                bool force = alsoCloseSession && !streamCompleted;
                this.DisposeStreams(closingLowLevelStream, closingStream, force);
            }
            else
            {
                sessionLogger.Info("Session " + this.SessionId + " already terminated");
            }
        }

        internal virtual void DisposeStreams(Stream closingLowLevelStream, StreamReader closingStream, bool force)
        {
            new AnonymousClassThread1(closingLowLevelStream, closingStream, this).Start();
            if (force)
            {
            }
        }

        internal virtual bool IsTableCodeConsumed(int tableCode)
        {
            lock (codes)
            {
                int num = tableCode;
                return ((num > 0) && (num <= currCode));
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
            catch (IOException exception)
            {
                throw new PushConnException(exception);
            }
            catch (WebException exception2)
            {
                throw new PushConnException(exception2);
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
            catch (PushUserException exception)
            {
                protLogger.Debug("Refused constraints request", exception);
                throw new PushServerException(9);
            }
            catch (IOException exception2)
            {
                throw new PushConnException(exception2);
            }
            catch (WebException exception3)
            {
                throw new PushConnException(exception3);
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
            catch (IOException exception)
            {
                throw new PushConnException(exception);
            }
            catch (WebException exception2)
            {
                throw new PushConnException(exception2);
            }
            this.Check();
        }

        internal virtual void ResyncSession()
        {
            Stream stream = null;
            StreamReader answer = null;
            PushServerProxyInfo info = null;
            sessionLogger.Info("Rebinding session " + this.serverInfo.sessionId);
            try
            {
                stream = this.serverTranslator.CallResync(this.serverInfo, null);
                answer = new StreamReader(stream, Encoding.UTF8);
                this.serverTranslator.CheckAnswer(answer);
                info = this.serverTranslator.ReadSessionId(answer);
            }
            catch (PushUserException exception)
            {
                sessionLogger.Info("Refused resync request " + this.serverInfo.sessionId);
                protLogger.Debug("Refused resync request", exception);
                throw new PushServerException(9);
            }
            catch (IOException exception2)
            {
                sessionLogger.Info("Unsuccessful rebinding of session " + this.serverInfo.sessionId);
                sessionLogger.Debug("Unsuccessful rebinding of session " + this.serverInfo.sessionId, exception2);
                throw new PushConnException(exception2);
            }
            catch (WebException exception3)
            {
                sessionLogger.Info("Unsuccessful rebinding of session " + this.serverInfo.sessionId);
                sessionLogger.Debug("Unsuccessful rebinding of session " + this.serverInfo.sessionId, exception3);
                throw new PushConnException(exception3);
            }
            bool flag = false;
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
                    flag = true;
                }
            }
            if (!flag)
            {
                sessionLogger.Info("Rebind successful on session " + this.serverInfo.sessionId);
            }
            else
            {
                sessionLogger.Info("Rebind successful but no longer requested");
                this.DisposeStreams(stream, answer, true);
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
            catch (IOException exception)
            {
                throw new PushConnException(exception);
            }
            catch (WebException exception2)
            {
                throw new PushConnException(exception2);
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
            catch (IOException exception)
            {
                sessionLogger.Info("Unsuccessful start of new session");
                sessionLogger.Debug("Unsuccessful start of new session", exception);
                throw new PushConnException(exception);
            }
            catch (WebException exception2)
            {
                sessionLogger.Info("Unsuccessful start of new session");
                sessionLogger.Debug("Unsuccessful start of new session", exception2);
                throw new PushConnException(exception2);
            }
            bool flag = false;
            lock (this)
            {
                if (!this.closed)
                {
                    flag = true;
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
            if (!flag)
            {
                sessionLogger.Info("Started session " + this.serverInfo.sessionId);
            }
            else
            {
                IOException exception3;
                sessionLogger.Info("Session started but no longer requested");
                try
                {
                    streamLogger.Debug("Closing stream connection");
                    answer.Close();
                }
                catch (IOException exception6)
                {
                    exception3 = exception6;
                    streamLogger.Debug("Error closing the stream connection", exception3);
                }
                try
                {
                    stream.Close();
                }
                catch (IOException exception7)
                {
                    exception3 = exception7;
                    streamLogger.Debug("Error closing the connection", exception3);
                }
                throw new PhaseException();
            }
        }

        private string WaitCommand(ServerManager.ActivityController activityController)
        {
            StreamReader pushStream;
            string str2;
            PushServerProxy proxy;
            lock ((proxy = this))
            {
                this.Check();
                pushStream = this.pushStream;
            }
            try
            {
                string str = this.serverTranslator.WaitCommand(pushStream);
                activityController.OnActivity();
                str2 = str;
            }
            catch (PushLengthException exception)
            {
                lock ((proxy = this))
                {
                    this.Check();
                    this.streamCompleted = true;
                }
                activityController.StopKeepalives();
                throw exception;
            }
            catch (IOException exception2)
            {
                lock ((proxy = this))
                {
                    this.Check();
                    this.streamCompleted = true;
                }
                throw new PushConnException(exception2);
            }
            catch (WebException exception3)
            {
                lock ((proxy = this))
                {
                    this.Check();
                    this.streamCompleted = true;
                }
                throw new PushConnException(exception3);
            }
            return str2;
        }

        internal virtual Lightstreamer.DotNet.Client.ServerUpdateEvent WaitUpdate(ServerManager.ActivityController activityController)
        {
            while (true)
            {
                this.Check();
                string pushData = this.WaitCommand(activityController);
                if (pushData != null)
                {
                    Lightstreamer.DotNet.Client.ServerUpdateEvent event2;
                    try
                    {
                        event2 = this.serverTranslator.ParsePushData(pushData);
                    }
                    catch (PushServerException exception)
                    {
                        throw exception;
                    }
                    catch (Exception exception2)
                    {
                        throw new PushServerException(12, exception2);
                    }
                    lock (this)
                    {
                        this.totalBytes += pushData.Length + 2;
                    }
                    this.Check();
                    return event2;
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
                IOException exception;
                try
                {
                    this.closingLowLevelStream.Close();
                }
                catch (IOException exception1)
                {
                    exception = exception1;
                    PushServerProxy.streamLogger.Debug("Error closing the stream connection", exception);
                }
                try
                {
                    PushServerProxy.streamLogger.Debug("Closing stream connection");
                    this.closingStream.Close();
                }
                catch (IOException exception2)
                {
                    exception = exception2;
                    PushServerProxy.streamLogger.Debug("Error closing the stream connection", exception);
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

