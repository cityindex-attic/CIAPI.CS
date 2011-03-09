﻿using System.Collections.Generic;

namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections;
    using System.Threading;

    internal class TimerSupport
    {
        private IList _timers = new List<Timer>();

        private void OnTimedEvent(object info)
        {
            TimerContext context = (TimerContext) info;
            lock (this)
            {
                this._timers.Remove(context.timer);
            }
            context.task.Run();
            context.timer.Dispose();
        }

        public void Schedule(IThreadRunnable task, long delay)
        {
            TimerCallback callback = new TimerCallback(this.OnTimedEvent);
            TimerContext state = new TimerContext {
                task = task
            };
            Timer timer = new Timer(callback, state, -1, 0);
            state.timer = timer;
            lock (this)
            {
                this._timers.Add(timer);
            }
            timer.Change((delay < 1L) ? 1L : delay, 0L);
        }

        private class TimerContext
        {
            public IThreadRunnable task;
            public Timer timer;
        }
    }
}

