using System;
using System.Collections.Generic;

namespace CityIndex.JsonClient
{
    ///<summary>
    ///</summary>
    public interface IRequestController : IDisposable
    {
        ///<summary>
        ///</summary>
        ContentType ContentType { get; set; }

        ///<summary>
        ///</summary>
        string BasicHttpAuthUsername { get; set; }
        ///<summary>
        ///</summary>
        string BasicHttpAuthPassword { get; set; } 
        ///<summary>
        ///</summary>
        string UserAgent { get; set; }
        ///<summary>
        ///</summary>
        IRequestCache Cache { get; }

        ///<summary>
        ///</summary>
        ///<param name="key"></param>
        IThrottledRequestQueue this[string key] { get; }

        ///<summary>
        ///</summary>
        int RetryCount { get; }

        ///<summary>
        ///</summary>
        IRequestFactory RequestFactory { get; }

        ///<summary>
        ///</summary>
        event EventHandler<CacheItemEventArgs> BeforeBuildUrl;
        ///<summary>
        ///</summary>
        event EventHandler<CacheItemEventArgs> BeforeIssueRequest;

        ///<summary>
        ///</summary>
        ///<param name="target"></param>
        ///<param name="uriTemplate"></param>
        ///<param name="method"></param>
        ///<param name="parameters"></param>
        ///<param name="cacheDuration"></param>
        ///<param name="throttleScope"></param>
        ///<param name="url"></param>
        ///<param name="cb"></param>
        ///<param name="state"></param>
        ///<typeparam name="TDTO"></typeparam>
        void ProcessCacheItem<TDTO>(string target, string uriTemplate, string method,
                                    Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope,
                                    string url, ApiAsyncCallback<TDTO> cb, object state) ;

        ///<summary>
        ///</summary>
        ///<param name="url"></param>
        ///<typeparam name="TDTO"></typeparam>
        void CreateRequest<TDTO>(string url);
    }
}