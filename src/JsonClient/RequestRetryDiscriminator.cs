using System.Net;

namespace CityIndex.JsonClient
{      
    /// <summary>
    /// allow for retries on select exception types
    /// e.g. 50* server errors, timeouts and transport errors
    /// DO NOT RETRY THROTTLE, AUTHENTICATION OR ARGUMENT EXCEPTIONS ETC
    /// </summary>
    internal class RequestRetryDiscriminator
    {
        public bool ShouldRetry(WebException webException)
        {
            if (webException.Message.ToLower().Contains("(500) internal server error")) return true;
            if (webException.Message.ToLower().Contains("(504) gateway timeout")) return true;
            if (webException.Message.ToLower().Contains("(408) request timeout")) return true;
            if (webException.Message.ToLower().Contains("aborted"))
            {
                return true;
            }
            return false;
        }
    }
}