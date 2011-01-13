using System.Net;

namespace CityIndex.JsonClient
{
    internal class RequestRetryDiscriminator
    {
        public bool ShouldRetry(WebException webException)
        {
            if (webException.Message.ToLower().Contains("(500) internal server error")) return true;
            if (webException.Message.ToLower().Contains("(504) gateway timeout")) return true;
            if (webException.Message.ToLower().Contains("(408) request timeout")) return true;
            return false;
        }
    }
}