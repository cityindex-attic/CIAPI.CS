using System;
using System.Threading;
#if !SILVERLIGHT
#else
#if !WINDOWS_PHONE
#else
#endif
#endif

namespace Salient.ReliableHttpClient
{
    public enum RequestNotificationStatus
    {
        ///<summary>
        ///</summary>
        Continue,
        ///<summary>
        ///</summary>
        Pending,
        ///<summary>
        ///</summary>
        FinishRequest
    }

    public class ReliableAsyncResult : IAsyncResult
    {
        // Fields
        private readonly object _asyncState;
        private readonly ReliableAsyncCallback _callback;
        private readonly Guid _id;
        private bool _completed;
        private bool _completedSynchronously;
        private ReliableHttpException _error;
        public string ResponseText { get; private set; }
        private RequestNotificationStatus _status;



        public ReliableAsyncResult(ReliableAsyncCallback cb, object state, bool completed, string responseText, ReliableHttpException error)
        {

            _id = Guid.NewGuid();
            _callback = cb;
            _asyncState = state;
            _completed = completed;
            _completedSynchronously = completed;
            ResponseText = responseText;
            _error = error;
            _status = RequestNotificationStatus.Continue;
            if (_completed && (_callback != null))
            {
                _callback(this);
            }
        }

        ///<summary>
        ///</summary>
        public Guid Id
        {
            get { return _id; }
        }

        internal Exception Error
        {
            get { return _error; }
        }

        internal RequestNotificationStatus Status
        {
            get { return _status; }
        }


        // Properties

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return _asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return null; }
        }

        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }

        public bool IsCompleted
        {
            get { return _completed; }
        }

        #endregion

        internal void Complete(bool synchronous, string responseText, ReliableHttpException error)
        {
            Complete(synchronous, responseText, error, RequestNotificationStatus.Continue);
        }

        internal void Complete(bool synchronous, string responseText, ReliableHttpException error, RequestNotificationStatus status)
        {
            _completed = true;
            _completedSynchronously = synchronous;
            ResponseText = responseText;
            _error = error;
            _status = status;
            if (_callback != null)
            {
                _callback(this);
            }
        }

        /// <summary>
        /// This could be made public to obviate the need for a reference to the context but
        /// in doing so, we 1) break the standard async pattern, and 2) give up the ability to monitor and possibly
        /// preprocess the response before letting the caller have it. I vote NO.
        /// </summary>
        /// <returns></returns>
        internal string End()
        {
            // if already apiexception just throw
            if (_error != null)
            {
                throw _error;
            }
            return ResponseText;

        }

        internal void SetComplete()
        {
            _completed = true;
        }
    }
}