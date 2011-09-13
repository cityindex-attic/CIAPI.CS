namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
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

        public BatchingHttpProvider(string address, long limit, CookieContainer cookies) : base(address, cookies)
        {
            this.consumed = false;
            this.limit = limit;
        }

        public virtual void Abort(Exception error)
        {
            BufferedReaderMonitor myReaders;
            lock (this)
            {
                if (this.consumed)
                {
                }
                this.consumed = true;
                myReaders = this.firstReader;
            }
            if (myReaders != null)
            {
                myReaders.Error = error;
            }
        }

        public virtual BufferedReaderMonitor AddCall(IDictionary parameters)
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
                BufferedReaderMonitor newReader = new BufferedReaderMonitor();
                if (this.lastReader == null)
                {
                    this.firstReader = newReader;
                }
                else
                {
                    this.lastReader.Next = newReader;
                }
                this.lastReader = newReader;
                return newReader;
            }
        }

        public virtual void DoPosts()
        {
            BufferedReaderMonitor myReaders;
            lock (this)
            {
                if (this.consumed)
                {
                    return;
                }
                this.consumed = true;
                myReaders = this.firstReader;
            }
            if (myReaders != null)
            {
                Stream input;
                try
                {
                    input = this.DoHTTP(true);
                }
                catch (IOException e)
                {
                    streamLogger.Error("Error in batch operation: " + e.Message);
                    this.Abort(e);
                    return;
                }
                catch (WebException e)
                {
                    streamLogger.Error("Error in batch operation: " + e.Message);
                    this.Abort(e);
                    return;
                }
                MyReader answers = new MyReader(new StreamReader(input, Encoding.UTF8));
                myReaders.SetReader(answers);
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
                        catch (Exception)
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
                    BatchingHttpProvider.streamLogger.Debug("Closing control connection");
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

