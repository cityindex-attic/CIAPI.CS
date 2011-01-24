namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class NotificationQueue
    {
        private ManualResetEvent available = new ManualResetEvent(false);
        private bool closed = false;
        private List<Notify> myList = new List<Notify>();
        private string name;

        public NotificationQueue(string name, bool started)
        {
            this.name = name;
            if (started)
            {
                lock (this)
                {
                    this.Start();
                }
            }
        }

        public void Add(Notify fun)
        {
            lock (this)
            {
                if (!this.closed)
                {
                    this.myList.Add(fun);
                    this.available.Set();
                }
            }
        }

        private void dequeue()
        {
            while (true)
            {
                Notify notify = null;
                lock (this)
                {
                    if (this.myList.Count > 0)
                    {
                        notify = this.myList[0];
                        this.myList.RemoveAt(0);
                    }
                    else
                    {
                        if (this.closed)
                        {
                            return;
                        }
                        this.available.Reset();
                    }
                }
                if (notify != null)
                {
                    try
                    {
                        notify();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    this.available.WaitOne();
                }
            }
        }

        public void End()
        {
            lock (this)
            {
                this.closed = true;
                this.available.Set();
            }
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this.dequeue));
            lock (this)
            {
                thread.Name = this.name;
            }
            thread.IsBackground = true;
            thread.Start();
        }

        public delegate void Notify();
    }
}

