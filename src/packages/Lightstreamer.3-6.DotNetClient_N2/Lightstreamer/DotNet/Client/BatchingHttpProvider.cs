namespace Lightstreamer.DotNet.Client
{
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;

    internal class BatchingHttpProvider : HttpProvider
    {
        private bool consumed;
        private BufferedReaderMonitor firstReader;
        private BufferedReaderMonitor lastReader;
        private long limit;
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");

        public BatchingHttpProvider(string address, long limit) : base(address)
        {
            this.consumed = false;
            this.limit = limit;
        }

        public virtual void Abort(Exception error)
        {
            BufferedReaderMonitor firstReader;
            lock (this)
            {
                if (this.consumed)
                {
                }
                this.consumed = true;
                firstReader = this.firstReader;
            }
            if (firstReader != null)
            {
                firstReader.Error = error;
            }
        }

        public virtual BufferedReaderMonitor AddCall(Hashtable parameters)
        {
            lock (this)
            {
                if (this.consumed)
                {
                    throw new SubscrException("Illegal use of a batch");
                }
                if (!this.AddLine(parameters, this.limit))
                {
                    return null;
                }
                BufferedReaderMonitor monitor = new BufferedReaderMonitor();
                if (this.lastReader == null)
                {
                    this.firstReader = monitor;
                }
                else
                {
                    this.lastReader.Next = monitor;
                }
                this.lastReader = monitor;
                return monitor;
            }
        }

        public virtual void DoPosts()
        {
            BufferedReaderMonitor firstReader;
            lock (this)
            {
                if (this.consumed)
                {
                    return;
                }
                this.consumed = true;
                firstReader = this.firstReader;
            }
            if (firstReader != null)
            {
                Stream responseStream;
                IOException exception;
                try
                {
                    HttpWebRequest request = this.SendPost();
                    WebResponse response = null;
                    try
                    {
                        response = request.GetResponse();
                        responseStream = response.GetResponseStream();
                    }
                    catch (IOException exception1)
                    {
                        exception = exception1;
                        try
                        {
                            response.Close();
                        }
                        catch (Exception)
                        {
                        }
                        throw exception;
                    }
                }
                catch (IOException exception4)
                {
                    exception = exception4;
                    streamLogger.Error("Error in batch operation: " + exception.Message);
                    this.Abort(exception);
                    return;
                }
                catch (WebException exception2)
                {
                    streamLogger.Error("Error in batch operation: " + exception2.Message);
                    this.Abort(exception2);
                    return;
                }
                MyReader answer = new MyReader(new StreamReader(responseStream, Encoding.Default));
                firstReader.SetReader(answer);
            }
        }

        internal virtual bool Empty
        {
            get
            {
                lock (this)
                {
                    return (this.firstReader == null);
                }
            }
        }

        public class BufferedReaderMonitor
        {
            private BatchingHttpProvider.MyReader answer;
            private static AutoResetEvent autoEvent = new AutoResetEvent(false);
            private Exception error;
            private BatchingHttpProvider.BufferedReaderMonitor next;
            private static Mutex waiter = new Mutex();

            public virtual StreamReader GetReader()
            {
                lock (this)
                {
                    while ((this.answer == null) && (this.error == null))
                    {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException)
                        {
                        }
                    }
                    if (this.error != null)
                    {
                        if (this.error is IOException)
                        {
                            throw ((IOException) this.error);
                        }
                        if (this.error is WebException)
                        {
                            throw ((WebException) this.error);
                        }
                        if (this.error is SubscrException)
                        {
                            throw ((SubscrException) this.error);
                        }
                        throw new SubscrException(string.Concat(new object[] { "Unexpected ", this.error.GetType(), " :", this.error.Message }));
                    }
                    return this.answer;
                }
            }

            public bool OnReaderClose()
            {
                if (this.next != null)
                {
                    this.next.SetReader(this.answer);
                    return true;
                }
                return false;
            }

            public void SetReader(BatchingHttpProvider.MyReader answer)
            {
                lock (this)
                {
                    this.answer = answer;
                    answer.Master = this;
                    Monitor.Pulse(this);
                }
            }

            public virtual Exception Error
            {
                set
                {
                    lock (this)
                    {
                        this.error = value;
                        Monitor.Pulse(this);
                    }
                    if (this.next != null)
                    {
                        this.next.Error = value;
                    }
                }
            }

            public virtual BatchingHttpProvider.BufferedReaderMonitor Next
            {
                set
                {
                    this.next = value;
                }
            }
        }

        internal class MyReader : StreamReader
        {
            private BatchingHttpProvider.BufferedReaderMonitor master;

            public MyReader(StreamReader reader) : base(reader.BaseStream, reader.CurrentEncoding)
            {
            }

            public override void Close()
            {
                if (!this.master.OnReaderClose())
                {
                    BatchingHttpProvider.streamLogger.Debug("Closing connection");
                    base.Close();
                }
            }

            public virtual BatchingHttpProvider.BufferedReaderMonitor Master
            {
                set
                {
                    this.master = value;
                }
            }
        }
    }
}

