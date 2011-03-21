using System;
using System.Collections.Generic;
using CityIndex.JsonClient;

namespace SOAPI.CS2
{
    /// <summary>
    /// TODO: implement the private key 
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
                new Uri(url), new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(), new NullJsonExceptionFactory(), Throttle.Instance)
                )
        {
            ApiKey = apiKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="uri"></param>
        /// <param name="requestController"></param>
        public StackExchangeClient(string apiKey, Uri uri, IRequestController requestController)
            : base(uri, requestController)
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
        protected override void BeforeBuildUrl(string target, string uriTemplate, string method,
                                               Dictionary<string, object> parameters, TimeSpan cacheDuration,
                                               string throttleScope)
        {
            if (!string.IsNullOrEmpty(ApiKey))
            {
                parameters.Add("key", ApiKey);
            }
        }
    }
}