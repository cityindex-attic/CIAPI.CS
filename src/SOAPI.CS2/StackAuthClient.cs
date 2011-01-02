using System;
using System.Collections.Generic;
using System.Net;
using CityIndex.JsonClient;

namespace SOAPI.CS2
{
    /// <summary>
    /// http://stackauth.com/1.0/help/method?method=sites
    /// </summary>
    public partial class StackAuthClient : Client
    {
        #region cTor

        public StackAuthClient()
            : base(
                new Uri("http://stackauth.com/1.0/"), new RequestCache(), new RequestFactory(),
                new Dictionary<string, IThrottedRequestQueue> { { "", Throttle.Instance } }, 3)
        {
        }

        public StackAuthClient(Uri uri, RequestCache cache, IRequestFactory requestFactory,
                               Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
            : base(uri, cache, requestFactory, throttleScopes, retryCount)
        {
        }

        #endregion

        protected override void BeforeIssueRequest(System.Net.WebRequest request, string url, string target, string uriTemplate, string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope)
        {
            if (typeof(HttpWebRequest) == request.GetType())
            {
                ((HttpWebRequest) request).AutomaticDecompression = DecompressionMethods.GZip;
            }

            base.BeforeIssueRequest(request, url, target, uriTemplate, method, parameters, cacheDuration, throttleScope);
        }
    }
}