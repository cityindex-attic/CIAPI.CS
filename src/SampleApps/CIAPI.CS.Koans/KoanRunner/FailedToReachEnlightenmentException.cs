using System;

namespace CIAPI.CS.Koans.KoanRunner
{
    public class FailedToReachEnlightenmentException : ApplicationException
    {
        public FailedToReachEnlightenmentException(string message):
            base("You have yet to reach enlightenment because: " + message)
        {
        }
    }
}