namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;

    internal class BatchManager
    {
        private BatchingHttpProvider batchingProvider;
        private CookieContainer cookies;
        private long limit = 0L;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");

        public BatchManager(CookieContainer cookies)
        {
            this.cookies = cookies;
        }

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
            BatchingHttpProvider batchToClose;
            lock (this)
            {
                batchToClose = this.batchingProvider;
                this.batchingProvider = null;
            }
            if (batchToClose != null)
            {
                DoAsyncPost(batchToClose);
            }
        }

        private static void DoAsyncPost(BatchingHttpProvider batchToClose)
        {
            new AnonymousClassThread(batchToClose).Start(true);
        }

        internal virtual StreamReader GetAnswer(string controlUrl, IDictionary parameters, BatchMonitor batch)
        {
            BatchingHttpProvider batchToClose = null;
            BatchingHttpProvider.BufferedReaderMonitor answerMonitor = null;
            bool retry = false;
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
                            answerMonitor = this.batchingProvider.AddCall(parameters);
                            if (answerMonitor != null)
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
                                this.batchingProvider = new BatchingHttpProvider(controlUrl, this.limit, this.cookies);
                                retry = true;
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
            if (answerMonitor != null)
            {
                batch.BatchedOne();
                return answerMonitor.GetReader();
            }
            if (retry)
            {
                return this.GetAnswer(controlUrl, parameters, batch);
            }
            return this.GetNotBatchedAnswer(controlUrl, parameters);
        }

        internal virtual StreamReader GetNotBatchedAnswer(string controlUrl, IDictionary parameters)
        {
            HttpProvider pendingCall = new HttpProvider(controlUrl, this.cookies);
            protLogger.Info("Opening control connection");
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Control params: " + CollectionsSupport.ToString(parameters));
            }
            return new StreamReader(pendingCall.DoHTTP(parameters, true), Encoding.UTF8);
        }

        internal virtual void StartBatch(string controlUrl)
        {
            lock (this)
            {
                if (this.batchingProvider != null)
                {
                    this.batchingProvider.Abort(new SubscrException("requests batch discarded"));
                }
                this.batchingProvider = new BatchingHttpProvider(controlUrl, this.limit, this.cookies);
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
                catch (Exception t)
                {
                    BatchManager.protLogger.Error("Error in batch operation: " + t.Message);
                    this.batchToClose.Abort(t);
                }
            }
        }
    }
}

