using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lightstreamer.Adapters.PortfolioDemo
{
    /// <summary>
    /// Used to provide an executor of tasks in a single dedicated thread.
    /// More complete implementations of this service
    /// may be provided by standard tool libraries.
    /// </summary>
    class NotificationQueue
    {
        public delegate void Notify();

        private bool closed = false;

        private List<Notify> myList = new List<Notify>();
        private ManualResetEvent available = new ManualResetEvent(false);

        public void Add(Notify fun)
        {
            lock (this)
            {
                if (!closed)
                {
                    myList.Add(fun);
                    available.Set();
                }
            }
        }

        public void Start()
        {
            Thread t = new Thread(dequeue);
            t.IsBackground = true;
            t.Start();
        }

        public void End()
        {
            lock (this)
            {
                closed = true;
                available.Set();
            }
        }

        void dequeue()
        {
            while (true)
            {
                Notify fun = null;
                lock (this)
                {
                    if (myList.Count > 0)
                    {
                        fun = myList[0];
                        myList.RemoveAt(0);
                    }
                    else if (closed)
                    {
                        return;
                    }
                    else
                    {
                        available.Reset();
                    }
                }
                if (fun != null)
                {
                    try
                    {
                        fun();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    available.WaitOne();
                }
            }
        }
    }
}
