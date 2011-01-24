namespace Lightstreamer.DotNet.Client
{
    using Common.Logging;
    using System;

    internal class MyServerListener : ServerManager.IServerListener
    {
        private static ILog actionsLogger = LogManager.GetLogger("com.lightstreamer.ls_client.actions");
        private int currPhase;
        private bool failed = false;
        private IConnectionListener initialListener;
        private LSClient owner;
        private NotificationQueue queue = new NotificationQueue("Notifications queue", false);

        internal MyServerListener(LSClient owner, IConnectionListener initialListener, int currPhase)
        {
            this.owner = owner;
            this.initialListener = initialListener;
            this.currPhase = currPhase;
            this.queue.Start();
        }

        public virtual bool OnActivityWarning(bool warningOn)
        {
            NotificationQueue.Notify fun = null;
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    if (fun == null)
                    {
                        fun = delegate {
                            activeListener.OnActivityWarning(warningOn);
                        };
                    }
                    this.queue.Add(fun);
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
            NotificationQueue.Notify fun = null;
            lock (this)
            {
                this.failed = true;
                if (closedListener != null)
                {
                    if (fun == null)
                    {
                        fun = delegate {
                            closedListener.OnClose();
                        };
                    }
                    this.queue.Add(fun);
                }
                this.queue.End();
            }
        }

        public virtual void OnConnectException(Exception e)
        {
            NotificationQueue.Notify fun = null;
            if (this.initialListener is ExtConnectionListener)
            {
                if (fun == null)
                {
                    fun = delegate {
                        ((ExtConnectionListener) this.initialListener).OnConnectException(e);
                    };
                }
                this.queue.Add(fun);
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
            NotificationQueue.Notify fun = null;
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    if (fun == null)
                    {
                        fun = delegate {
                            activeListener.OnDataError(e);
                        };
                    }
                    this.queue.Add(fun);
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnEnd(int cause)
        {
            NotificationQueue.Notify fun = null;
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    if (fun == null)
                    {
                        fun = delegate {
                            activeListener.OnEnd(cause);
                        };
                    }
                    this.queue.Add(fun);
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnFailure(PushConnException e)
        {
            NotificationQueue.Notify fun = null;
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    if (fun == null)
                    {
                        fun = delegate {
                            activeListener.OnFailure(e);
                        };
                    }
                    this.queue.Add(fun);
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnFailure(PushServerException e)
        {
            NotificationQueue.Notify fun = null;
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    this.failed = true;
                    if (fun == null)
                    {
                        fun = delegate {
                            activeListener.OnFailure(e);
                        };
                    }
                    this.queue.Add(fun);
                    return true;
                }
                return false;
            }
        }

        public virtual bool OnNewBytes(long bytes)
        {
            NotificationQueue.Notify fun = null;
            IConnectionListener activeListener = this.owner.GetActiveListener(this.currPhase);
            lock (this)
            {
                if ((activeListener != null) && !this.failed)
                {
                    if (fun == null)
                    {
                        fun = delegate {
                            activeListener.OnNewBytes(bytes);
                        };
                    }
                    this.queue.Add(fun);
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
                    NotificationQueue.Notify fun = null;
                    NotificationQueue.Notify notify2 = null;
                    this.failed = true;
                    PushServerException exc = new PushServerException(11);
                    if (activeListener is ExtConnectionListener)
                    {
                        if (fun == null)
                        {
                            fun = delegate {
                                ((ExtConnectionListener) activeListener).OnConnectTimeout(exc);
                            };
                        }
                        this.queue.Add(fun);
                    }
                    else
                    {
                        if (notify2 == null)
                        {
                            notify2 = delegate {
                                activeListener.OnFailure(exc);
                            };
                        }
                        this.queue.Add(notify2);
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
            NotificationQueue.Notify fun = null;
            if (this.initialListener is ExtConnectionListener)
            {
                if (fun == null)
                {
                    fun = delegate {
                        ((ExtConnectionListener) this.initialListener).OnStreamingReturned();
                    };
                }
                this.queue.Add(fun);
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
                catch (PushServerException exception)
                {
                    actionsLogger.Debug("Error in received values", exception);
                    this.OnDataError(exception);
                }
                return true;
            }
            return false;
        }
    }
}

