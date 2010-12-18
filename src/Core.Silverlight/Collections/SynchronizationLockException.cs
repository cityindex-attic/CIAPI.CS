using System;

namespace CIAPI.Core.Collections
{
    public class SynchronizationLockException : Exception
    {
        // Methods
        public SynchronizationLockException()
            //: base(Environment.GetResourceString("Arg_SynchronizationLockException"))
            : base("Arg_SynchronizationLockException")
        {
            //base.SetErrorCode(-2146233064);
        }

        public SynchronizationLockException(string message)
            : base(message)
        {
            //base.SetErrorCode(-2146233064);
        }


        //protected SynchronizationLockException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}

        public SynchronizationLockException(string message, Exception innerException)
            : base(message, innerException)
        {
            //base.SetErrorCode(-2146233064);
        }
    }
}