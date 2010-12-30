namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
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
                streamLogger.Debug("Closing connection");
                stream.Close();
            }
            catch (IOException exception4)
            {
                streamLogger.Debug("Error closing the connection", exception4);
            }
            throw new PhaseException();
        }

        internal virtual void DelSubscrs(Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys, BatchMonitor batch)
        {
            string[] winCodes = new string[subscrKeys.Length];
            for (int i = 0; i < subscrKeys.Length; i++)
            {
                winCodes[i] = subscrKeys[i].KeyValue.ToString();
            }
            this.Check();
            try
            {
                this.serverTranslator.CallDelete(this.userId, this.serverInfo, winCodes, batch);
            }
            catch (PushUserException exception)
            {
                protLogger.Debug("Refused delete request", exception);
                throw new PushServerException(7);
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

        internal virtual void Dispose()
        {
            Stream pushLowLevelStream = null;
            StreamReader pushStream = null;
            bool flag = false;
            lock (this)
            {
                if (!this.closed)
                {
                    pushLowLevelStream = this.pushLowLevelStream;
                    pushStream = this.pushStream;
                    this.pushLowLevelStream = null;
                    this.pushStream = null;
                    this.closed = true;
                    this.serverTranslator.AbortControlBatch();
                }
                else
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                Stream closingLowLevelStream = pushLowLevelStream;
                StreamReader closingStream = pushStream;
                new AnonymousClassThread(closingLowLevelStream, closingStream, this).Start();
            }
            else
            {
                sessionLogger.Info("Session " + this.SessionId + " already terminated");
            }
        }

        internal virtual bool IsWindowCodeConsumed(int winCode)
        {
            lock (codes)
            {
                int num = winCode;
                return ((num > 0) && (num <= currCode));
            }
        }

        internal virtual void RequestItemsSubscr(VirtualTableManager table, Lightstreamer.DotNet.Client.SubscribedTableKey[] subscrKeys, BatchMonitor batch)
        {
            string[] winCodes = new string[subscrKeys.Length];
            for (int i = 0; i < subscrKeys.Length; i++)
            {
                winCodes[i] = subscrKeys[i].KeyValue.ToString();
            }
            this.Check();
            try
            {
                this.serverTranslator.CallItemsRequest(this.serverInfo, winCodes, table, batch);
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
                throw new PushServerException(7);
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
            string winCode = subscrKey.KeyValue.ToString();
            this.Check();
            try
            {
                this.serverTranslator.CallTableRequest(this.serverInfo, winCode, table, batch);
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
            IOException exception2;
            sessionLogger.Info("Rebinding session " + this.serverInfo.sessionId);
            try
            {
                stream = this.serverTranslator.CallResync(this.serverInfo, null);
                answer = new StreamReader(stream, Encoding.Default);
                this.serverTranslator.CheckAnswer(answer);
                info = this.serverTranslator.ReadSessionId(answer);
            }
            catch (PushUserException exception)
            {
                sessionLogger.Info("Refused resync request" + this.serverInfo.sessionId);
                protLogger.Debug("Refused resync request", exception);
                throw new PushServerException(7);
            }
            catch (IOException exception4)
            {
                exception2 = exception4;
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
                    this.Dispose();
                    this.pushLowLevelStream = stream;
                    this.pushStream = answer;
                    this.serverInfo = info;
                    this.closed = false;
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
                try
                {
                    streamLogger.Debug("Closing connection");
                    answer.Close();
                }
                catch (IOException exception6)
                {
                    exception2 = exception6;
                    streamLogger.Debug("Error closing the connection", exception2);
                }
                try
                {
                    stream.Close();
                }
                catch (IOException exception7)
                {
                    exception2 = exception7;
                    streamLogger.Debug("Error closing the connection", exception2);
                }
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
                answer = new StreamReader(stream, Encoding.Default);
                this.serverTranslator.CheckAnswer(answer);
                info = this.serverTranslator.ReadSessionId(answer);
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
                    streamLogger.Debug("Closing connection");
                    answer.Close();
                }
                catch (IOException exception5)
                {
                    exception3 = exception5;
                    streamLogger.Debug("Error closing the connection", exception3);
                }
                try
                {
                    stream.Close();
                }
                catch (IOException exception6)
                {
                    exception3 = exception6;
                    streamLogger.Debug("Error closing the connection", exception3);
                }
                throw new PhaseException();
            }
        }

        private string WaitCommand(ServerManager.ActivityController activityController)
        {
            string str2;
            try
            {
                string str = this.serverTranslator.WaitCommand(this.pushStream);
                activityController.OnActivity();
                str2 = str;
            }
            catch (PushLengthException exception)
            {
                this.Check();
                activityController.StopKeepalives();
                throw exception;
            }
            catch (IOException exception2)
            {
                this.Check();
                throw new PushConnException(exception2);
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

        internal virtual Lightstreamer.DotNet.Client.SubscribedTableKey WindowCode
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

        private class AnonymousClassThread : ThreadSupport
        {
            private Stream closingLowLevelStream;
            private StreamReader closingStream;
            private PushServerProxy enclosingInstance;

            public AnonymousClassThread(Stream closingLowLevelStream, StreamReader closingStream, PushServerProxy enclosingInstance)
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
                    PushServerProxy.streamLogger.Debug("Error closing the connection", exception);
                }
                try
                {
                    PushServerProxy.streamLogger.Debug("Closing connection");
                    this.closingStream.Close();
                }
                catch (IOException exception2)
                {
                    exception = exception2;
                    PushServerProxy.streamLogger.Debug("Error closing the connection", exception);
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

