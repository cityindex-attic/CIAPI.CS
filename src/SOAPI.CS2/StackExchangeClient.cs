using System;
using System.Collections.Generic;
using System.Net;
using CityIndex.JsonClient;

namespace SOAPI.CS2
{
    /// <summary>
    /// </summary>
    public partial class StackExchangeClient : Client
    {
        /// <summary>
        /// 
        /// </summary>
        public string ApiKey { get; set; }
        #region cTor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="url"></param>
        public StackExchangeClient(string apiKey, string url)
            : base(
                new Uri(url), new RequestCache(), new RequestFactory(),
                new Dictionary<string, IThrottedRequestQueue> { { "", Throttle.Instance } }, 3)
        {
            ApiKey = apiKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="uri"></param>
        /// <param name="cache"></param>
        /// <param name="requestFactory"></param>
        /// <param name="throttleScopes"></param>
        /// <param name="retryCount"></param>
        public StackExchangeClient(string apiKey, Uri uri, IRequestCache cache, IRequestFactory requestFactory,
                               Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
            : base(uri, cache, requestFactory, throttleScopes, retryCount)
        {
            ApiKey = apiKey;
        }

        #endregion

 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected override void BeforeBuildUrl(string target, string uriTemplate, string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope)
        {
            if (!string.IsNullOrEmpty(ApiKey))
            {
                parameters.Add("key", ApiKey);
            }
        }
    }
}