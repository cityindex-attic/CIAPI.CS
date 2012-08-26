using System;
using System.Collections.Generic;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Serialization;
using CIAPI.Serialization;
using CIAPI.DTO;
using CIAPI.Streaming;
namespace CIAPI.Rpc
{
    public partial class Client
    {

      public _Authentication Authentication{get; private set;}
      public _PriceHistory PriceHistory{get; private set;}
      public _News News{get; private set;}
      public _CFDMarkets CFDMarkets{get; private set;}
      public _SpreadMarkets SpreadMarkets{get; private set;}
      public _Market Market{get; private set;}
      public _Preference Preference{get; private set;}
      public _TradesAndOrders TradesAndOrders{get; private set;}
      public _AccountInformation AccountInformation{get; private set;}
      public _Messaging Messaging{get; private set;}
      public _Watchlist Watchlist{get; private set;}
      public _ClientApplication ClientApplication{get; private set;}
private Client _client;
public string AppKey { get; set; }
        public Client(Uri rpcUri, Uri streamingUri, string appKey)
            : base(new Serializer())
        {
	#if SILVERLIGHT
	#if WINDOWS_PHONE
	        UserAgent = "CIAPI.PHONE7."+ GetVersionNumber();
	#else
	        UserAgent = "CIAPI.SILVERLIGHT."+ GetVersionNumber();
	#endif
	#else
	        UserAgent = "CIAPI.CS." + GetVersionNumber();
	#endif
        _streamingFactory=new LightStreamerStreamingClientFactory();
        AppKey=appKey;
        _client=this;
        _rootUri = rpcUri;
        _streamingUri = streamingUri;

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. Market = new _Market(this);
            this. Preference = new _Preference(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ClientApplication = new _ClientApplication(this);
        Log.Debug("Rpc.Client created for " + _rootUri.AbsoluteUri);
        }
        public Client(Uri rpcUri, Uri streamingUri, string appKey, int backgroundInterval)
            : base(new Serializer(),backgroundInterval)
        {
	#if SILVERLIGHT
	#if WINDOWS_PHONE
	        UserAgent = "CIAPI.PHONE7."+ GetVersionNumber();
	#else
	        UserAgent = "CIAPI.SILVERLIGHT."+ GetVersionNumber();
	#endif
	#else
	        UserAgent = "CIAPI.CS." + GetVersionNumber();
	#endif
        _streamingFactory=new LightStreamerStreamingClientFactory();
        AppKey=appKey;
        _client=this;
        _rootUri = rpcUri;
        _streamingUri = streamingUri;

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. Market = new _Market(this);
            this. Preference = new _Preference(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ClientApplication = new _ClientApplication(this);
        Log.Debug("Rpc.Client created for " + _rootUri.AbsoluteUri);
        }
        public Client(Uri rpcUri, Uri streamingUri, string appKey,IJsonSerializer serializer, IRequestFactory requestFactory, IStreamingClientFactory streamingFactory)
            : base(serializer, requestFactory)
        {
	#if SILVERLIGHT
	#if WINDOWS_PHONE
	        UserAgent = "CIAPI.PHONE7."+ GetVersionNumber();
	#else
	        UserAgent = "CIAPI.SILVERLIGHT."+ GetVersionNumber();
	#endif
	#else
	        UserAgent = "CIAPI.CS." + GetVersionNumber();
	#endif
        _streamingFactory=streamingFactory;
        AppKey=appKey;
        _client=this;
        _rootUri = rpcUri;
        _streamingUri = streamingUri;

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. Market = new _Market(this);
            this. Preference = new _Preference(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ClientApplication = new _ClientApplication(this);
        Log.Debug("Rpc.Client created for " + _rootUri.AbsoluteUri);
        }

        public class _Authentication
        {
            private Client _client;
            public _Authentication(Client client){ this._client = client;}

        // ***********************************
        // LogOn
        // ***********************************


        /// <summary>
        /// Create a new session. This is how you "log on" to the CIAPI.
        /// </summary>
        /// <param name="apiLogOnRequest">The request to create a session *(log on)*.</param>
        internal virtual ApiLogOnResponseDTO LogOn(ApiLogOnRequestDTO apiLogOnRequest)
        {
            string uriTemplate = "/";
            return _client.Request<ApiLogOnResponseDTO>(RequestMethod.POST,"session", uriTemplate ,
            new Dictionary<string, object>
            {
                { "apiLogOnRequest", apiLogOnRequest}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Create a new session. This is how you "log on" to the CIAPI.
        /// </summary>
        /// <param name="apiLogOnRequest">The request to create a session *(log on)*.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal virtual void BeginLogOn(ApiLogOnRequestDTO apiLogOnRequest, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/";
            _client.BeginRequest(RequestMethod.POST, "session", uriTemplate , 
            new Dictionary<string, object>
            {
                { "apiLogOnRequest", apiLogOnRequest}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        internal ApiLogOnResponseDTO EndLogOn(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiLogOnResponseDTO>(asyncResult);
        }


        // ***********************************
        // DeleteSession
        // ***********************************


        /// <summary>
        /// Delete a session. This is how you "log off" from the CIAPI.
        /// </summary>
        /// <param name="UserName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
        /// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        internal virtual ApiLogOffResponseDTO DeleteSession(string UserName, string session)
        {
            string uriTemplate = "/deleteSession?UserName={UserName}&session={session}";
            return _client.Request<ApiLogOffResponseDTO>(RequestMethod.POST,"session", uriTemplate ,
            new Dictionary<string, object>
            {
                { "UserName", UserName}, 
                { "session", session}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Delete a session. This is how you "log off" from the CIAPI.
        /// </summary>
        /// <param name="UserName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
        /// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal virtual void BeginDeleteSession(string UserName, string session, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/deleteSession?UserName={UserName}&session={session}";
            _client.BeginRequest(RequestMethod.POST, "session", uriTemplate , 
            new Dictionary<string, object>
            {
                { "UserName", UserName}, 
                { "session", session}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        internal ApiLogOffResponseDTO EndDeleteSession(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiLogOffResponseDTO>(asyncResult);
        }


        // ***********************************
        // ChangePassword
        // ***********************************


        /// <summary>
        /// Change a user's password.
        /// </summary>
        /// <param name="apiChangePasswordRequest">The change password request details.</param>
        public virtual ApiChangePasswordResponseDTO ChangePassword(ApiChangePasswordRequestDTO apiChangePasswordRequest)
        {
            string uriTemplate = "/changePassword";
            return _client.Request<ApiChangePasswordResponseDTO>(RequestMethod.POST,"session", uriTemplate ,
            new Dictionary<string, object>
            {
                { "apiChangePasswordRequest", apiChangePasswordRequest}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Change a user's password.
        /// </summary>
        /// <param name="apiChangePasswordRequest">The change password request details.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginChangePassword(ApiChangePasswordRequestDTO apiChangePasswordRequest, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/changePassword";
            _client.BeginRequest(RequestMethod.POST, "session", uriTemplate , 
            new Dictionary<string, object>
            {
                { "apiChangePasswordRequest", apiChangePasswordRequest}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiChangePasswordResponseDTO EndChangePassword(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiChangePasswordResponseDTO>(asyncResult);
        }


        }            
        public class _PriceHistory
        {
            private Client _client;
            public _PriceHistory(Client client){ this._client = client;}

        // ***********************************
        // GetPriceBars
        // ***********************************


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="MarketId">The ID of the market.</param>
        /// <param name="interval">The pricebar interval.</param>
        /// <param name="span">The number of each interval per pricebar.</param>
        /// <param name="PriceBars">The total number of price bars to return.</param>
        public virtual GetPriceBarResponseDTO GetPriceBars(string MarketId, string interval, int span, string PriceBars)
        {
            string uriTemplate = "/{MarketId}/barhistory?interval={interval}&span={span}&PriceBars={PriceBars}";
            return _client.Request<GetPriceBarResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "MarketId", MarketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "PriceBars", PriceBars}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="MarketId">The ID of the market.</param>
        /// <param name="interval">The pricebar interval.</param>
        /// <param name="span">The number of each interval per pricebar.</param>
        /// <param name="PriceBars">The total number of price bars to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceBars(string MarketId, string interval, int span, string PriceBars, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{MarketId}/barhistory?interval={interval}&span={span}&PriceBars={PriceBars}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "MarketId", MarketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "PriceBars", PriceBars}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetPriceBarResponseDTO EndGetPriceBars(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetPriceBarResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetPriceTicks
        // ***********************************


        /// <summary>
        /// Get historic price ticks for the specified market. Returns price ticks in ascending order up to the current time. The length of time that elapses between each tick is usually different.
        /// </summary>
        /// <param name="MarketId">The market ID.</param>
        /// <param name="PriceTicks">The total number of price ticks to return.</param>
        public virtual GetPriceTickResponseDTO GetPriceTicks(string MarketId, string PriceTicks)
        {
            string uriTemplate = "/{MarketId}/tickhistory?PriceTicks={PriceTicks}";
            return _client.Request<GetPriceTickResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "MarketId", MarketId}, 
                { "PriceTicks", PriceTicks}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get historic price ticks for the specified market. Returns price ticks in ascending order up to the current time. The length of time that elapses between each tick is usually different.
        /// </summary>
        /// <param name="MarketId">The market ID.</param>
        /// <param name="PriceTicks">The total number of price ticks to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceTicks(string MarketId, string PriceTicks, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{MarketId}/tickhistory?PriceTicks={PriceTicks}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "MarketId", MarketId}, 
                { "PriceTicks", PriceTicks}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetPriceTickResponseDTO EndGetPriceTicks(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetPriceTickResponseDTO>(asyncResult);
        }


        }            
        public class _News
        {
            private Client _client;
            public _News(Client client){ this._client = client;}

        // ***********************************
        // ListNewsHeadlinesWithSource
        // ***********************************


        /// <summary>
        /// Get a list of current news headlines.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are: **dj**|**mni**|**ci**.</param>
        /// <param name="category">Filter headlines by category. Valid categories depend on the source used:  for **dj**: *uk*|*aus*, for **ci**: *SEMINARSCHINA*, for **mni**: *ALL*.</param>
        /// <param name="maxResults">Specify the maximum number of headlines returned.</param>
        public virtual ListNewsHeadlinesResponseDTO ListNewsHeadlinesWithSource(string source, string category, int maxResults)
        {
            string uriTemplate = "/{source}/{category}?MaxResults={maxResults}";
            return _client.Request<ListNewsHeadlinesResponseDTO>(RequestMethod.GET,"news", uriTemplate ,
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "category", category}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(10000),30000,0 );
        }


        /// <summary>
        /// Get a list of current news headlines.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are: **dj**|**mni**|**ci**.</param>
        /// <param name="category">Filter headlines by category. Valid categories depend on the source used:  for **dj**: *uk*|*aus*, for **ci**: *SEMINARSCHINA*, for **mni**: *ALL*.</param>
        /// <param name="maxResults">Specify the maximum number of headlines returned.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListNewsHeadlinesWithSource(string source, string category, int maxResults, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{source}/{category}?MaxResults={maxResults}";
            _client.BeginRequest(RequestMethod.GET, "news", uriTemplate , 
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "category", category}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(10000), 30000,2 ,callback, state);
        }

        public ListNewsHeadlinesResponseDTO EndListNewsHeadlinesWithSource(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListNewsHeadlinesResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListNewsHeadlines
        // ***********************************


        /// <summary>
        /// Get a list of current news headlines.
        /// </summary>
        /// <param name="request">Object specifing the various request parameters.</param>
        public virtual ListNewsHeadlinesResponseDTO ListNewsHeadlines(ListNewsHeadlinesRequestDTO request)
        {
            string uriTemplate = "/headlines";
            return _client.Request<ListNewsHeadlinesResponseDTO>(RequestMethod.POST,"news", uriTemplate ,
            new Dictionary<string, object>
            {
                { "request", request}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get a list of current news headlines.
        /// </summary>
        /// <param name="request">Object specifing the various request parameters.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListNewsHeadlines(ListNewsHeadlinesRequestDTO request, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/headlines";
            _client.BeginRequest(RequestMethod.POST, "news", uriTemplate , 
            new Dictionary<string, object>
            {
                { "request", request}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListNewsHeadlinesResponseDTO EndListNewsHeadlines(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListNewsHeadlinesResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetNewsDetail
        // ***********************************


        /// <summary>
        /// Get the detail of the specific news story matching the story ID in the parameter.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are **dj**|**mni**|**ci**.</param>
        /// <param name="storyId">The news story ID.</param>
        public virtual GetNewsDetailResponseDTO GetNewsDetail(string source, string storyId)
        {
            string uriTemplate = "/{source}/{storyId}";
            return _client.Request<GetNewsDetailResponseDTO>(RequestMethod.GET,"news", uriTemplate ,
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "storyId", storyId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(10000),30000,0 );
        }


        /// <summary>
        /// Get the detail of the specific news story matching the story ID in the parameter.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are **dj**|**mni**|**ci**.</param>
        /// <param name="storyId">The news story ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetNewsDetail(string source, string storyId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{source}/{storyId}";
            _client.BeginRequest(RequestMethod.GET, "news", uriTemplate , 
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "storyId", storyId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(10000), 30000,2 ,callback, state);
        }

        public GetNewsDetailResponseDTO EndGetNewsDetail(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetNewsDetailResponseDTO>(asyncResult);
        }


        }            
        public class _CFDMarkets
        {
            private Client _client;
            public _CFDMarkets(Client client){ this._client = client;}

        // ***********************************
        // ListCfdMarkets
        // ***********************************


        /// <summary>
        /// Returns a list of CFD markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.
        /// </summary>
        /// <param name="searchByMarketName">The characters that the CFD market name starts with. *(Optional)*.</param>
        /// <param name="searchByMarketCode">The characters that the market code starts with, normally this is the RIC code for the market. *(Optional)*.</param>
        /// <param name="ClientAccountId">The logged on user's ClientAccountId. This only shows you the markets that the user can trade. *(Required)*.</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        public virtual ListCfdMarketsResponseDTO ListCfdMarkets(string searchByMarketName, string searchByMarketCode, int ClientAccountId, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={ClientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<ListCfdMarketsResponseDTO>(RequestMethod.GET,"cfd/markets", uriTemplate ,
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "ClientAccountId", ClientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns a list of CFD markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.
        /// </summary>
        /// <param name="searchByMarketName">The characters that the CFD market name starts with. *(Optional)*.</param>
        /// <param name="searchByMarketCode">The characters that the market code starts with, normally this is the RIC code for the market. *(Optional)*.</param>
        /// <param name="ClientAccountId">The logged on user's ClientAccountId. This only shows you the markets that the user can trade. *(Required)*.</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListCfdMarkets(string searchByMarketName, string searchByMarketCode, int ClientAccountId, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={ClientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "cfd/markets", uriTemplate , 
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "ClientAccountId", ClientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListCfdMarketsResponseDTO EndListCfdMarkets(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListCfdMarketsResponseDTO>(asyncResult);
        }


        }            
        public class _SpreadMarkets
        {
            private Client _client;
            public _SpreadMarkets(Client client){ this._client = client;}

        // ***********************************
        // ListSpreadMarkets
        // ***********************************


        /// <summary>
        /// Returns a list of Spread Betting markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.
        /// </summary>
        /// <param name="searchByMarketName">The characters that the Spread market name starts with. *(Optional)*.</param>
        /// <param name="searchByMarketCode">The characters that the Spread market code starts with, normally this is the RIC code for the market. *(Optional)*.</param>
        /// <param name="ClientAccountId">The logged on user's ClientAccountId. *(This only shows you markets that you can trade on.)*</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        public virtual ListSpreadMarketsResponseDTO ListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int ClientAccountId, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={ClientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<ListSpreadMarketsResponseDTO>(RequestMethod.GET,"spread/markets", uriTemplate ,
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "ClientAccountId", ClientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns a list of Spread Betting markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.
        /// </summary>
        /// <param name="searchByMarketName">The characters that the Spread market name starts with. *(Optional)*.</param>
        /// <param name="searchByMarketCode">The characters that the Spread market code starts with, normally this is the RIC code for the market. *(Optional)*.</param>
        /// <param name="ClientAccountId">The logged on user's ClientAccountId. *(This only shows you markets that you can trade on.)*</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int ClientAccountId, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={ClientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "spread/markets", uriTemplate , 
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "ClientAccountId", ClientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListSpreadMarketsResponseDTO EndListSpreadMarkets(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListSpreadMarketsResponseDTO>(asyncResult);
        }


        }            
        public class _Market
        {
            private Client _client;
            public _Market(Client client){ this._client = client;}

        // ***********************************
        // GetMarketInformation
        // ***********************************


        /// <summary>
        /// Get Market Information for the single specified market supplied in the parameter.
        /// </summary>
        /// <param name="MarketId">The market ID.</param>
        public virtual GetMarketInformationResponseDTO GetMarketInformation(string MarketId)
        {
            string uriTemplate = "/{MarketId}/information";
            return _client.Request<GetMarketInformationResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "MarketId", MarketId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(1000),30000,0 );
        }


        /// <summary>
        /// Get Market Information for the single specified market supplied in the parameter.
        /// </summary>
        /// <param name="MarketId">The market ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetMarketInformation(string MarketId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{MarketId}/information";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "MarketId", MarketId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(1000), 30000,2 ,callback, state);
        }

        public GetMarketInformationResponseDTO EndGetMarketInformation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetMarketInformationResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListMarketInformationSearch
        // ***********************************


        /// <summary>
        /// Returns market information for the markets that meet the search criteria. The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.
        /// </summary>
        /// <param name="searchByMarketCode">Sets the search to use market code.</param>
        /// <param name="searchByMarketName">Sets the search to use market Name.</param>
        /// <param name="spreadProductType">Sets the search to include spread bet markets.</param>
        /// <param name="cfdProductType">Sets the search to include CFD markets.</param>
        /// <param name="binaryProductType">Sets the search to include binary markets.</param>
        /// <param name="query">The text to search for. Matches part of market name / code from the start.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form.  Helpful when displaying data on a small screen.</param>
        public virtual ListMarketInformationSearchResponseDTO ListMarketInformationSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<ListMarketInformationSearchResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "searchByMarketCode", searchByMarketCode}, 
                { "searchByMarketName", searchByMarketName}, 
                { "spreadProductType", spreadProductType}, 
                { "cfdProductType", cfdProductType}, 
                { "binaryProductType", binaryProductType}, 
                { "query", query}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns market information for the markets that meet the search criteria. The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.
        /// </summary>
        /// <param name="searchByMarketCode">Sets the search to use market code.</param>
        /// <param name="searchByMarketName">Sets the search to use market Name.</param>
        /// <param name="spreadProductType">Sets the search to include spread bet markets.</param>
        /// <param name="cfdProductType">Sets the search to include CFD markets.</param>
        /// <param name="binaryProductType">Sets the search to include binary markets.</param>
        /// <param name="query">The text to search for. Matches part of market name / code from the start.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form.  Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListMarketInformationSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "searchByMarketCode", searchByMarketCode}, 
                { "searchByMarketName", searchByMarketName}, 
                { "spreadProductType", spreadProductType}, 
                { "cfdProductType", cfdProductType}, 
                { "binaryProductType", binaryProductType}, 
                { "query", query}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListMarketInformationSearchResponseDTO EndListMarketInformationSearch(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListMarketInformationSearchResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListMarketSearch
        // ***********************************


        /// <summary>
        /// Returns a list of markets that meet the search criteria. The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets. Leave the query string empty to return all markets available to the user.
        /// </summary>
        /// <param name="searchByMarketCode">Sets the search to use market code.</param>
        /// <param name="searchByMarketName">Sets the search to use market Name.</param>
        /// <param name="spreadProductType">Sets the search to include spread bet markets.</param>
        /// <param name="cfdProductType">Sets the search to include CFD markets.</param>
        /// <param name="binaryProductType">Sets the search to include binary markets.</param>
        /// <param name="query">The text to search for. Matches part of market name / code from the start. *(Optional)*.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form.  Helpful when displaying data on a small screen.</param>
        public virtual ListMarketSearchResponseDTO ListMarketSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "/search?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<ListMarketSearchResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "searchByMarketCode", searchByMarketCode}, 
                { "searchByMarketName", searchByMarketName}, 
                { "spreadProductType", spreadProductType}, 
                { "cfdProductType", cfdProductType}, 
                { "binaryProductType", binaryProductType}, 
                { "query", query}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns a list of markets that meet the search criteria. The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets. Leave the query string empty to return all markets available to the user.
        /// </summary>
        /// <param name="searchByMarketCode">Sets the search to use market code.</param>
        /// <param name="searchByMarketName">Sets the search to use market Name.</param>
        /// <param name="spreadProductType">Sets the search to include spread bet markets.</param>
        /// <param name="cfdProductType">Sets the search to include CFD markets.</param>
        /// <param name="binaryProductType">Sets the search to include binary markets.</param>
        /// <param name="query">The text to search for. Matches part of market name / code from the start. *(Optional)*.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form.  Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListMarketSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/search?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "searchByMarketCode", searchByMarketCode}, 
                { "searchByMarketName", searchByMarketName}, 
                { "spreadProductType", spreadProductType}, 
                { "cfdProductType", cfdProductType}, 
                { "binaryProductType", binaryProductType}, 
                { "query", query}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListMarketSearchResponseDTO EndListMarketSearch(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListMarketSearchResponseDTO>(asyncResult);
        }


        // ***********************************
        // SearchWithTags
        // ***********************************


        /// <summary>
        /// Get market information and tags for the markets that meet the search criteria. Leave the query string empty to return all markets and tags available to the user.
        /// </summary>
        /// <param name="query">The text to search for. Matches part of market name / code from the start. *(Optional)*.</param>
        /// <param name="tagId">The ID for the tag to be searched. *(Optional)*.</param>
        /// <param name="maxResults">The maximum number of results to return. Default is 20.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        public virtual MarketInformationSearchWithTagsResponseDTO SearchWithTags(string query, int tagId, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "/searchwithtags?Query={query}&TagId={tagId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<MarketInformationSearchWithTagsResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "query", query}, 
                { "tagId", tagId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get market information and tags for the markets that meet the search criteria. Leave the query string empty to return all markets and tags available to the user.
        /// </summary>
        /// <param name="query">The text to search for. Matches part of market name / code from the start. *(Optional)*.</param>
        /// <param name="tagId">The ID for the tag to be searched. *(Optional)*.</param>
        /// <param name="maxResults">The maximum number of results to return. Default is 20.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSearchWithTags(string query, int tagId, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/searchwithtags?Query={query}&TagId={tagId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "query", query}, 
                { "tagId", tagId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public MarketInformationSearchWithTagsResponseDTO EndSearchWithTags(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<MarketInformationSearchWithTagsResponseDTO>(asyncResult);
        }


        // ***********************************
        // TagLookup
        // ***********************************


        /// <summary>
        /// Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy. There are no parameters in this call.
        /// </summary>
        public virtual MarketInformationTagLookupResponseDTO TagLookup()
        {
            string uriTemplate = "/taglookup";
            return _client.Request<MarketInformationTagLookupResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy. There are no parameters in this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginTagLookup( ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/taglookup";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public MarketInformationTagLookupResponseDTO EndTagLookup(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<MarketInformationTagLookupResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListMarketInformation
        // ***********************************


        /// <summary>
        /// Get Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestDTO">Get Market Information for the specified list of markets.</param>
        public virtual ListMarketInformationResponseDTO ListMarketInformation(ListMarketInformationRequestDTO listMarketInformationRequestDTO)
        {
            string uriTemplate = "/information";
            return _client.Request<ListMarketInformationResponseDTO>(RequestMethod.POST,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestDTO", listMarketInformationRequestDTO}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(1000),30000,0 );
        }


        /// <summary>
        /// Get Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestDTO">Get Market Information for the specified list of markets.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListMarketInformation(ListMarketInformationRequestDTO listMarketInformationRequestDTO, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/information";
            _client.BeginRequest(RequestMethod.POST, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestDTO", listMarketInformationRequestDTO}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(1000), 30000,2 ,callback, state);
        }

        public ListMarketInformationResponseDTO EndListMarketInformation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListMarketInformationResponseDTO>(asyncResult);
        }


        // ***********************************
        // SaveMarketInformation
        // ***********************************


        /// <summary>
        /// Save Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestSaveDTO">Save Market Information for the specified list of markets.</param>
        public virtual ApiSaveMarketInformationResponseDTO SaveMarketInformation(SaveMarketInformationRequestDTO listMarketInformationRequestSaveDTO)
        {
            string uriTemplate = "/information/save";
            return _client.Request<ApiSaveMarketInformationResponseDTO>(RequestMethod.POST,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestSaveDTO", listMarketInformationRequestSaveDTO}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Save Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestSaveDTO">Save Market Information for the specified list of markets.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveMarketInformation(SaveMarketInformationRequestDTO listMarketInformationRequestSaveDTO, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/information/save";
            _client.BeginRequest(RequestMethod.POST, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestSaveDTO", listMarketInformationRequestSaveDTO}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiSaveMarketInformationResponseDTO EndSaveMarketInformation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiSaveMarketInformationResponseDTO>(asyncResult);
        }


        }            
        public class _Preference
        {
            private Client _client;
            public _Preference(Client client){ this._client = client;}

        // ***********************************
        // Save
        // ***********************************


        /// <summary>
        /// Save client preferences.
        /// </summary>
        /// <param name="saveClientPreferenceRequestDTO">The client preferences key/value pairs to save.</param>
        public virtual UpdateDeleteClientPreferenceResponseDTO Save(SaveClientPreferenceRequestDTO saveClientPreferenceRequestDTO)
        {
            string uriTemplate = "/save";
            return _client.Request<UpdateDeleteClientPreferenceResponseDTO>(RequestMethod.POST,"clientpreference", uriTemplate ,
            new Dictionary<string, object>
            {
                { "saveClientPreferenceRequestDTO", saveClientPreferenceRequestDTO}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Save client preferences.
        /// </summary>
        /// <param name="saveClientPreferenceRequestDTO">The client preferences key/value pairs to save.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSave(SaveClientPreferenceRequestDTO saveClientPreferenceRequestDTO, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/save";
            _client.BeginRequest(RequestMethod.POST, "clientpreference", uriTemplate , 
            new Dictionary<string, object>
            {
                { "saveClientPreferenceRequestDTO", saveClientPreferenceRequestDTO}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public UpdateDeleteClientPreferenceResponseDTO EndSave(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<UpdateDeleteClientPreferenceResponseDTO>(asyncResult);
        }


        // ***********************************
        // Get
        // ***********************************


        /// <summary>
        /// Get client preferences.
        /// </summary>
        /// <param name="clientPreferenceRequestDto">The client preference key to get.</param>
        public virtual GetClientPreferenceResponseDTO Get(ClientPreferenceRequestDTO clientPreferenceRequestDto)
        {
            string uriTemplate = "/get";
            return _client.Request<GetClientPreferenceResponseDTO>(RequestMethod.POST,"clientpreference", uriTemplate ,
            new Dictionary<string, object>
            {
                { "clientPreferenceRequestDto", clientPreferenceRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get client preferences.
        /// </summary>
        /// <param name="clientPreferenceRequestDto">The client preference key to get.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGet(ClientPreferenceRequestDTO clientPreferenceRequestDto, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/get";
            _client.BeginRequest(RequestMethod.POST, "clientpreference", uriTemplate , 
            new Dictionary<string, object>
            {
                { "clientPreferenceRequestDto", clientPreferenceRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetClientPreferenceResponseDTO EndGet(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetClientPreferenceResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetKeyList
        // ***********************************


        /// <summary>
        /// Get list of client preferences keys. There are no parameters in this call.
        /// </summary>
        public virtual GetKeyListClientPreferenceResponseDTO GetKeyList()
        {
            string uriTemplate = "/getkeylist";
            return _client.Request<GetKeyListClientPreferenceResponseDTO>(RequestMethod.GET,"clientpreference", uriTemplate ,
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get list of client preferences keys. There are no parameters in this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetKeyList( ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/getkeylist";
            _client.BeginRequest(RequestMethod.GET, "clientpreference", uriTemplate , 
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetKeyListClientPreferenceResponseDTO EndGetKeyList(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetKeyListClientPreferenceResponseDTO>(asyncResult);
        }


        // ***********************************
        // Delete
        // ***********************************


        /// <summary>
        /// Delete client preference key.
        /// </summary>
        /// <param name="clientPreferenceKey">The client preference key to delete.</param>
        public virtual UpdateDeleteClientPreferenceResponseDTO Delete(ClientPreferenceRequestDTO clientPreferenceKey)
        {
            string uriTemplate = "/delete";
            return _client.Request<UpdateDeleteClientPreferenceResponseDTO>(RequestMethod.POST,"clientpreference", uriTemplate ,
            new Dictionary<string, object>
            {
                { "clientPreferenceKey", clientPreferenceKey}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Delete client preference key.
        /// </summary>
        /// <param name="clientPreferenceKey">The client preference key to delete.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginDelete(ClientPreferenceRequestDTO clientPreferenceKey, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/delete";
            _client.BeginRequest(RequestMethod.POST, "clientpreference", uriTemplate , 
            new Dictionary<string, object>
            {
                { "clientPreferenceKey", clientPreferenceKey}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public UpdateDeleteClientPreferenceResponseDTO EndDelete(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<UpdateDeleteClientPreferenceResponseDTO>(asyncResult);
        }


        }            
        public class _TradesAndOrders
        {
            private Client _client;
            public _TradesAndOrders(Client client){ this._client = client;}

        // ***********************************
        // Order
        // ***********************************


        /// <summary>
        /// Place an order on a particular market.  Do not set any order ID fields when requesting a new order, the platform will generate them.
        /// </summary>
        /// <param name="order">The order request.</param>
        public virtual ApiTradeOrderResponseDTO Order(NewStopLimitOrderRequestDTO order)
        {
            string uriTemplate = "/newstoplimitorder";
            return _client.Request<ApiTradeOrderResponseDTO>(RequestMethod.POST,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "order", order}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Place an order on a particular market.  Do not set any order ID fields when requesting a new order, the platform will generate them.
        /// </summary>
        /// <param name="order">The order request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginOrder(NewStopLimitOrderRequestDTO order, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/newstoplimitorder";
            _client.BeginRequest(RequestMethod.POST, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "order", order}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiTradeOrderResponseDTO EndOrder(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiTradeOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // CancelOrder
        // ***********************************


        /// <summary>
        /// Cancel an order.
        /// </summary>
        /// <param name="cancelOrder">The cancel order request.</param>
        public virtual ApiTradeOrderResponseDTO CancelOrder(CancelOrderRequestDTO cancelOrder)
        {
            string uriTemplate = "/cancel";
            return _client.Request<ApiTradeOrderResponseDTO>(RequestMethod.POST,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "cancelOrder", cancelOrder}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Cancel an order.
        /// </summary>
        /// <param name="cancelOrder">The cancel order request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginCancelOrder(CancelOrderRequestDTO cancelOrder, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/cancel";
            _client.BeginRequest(RequestMethod.POST, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "cancelOrder", cancelOrder}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiTradeOrderResponseDTO EndCancelOrder(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiTradeOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // UpdateOrder
        // ***********************************


        /// <summary>
        /// Update an order *(for adding a stop/limit or attaching an OCO relationship)*.
        /// </summary>
        /// <param name="order">The update order request.</param>
        public virtual ApiTradeOrderResponseDTO UpdateOrder(UpdateStopLimitOrderRequestDTO order)
        {
            string uriTemplate = "/updatestoplimitorder";
            return _client.Request<ApiTradeOrderResponseDTO>(RequestMethod.POST,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "order", order}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Update an order *(for adding a stop/limit or attaching an OCO relationship)*.
        /// </summary>
        /// <param name="order">The update order request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginUpdateOrder(UpdateStopLimitOrderRequestDTO order, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/updatestoplimitorder";
            _client.BeginRequest(RequestMethod.POST, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "order", order}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiTradeOrderResponseDTO EndUpdateOrder(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiTradeOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListOpenPositions
        // ***********************************


        /// <summary>
        /// Queries for a specified trading account's trades / open positions.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetOpenPosition](http://labs.cityindex.com/docs/#HTTP%20Services/GetOpenPosition.htm) when you get updates on the order stream to get the updated data in this format.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        public virtual ListOpenPositionsResponseDTO ListOpenPositions(int TradingAccountId)
        {
            string uriTemplate = "/openpositions?TradingAccountId={TradingAccountId}";
            return _client.Request<ListOpenPositionsResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for a specified trading account's trades / open positions.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetOpenPosition](http://labs.cityindex.com/docs/#HTTP%20Services/GetOpenPosition.htm) when you get updates on the order stream to get the updated data in this format.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListOpenPositions(int TradingAccountId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/openpositions?TradingAccountId={TradingAccountId}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListOpenPositionsResponseDTO EndListOpenPositions(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListOpenPositionsResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListActiveStopLimitOrders
        // ***********************************


        /// <summary>
        /// Queries for a specified trading account's active stop / limit orders.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetActiveStopLimitOrder](http://labs.cityindex.com/docs/#HTTP%20Services/GetActiveStopLimitOrder.htm) when you get updates on the order stream to get the updated data in this format.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        public virtual ListActiveStopLimitOrderResponseDTO ListActiveStopLimitOrders(int TradingAccountId)
        {
            string uriTemplate = "/activestoplimitorders?TradingAccountId={TradingAccountId}";
            return _client.Request<ListActiveStopLimitOrderResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for a specified trading account's active stop / limit orders.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetActiveStopLimitOrder](http://labs.cityindex.com/docs/#HTTP%20Services/GetActiveStopLimitOrder.htm) when you get updates on the order stream to get the updated data in this format.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListActiveStopLimitOrders(int TradingAccountId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/activestoplimitorders?TradingAccountId={TradingAccountId}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListActiveStopLimitOrderResponseDTO EndListActiveStopLimitOrders(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListActiveStopLimitOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetActiveStopLimitOrder
        // ***********************************


        /// <summary>
        /// Queries for an active stop limit order with a specified order ID. It returns a null value if the order doesn't exist, or is not an active stop limit order.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service [ListActiveStopLimitOrders](http://labs.cityindex.com/docs/#HTTP%20Services/ListActiveStopLimitOrders.htm) for the initial data to display in the grid, and call this URI when you get updates on the order stream to get the updated data in this format. For a more comprehensive order response, see the HTTP service [GetOrder](http://labs.cityindex.com/docs/#HTTP%20Services/GetOrder.htm).
        /// </summary>
        /// <param name="OrderId">The requested order ID.</param>
        public virtual GetActiveStopLimitOrderResponseDTO GetActiveStopLimitOrder(string OrderId)
        {
            string uriTemplate = "/{OrderId}/activestoplimitorder";
            return _client.Request<GetActiveStopLimitOrderResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "OrderId", OrderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for an active stop limit order with a specified order ID. It returns a null value if the order doesn't exist, or is not an active stop limit order.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service [ListActiveStopLimitOrders](http://labs.cityindex.com/docs/#HTTP%20Services/ListActiveStopLimitOrders.htm) for the initial data to display in the grid, and call this URI when you get updates on the order stream to get the updated data in this format. For a more comprehensive order response, see the HTTP service [GetOrder](http://labs.cityindex.com/docs/#HTTP%20Services/GetOrder.htm).
        /// </summary>
        /// <param name="OrderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetActiveStopLimitOrder(string OrderId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{OrderId}/activestoplimitorder";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "OrderId", OrderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetActiveStopLimitOrderResponseDTO EndGetActiveStopLimitOrder(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetActiveStopLimitOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetOpenPosition
        // ***********************************


        /// <summary>
        /// Queries for a trade / open position with a specified order ID. It returns a null value if the order doesn't exist, or is not a trade / open position.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service [ListOpenPositions](http://labs.cityindex.com/docs/#HTTP%20Services/ListOpenPositions.htm) for the initial data to display in the grid, and call this URI when you get updates on the order stream to get the updated data in this format.  For a more comprehensive order response, see the HTTP service [GetOrder](http://labs.cityindex.com/docs/#HTTP%20Services/GetOrder.htm).
        /// </summary>
        /// <param name="OrderId">The requested order ID.</param>
        public virtual GetOpenPositionResponseDTO GetOpenPosition(string OrderId)
        {
            string uriTemplate = "/{OrderId}/openposition";
            return _client.Request<GetOpenPositionResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "OrderId", OrderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for a trade / open position with a specified order ID. It returns a null value if the order doesn't exist, or is not a trade / open position.   This URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service [ListOpenPositions](http://labs.cityindex.com/docs/#HTTP%20Services/ListOpenPositions.htm) for the initial data to display in the grid, and call this URI when you get updates on the order stream to get the updated data in this format.  For a more comprehensive order response, see the HTTP service [GetOrder](http://labs.cityindex.com/docs/#HTTP%20Services/GetOrder.htm).
        /// </summary>
        /// <param name="OrderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOpenPosition(string OrderId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{OrderId}/openposition";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "OrderId", OrderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetOpenPositionResponseDTO EndGetOpenPosition(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetOpenPositionResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListTradeHistory
        // ***********************************


        /// <summary>
        /// Queries for a specified trading account's trade history. The result set will contain orders with a status of __(3 - Open, 9 - Closed)__, and includes __orders that were a trade / stop / limit order__. There's currently no corresponding GetTradeHistory *(as with [ListOpenPositions](http://labs.cityindex.com/docs/#HTTP%20Services/ListOpenPositions.htm))*.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        public virtual ListTradeHistoryResponseDTO ListTradeHistory(int TradingAccountId, int maxResults)
        {
            string uriTemplate = "/order/tradehistory?TradingAccountId={TradingAccountId}&MaxResults={maxResults}";
            return _client.Request<ListTradeHistoryResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for a specified trading account's trade history. The result set will contain orders with a status of __(3 - Open, 9 - Closed)__, and includes __orders that were a trade / stop / limit order__. There's currently no corresponding GetTradeHistory *(as with [ListOpenPositions](http://labs.cityindex.com/docs/#HTTP%20Services/ListOpenPositions.htm))*.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListTradeHistory(int TradingAccountId, int maxResults, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/order/tradehistory?TradingAccountId={TradingAccountId}&MaxResults={maxResults}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListTradeHistoryResponseDTO EndListTradeHistory(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListTradeHistoryResponseDTO>(asyncResult);
        }


        // ***********************************
        // ListStopLimitOrderHistory
        // ***********************************


        /// <summary>
        /// Queries for a specified trading account's stop / limit order history. The result set includes __only orders that were originally stop / limit orders__ that currently have one of the following statuses __(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)__.  There is currently no corresponding GetStopLimitOrderHistory *(as with [ListActiveStopLimitOrders](http://labs.cityindex.com/docs/#HTTP%20Services/ListActiveStopLimitOrders.htm))*.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        public virtual ListStopLimitOrderHistoryResponseDTO ListStopLimitOrderHistory(int TradingAccountId, int maxResults)
        {
            string uriTemplate = "/stoplimitorderhistory?TradingAccountId={TradingAccountId}&MaxResults={maxResults}";
            return _client.Request<ListStopLimitOrderHistoryResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for a specified trading account's stop / limit order history. The result set includes __only orders that were originally stop / limit orders__ that currently have one of the following statuses __(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)__.  There is currently no corresponding GetStopLimitOrderHistory *(as with [ListActiveStopLimitOrders](http://labs.cityindex.com/docs/#HTTP%20Services/ListActiveStopLimitOrders.htm))*.
        /// </summary>
        /// <param name="TradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListStopLimitOrderHistory(int TradingAccountId, int maxResults, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/stoplimitorderhistory?TradingAccountId={TradingAccountId}&MaxResults={maxResults}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "TradingAccountId", TradingAccountId}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListStopLimitOrderHistoryResponseDTO EndListStopLimitOrderHistory(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListStopLimitOrderHistoryResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetOrder
        // ***********************************


        /// <summary>
        /// Queries for an order by a specific order ID. The current implementation only returns active orders *(i.e. those with a status of __1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered__)*.
        /// </summary>
        /// <param name="OrderId">The requested order ID.</param>
        public virtual GetOrderResponseDTO GetOrder(string OrderId)
        {
            string uriTemplate = "/{OrderId}";
            return _client.Request<GetOrderResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "OrderId", OrderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Queries for an order by a specific order ID. The current implementation only returns active orders *(i.e. those with a status of __1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered__)*.
        /// </summary>
        /// <param name="OrderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOrder(string OrderId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{OrderId}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "OrderId", OrderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public GetOrderResponseDTO EndGetOrder(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // Trade
        // ***********************************


        /// <summary>
        /// Place a trade on a particular market.  Do not set any order ID fields when requesting a new trade, the platform will generate them.
        /// </summary>
        /// <param name="trade">The trade request.</param>
        public virtual ApiTradeOrderResponseDTO Trade(NewTradeOrderRequestDTO trade)
        {
            string uriTemplate = "/newtradeorder";
            return _client.Request<ApiTradeOrderResponseDTO>(RequestMethod.POST,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "trade", trade}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Place a trade on a particular market.  Do not set any order ID fields when requesting a new trade, the platform will generate them.
        /// </summary>
        /// <param name="trade">The trade request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginTrade(NewTradeOrderRequestDTO trade, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/newtradeorder";
            _client.BeginRequest(RequestMethod.POST, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "trade", trade}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiTradeOrderResponseDTO EndTrade(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiTradeOrderResponseDTO>(asyncResult);
        }


        // ***********************************
        // UpdateTrade
        // ***********************************


        /// <summary>
        /// Update a trade *(for adding a stop/limit etc)*.
        /// </summary>
        /// <param name="update">The update trade request.</param>
        public virtual ApiTradeOrderResponseDTO UpdateTrade(UpdateTradeOrderRequestDTO update)
        {
            string uriTemplate = "/updatetradeorder";
            return _client.Request<ApiTradeOrderResponseDTO>(RequestMethod.POST,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "update", update}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Update a trade *(for adding a stop/limit etc)*.
        /// </summary>
        /// <param name="update">The update trade request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginUpdateTrade(UpdateTradeOrderRequestDTO update, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/updatetradeorder";
            _client.BeginRequest(RequestMethod.POST, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "update", update}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiTradeOrderResponseDTO EndUpdateTrade(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiTradeOrderResponseDTO>(asyncResult);
        }


        }            
        public class _AccountInformation
        {
            private Client _client;
            public _AccountInformation(Client client){ this._client = client;}

        // ***********************************
        // GetClientAndTradingAccount
        // ***********************************


        /// <summary>
        /// Returns the User's ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>
        public virtual AccountInformationResponseDTO GetClientAndTradingAccount()
        {
            string uriTemplate = "/ClientAndTradingAccount";
            return _client.Request<AccountInformationResponseDTO>(RequestMethod.GET,"useraccount", uriTemplate ,
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns the User's ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientAndTradingAccount( ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/ClientAndTradingAccount";
            _client.BeginRequest(RequestMethod.GET, "useraccount", uriTemplate , 
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public AccountInformationResponseDTO EndGetClientAndTradingAccount(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<AccountInformationResponseDTO>(asyncResult);
        }


        // ***********************************
        // SaveAccountInformation
        // ***********************************


        /// <summary>
        /// Saves the users account information.
        /// </summary>
        /// <param name="saveAccountInformationRequest">Saves the users account information.</param>
        public virtual ApiSaveAccountInformationResponseDTO SaveAccountInformation(ApiSaveAccountInformationRequestDTO saveAccountInformationRequest)
        {
            string uriTemplate = "/Save";
            return _client.Request<ApiSaveAccountInformationResponseDTO>(RequestMethod.POST,"useraccount", uriTemplate ,
            new Dictionary<string, object>
            {
                { "saveAccountInformationRequest", saveAccountInformationRequest}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Saves the users account information.
        /// </summary>
        /// <param name="saveAccountInformationRequest">Saves the users account information.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveAccountInformation(ApiSaveAccountInformationRequestDTO saveAccountInformationRequest, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/Save";
            _client.BeginRequest(RequestMethod.POST, "useraccount", uriTemplate , 
            new Dictionary<string, object>
            {
                { "saveAccountInformationRequest", saveAccountInformationRequest}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiSaveAccountInformationResponseDTO EndSaveAccountInformation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiSaveAccountInformationResponseDTO>(asyncResult);
        }


        }            
        public class _Messaging
        {
            private Client _client;
            public _Messaging(Client client){ this._client = client;}

        // ***********************************
        // GetSystemLookup
        // ***********************************


        /// <summary>
        /// Use the message lookup service to get localised textual names for the various status code & IDs returned by the API. For example, a query for **OrderStatusReason** will contain text names for all the possible values of **OrderStatusReason** in the [ApiOrderResponseDTO](http://labs.cityindex.com/docs/#Data%20Types/ApiOrderResponseDTO.htm). You should only request the list once per session *(for each entity you're interested in)*.
        /// </summary>
        /// <param name="LookupEntityName">The entity to lookup. For example: **OrderStatusReason**, **InstructionStatusReason**, **OrderApplicability** or **Culture**.</param>
        /// <param name="CultureId">The Culture ID used to override the translated text description. *(Optional)*.</param>
        public virtual ApiLookupResponseDTO GetSystemLookup(string LookupEntityName, int CultureId)
        {
            string uriTemplate = "/lookup?LookupEntityName={LookupEntityName}&CultureId={CultureId}";
            return _client.Request<ApiLookupResponseDTO>(RequestMethod.GET,"message", uriTemplate ,
            new Dictionary<string, object>
            {
                { "LookupEntityName", LookupEntityName}, 
                { "CultureId", CultureId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(3600000),30000,0 );
        }


        /// <summary>
        /// Use the message lookup service to get localised textual names for the various status code & IDs returned by the API. For example, a query for **OrderStatusReason** will contain text names for all the possible values of **OrderStatusReason** in the [ApiOrderResponseDTO](http://labs.cityindex.com/docs/#Data%20Types/ApiOrderResponseDTO.htm). You should only request the list once per session *(for each entity you're interested in)*.
        /// </summary>
        /// <param name="LookupEntityName">The entity to lookup. For example: **OrderStatusReason**, **InstructionStatusReason**, **OrderApplicability** or **Culture**.</param>
        /// <param name="CultureId">The Culture ID used to override the translated text description. *(Optional)*.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetSystemLookup(string LookupEntityName, int CultureId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/lookup?LookupEntityName={LookupEntityName}&CultureId={CultureId}";
            _client.BeginRequest(RequestMethod.GET, "message", uriTemplate , 
            new Dictionary<string, object>
            {
                { "LookupEntityName", LookupEntityName}, 
                { "CultureId", CultureId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(3600000), 30000,2 ,callback, state);
        }

        public ApiLookupResponseDTO EndGetSystemLookup(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiLookupResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetClientApplicationMessageTranslation
        // ***********************************


        /// <summary>
        /// Use the message translation service to get client specific translated text strings.
        /// </summary>
        /// <param name="ClientApplicationId">Client application identifier. *(Optional)*</param>
        /// <param name="CultureId">Culture ID which corresponds to a culture code. *(Optional)*</param>
        /// <param name="AccountOperatorId">Account operator identifier. *(Optional)*</param>
        public virtual ApiClientApplicationMessageTranslationResponseDTO GetClientApplicationMessageTranslation(int ClientApplicationId, int CultureId, int AccountOperatorId)
        {
            string uriTemplate = "/translation?ClientApplicationId={ClientApplicationId}&CultureId={CultureId}&AccountOperatorId={AccountOperatorId}";
            return _client.Request<ApiClientApplicationMessageTranslationResponseDTO>(RequestMethod.GET,"message", uriTemplate ,
            new Dictionary<string, object>
            {
                { "ClientApplicationId", ClientApplicationId}, 
                { "CultureId", CultureId}, 
                { "AccountOperatorId", AccountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(3600000),30000,0 );
        }


        /// <summary>
        /// Use the message translation service to get client specific translated text strings.
        /// </summary>
        /// <param name="ClientApplicationId">Client application identifier. *(Optional)*</param>
        /// <param name="CultureId">Culture ID which corresponds to a culture code. *(Optional)*</param>
        /// <param name="AccountOperatorId">Account operator identifier. *(Optional)*</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientApplicationMessageTranslation(int ClientApplicationId, int CultureId, int AccountOperatorId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/translation?ClientApplicationId={ClientApplicationId}&CultureId={CultureId}&AccountOperatorId={AccountOperatorId}";
            _client.BeginRequest(RequestMethod.GET, "message", uriTemplate , 
            new Dictionary<string, object>
            {
                { "ClientApplicationId", ClientApplicationId}, 
                { "CultureId", CultureId}, 
                { "AccountOperatorId", AccountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(3600000), 30000,2 ,callback, state);
        }

        public ApiClientApplicationMessageTranslationResponseDTO EndGetClientApplicationMessageTranslation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiClientApplicationMessageTranslationResponseDTO>(asyncResult);
        }


        // ***********************************
        // GetClientApplicationMessageTranslationWithInterestingItems
        // ***********************************


        /// <summary>
        /// Use the message translation service to get client specific translated textual strings for specific keys.
        /// </summary>
        /// <param name="apiClientApplicationMessageTranslationRequestDto">DTO of the required data for translation lookup.</param>
        public virtual ApiClientApplicationMessageTranslationResponseDTO GetClientApplicationMessageTranslationWithInterestingItems(ApiClientApplicationMessageTranslationRequestDTO apiClientApplicationMessageTranslationRequestDto)
        {
            string uriTemplate = "/translationWithInterestingItems";
            return _client.Request<ApiClientApplicationMessageTranslationResponseDTO>(RequestMethod.POST,"message", uriTemplate ,
            new Dictionary<string, object>
            {
                { "apiClientApplicationMessageTranslationRequestDto", apiClientApplicationMessageTranslationRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Use the message translation service to get client specific translated textual strings for specific keys.
        /// </summary>
        /// <param name="apiClientApplicationMessageTranslationRequestDto">DTO of the required data for translation lookup.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientApplicationMessageTranslationWithInterestingItems(ApiClientApplicationMessageTranslationRequestDTO apiClientApplicationMessageTranslationRequestDto, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/translationWithInterestingItems";
            _client.BeginRequest(RequestMethod.POST, "message", uriTemplate , 
            new Dictionary<string, object>
            {
                { "apiClientApplicationMessageTranslationRequestDto", apiClientApplicationMessageTranslationRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiClientApplicationMessageTranslationResponseDTO EndGetClientApplicationMessageTranslationWithInterestingItems(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiClientApplicationMessageTranslationResponseDTO>(asyncResult);
        }


        }            
        public class _Watchlist
        {
            private Client _client;
            public _Watchlist(Client client){ this._client = client;}

        // ***********************************
        // GetWatchlists
        // ***********************************


        /// <summary>
        /// Gets all watchlists for the user account. There are no parameters for this call.
        /// </summary>
        public virtual ListWatchlistResponseDTO GetWatchlists()
        {
            string uriTemplate = "/";
            return _client.Request<ListWatchlistResponseDTO>(RequestMethod.GET,"watchlists", uriTemplate ,
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Gets all watchlists for the user account. There are no parameters for this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetWatchlists( ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/";
            _client.BeginRequest(RequestMethod.GET, "watchlists", uriTemplate , 
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ListWatchlistResponseDTO EndGetWatchlists(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ListWatchlistResponseDTO>(asyncResult);
        }


        // ***********************************
        // SaveWatchlist
        // ***********************************


        /// <summary>
        /// Save watchlist.
        /// </summary>
        /// <param name="apiSaveWatchlistRequestDto">The watchlist to save.</param>
        public virtual ApiSaveWatchlistResponseDTO SaveWatchlist(ApiSaveWatchlistRequestDTO apiSaveWatchlistRequestDto)
        {
            string uriTemplate = "/Save";
            return _client.Request<ApiSaveWatchlistResponseDTO>(RequestMethod.POST,"watchlist", uriTemplate ,
            new Dictionary<string, object>
            {
                { "apiSaveWatchlistRequestDto", apiSaveWatchlistRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Save watchlist.
        /// </summary>
        /// <param name="apiSaveWatchlistRequestDto">The watchlist to save.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveWatchlist(ApiSaveWatchlistRequestDTO apiSaveWatchlistRequestDto, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/Save";
            _client.BeginRequest(RequestMethod.POST, "watchlist", uriTemplate , 
            new Dictionary<string, object>
            {
                { "apiSaveWatchlistRequestDto", apiSaveWatchlistRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiSaveWatchlistResponseDTO EndSaveWatchlist(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiSaveWatchlistResponseDTO>(asyncResult);
        }


        // ***********************************
        // DeleteWatchlist
        // ***********************************


        /// <summary>
        /// Delete a watchlist.
        /// </summary>
        /// <param name="deleteWatchlistRequestDto">The watchlist to delete.</param>
        public virtual ApiDeleteWatchlistResponseDTO DeleteWatchlist(ApiDeleteWatchlistRequestDTO deleteWatchlistRequestDto)
        {
            string uriTemplate = "/delete";
            return _client.Request<ApiDeleteWatchlistResponseDTO>(RequestMethod.POST,"watchlist", uriTemplate ,
            new Dictionary<string, object>
            {
                { "deleteWatchlistRequestDto", deleteWatchlistRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Delete a watchlist.
        /// </summary>
        /// <param name="deleteWatchlistRequestDto">The watchlist to delete.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginDeleteWatchlist(ApiDeleteWatchlistRequestDTO deleteWatchlistRequestDto, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/delete";
            _client.BeginRequest(RequestMethod.POST, "watchlist", uriTemplate , 
            new Dictionary<string, object>
            {
                { "deleteWatchlistRequestDto", deleteWatchlistRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiDeleteWatchlistResponseDTO EndDeleteWatchlist(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiDeleteWatchlistResponseDTO>(asyncResult);
        }


        }            
        public class _ClientApplication
        {
            private Client _client;
            public _ClientApplication(Client client){ this._client = client;}

        // ***********************************
        // GetVersionInformation
        // ***********************************


        /// <summary>
        /// Gets version information for a specific client application and *(optionally)* account operator.
        /// </summary>
        /// <param name="AppKey">A string to uniquely identify the application.</param>
        /// <param name="AccountOperatorId">An optional parameter to identify the account operator string to uniquely identify the application.</param>
        public virtual GetVersionInformationResponseDTO GetVersionInformation(string AppKey, int AccountOperatorId)
        {
            string uriTemplate = "?AppKey={AppKey}&AccountOperatorId={AccountOperatorId}";
            return _client.Request<GetVersionInformationResponseDTO>(RequestMethod.GET,"clientapplication/versioninformation", uriTemplate ,
            new Dictionary<string, object>
            {
                { "AppKey", AppKey}, 
                { "AccountOperatorId", AccountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(360000),30000,0 );
        }


        /// <summary>
        /// Gets version information for a specific client application and *(optionally)* account operator.
        /// </summary>
        /// <param name="AppKey">A string to uniquely identify the application.</param>
        /// <param name="AccountOperatorId">An optional parameter to identify the account operator string to uniquely identify the application.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetVersionInformation(string AppKey, int AccountOperatorId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "?AppKey={AppKey}&AccountOperatorId={AccountOperatorId}";
            _client.BeginRequest(RequestMethod.GET, "clientapplication/versioninformation", uriTemplate , 
            new Dictionary<string, object>
            {
                { "AppKey", AppKey}, 
                { "AccountOperatorId", AccountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(360000), 30000,2 ,callback, state);
        }

        public GetVersionInformationResponseDTO EndGetVersionInformation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetVersionInformationResponseDTO>(asyncResult);
        }


        }            
    }
}