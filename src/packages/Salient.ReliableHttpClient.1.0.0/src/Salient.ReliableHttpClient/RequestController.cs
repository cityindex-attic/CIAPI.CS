using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient
{
    /// <summary>
    /// RequestController will encapsulate threadsafe caching and throttling
    /// items with a cacheduration of 0 will not be cached
    /// items flagged as unique
    /// </summary>
    public class RequestController : IDisposable
    {
        private bool _disposed;
        public event EventHandler<RequestCompletedEventArgs> RequestCompleted;

        public virtual void OnRequestCompleted(RequestInfoBase info)
        {
            if(_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            var e = new RequestCompletedEventArgs(info);
            EventHandler<RequestCompletedEventArgs> handler = RequestCompleted;
            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch
                {

                    Log.Error("Error in request completion handler\r\n" + e.Info);
                }
            }
        }

        public bool IncludeIndexInHeaders { get; set; }
        private const int BackgroundInterval = 50;
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestController));
        private readonly Thread _backgroundThread;


        private readonly object _lockTarget = new object();

        private readonly int _maxPendingRequests = 10;
        private readonly List<RequestInfo> _requestCache;
        private readonly IRequestFactory _requestFactory;
        private readonly Queue<RequestInfo> _requestQueue = new Queue<RequestInfo>();
        private readonly Queue<DateTimeOffset> _requestTimes = new Queue<DateTimeOffset>();
        private readonly int _throttleWindowCount = 30;
        private readonly TimeSpan _throttleWindowTime = TimeSpan.FromSeconds(10);

        private readonly AutoResetEvent _waitHandle;
        private int _dispatchedCount;

        private volatile bool _disposing;


        private bool _notifiedWaitingOnMaxPending;
        private bool _notifiedWaitingOnWindow;
        private int _outstandingRequests;
        private bool _processingQueue;
        private IJsonSerializer _serializer;

        public RequestController(IJsonSerializer serializer,
                                 IRequestFactory requestFactory)
            : this(serializer)
        {
            _requestFactory = requestFactory;
        }


        public RequestController(IJsonSerializer serializer)
        {
            _serializer = serializer;
            //Recorder = new Recorder(_serializer);
            Id = Guid.NewGuid();
            Log.Debug("creating RequestController: " + Id);
            _requestFactory = new RequestFactory();
            _requestCache = new List<RequestInfo>();
            _requestQueue = new Queue<RequestInfo>();
            _waitHandle = new AutoResetEvent(false);
            _backgroundThread = new Thread(BackgroundProcess);
            _backgroundThread.Start();
            Log.Debug("created RequestController: " + Id);
        }

        //public Recorder Recorder { get; set; }
        public string UserAgent { get; set; }
        public Guid Id { get; private set; }

        private void BackgroundProcess(object ignored)
        {
            Log.Debug("RequestController  " + Id + " background process created");


            // NOTE: this is an intentional closed background processing loop
            // it will finish when the controller is disposed.
            while (true)
            {
                lock (_lockTarget)
                {
                    // passive shut down of thread to avoid spurious ThreadAbortException from
                    // popping up in arbitrary places as is wont to happen when just killing a thread.
                    if (_disposing)
                    {
                        Log.Debug("RequestController  " + Id + " shutting down");
                        return;
                    }

                    // TODO: how/why/should we surface exceptions?
                    // Any exceptions leaking from this point should be critical
                    // unhandled exceptions. Most exceptions will just be passed to the
                    // async completion callbacks.

                    PurgeExpiredItems();


                    ProcessQueue();
                }
                _waitHandle.WaitOne(BackgroundInterval);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void ProcessQueue()
        {
            lock (_lockTarget)
            {
                if (_processingQueue) return;
                // should i be locking on the queue? (NOTE: concurrent libs cannot be used due to SL platform)
                if (_requestQueue.Count == 0) return;

                _processingQueue = true;


                RequestInfo request = _requestQueue.Peek();


                try
                {
                    if (ThereAreMoreOutstandingRequestsThanIsAllowed()) return;

                    if (_requestTimes.Count > _throttleWindowCount)
                    {
                        throw new ReliableHttpException("request time queue got to be longer than window somehow");
                    }

                    if (_requestTimes.Count == _throttleWindowCount)
                    {
                        DateTimeOffset head = _requestTimes.Peek();
                        TimeSpan waitTime = (_throttleWindowTime - (DateTimeOffset.UtcNow - head));

                        if (waitTime.TotalMilliseconds > 0)
                        {
                            if (!_notifiedWaitingOnWindow)
                            {
                                string msgWaiting = string.Format("Waiting: " + waitTime + " to send " + request.Uri);
                                Log.Info(msgWaiting);

                                _notifiedWaitingOnWindow = true;
                            }
                            return;
                        }
                        _requestTimes.Dequeue();
                    }


                    // good to go. 
                    _notifiedWaitingOnWindow = false;

                    _requestTimes.Enqueue(DateTimeOffset.UtcNow);
                    _dispatchedCount += 1;

                    request.Index = _dispatchedCount;
                    if (IncludeIndexInHeaders)
                    {
                        request._headers["x-request-index"] = request.Index.ToString();
                        request.Request.Headers["x-request-index"] = request.Index.ToString();
                    }
                    try
                    {
                        request.Issued = DateTimeOffset.UtcNow;
                        IAsyncResult webRequestAsyncResult = request.Request.BeginGetResponse(ar =>
                            {
                                Log.Info(string.Format("Received #{0} : {1} ", request.Index, request.Uri));

                                // let's try to complete the request
                                _outstandingRequests--;
                                try
                                {
                                    request.CompleteRequest(ar);
                                }
                                catch (Exception ex)
                                {
                                    // the only time an exception will come out of CompleteRequest is if the request wants to be retried
                                    request.AttemptedRetries++;
                                    Log.Warn(string.Format("retrying request {3} {0}\r\nattempt #{1}\r\nerror:{2} \r\nrequest:\r\n{4}",
                                        request.Id, request.AttemptedRetries, ex.Message, request.Index, request.ToString()));
                                    // create a new httprequest for this guy
                                    request.BuildRequest(_requestFactory);
                                    //put it back in the queue. if it belongs in the cache it is already there.
                                    _requestQueue.Enqueue(request);
                                }
                            }, null);


                        //EnsureRequestWillAbortAfterTimeout(request, webRequestAsyncResult);

                        Log.Info(string.Format("Dispatched #{0} : {1} ", request.Index, request.Uri));
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Error dispatching #{0} : {1} \r\n{2} \r\n{3}", request.Index,
                                                       request.Uri, ex.Message, request.ToString());
                        Log.Error(message);
                        ReliableHttpException ex2 = ReliableHttpException.Create(message, ex);
                        throw ex2;
                    }
                    finally
                    {
                        _requestQueue.Dequeue();
                        _outstandingRequests++;
                        // TODO: should this really be here if there was an error that prevented dispatch?
                    }
                }
                finally
                {
                    _processingQueue = false;
                }
                //Log.DebugFormat("exiting lock for ProcessQueue()");
            }
            //Log.DebugFormat("outside lock for ProcessQueue()");
        }

        private bool ThereAreMoreOutstandingRequestsThanIsAllowed()
        {
            if (_outstandingRequests > _maxPendingRequests)
            {
                if (!_notifiedWaitingOnMaxPending)
                {
                    string msgMaxPending = string.Format("Waiting: pending requests {0}", _outstandingRequests);
                    Log.Info(msgMaxPending);

                    _notifiedWaitingOnMaxPending = true;
                }

                return true;
            }

            _notifiedWaitingOnMaxPending = false;
            return false;
        }

        //        private static void EnsureRequestWillAbortAfterTimeout(RequestInfo request, IAsyncResult result)
        //        {
        //            //TODO: How can we timeout a request for Silverlight, when calls to AsyncWaitHandle throw the following:
        //            //   Specified method is not supported. at System.Net.Browser.OHWRAsyncResult.get_AsyncWaitHandle() 

        //            // DAVID: i don't think that the async methods have a timeout parameter. we will need to build one into 
        //            // it. will not be terribly clean as it will prolly have to span both the throttle and the cache. I will look into it


        //#if !SILVERLIGHT
        //            ThreadPool.RegisterWaitForSingleObject(
        //                waitObject: result.AsyncWaitHandle,
        //                callBack: (state, isTimedOut) =>
        //                              {
        //                                  if (!isTimedOut) return;
        //                                  if (state.GetType() != typeof(RequestInfo)) return;

        //                                  var rh = (RequestInfo)state;
        //                                  Log.Error(string.Format("Aborting #{0} : {1} because it has exceeded timeout {2}",
        //                                                          rh.Index, rh.Request.RequestUri, rh.Request.Timeout));
        //                                  rh.Request.Abort();
        //                              },
        //                state: request,
        //                timeout: TimeSpan.FromMilliseconds(request.Request.Timeout),
        //                executeOnlyOnce: true);
        //#endif
        //        }

        private void PurgeExpiredItems()
        {
            lock (_lockTarget)
            {
                var toRemove = new List<RequestInfo>();

                // ReSharper disable LoopCanBeConvertedToQuery
                foreach (RequestInfo item in _requestCache)
                // ReSharper restore LoopCanBeConvertedToQuery
                {
                    if (item.CacheExpiration <= DateTimeOffset.UtcNow &&
                        item.State == RequestItemState.Complete)
                    {
                        toRemove.Add(item);
                    }
                }

                foreach (RequestInfo item in toRemove)
                {
                    try
                    {
                        _requestCache.Remove(item);
                        Log.Info(string.Format("Removed {1} {0} from cache", item.Uri, item.Index));
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(string.Format("Unable to remove item {1} {0} from cache", item.Uri, item.Index), ex);
                        // swallow
                    }
                }
            }
        }

        private RequestInfo GetRequestInfo(Uri uri)
        {
            lock (_lockTarget)
            {
                RequestInfo info = _requestCache.FirstOrDefault(r => r.Uri.AbsoluteUri == uri.AbsoluteUri);
                if (info != null)
                {
                    switch (info.State)
                    {
                        case RequestItemState.Processing:
                            // processing of this item's callbacks has already commenced. it is not viable
                            return null;
                        case RequestItemState.New:
                        case RequestItemState.Preparing:
                            throw new DefectException(
                                "this class is supposed to be threadsafe, an item should never be available as New or Preparing");

                        case RequestItemState.Ready:
                        case RequestItemState.Pending:
                            // this item is eligible for adding a callback
                            return info;

                        case RequestItemState.Complete:
                            // this item is eligible for adding a callback
                            return info;
                        default:
                            throw new DefectException(
                                "a field has been added to the RequestItemState enum that we are not handling.");
                    }
                }
                return null;
            }
        }


        private RequestInfo CreateRequest(Uri uri, RequestMethod method, string body, Dictionary<string, string> headers,
                                          ContentType requestContentType, ContentType responseContentType,
                                          TimeSpan cacheDuration, int timeout, string target, string uriTemplate,
                                          int retryCount, Dictionary<string, object> parameters,
                                          ReliableAsyncCallback callback, object state)
        {
            RequestInfo info;


            info = RequestInfo.Create(method, target, uriTemplate, parameters, UserAgent, headers,
                                                  requestContentType, responseContentType, cacheDuration, timeout,
                                                  retryCount, uri, body, _requestFactory);

            info.BuildRequest(_requestFactory);
            info.AddCallback(ar =>
            {

                var info2 = (RequestInfo)ar.AsyncState;
                RequestInfoBase copy = info2.Copy();
                OnRequestCompleted(copy);
            }, info);


            info.ProcessingComplete += CompleteRequest;
            if (callback != null)
            {
                info.AddCallback(callback, state);
            }
            return info;
        }

        private void CompleteRequest(object sender, EventArgs e)
        {
            var info = (RequestInfo)sender;
            info.ProcessingComplete -= CompleteRequest;
            //RequestInfoBase copy = info.Copy();
            //Recorder.AddRequest(copy);
        }

  public virtual string EndRequest(ReliableAsyncResult result)
  {
      if (_disposed)
      {
          throw new ObjectDisposedException(GetType().FullName);
      }
      try
      {

          result.End();
          return result.ResponseText;
      }
#pragma warning disable 168
      catch (ReliableHttpException ex)
#pragma warning restore 168
      {
          // this throw is simply to isolate defects in request implementations
          throw;
      }
      catch (Exception ex)
      {
          throw new DefectException("expecting only ReliableHttpException here. See inner for details", ex);
      }
  }
        public Guid BeginRequest(Uri uri, RequestMethod method, string body, Dictionary<string, string> headers,
                                 ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration,
                                 int timeout, string target, string uriTemplate, int retryCount,
                                 Dictionary<string, object> parameters, ReliableAsyncCallback callback, object state)
        {
            lock (_lockTarget)
            {
                RequestInfo info = null;

                if (cacheDuration == TimeSpan.Zero || (method == RequestMethod.PUT) || (method == RequestMethod.POST))
                {
                    // this item should not enter the cache
                    info = CreateRequest(uri, method, body, headers, requestContentType, responseContentType,
                                         cacheDuration, timeout, target, uriTemplate, retryCount, parameters, callback,
                                         state);

                    info.State = RequestItemState.Ready;
                    _requestQueue.Enqueue(info);
                }
                else
                {
                    // this item can enter the cache
                    if (!string.IsNullOrEmpty(body))
                    {
                        throw new Exception(
                            "a request with body cannot be cached. body is supported on PUT and POST only");
                    }

                    info = GetRequestInfo(uri);

                    if (info == null)
                    {
                        info = CreateRequest(uri, method, body, headers, requestContentType, responseContentType,
                                             cacheDuration, timeout, target, uriTemplate, retryCount, parameters,
                                             callback, state);

                        info.State = RequestItemState.Ready;
                        _requestCache.Add(info);
                        _requestQueue.Enqueue(info);
                        Log.Info(string.Format("Added {1} {0} to cache", info.Uri, info.Index));
                    }
                    else
                    {
                        if (info.State == RequestItemState.Complete)
                        {
                            // #FIXME: this should be elsewhere?
                            new ReliableAsyncResult(callback, state, true, info.ResponseText, info.Exception);
                        }
                        else
                        {
                            Log.Info(string.Format("Added callback to {1} {0}", info.Uri, info.Index));
                            info.AddCallback(callback, state);
                        }
                    }
                }
                return info.Id;
            }
        }



        #region IDisposable Members

        public void Dispose()
        {
            _disposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposing = true;
                
                while (_backgroundThread.IsAlive)
                {
                    Thread.Sleep(100);
                }
                if(_waitHandle!=null)
                {
                    ((IDisposable)_waitHandle).Dispose();
                }
                
            }



        }
        #endregion

    }
}
