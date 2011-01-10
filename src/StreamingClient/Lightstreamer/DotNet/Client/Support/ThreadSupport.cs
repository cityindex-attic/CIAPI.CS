namespace Lightstreamer.DotNet.Client.Support
{
    using System;
    using System.Threading;

    public class ThreadSupport : IThreadRunnable
    {
        private Thread threadField;

        public ThreadSupport()
        {
            this.threadField = new Thread(new ThreadStart(this.Run));
        }

        public ThreadSupport(string Name)
        {
            this.threadField = new Thread(new ThreadStart(this.Run));
            this.Name = Name;
        }

        public ThreadSupport(ThreadStart Start)
        {
            this.threadField = new Thread(Start);
        }

        public ThreadSupport(ThreadStart Start, string Name)
        {
            this.threadField = new Thread(Start);
            this.Name = Name;
        }

        public void Abort()
        {
            this.threadField.Abort();
        }

        public void Abort(object stateInfo)
        {
            lock (this)
            {
                this.threadField.Abort(stateInfo);
            }
        }

        public static ThreadSupport Current()
        {
            return new ThreadSupport { Instance = Thread.CurrentThread };
        }

        public virtual void Interrupt()
        {
            this.threadField.Interrupt();
        }

        public void Join()
        {
            this.threadField.Join();
        }

        public void Join(long MilliSeconds)
        {
            lock (this)
            {
                this.threadField.Join(new TimeSpan(MilliSeconds * 0x2710L));
            }
        }

        public void Join(long MilliSeconds, int NanoSeconds)
        {
            lock (this)
            {
                this.threadField.Join(new TimeSpan((MilliSeconds * 0x2710L) + (NanoSeconds * 100)));
            }
        }

        public virtual void Run()
        {
        }

        public virtual void Start()
        {
            this.threadField.Start();
        }

        public override string ToString()
        {
            return ("Thread[" + this.Name + "," + this.Priority.ToString() + ",]");
        }

        public Thread Instance
        {
            get
            {
                return this.threadField;
            }
            set
            {
                this.threadField = value;
            }
        }

        public bool IsAlive
        {
            get
            {
                return this.threadField.IsAlive;
            }
        }

        public bool IsBackground
        {
            get
            {
                return this.threadField.IsBackground;
            }
            set
            {
                this.threadField.IsBackground = value;
            }
        }

        public string Name
        {
            get
            {
                return this.threadField.Name;
            }
            set
            {
                if (this.threadField.Name == null)
                {
                    this.threadField.Name = value;
                }
            }
        }

        public ThreadPriority Priority
        {
            get
            {
                return this.threadField.Priority;
            }
            set
            {
                this.threadField.Priority = value;
            }
        }
    }
}

