namespace Lightstreamer.DotNet.Client.Support
{
    using System;
    using System.Collections;
    using System.Timers;

    public class TimerSupport
    {
        private ArrayList _timers = new ArrayList();

        private void OnTimedEvent(Timer timer, IThreadRunnable task)
        {
            lock (this)
            {
                this._timers.Remove(timer);
            }
            task.Run();
        }

        public void Schedule(IThreadRunnable task, long delay)
        {
            Timer timer = new Timer();
            TimerTask task2 = new TimerTask(this, timer, task);
            timer.Elapsed += new ElapsedEventHandler(task2.OnTimedEvent);
            timer.AutoReset = false;
            timer.Interval = (delay < 1L) ? ((double) 1L) : ((double) delay);
            lock (this)
            {
                this._timers.Add(timer);
            }
            timer.Start();
        }

        private class TimerTask
        {
            private TimerSupport parent;
            private IThreadRunnable task;
            private Timer timer;

            public TimerTask(TimerSupport parent, Timer timer, IThreadRunnable task)
            {
                this.parent = parent;
                this.timer = timer;
                this.task = task;
            }

            public void OnTimedEvent(object source, ElapsedEventArgs e)
            {
                this.parent.OnTimedEvent(this.timer, this.task);
            }
        }
    }
}

