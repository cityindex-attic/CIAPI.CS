using System.Net;
using log4net;

namespace CIAPI.Core
{

    public class RequestFactory : IRequestFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));

        public WebRequest Create(string uri)
        {
            return WebRequest.Create(uri);
        }
    }
}