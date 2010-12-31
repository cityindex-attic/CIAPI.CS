using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace CIAPI.Core
{
    public abstract class ApiAsyncResult { }
    /// <summary>
    /// Currently this is a simple implementation of HttpAsyncResult
    /// </summary>
    public class ApiAsyncResult<TDTO> : ApiAsyncResult, IAsyncResult where TDTO : class,new()
    {
        // Fields
        private object _asyncState;
        private ApiAsyncCallback<TDTO> _callback;
        private bool _completed;
        private bool _completedSynchronously;
        private Exception _error;
        private string _responseText;
        private RequestNotificationStatus _status;
        private readonly Guid _id;
        public Guid Id
        {
            get
            {
                return _id;
            }
        }
        // Methods
        internal ApiAsyncResult(ApiAsyncCallback<TDTO> cb, object state)
        {
            _id = Guid.NewGuid();
            _callback = cb;
            _asyncState = state;
            _status = RequestNotificationStatus.Continue;
        }

        internal ApiAsyncResult(ApiAsyncCallback<TDTO> cb, object state, bool completed, string responseText, Exception error)
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
        /// TODO: determine if it is really in our best interest to stick to begin/end async pattern, requiring a reference to the request factory
        /// when we have the api response here in the response. We could make this public and drastically simplify the workflow.
        /// </summary>
        /// <returns></returns>
        internal TDTO End()
        {
            if (_error != null)
            {
                throw new ApiException(_error);
            }
            return JsonConvert.DeserializeObject<TDTO>(_responseText);
        }

        internal void SetComplete()
        {
            _completed = true;
        }

        // Properties
        public object AsyncState
        {
            get
            {
                return _asyncState;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return null;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return _completedSynchronously;
            }
        }

        internal Exception Error
        {
            get
            {
                return _error;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _completed;
            }
        }

        internal RequestNotificationStatus Status
        {
            get
            {
                return _status;
            }
        }
    }
}
