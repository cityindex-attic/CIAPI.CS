using System;
using System.Threading;
using Newtonsoft.Json;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// The asynch request handle that is passed back to the <see cref="Client"/> to retrieve the result of the request.
    /// </summary>
    public class ApiAsyncResult<TDTO> : ApiAsyncResultBase, IAsyncResult where TDTO : class, new()
    {
        // Fields
        private readonly object _asyncState;
        private readonly ApiAsyncCallback<TDTO> _callback;
        private readonly Guid _id;
        private bool _completed;
        private bool _completedSynchronously;
        private Exception _error;
        private string _responseText;
        private RequestNotificationStatus _status;

        // Methods
        internal ApiAsyncResult(ApiAsyncCallback<TDTO> cb, object state)
        {
            _id = Guid.NewGuid();
            _callback = cb;
            _asyncState = state;
            _status = RequestNotificationStatus.Continue;
        }

        internal ApiAsyncResult(ApiAsyncCallback<TDTO> cb, object state, bool completed, string responseText,
                                Exception error)
        {
            _id = Guid.NewGuid();
            _callback = cb;
            _asyncState = state;
            _completed = completed;
            _completedSynchronously = completed;
            _responseText = responseText;
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

        internal void Complete(bool synchronous, string responseText, Exception error)
        {
            Complete(synchronous, responseText, error, RequestNotificationStatus.Continue);
        }

        internal void Complete(bool synchronous, string responseText, Exception error, RequestNotificationStatus status)
        {
            _completed = true;
            _completedSynchronously = synchronous;
            _responseText = responseText;
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
        internal TDTO End()
        {
            // if already apiexception just throw
            if (_error != null)
            {
                throw ApiException.Create(_error);
            }

            try
            {
                return JsonConvert.DeserializeObject<TDTO>(_responseText);
            }
            catch
            {
                throw new ApiSerializationException("Invalid JSON.", _responseText);
            }
        }

        internal void SetComplete()
        {
            _completed = true;
        }
    }
}