using System.Net;

namespace CIAPI.Core
{

    public class RequestFactory : IRequestFactory
    {
        public WebRequest Create(string uri)
        {
            return WebRequest.Create(uri);
        }
    }
}