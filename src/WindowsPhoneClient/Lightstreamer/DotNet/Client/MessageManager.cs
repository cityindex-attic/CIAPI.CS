namespace Lightstreamer.DotNet.Client
{
    using System;

    internal class MessageManager
    {
        private bool aborted = false;
        private ISendMessageListener listener;
        private MessageInfo message;
        private Exception problem = null;
        private Lightstreamer.DotNet.Client.ServerUpdateEvent processed = null;
        private int prog = 0;

        internal MessageManager(MessageInfo message, ISendMessageListener listener)
        {
            this.message = message;
            this.listener = listener;
        }

        internal void Enqueued(int prog)
        {
            lock (this)
            {
                this.prog = prog;
            }
        }

        public bool HasOutcome()
        {
            lock (this)
            {
                if (!(this.aborted || (this.processed != null)))
                {
                    return false;
                }
                return true;
            }
        }

        public void NotifyListener()
        {
            lock (this)
            {
                if (!this.HasOutcome())
                {
                    throw new PushServerException(13);
                }
                if (this.listener != null)
                {
                    if (this.aborted)
                    {
                        this.listener.OnAbort(this.message, this.prog, this.problem);
                    }
                    else if (this.processed != null)
                    {
                        if (this.processed.ErrorMessage == null)
                        {
                            this.listener.OnProcessed(this.message, this.prog);
                        }
                        else
                        {
                            this.listener.OnError(this.processed.ErrorCode, this.processed.ErrorMessage, this.message, this.prog);
                        }
                    }
                }
            }
        }

        public bool SetAbort(Exception problem)
        {
            lock (this)
            {
                if (!this.HasOutcome())
                {
                    this.aborted = true;
                    this.problem = problem;
                    return true;
                }
                return false;
            }
        }

        public bool SetOutcome(Lightstreamer.DotNet.Client.ServerUpdateEvent values)
        {
            lock (this)
            {
                if (!this.HasOutcome())
                {
                    this.processed = values;
                    return true;
                }
                return false;
            }
        }

        internal int DelayTimeout
        {
            get
            {
                lock (this)
                {
                    return this.message.DelayTimeout;
                }
            }
        }

        internal string Message
        {
            get
            {
                lock (this)
                {
                    return this.message.Message;
                }
            }
        }

        internal int Prog
        {
            get
            {
                lock (this)
                {
                    return this.prog;
                }
            }
        }

        internal string Sequence
        {
            get
            {
                lock (this)
                {
                    return this.message.Sequence;
                }
            }
        }
    }
}

