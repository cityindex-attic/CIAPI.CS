using System;

namespace CIAPI.Core.Collections
{
    public class LockRecursionException : Exception
    {
        // Methods

        public LockRecursionException()
        {
        }


        public LockRecursionException(string message)
            : base(message)
        {
        }


        //protected LockRecursionException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}


        public LockRecursionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}