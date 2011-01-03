using System.Net;
using log4net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Default RequestFactory. Returns instances of <see cref="WebRequest"/>.
    /// </summary>
    public class RequestFactory : IRequestFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));

        ///<summary>
        ///</summary>
        public RequestFactory()
        {
#if !SILVERLIGHT
            DecompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate;
#endif
        }

#if !SILVERLIGHT
        ///<summary>
        ///</summary>
        public DecompressionMethods DecompressionMethods { get; set; }
#endif 
        ///<summary>
        /// Returns instances of <see cref="WebRequest"/>.
        ///</summary>
        ///<param name="uri"></param>
        ///<returns></returns>
        public WebRequest Create(string uri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);

#if !SILVERLIGHT
            request.AutomaticDecompression = DecompressionMethods;
#endif
            
            
            return request;
        }
    }
}