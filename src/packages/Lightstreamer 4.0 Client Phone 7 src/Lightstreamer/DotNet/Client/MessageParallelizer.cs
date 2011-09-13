namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class MessageParallelizer : IBatchListener
    {
        private int batched = 0;
        private const int BATCHING = 2;
        private const int EMPTY = 1;
        private ServerManager manager;
        private BatchMonitor monitor;
        private Queue<MessageManager> queue = new Queue<MessageManager>();
        private const int SENDING = 3;
        private int status = 1;
        private int waitingToBeBatched = 0;

        public MessageParallelizer(BatchMonitor monitor, ServerManager manager)
        {
            this.monitor = monitor;
            monitor.Listener = this;
            this.manager = manager;
        }

        private void BatchMessage()
        {
            lock (this)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.BatchMessageImpl));
            }
        }

        private void BatchMessageImpl(object stateInfo)
        {
            MessageManager message = null;
            MessageParallelizer CS$2$0000;
            Monitor.Enter(CS$2$0000 = this);
            try
            {
                message = this.queue.Dequeue();
            }
            catch (InvalidOperationException)
            {
            }
            finally
            {
                Monitor.Exit(CS$2$0000);
            }
            if (message != null)
            {
                try
                {
                    this.manager.SendMessage(message, message.Prog);
                }
                catch (PhaseException)
                {
                }
                catch (PushConnException)
                {
                }
                catch (PushServerException)
                {
                }
                catch (PushUserException)
                {
                }
                catch (SubscrException)
                {
                }
                finally
                {
                    this.OnProcessed();
                }
            }
        }

        internal void EnqueueMessage(MessageManager message, int prog)
        {
            lock (this)
            {
                if (this.status == 1)
                {
                    this.status = 2;
                }
                this.queue.Enqueue(message);
                this.waitingToBeBatched++;
                if (this.waitingToBeBatched == 1)
                {
                    this.BatchMessage();
                }
            }
        }

        public void OnMessageBatched()
        {
            lock (this)
            {
                this.waitingToBeBatched--;
                this.batched++;
                this.BatchMessage();
                if (this.status == 2)
                {
                    this.PrepareCloseBatch();
                    this.status = 3;
                }
            }
        }

        private void OnProcessed()
        {
            lock (this)
            {
                this.batched--;
                if ((this.batched == 0) && (this.waitingToBeBatched == 0))
                {
                    this.status = 1;
                }
                else if (this.batched > 0)
                {
                    this.PrepareCloseBatch();
                    this.status = 3;
                }
                else
                {
                    this.status = 2;
                }
            }
        }

        private void PrepareCloseBatch()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.PrepareCloseBatchImpl));
        }

        private void PrepareCloseBatchImpl(object stateInfo)
        {
            lock (this.monitor)
            {
                if (!this.monitor.Empty)
                {
                    this.manager.CloseMessageBatch();
                    try
                    {
                        this.manager.BatchMessageRequests(0);
                    }
                    catch (PhaseException)
                    {
                    }
                }
            }
        }
    }
}

