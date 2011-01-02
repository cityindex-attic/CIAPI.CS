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

        /// <summary>
        /// 
        /// </summary>
        public StackAuthClient()
            : base(
                new Uri("http://stackauth.com/1.0/"), new RequestCache(), new RequestFactory(),
                new Dictionary<string, IThrottedRequestQueue> { { "", Throttle.Instance } }, 3)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cache"></param>
        /// <param name="requestFactory"></param>
        /// <param name="throttleScopes"></param>
        /// <param name="retryCount"></param>
        public StackAuthClient(Uri uri, IRequestCache cache, IRequestFactory requestFactory,
                               Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
            : base(uri, cache, requestFactory, throttleScopes, retryCount)
        {
            
        }

        #endregion


        /// <summary>
        /// This is a place to add headers or anything else you would like to do just before the
        /// request is placed in the execution queue.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected override void BeforeIssueRequest(WebRequest request, string url, string target, string uriTemplate, string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope)
        {
            if (typeof(HttpWebRequest) == request.GetType())
            {
                ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.GZip;
            }
        }


    }
}