using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;

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
        private TDTO _result;
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

        internal ApiAsyncResult(ApiAsyncCallback<TDTO> cb, object state, bool completed, TDTO result, Exception error)
        {
            _id = Guid.NewGuid();
            _callback = cb;
            _asyncState = state;
            _completed = completed;
            _completedSynchronously = completed;
            _result = result;
            _error = error;
            _status = RequestNotificationStatus.Continue;
            if (_completed && (_callback != null))
            {
                _callback(this);
            }
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal void Complete(bool synchronous, TDTO result, Exception error)
        {
            Complete(synchronous, result, error, RequestNotificationStatus.Continue);
        }

        internal void Complete(bool synchronous, TDTO result, Exception error, RequestNotificationStatus status)
        {
            _completed = true;
            _completedSynchronously = synchronous;
            _result = result;
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
            return _result;
        }

        internal void SetComplete()
        {
            _completed = true;
        }

        // Properties
        public object AsyncState
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
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
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return _completedSynchronously;
            }
        }

        internal Exception Error
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return _error;
            }
        }

        public bool IsCompleted
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return _completed;
            }
        }

        internal RequestNotificationStatus Status
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return _status;
            }
        }
    }
}
