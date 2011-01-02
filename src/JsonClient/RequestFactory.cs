using System.Net;
using log4net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Default RequestFactory. Returns instances of <see cref="WebRequest"/>
    /// </summary>
    public class RequestFactory : IRequestFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));

        public WebRequest Create(string uri)
        {
            return WebRequest.Create(uri);
        }
    }
}