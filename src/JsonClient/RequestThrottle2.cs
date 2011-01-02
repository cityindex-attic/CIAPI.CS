using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace CIAPI.Core
{

    /// <summary>
    /// Throttles requests to a rate of N in any duration D while allowing only O pending requests.
    /// Where N = windowCount, D = windowDuration and O = outstandingRequests
    /// </summary>
    public class RequestThrottle2
    {
        private readonly int _windowCount;
        private readonly TimeSpan _windowDuration;
        private readonly int _outstandingRequests;
        private readonly int _queuePollInterval;
        private int _pendingRequestCount;

        public RequestThrottle2() : this(30, TimeSpan.FromSeconds(5), 10, 100)
        {
        }

        public RequestThrottle2(int windowCount,TimeSpan windowDuration,int outstandingRequests,int queuePollInterval)
        {
            _outstandingRequests = outstandingRequests;
            _windowDuration = windowDuration;
            _windowCount = windowCount;
            _queuePollInterval = queuePollInterval;
            _queue = new Queue<Action>();
            new Timer(ProcessQueue, null, 100, _queuePollInterval);
        }

        private void ProcessQueue(object ignored)
        {

        }

        private readonly Queue<Action> _queue;

        public void Enqueue(Action request)
        {
            _queue.Enqueue(request);
        }

    }

}