using System;
using System.Collections.Generic;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Serialization;
using CIAPI.Serialization;
using CIAPI.DTO;
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
      public _ExceptionHandling ExceptionHandling{get; private set;}
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
            this. ExceptionHandling = new _ExceptionHandling(this);
        Log.Debug("Rpc.Client created for " + _rootUri.AbsoluteUri);
        }
        public Client(Uri rpcUri, Uri streamingUri, string appKey,IJsonSerializer serializer, IRequestFactory factory)
            : base(serializer, factory)
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
            this. ExceptionHandling = new _ExceptionHandling(this);
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
        /// <param name="apiLogOnRequest">The request to create a session (log on).</param>
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
        /// <param name="apiLogOnRequest">The request to create a session (log on).</param>
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
        /// <p>Delete a session. This is how you "log off" from the CIAPI.</p>
        /// </summary>
        /// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
        /// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        internal virtual ApiLogOffResponseDTO DeleteSession(string userName, string session)
        {
            string uriTemplate = "/deleteSession?userName={userName}&session={session}";
            return _client.Request<ApiLogOffResponseDTO>(RequestMethod.POST,"session", uriTemplate ,
            new Dictionary<string, object>
            {
                { "userName", userName}, 
                { "session", session}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Delete a session. This is how you "log off" from the CIAPI.</p>
        /// </summary>
        /// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
        /// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal virtual void BeginDeleteSession(string userName, string session, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/deleteSession?userName={userName}&session={session}";
            _client.BeginRequest(RequestMethod.POST, "session", uriTemplate , 
            new Dictionary<string, object>
            {
                { "userName", userName}, 
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
        /// Get historic price bars for the specified market in OHLC (open, high, low, close) format, suitable for plotting in candlestick charts. Returns price bars in ascending order up to the current time. When there are no prices for a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has "gaps", i.e. the gap between the date & time of each price bar might not be equal to interval x span. Sample Urls: <ul> <li>/market/1234/history?interval=MINUTE&span=15&pricebars=180</li> <li>/market/735/history?interval=HOUR&span=1&pricebars=240</li> <li>/market/1577/history?interval=DAY&span=1&pricebars=10</li> </ul>
        /// </summary>
        /// <param name="marketId">The ID of the market.</param>
        /// <param name="interval">The pricebar interval.</param>
        /// <param name="span">The number of each interval per pricebar.</param>
        /// <param name="priceBars">The total number of pricebars to return.</param>
        public virtual GetPriceBarResponseDTO GetPriceBars(string marketId, string interval, int span, string priceBars)
        {
            string uriTemplate = "/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}";
            return _client.Request<GetPriceBarResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "priceBars", priceBars}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get historic price bars for the specified market in OHLC (open, high, low, close) format, suitable for plotting in candlestick charts. Returns price bars in ascending order up to the current time. When there are no prices for a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has "gaps", i.e. the gap between the date & time of each price bar might not be equal to interval x span. Sample Urls: <ul> <li>/market/1234/history?interval=MINUTE&span=15&pricebars=180</li> <li>/market/735/history?interval=HOUR&span=1&pricebars=240</li> <li>/market/1577/history?interval=DAY&span=1&pricebars=10</li> </ul>
        /// </summary>
        /// <param name="marketId">The ID of the market.</param>
        /// <param name="interval">The pricebar interval.</param>
        /// <param name="span">The number of each interval per pricebar.</param>
        /// <param name="priceBars">The total number of pricebars to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceBars(string marketId, string interval, int span, string priceBars, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "priceBars", priceBars}
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
        /// <param name="marketId">The market ID.</param>
        /// <param name="priceTicks">The total number of price ticks to return.</param>
        public virtual GetPriceTickResponseDTO GetPriceTicks(string marketId, string priceTicks)
        {
            string uriTemplate = "/{marketId}/tickhistory?priceticks={priceTicks}";
            return _client.Request<GetPriceTickResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "priceTicks", priceTicks}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Get historic price ticks for the specified market. Returns price ticks in ascending order up to the current time. The length of time that elapses between each tick is usually different.
        /// </summary>
        /// <param name="marketId">The market ID.</param>
        /// <param name="priceTicks">The total number of price ticks to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceTicks(string marketId, string priceTicks, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{marketId}/tickhistory?priceticks={priceTicks}";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "priceTicks", priceTicks}
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
        /// <param name="source">The news feed source provider. Valid options are: dj|mni|ci.</param>
        /// <param name="category">Filter headlines by category. Valid categories depend on the source used:  for dj: uk|aus, for ci: SEMINARSCHINA, for mni: ALL.</param>
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
        /// <param name="source">The news feed source provider. Valid options are: dj|mni|ci.</param>
        /// <param name="category">Filter headlines by category. Valid categories depend on the source used:  for dj: uk|aus, for ci: SEMINARSCHINA, for mni: ALL.</param>
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
        // GetNewsDetail
        // ***********************************


        /// <summary>
        /// Get the detail of the specific news story matching the story ID in the parameter.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are dj|mni|ci.</param>
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
        /// <param name="source">The news feed source provider. Valid options are dj|mni|ci.</param>
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
        /// <param name="searchByMarketName">The characters that the CFD market name starts with. (Optional).</param>
        /// <param name="searchByMarketCode">The characters that the market code starts with, normally this is the RIC code for the market. (Optional).</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId. This only shows you the markets that the user can trade. (Required).</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        public virtual ListCfdMarketsResponseDTO ListCfdMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<ListCfdMarketsResponseDTO>(RequestMethod.GET,"cfd/markets", uriTemplate ,
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns a list of CFD markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.
        /// </summary>
        /// <param name="searchByMarketName">The characters that the CFD market name starts with. (Optional).</param>
        /// <param name="searchByMarketCode">The characters that the market code starts with, normally this is the RIC code for the market. (Optional).</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId. This only shows you the markets that the user can trade. (Required).</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListCfdMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "cfd/markets", uriTemplate , 
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
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
        /// <param name="searchByMarketName">The characters that the Spread market name starts with. (Optional).</param>
        /// <param name="searchByMarketCode">The characters that the Spread market code starts with, normally this is the RIC code for the market. (Optional).</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId. (This only shows you markets that you can trade on.)</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        public virtual ListSpreadMarketsResponseDTO ListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, bool useMobileShortName)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            return _client.Request<ListSpreadMarketsResponseDTO>(RequestMethod.GET,"spread/markets", uriTemplate ,
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Returns a list of Spread Betting markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.
        /// </summary>
        /// <param name="searchByMarketName">The characters that the Spread market name starts with. (Optional).</param>
        /// <param name="searchByMarketCode">The characters that the Spread market code starts with, normally this is the RIC code for the market. (Optional).</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId. (This only shows you markets that you can trade on.)</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="useMobileShortName">True if the market name should be in short form. Helpful when displaying data on a small screen.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, bool useMobileShortName, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}";
            _client.BeginRequest(RequestMethod.GET, "spread/markets", uriTemplate , 
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
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
        /// <p>Get Market Information for the single specified market supplied in the parameter.</p>
        /// </summary>
        /// <param name="marketId">The market ID.</param>
        public virtual GetMarketInformationResponseDTO GetMarketInformation(string marketId)
        {
            string uriTemplate = "/{marketId}/information";
            return _client.Request<GetMarketInformationResponseDTO>(RequestMethod.GET,"market", uriTemplate ,
            new Dictionary<string, object>
            {
                { "marketId", marketId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(1000),30000,0 );
        }


        /// <summary>
        /// <p>Get Market Information for the single specified market supplied in the parameter.</p>
        /// </summary>
        /// <param name="marketId">The market ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetMarketInformation(string marketId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{marketId}/information";
            _client.BeginRequest(RequestMethod.GET, "market", uriTemplate , 
            new Dictionary<string, object>
            {
                { "marketId", marketId}
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
        /// <p>Returns market information for the markets that meet the search criteria.</p> The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.
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
        /// <p>Returns market information for the markets that meet the search criteria.</p> The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.
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
        /// <p>Returns market information for the markets that meet the search criteria.</p> The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.
        /// </summary>
        /// <param name="searchByMarketCode">Sets the search to use market code.</param>
        /// <param name="searchByMarketName">Sets the search to use market Name.</param>
        /// <param name="spreadProductType">Sets the search to include spread bet markets.</param>
        /// <param name="cfdProductType">Sets the search to include CFD markets.</param>
        /// <param name="binaryProductType">Sets the search to include binary markets.</param>
        /// <param name="query">The text to search for. Matches part of market name / code from the start.</param>
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
        /// <p>Returns market information for the markets that meet the search criteria.</p> The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.
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
        /// Get market information and tags for the markets that meet the search criteria.
        /// </summary>
        /// <param name="query">The text to search for. Matches part of market name / code from the start.</param>
        /// <param name="tagId">The ID for the tag to be searched. (Optional).</param>
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
        /// Get market information and tags for the markets that meet the search criteria.
        /// </summary>
        /// <param name="query">The text to search for. Matches part of market name / code from the start.</param>
        /// <param name="tagId">The ID for the tag to be searched. (Optional).</param>
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
        /// <p>Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy.</p> There are no parameters in this call.
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
        /// <p>Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy.</p> There are no parameters in this call.
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
        /// <param name="saveClientPreferenceRequestDTO">Save client preferences.</param>
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
        /// <param name="saveClientPreferenceRequestDTO">Save client preferences.</param>
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
        /// get client preferences.
        /// </summary>
        /// <param name="clientPreferenceRequestDto">get client preferences.</param>
        public virtual GetClientPreferenceResponseDTO Get(ClientPreferenceRequestDTO clientPreferenceRequestDto)
        {
            string uriTemplate = "/get";
            return _client.Request<GetClientPreferenceResponseDTO>(RequestMethod.POST,"clientpreference/save", uriTemplate ,
            new Dictionary<string, object>
            {
                { "clientPreferenceRequestDto", clientPreferenceRequestDto}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// get client preferences.
        /// </summary>
        /// <param name="clientPreferenceRequestDto">get client preferences.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGet(ClientPreferenceRequestDTO clientPreferenceRequestDto, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/get";
            _client.BeginRequest(RequestMethod.POST, "clientpreference/save", uriTemplate , 
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
        /// get list of client preferences keys.
        /// </summary>
        public virtual GetKeyListClientPreferenceResponseDTO GetKeyList()
        {
            string uriTemplate = "/getkeylist";
            return _client.Request<GetKeyListClientPreferenceResponseDTO>(RequestMethod.GET,"clientpreference/save", uriTemplate ,
            new Dictionary<string, object>
            {

            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// get list of client preferences keys.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetKeyList( ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/getkeylist";
            _client.BeginRequest(RequestMethod.GET, "clientpreference/save", uriTemplate , 
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
        /// delete client preference key.
        /// </summary>
        /// <param name="clientPreferenceKey">delete client preference key.</param>
        public virtual UpdateDeleteClientPreferenceResponseDTO Delete(ClientPreferenceRequestDTO clientPreferenceKey)
        {
            string uriTemplate = "/delete";
            return _client.Request<UpdateDeleteClientPreferenceResponseDTO>(RequestMethod.POST,"clientpreference/save", uriTemplate ,
            new Dictionary<string, object>
            {
                { "clientPreferenceKey", clientPreferenceKey}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// delete client preference key.
        /// </summary>
        /// <param name="clientPreferenceKey">delete client preference key.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginDelete(ClientPreferenceRequestDTO clientPreferenceKey, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/delete";
            _client.BeginRequest(RequestMethod.POST, "clientpreference/save", uriTemplate , 
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
        /// <p>Place an order on a particular market. <p>Do not set any order ID fields when requesting a new order, the platform will generate them.</p>
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
        /// <p>Place an order on a particular market. <p>Do not set any order ID fields when requesting a new order, the platform will generate them.</p>
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
        /// <p>Cancel an order.
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
        /// <p>Cancel an order.
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
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship).
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
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship).
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
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service GetOpenPosition when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        public virtual ListOpenPositionsResponseDTO ListOpenPositions(int tradingAccountId)
        {
            string uriTemplate = "/openpositions?TradingAccountId={tradingAccountId}";
            return _client.Request<ListOpenPositionsResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service GetOpenPosition when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListOpenPositions(int tradingAccountId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/openpositions?TradingAccountId={tradingAccountId}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
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
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service GetActiveStopLimitOrder when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        public virtual ListActiveStopLimitOrderResponseDTO ListActiveStopLimitOrders(int tradingAccountId)
        {
            string uriTemplate = "/activestoplimitorders?TradingAccountId={tradingAccountId}";
            return _client.Request<ListActiveStopLimitOrderResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service GetActiveStopLimitOrder when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListActiveStopLimitOrders(int tradingAccountId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/activestoplimitorders?TradingAccountId={tradingAccountId}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
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
        /// <p>Queries for an active stop limit order with a specified order ID. It returns a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service ListActiveStopLimitOrders for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see the HTTP service GetOrder.
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        public virtual GetActiveStopLimitOrderResponseDTO GetActiveStopLimitOrder(string orderId)
        {
            string uriTemplate = "/{orderId}/activestoplimitorder";
            return _client.Request<GetActiveStopLimitOrderResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for an active stop limit order with a specified order ID. It returns a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service ListActiveStopLimitOrders for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see the HTTP service GetOrder.
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetActiveStopLimitOrder(string orderId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{orderId}/activestoplimitorder";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "orderId", orderId}
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
        /// <p>Queries for a trade / open position with a specified order ID. It returns a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service ListOpenPositions for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see the HTTP service GetOrder.
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        public virtual GetOpenPositionResponseDTO GetOpenPosition(string orderId)
        {
            string uriTemplate = "/{orderId}/openposition";
            return _client.Request<GetOpenPositionResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for a trade / open position with a specified order ID. It returns a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service ListOpenPositions for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see the HTTP service GetOrder.
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOpenPosition(string orderId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{orderId}/openposition";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "orderId", orderId}
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
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        public virtual ListTradeHistoryResponseDTO ListTradeHistory(int tradingAccountId, int maxResults)
        {
            string uriTemplate = "/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}";
            return _client.Request<ListTradeHistoryResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListTradeHistory(int tradingAccountId, int maxResults, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
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
        /// <p>Queries for a specified trading account's stop / limit order history. The result set includes <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b>. </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        public virtual ListStopLimitOrderHistoryResponseDTO ListStopLimitOrderHistory(int tradingAccountId, int maxResults)
        {
            string uriTemplate = "/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}";
            return _client.Request<ListStopLimitOrderHistoryResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for a specified trading account's stop / limit order history. The result set includes <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b>. </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListStopLimitOrderHistory(int tradingAccountId, int maxResults, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
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
        /// <p>Queries for an order by a specific order ID.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered</b>).</p>
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        public virtual GetOrderResponseDTO GetOrder(string orderId)
        {
            string uriTemplate = "/{orderId}";
            return _client.Request<GetOrderResponseDTO>(RequestMethod.GET,"order", uriTemplate ,
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// <p>Queries for an order by a specific order ID.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered</b>).</p>
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOrder(string orderId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/{orderId}";
            _client.BeginRequest(RequestMethod.GET, "order", uriTemplate , 
            new Dictionary<string, object>
            {
                { "orderId", orderId}
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
        /// <p>Place a trade on a particular market.</p> <p>Do not set any order ID fields when requesting a new trade, the platform will generate them.</p>
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
        /// <p>Place a trade on a particular market.</p> <p>Do not set any order ID fields when requesting a new trade, the platform will generate them.</p>
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
        /// Update a trade (for adding a stop/limit etc).
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
        /// Update a trade (for adding a stop/limit etc).
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
        /// Use the message lookup service to get localised textual names for the various status code & Ids returned by the API. For example, a query for OrderStatusReasons will contain text names for all the possible values of OrderStatusReason in the ApiOrderResponseDTO. You should only request the list once per session (for each entity you're interested in).
        /// </summary>
        /// <param name="lookupEntityName">The entity to lookup (eg OrderStatusReason, InstructionStatusReason, OrderApplicability or Culture).</param>
        /// <param name="cultureId">The cultureId used to override the translated text description. (Optional)</param>
        public virtual ApiLookupResponseDTO GetSystemLookup(string lookupEntityName, int cultureId)
        {
            string uriTemplate = "/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}";
            return _client.Request<ApiLookupResponseDTO>(RequestMethod.GET,"message", uriTemplate ,
            new Dictionary<string, object>
            {
                { "lookupEntityName", lookupEntityName}, 
                { "cultureId", cultureId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(3600000),30000,0 );
        }


        /// <summary>
        /// Use the message lookup service to get localised textual names for the various status code & Ids returned by the API. For example, a query for OrderStatusReasons will contain text names for all the possible values of OrderStatusReason in the ApiOrderResponseDTO. You should only request the list once per session (for each entity you're interested in).
        /// </summary>
        /// <param name="lookupEntityName">The entity to lookup (eg OrderStatusReason, InstructionStatusReason, OrderApplicability or Culture).</param>
        /// <param name="cultureId">The cultureId used to override the translated text description. (Optional)</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetSystemLookup(string lookupEntityName, int cultureId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}";
            _client.BeginRequest(RequestMethod.GET, "message", uriTemplate , 
            new Dictionary<string, object>
            {
                { "lookupEntityName", lookupEntityName}, 
                { "cultureId", cultureId}
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
        /// Use the message translation service to get client specific translated textual strings.
        /// </summary>
        /// <param name="clientApplicationId">Client application identifier. (Optional)</param>
        /// <param name="cultureId">CultureId which corresponds to a culture code. (Optional)</param>
        /// <param name="accountOperatorId">Account operator identifier. (Optional)</param>
        public virtual ApiClientApplicationMessageTranslationResponseDTO GetClientApplicationMessageTranslation(int clientApplicationId, int cultureId, int accountOperatorId)
        {
            string uriTemplate = "/translation?clientApplicationId={clientApplicationId}&cultureId={cultureId}&accountOperatorId={accountOperatorId}";
            return _client.Request<ApiClientApplicationMessageTranslationResponseDTO>(RequestMethod.GET,"message", uriTemplate ,
            new Dictionary<string, object>
            {
                { "clientApplicationId", clientApplicationId}, 
                { "cultureId", cultureId}, 
                { "accountOperatorId", accountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(3600000),30000,0 );
        }


        /// <summary>
        /// Use the message translation service to get client specific translated textual strings.
        /// </summary>
        /// <param name="clientApplicationId">Client application identifier. (Optional)</param>
        /// <param name="cultureId">CultureId which corresponds to a culture code. (Optional)</param>
        /// <param name="accountOperatorId">Account operator identifier. (Optional)</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientApplicationMessageTranslation(int clientApplicationId, int cultureId, int accountOperatorId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/translation?clientApplicationId={clientApplicationId}&cultureId={cultureId}&accountOperatorId={accountOperatorId}";
            _client.BeginRequest(RequestMethod.GET, "message", uriTemplate , 
            new Dictionary<string, object>
            {
                { "clientApplicationId", clientApplicationId}, 
                { "cultureId", cultureId}, 
                { "accountOperatorId", accountOperatorId}
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
        /// <param name="apiClientApplicationMessageTranslationRequestDto">Dto of the required data for translation lookup for user.</param>
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
        /// <param name="apiClientApplicationMessageTranslationRequestDto">Dto of the required data for translation lookup for user.</param>
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
        /// Gets version information for a specific client application and (optionally) account operator.
        /// </summary>
        /// <param name="appKey">a string to uniquely identify the application.</param>
        /// <param name="accountOperatorId">an optional parameter to identify the account operator string to uniquely identify the application.</param>
        public virtual GetVersionInformationResponseDTO GetVersionInformation(string appKey, int accountOperatorId)
        {
            string uriTemplate = "/versioninformation?AppKey={appKey}&AccountOperatorId={accountOperatorId}";
            return _client.Request<GetVersionInformationResponseDTO>(RequestMethod.GET,"clientapplication", uriTemplate ,
            new Dictionary<string, object>
            {
                { "appKey", appKey}, 
                { "accountOperatorId", accountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(360000),30000,0 );
        }


        /// <summary>
        /// Gets version information for a specific client application and (optionally) account operator.
        /// </summary>
        /// <param name="appKey">a string to uniquely identify the application.</param>
        /// <param name="accountOperatorId">an optional parameter to identify the account operator string to uniquely identify the application.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetVersionInformation(string appKey, int accountOperatorId, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "/versioninformation?AppKey={appKey}&AccountOperatorId={accountOperatorId}";
            _client.BeginRequest(RequestMethod.GET, "clientapplication", uriTemplate , 
            new Dictionary<string, object>
            {
                { "appKey", appKey}, 
                { "accountOperatorId", accountOperatorId}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(360000), 30000,2 ,callback, state);
        }

        public GetVersionInformationResponseDTO EndGetVersionInformation(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<GetVersionInformationResponseDTO>(asyncResult);
        }


        }            
        public class _ExceptionHandling
        {
            private Client _client;
            public _ExceptionHandling(Client client){ this._client = client;}

        // ***********************************
        // GenerateException
        // ***********************************


        /// <summary>
        /// Raises an error condition when an unexpected or uncontrolled event occurs.
        /// </summary>
        /// <param name="errorCode">The error code for the condition encountered.</param>
        public virtual ApiErrorResponseDTO GenerateException(int errorCode)
        {
            string uriTemplate = "?errorCode={errorCode}";
            return _client.Request<ApiErrorResponseDTO>(RequestMethod.GET,"errors", uriTemplate ,
            new Dictionary<string, object>
            {
                { "errorCode", errorCode}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0),30000,0 );
        }


        /// <summary>
        /// Raises an error condition when an unexpected or uncontrolled event occurs.
        /// </summary>
        /// <param name="errorCode">The error code for the condition encountered.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGenerateException(int errorCode, ReliableAsyncCallback callback, object state)
        {
            string uriTemplate = "?errorCode={errorCode}";
            _client.BeginRequest(RequestMethod.GET, "errors", uriTemplate , 
            new Dictionary<string, object>
            {
                { "errorCode", errorCode}
            },ContentType.JSON,ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000,2 ,callback, state);
        }

        public ApiErrorResponseDTO EndGenerateException(ReliableAsyncResult asyncResult)
        {
            return _client.EndRequest<ApiErrorResponseDTO>(asyncResult);
        }


        }            
    }
}