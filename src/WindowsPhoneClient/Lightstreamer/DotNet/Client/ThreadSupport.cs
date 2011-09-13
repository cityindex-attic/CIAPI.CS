namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Threading;

    internal class ThreadSupport : IThreadRunnable
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

        public virtual void Run()
        {
        }

        public virtual void Start(bool isBackground)
        {
            this.threadField.IsBackground = isBackground;
            this.threadField.Start();
        }

        public override string ToString()
        {
            return ("Thread[" + this.Name + ",]");
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
    }
}

