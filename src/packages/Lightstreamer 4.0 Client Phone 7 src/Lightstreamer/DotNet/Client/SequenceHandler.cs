namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections.Generic;

    internal class SequenceHandler
    {
        private Dictionary<int, MessageManager> messages = new Dictionary<int, MessageManager>();
        private int peek = 1;
        private int prog = 0;

        public int Enqueue(MessageManager message)
        {
            lock (this)
            {
                this.prog++;
                this.messages[this.prog] = message;
                message.Enqueued(this.prog);
                return this.prog;
            }
        }

        public MessageManager GetMessage(int prog)
        {
            lock (this)
            {
                if ((this.messages.Count > 0) && this.messages.ContainsKey(prog))
                {
                    return this.messages[prog];
                }
                return null;
            }
        }

        public MessageManager IfFirstHasOutcomeExtractIt()
        {
            lock (this)
            {
                MessageManager peekEl = this.IfHasOutcomeExtractIt(this.peek);
                if (peekEl == null)
                {
                    return null;
                }
                this.peek++;
                return peekEl;
            }
        }

        public MessageManager IfHasOutcomeExtractIt(int num)
        {
            lock (this)
            {
                MessageManager mex = this.GetMessage(num);
                if (mex == null)
                {
                    return null;
                }
                if (!mex.HasOutcome())
                {
                    return null;
                }
                this.messages.Remove(num);
                return mex;
            }
        }

        public MessageManager[] Iterator()
        {
            lock (this)
            {
                MessageManager[] ret = new MessageManager[this.messages.Count];
                this.messages.Values.CopyTo(ret, 0);
                return ret;
            }
        }
    }
}

