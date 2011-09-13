namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;

    internal class MyServerListener : ServerManager.IServerListener
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private int currPhase;
        private bool failed = false;
        private IConnectionListener initialListener;
        private NotificationQueue messageQueue = new NotificationQueue("Messages notifications queue", false);
        private LSClient owner;
        private NotificationQueue queue = new NotificationQueue("Notifications queue", false);

        internal MyServerListener(LSClient owner, IConnectionListener initialListener, int currPhase)
        {
            this.owner = owner;
            this.initialListener = initialListener;
            this.currPhase = currPhase;
            this.queue.Start();
            this.messageQueue.Start();
        }

        public virtual bool OnActivityWarning(bool warningOn)
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.queue.Add(delegate {
                        activeListener.OnActivityWarning(warningOn);
                    });
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnClose()
        {
            if (this.owner.GetActiveListener(this.currPhase) != null)
            {
                this.owner.CloseConnection();
                return true;
            }
            return false;
        }

        public virtual void OnClosed(IConnectionListener closedListener)
        {
            lock (this)
            {
                this.failed = true;
                if (closedListener != null)
                {
                    this.queue.Add(delegate {
                        closedListener.OnClose();
                    });
                }
                this.queue.End();
            }
        }

        public virtual void OnConnectException(Exception e)
        {
            if (this.initialListener is ExtConnectionListener)
            {
                this.queue.Add(delegate {
                    ((ExtConnectionListener) this.initialListener).OnConnectException(e);
                });
            }
        }

        public virtual void OnConnectionEstablished()
        {
            this.queue.Add(delegate {
                this.initialListener.OnConnectionEstablished();
            });
        }

        public virtual void OnConnectTimeout()
        {
            if (this.initialListener is ExtConnectionListener)
            {
                PushServerException exc = new PushServerException(11);
                this.queue.Add(delegate {
                    ((ExtConnectionListener) this.initialListener).OnConnectTimeout(exc);
                });
            }
        }

        public virtual bool OnDataError(PushServerException e)
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.queue.Add(delegate {
                        activeListener.OnDataError(e);
                    });
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnEnd(int cause)
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    this.queue.Add(delegate {
                        activeListener.OnEnd(cause);
                    });
                    return true;
                }
                return false;
            }
        }

        public void OnEndMessages()
        {
            this.messageQueue.End();
        }

        public virtual bool OnFailure(PushConnException e)
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    this.queue.Add(delegate {
                        activeListener.OnFailure(e);
                    });
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnFailure(PushServerException e)
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    this.queue.Add(delegate {
                        activeListener.OnFailure(e);
                    });
                    return true;
                }
                return false;
            }
        }

        public bool OnMessageOutcome(MessageManager message, SequenceHandler sequence, Lightstreamer.DotNet.Client.ServerUpdateEvent values, Exception problem)
        {
            bool executed;
            if (values == null)
            {
                executed = message.SetAbort(problem);
            }
            else
            {
                executed = message.SetOutcome(values);
            }
            if (!executed)
            {
                this.OnDataError(new PushServerException(13));
                return false;
            }
            if (message.Sequence == "UNORDERED_MESSAGES")
            {
                int num = message.Prog;
                this.messageQueue.Add(delegate {
                    MessageManager mex = sequence.IfHasOutcomeExtractIt(num);
                    if (mex != null)
                    {
                        try
                        {
                            mex.NotifyListener();
                        }
                        catch (PushServerException e)
                        {
                            this.OnDataError(e);
                        }
                        catch (Exception)
                        {
                        }
                    }
                });
            }
            else
            {
                this.messageQueue.Add(delegate {
                    MessageManager mex;
                    while ((mex = sequence.IfFirstHasOutcomeExtractIt()) != null)
                    {
                        try
                        {
                            mex.NotifyListener();
                        }
                        catch (PushServerException pse)
                        {
                            this.OnDataError(pse);
                        }
                        catch (Exception)
                        {
                        }
                    }
                });
            }
            return true;
        }

        public virtual bool OnNewBytes(long bytes)
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.queue.Add(delegate {
                        activeListener.OnNewBytes(bytes);
                    });
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnReconnectTimeout()
        {
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    PushServerException exc = new PushServerException(11);
                    if (activeListener is ExtConnectionListener)
                    {
                        this.queue.Add(delegate {
                            ((ExtConnectionListener) activeListener).OnConnectTimeout(exc);
                        });
                    }
                    else
                    {
                        this.queue.Add(delegate {
                            activeListener.OnFailure(exc);
                        });
                    }
                    return true;
                }
                return false;
            }
        }

        public virtual void OnSessionStarted(bool isPolling)
        {
            this.queue.Add(delegate {
                this.initialListener.OnSessionStarted(isPolling);
            });
        }

        public virtual void OnStreamingReturned()
        {
            if (this.initialListener is ExtConnectionListener)
            {
                this.queue.Add(delegate {
                    ((ExtConnectionListener) this.initialListener).OnStreamingReturned();
                });
            }
        }

        public virtual bool OnUpdate(ITableManager table, Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            if (this.owner.GetActiveListener(this.currPhase) != null)
            {
                try
                {
                    table.DoUpdate(values);
                }
                catch (PushServerException e)
                {
                    actionsLogger.Debug("Error in received values", e);
                    this.OnDataError(e);
                }
                return true;
            }
            return false;
        }
    }
}

