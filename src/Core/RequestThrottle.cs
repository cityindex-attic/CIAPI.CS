//  
//  Project: SOAPI
//  http://soapics.codeplex.com
//  http://stackapps.com/questions/386
//  
//  Copyright 2010, Sky Sanders
//  Licensed under the GPL Version 2 license.
//  http://soapics.codeplex.com/license
//  
//  Date: Aug 08 2010 
//  API ver 1.0 rev 2010.0709.04
//  

#region
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using CIAPI.Core;
using log4net;

#if !SILVERLIGHT

#endif
#endregion

namespace Soapi.Net
{
    /// <summary>
    ///   This is a fully configurabel, thread-safe web request throttle that fully complies with the 
    ///   published usage guidelines. In addition to compliance with the letter of the law, testing
    ///   has exposed further limitations that are compensated. See code comments for more detail.
    /// 
    ///   Simply route all WebRequest.Create calls through RequestThrottle.Instance.Create();
    /// 
    ///   Upon completion of an issued request, regardless of status, you must call 
    ///   RequestThrottle.Instance.Complete() to decrement the outstanding request count.
    /// 
    ///   NOTE: you can use this as a global throttle using WebRequest.RegisterPrefix
    ///   http://msdn.microsoft.com/en-us/library/system.net.webrequest.registerprefix.aspx
    ///   but this is not a viable option for silverlight so in Soapi, where requests
    ///   are created in one place, we just call it explicitly.
    /// </summary>
    /// <remarks>
    /// Throttling conversation here: http://stackapps.com/questions/1143/request-throttling-limits
    /// </remarks>
    public sealed class RequestThrottle : IRequestThrottle // : IWebRequestCreate 
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestThrottle));


        

        #region Fields

        private int _outstandingRequests;

        private readonly Queue<DateTimeOffset> _requestTimes = new Queue<DateTimeOffset>();
        private int _dispatchedCount;
        public int DispatchedCount
        {
            get
            {
                return _dispatchedCount;
            }

        }
        #endregion

        #region Constructors

        private RequestThrottle()
        {
            ThrottleWindowTime = new TimeSpan(0, 0, 0, 7);
            ThrottleWindowCount = 30;
            MaxPendingRequests = 10;
        }

        #endregion

        #region Properties

        ///<summary>
        ///</summary>
        public static RequestThrottle Instance
        {
            get { return Nested.instance; }
        }


        private int _maxPendingRequests;

        /// <summary>
        ///   The maximum number of allowed pending request.
        /// 
        ///   The throttle window will keep us in compliance with the 
        ///   letter of the law, but testing has shown that a large 
        ///   number of outstanding requests result in a cascade of 
        ///   (500) errors that does not stop. 
        /// 
        ///   So we will block while there are > MaxPendingRequests 
        ///   regardless of throttle window.
        /// 
        ///   Defaults to 15 which has proven to be reliable.
        /// </summary>
        public int MaxPendingRequests
        {
            get { return _maxPendingRequests; }
            set
            {
                _maxPendingRequests = value;

                Log.Debug("MaxPendingRequests: " + value);

            }
        }

        /// <summary>
        ///   If you are interested in monitoring
        /// </summary>
        public int OutstandingRequests
        {
            get { return _outstandingRequests; }
        }

        private int _throttleWindowCount;

        /// <summary>
        ///   The quantitive portion (xxx) of the of 30 requests per 5 seconds
        ///   Defaults to published guidelines of 5 seconds
        /// </summary>
        public int ThrottleWindowCount
        {
            get { return _throttleWindowCount; }
            set
            {
                _throttleWindowCount = value;

                Log.Debug("ThrottleCount: " + value);

            }
        }

        private TimeSpan _throttleWindowTime;

        /// <summary>
        ///   The temporal portion (yyy) of the of 30 requests per 5 seconds
        ///   Defaults to the published guidelines of 30
        /// </summary>
        public TimeSpan ThrottleWindowTime
        {
            get { return _throttleWindowTime; }
            set
            {

                _throttleWindowTime = value;

                Log.Debug("ThrottleWindowTime: " + value);


            }
        }

        public IRequestFactory RequestFactory { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   This decrements the outstanding request count.
        /// 
        ///   This MUST MUST MUST be called when a request has 
        ///   completed regardless of status.
        /// 
        ///   If a request fails, it may be wise to delay calling 
        ///   this, e.g. cool down, for a few seconds, before 
        ///   reissuing the request.
        /// </summary>
        public void Complete()
        {
            _outstandingRequests--;
        }

        /// <summary>
        ///   Create a WebRequest. This method will block if too many
        ///   outstanding requests are pending or the throttle window
        ///   threshold has been reached.
        /// </summary>
        /// <param name = "uri"></param>
        /// <returns></returns>
        public WebRequest Create(Uri uri)
        {
            lock (typeof(ThrottleLock))
            {
                // note: we could use a list of WeakReferences and 
                // may do so at a later date, but for now, this
                // works just fine as long as you call .Complete
                _outstandingRequests++;
                bool notifiedMaxPending = false;
                while (_outstandingRequests > MaxPendingRequests)
                {

                    if (!notifiedMaxPending)
                    {

                        Log.Debug(string.Format("Waiting: pending requests {0}", _outstandingRequests));

                    }
                    using (var throttleGate = new AutoResetEvent(false))
                    {
                        throttleGate.WaitOne(100);
                        throttleGate.Close();
                    }
                    notifiedMaxPending = true;
                }

                if (_requestTimes.Count == ThrottleWindowCount)
                {
                    // pull the earliest request of the bottom
                    DateTimeOffset tail = _requestTimes.Dequeue();
                    // calculate the interval between now (head) and tail
                    // to determine if we need to chill out for a few millisecons

                    TimeSpan waitTime = (ThrottleWindowTime - (DateTimeOffset.UtcNow - tail));

                    if (waitTime.TotalMilliseconds > 0)
                    {

                        Log.Debug(string.Format("Waiting: " + waitTime + " to send " + uri.AbsoluteUri));

                        using (var throttleGate = new AutoResetEvent(false))
                        {
                            throttleGate.WaitOne(waitTime);
                            throttleGate.Close();
                        }
                    }
                }

                // good to go. 
                _requestTimes.Enqueue(DateTimeOffset.UtcNow);
                _dispatchedCount += 1;

                Log.Debug(string.Format("Dispatched #{0} : {1} ", _dispatchedCount, uri.AbsoluteUri));




                return WebRequest.Create(uri);
            }
        }

        /// <summary>
        ///   Create a WebRequest. This method will block if too many
        ///   outstanding requests are pending or the throttle window
        ///   threshold has been reached.
        /// </summary>
        /// <param name = "url"></param>
        /// <returns></returns>
        public WebRequest Create(string url)
        {
            return Create(new Uri(url));
        }

        #endregion


        /// <summary>
        ///   lock handle
        /// </summary>
        private class ThrottleLock
        {
        }

        #region Singleton Plubming

        // the skeet singleton implementation
        // http://www.yoda.arachsys.com/csharp/singleton.html

        // ReSharper disable ClassNeverInstantiated.Local
        class Nested
        // ReSharper restore ClassNeverInstantiated.Local
        {

            // ReSharper disable EmptyConstructor
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            // ReSharper restore EmptyConstructor
            {
            }

            // ReSharper disable InconsistentNaming
            internal static readonly RequestThrottle instance = new RequestThrottle();
            // ReSharper restore InconsistentNaming
        }

        #endregion


    }
}
