namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    internal class BatchManager
    {
        private BatchingHttpProvider batchingProvider;
        private long limit = 0L;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");

        internal virtual void AbortBatch()
        {
            lock (this)
            {
                if (this.batchingProvider != null)
                {
                    this.batchingProvider.Abort(new SubscrException("requests batch aborted"));
                    this.batchingProvider = null;
                }
            }
        }

        internal virtual void CloseBatch()
        {
            BatchingHttpProvider batchingProvider;
            lock (this)
            {
                batchingProvider = this.batchingProvider;
                this.batchingProvider = null;
            }
            if (batchingProvider != null)
            {
                DoAsyncPost(batchingProvider);
            }
        }

        private static void DoAsyncPost(BatchingHttpProvider batchToClose)
        {
            new AnonymousClassThread(batchToClose).Start();
        }

        internal virtual StreamReader GetAnswer(string controlUrl, Hashtable parameters, BatchMonitor batch)
        {
            BatchingHttpProvider batchToClose = null;
            BatchingHttpProvider.BufferedReaderMonitor monitor = null;
            bool flag = false;
            lock (batch)
            {
                lock (this)
                {
                    if (!batch.Filled)
                    {
                        batch.UseOne();
                        if (this.batchingProvider != null)
                        {
                            protLogger.Info("Batching control request");
                            if (protLogger.IsDebugEnabled)
                            {
                                protLogger.Debug("Control params: " + CollectionsSupport.ToString(parameters));
                            }
                            monitor = this.batchingProvider.AddCall(parameters);
                            if (monitor != null)
                            {
                                if (batch.Filled)
                                {
                                    batchToClose = this.batchingProvider;
                                    this.batchingProvider = null;
                                }
                            }
                            else if (this.batchingProvider.Empty)
                            {
                                protLogger.Info("Batching failed; trying without batch");
                                if (batch.Filled)
                                {
                                    this.batchingProvider = null;
                                }
                            }
                            else
                            {
                                protLogger.Info("Batching failed; trying a new batch");
                                batchToClose = this.batchingProvider;
                                batch.Expand(1);
                                this.batchingProvider = new BatchingHttpProvider(controlUrl, this.limit);
                                flag = true;
                            }
                        }
                    }
                    else if (this.batchingProvider != null)
                    {
                        this.batchingProvider.Abort(new SubscrException("wrong requests batch"));
                        this.batchingProvider = null;
                    }
                }
            }
            if (batchToClose != null)
            {
                DoAsyncPost(batchToClose);
            }
            if (monitor != null)
            {
                return monitor.GetReader();
            }
            if (flag)
            {
                return this.GetAnswer(controlUrl, parameters, batch);
            }
            return this.GetNotBatchedAnswer(controlUrl, parameters);
        }

        internal virtual StreamReader GetNotBatchedAnswer(string controlUrl, Hashtable parameters)
        {
            HttpProvider provider = new HttpProvider(controlUrl);
            protLogger.Info("Opening control connection");
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Control params: " + CollectionsSupport.ToString(parameters));
            }
            return new StreamReader(provider.DoPost(parameters), Encoding.Default);
        }

        internal virtual void StartBatch(string controlUrl)
        {
            lock (this)
            {
                if (this.batchingProvider != null)
                {
                    this.batchingProvider.Abort(new SubscrException("requests batch discarded"));
                }
                this.batchingProvider = new BatchingHttpProvider(controlUrl, this.limit);
            }
        }

        internal virtual long Limit
        {
            set
            {
                this.limit = value;
            }
        }

        private class AnonymousClassThread : ThreadSupport
        {
            private BatchingHttpProvider batchToClose;

            public AnonymousClassThread(BatchingHttpProvider batchToClose)
            {
                this.batchToClose = batchToClose;
            }

            public override void Run()
            {
                try
                {
                    BatchManager.protLogger.Info("Opening control connection to send batched requests");
                    this.batchToClose.DoPosts();
                }
                catch (Exception exception)
                {
                    BatchManager.protLogger.Error("Error in batch operation: " + exception.Message);
                    this.batchToClose.Abort(exception);
                }
            }
        }
    }
}

