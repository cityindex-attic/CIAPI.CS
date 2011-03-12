using System;
using System.Collections.Generic;

namespace CityIndex.JsonClient
{
    ///<summary>
    ///</summary>
    public interface IRequestController : IDisposable
    {
        event EventHandler<CacheItemEventArgs> BeforeBuildUrl;
        event EventHandler<CacheItemEventArgs> BeforeIssueRequest;

        void ProcessCacheItem<TDTO>(string target, string uriTemplate, string method,
                                    Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope,
                                    string url, ApiAsyncCallback<TDTO> cb, object state) where TDTO : class, new();

        ///<summary>
        ///</summary>
        ///<param name="url"></param>
        ///<typeparam name="TDTO"></typeparam>
        void CreateRequest<TDTO>(string url)
            where TDTO : class, new();

        ///<summary>
        ///</summary>
        IRequestCache Cache { get; }

        ///<summary>
        ///</summary>
        ///<param name="key"></param>
        IThrottedRequestQueue this[string key] { get; }

        ///<summary>
        ///</summary>
        int RetryCount { get; }

        ///<summary>
        ///</summary>
        IRequestFactory RequestFactory { get; }
    }
}