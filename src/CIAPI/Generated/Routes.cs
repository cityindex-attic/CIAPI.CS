using System;
using System.Collections.Generic;
using System.Reflection;
using CityIndex.JsonClient;
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
      public _TradesAndOrders TradesAndOrders{get; private set;}
      public _AccountInformation AccountInformation{get; private set;}
      public _Messaging Messaging{get; private set;}
      public _Watchlist Watchlist{get; private set;}
      public _ExceptionHandling ExceptionHandling{get; private set;}
private Client _client;
        public Client(Uri uri)
            : base(uri, new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(), new ErrorResponseDTOJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "trading"),new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "default")) )
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
        _client=this;

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. Market = new _Market(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ExceptionHandling = new _ExceptionHandling(this);
        }
        public Client(Uri uri, IRequestController requestController)
            : base(uri, requestController)
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
        _client=this;

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. Market = new _Market(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ExceptionHandling = new _ExceptionHandling(this);
        }            

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
            string uriTemplate = _client.AppendApiKey("/clientapplication/versioninformation?AppKey={appKey}&AccountOperatorId={accountOperatorId}");
            return _client.Request<GetVersionInformationResponseDTO>("clientapplication/versioninformation", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "appKey", appKey}, 
                { "accountOperatorId", accountOperatorId}
            }, TimeSpan.FromMilliseconds(360000), "default");
        }


        /// <summary>
        /// Gets version information for a specific client application and (optionally) account operator.
        /// </summary>
        /// <param name="appKey">a string to uniquely identify the application.</param>
        /// <param name="accountOperatorId">an optional parameter to identify the account operator string to uniquely identify the application.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetVersionInformation(string appKey, int accountOperatorId, ApiAsyncCallback<GetVersionInformationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/clientapplication/versioninformation?AppKey={appKey}&AccountOperatorId={accountOperatorId}");
            _client.BeginRequest(callback, state, "clientapplication/versioninformation", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "appKey", appKey}, 
                { "accountOperatorId", accountOperatorId}
            }, TimeSpan.FromMilliseconds(360000), "default");
        }

        public GetVersionInformationResponseDTO EndGetVersionInformation(ApiAsyncResult<GetVersionInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/");
            return _client.Request<ApiLogOnResponseDTO>("session", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "apiLogOnRequest", apiLogOnRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Create a new session. This is how you "log on" to the CIAPI.
        /// </summary>
        /// <param name="apiLogOnRequest">The request to create a session (log on).</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal virtual void BeginLogOn(ApiLogOnRequestDTO apiLogOnRequest, ApiAsyncCallback<ApiLogOnResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/");
            _client.BeginRequest(callback, state, "session", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "apiLogOnRequest", apiLogOnRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        internal ApiLogOnResponseDTO EndLogOn(ApiAsyncResult<ApiLogOnResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/deleteSession?userName={userName}&session={session}");
            return _client.Request<ApiLogOffResponseDTO>("session", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "userName", userName}, 
                { "session", session}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// <p>Delete a session. This is how you "log off" from the CIAPI.</p>
        /// </summary>
        /// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
        /// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal virtual void BeginDeleteSession(string userName, string session, ApiAsyncCallback<ApiLogOffResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/deleteSession?userName={userName}&session={session}");
            _client.BeginRequest(callback, state, "session", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "userName", userName}, 
                { "session", session}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        internal ApiLogOffResponseDTO EndDeleteSession(ApiAsyncResult<ApiLogOffResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/changePassword");
            return _client.Request<ApiChangePasswordResponseDTO>("session", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "apiChangePasswordRequest", apiChangePasswordRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Change a user's password.
        /// </summary>
        /// <param name="apiChangePasswordRequest">The change password request details.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginChangePassword(ApiChangePasswordRequestDTO apiChangePasswordRequest, ApiAsyncCallback<ApiChangePasswordResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/changePassword");
            _client.BeginRequest(callback, state, "session", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "apiChangePasswordRequest", apiChangePasswordRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ApiChangePasswordResponseDTO EndChangePassword(ApiAsyncResult<ApiChangePasswordResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}");
            return _client.Request<GetPriceBarResponseDTO>("market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "priceBars", priceBars}
            }, TimeSpan.FromMilliseconds(0), "data");
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
        public virtual void BeginGetPriceBars(string marketId, string interval, int span, string priceBars, ApiAsyncCallback<GetPriceBarResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}");
            _client.BeginRequest(callback, state, "market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "priceBars", priceBars}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public GetPriceBarResponseDTO EndGetPriceBars(ApiAsyncResult<GetPriceBarResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{marketId}/tickhistory?priceticks={priceTicks}");
            return _client.Request<GetPriceTickResponseDTO>("market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "priceTicks", priceTicks}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Get historic price ticks for the specified market. Returns price ticks in ascending order up to the current time. The length of time that elapses between each tick is usually different.
        /// </summary>
        /// <param name="marketId">The market ID.</param>
        /// <param name="priceTicks">The total number of price ticks to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceTicks(string marketId, string priceTicks, ApiAsyncCallback<GetPriceTickResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{marketId}/tickhistory?priceticks={priceTicks}");
            _client.BeginRequest(callback, state, "market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "priceTicks", priceTicks}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public GetPriceTickResponseDTO EndGetPriceTicks(ApiAsyncResult<GetPriceTickResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{source}/{category}?MaxResults={maxResults}");
            return _client.Request<ListNewsHeadlinesResponseDTO>("news", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "category", category}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Get a list of current news headlines.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are: dj|mni|ci.</param>
        /// <param name="category">Filter headlines by category. Valid categories depend on the source used:  for dj: uk|aus, for ci: SEMINARSCHINA, for mni: ALL.</param>
        /// <param name="maxResults">Specify the maximum number of headlines returned.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListNewsHeadlinesWithSource(string source, string category, int maxResults, ApiAsyncCallback<ListNewsHeadlinesResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{source}/{category}?MaxResults={maxResults}");
            _client.BeginRequest(callback, state, "news", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "category", category}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }

        public ListNewsHeadlinesResponseDTO EndListNewsHeadlinesWithSource(ApiAsyncResult<ListNewsHeadlinesResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{source}/{storyId}");
            return _client.Request<GetNewsDetailResponseDTO>("news", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "storyId", storyId}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Get the detail of the specific news story matching the story ID in the parameter.
        /// </summary>
        /// <param name="source">The news feed source provider. Valid options are dj|mni|ci.</param>
        /// <param name="storyId">The news story ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetNewsDetail(string source, string storyId, ApiAsyncCallback<GetNewsDetailResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{source}/{storyId}");
            _client.BeginRequest(callback, state, "news", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "source", source}, 
                { "storyId", storyId}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }

        public GetNewsDetailResponseDTO EndGetNewsDetail(ApiAsyncResult<GetNewsDetailResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            return _client.Request<ListCfdMarketsResponseDTO>("cfd/markets", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            }, TimeSpan.FromMilliseconds(0), "data");
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
        public virtual void BeginListCfdMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, bool useMobileShortName, ApiAsyncCallback<ListCfdMarketsResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            _client.BeginRequest(callback, state, "cfd/markets", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ListCfdMarketsResponseDTO EndListCfdMarkets(ApiAsyncResult<ListCfdMarketsResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            return _client.Request<ListSpreadMarketsResponseDTO>("spread/markets", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            }, TimeSpan.FromMilliseconds(0), "data");
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
        public virtual void BeginListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, bool useMobileShortName, ApiAsyncCallback<ListSpreadMarketsResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            _client.BeginRequest(callback, state, "spread/markets", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ListSpreadMarketsResponseDTO EndListSpreadMarkets(ApiAsyncResult<ListSpreadMarketsResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{marketId}/information");
            return _client.Request<GetMarketInformationResponseDTO>("market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}
            }, TimeSpan.FromMilliseconds(1000), "data");
        }


        /// <summary>
        /// <p>Get Market Information for the single specified market supplied in the parameter.</p>
        /// </summary>
        /// <param name="marketId">The market ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetMarketInformation(string marketId, ApiAsyncCallback<GetMarketInformationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{marketId}/information");
            _client.BeginRequest(callback, state, "market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}
            }, TimeSpan.FromMilliseconds(1000), "data");
        }

        public GetMarketInformationResponseDTO EndGetMarketInformation(ApiAsyncResult<GetMarketInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/market/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            return _client.Request<ListMarketInformationSearchResponseDTO>("market", uriTemplate , "GET",
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
            }, TimeSpan.FromMilliseconds(0), "data");
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
        public virtual void BeginListMarketInformationSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults, bool useMobileShortName, ApiAsyncCallback<ListMarketInformationSearchResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/market/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            _client.BeginRequest(callback, state, "market", uriTemplate , "GET",
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
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ListMarketInformationSearchResponseDTO EndListMarketInformationSearch(ApiAsyncResult<ListMarketInformationSearchResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/market/searchwithtags?Query={query}&TagId={tagId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            return _client.Request<MarketInformationSearchWithTagsResponseDTO>("market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "query", query}, 
                { "tagId", tagId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            }, TimeSpan.FromMilliseconds(0), "data");
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
        public virtual void BeginSearchWithTags(string query, int tagId, int maxResults, bool useMobileShortName, ApiAsyncCallback<MarketInformationSearchWithTagsResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/market/searchwithtags?Query={query}&TagId={tagId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}");
            _client.BeginRequest(callback, state, "market", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "query", query}, 
                { "tagId", tagId}, 
                { "maxResults", maxResults}, 
                { "useMobileShortName", useMobileShortName}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public MarketInformationSearchWithTagsResponseDTO EndSearchWithTags(ApiAsyncResult<MarketInformationSearchWithTagsResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // TagLookup
        // ***********************************


        /// <summary>
        /// <p>Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy.</p> There are no parameters in this call.
        /// </summary>
        public virtual MarketInformationTagLookupResponseDTO TagLookup()
        {
            string uriTemplate = _client.AppendApiKey("/market/taglookup");
            return _client.Request<MarketInformationTagLookupResponseDTO>("market", uriTemplate , "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// <p>Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy.</p> There are no parameters in this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginTagLookup( ApiAsyncCallback<MarketInformationTagLookupResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/market/taglookup");
            _client.BeginRequest(callback, state, "market", uriTemplate , "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public MarketInformationTagLookupResponseDTO EndTagLookup(ApiAsyncResult<MarketInformationTagLookupResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/market/information");
            return _client.Request<ListMarketInformationResponseDTO>("market", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestDTO", listMarketInformationRequestDTO}
            }, TimeSpan.FromMilliseconds(1000), "data");
        }


        /// <summary>
        /// Get Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestDTO">Get Market Information for the specified list of markets.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListMarketInformation(ListMarketInformationRequestDTO listMarketInformationRequestDTO, ApiAsyncCallback<ListMarketInformationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/market/information");
            _client.BeginRequest(callback, state, "market", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestDTO", listMarketInformationRequestDTO}
            }, TimeSpan.FromMilliseconds(1000), "data");
        }

        public ListMarketInformationResponseDTO EndListMarketInformation(ApiAsyncResult<ListMarketInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/market/information/save");
            return _client.Request<ApiSaveMarketInformationResponseDTO>("market", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestSaveDTO", listMarketInformationRequestSaveDTO}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Save Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestSaveDTO">Save Market Information for the specified list of markets.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveMarketInformation(SaveMarketInformationRequestDTO listMarketInformationRequestSaveDTO, ApiAsyncCallback<ApiSaveMarketInformationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/market/information/save");
            _client.BeginRequest(callback, state, "market", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestSaveDTO", listMarketInformationRequestSaveDTO}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ApiSaveMarketInformationResponseDTO EndSaveMarketInformation(ApiAsyncResult<ApiSaveMarketInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/newstoplimitorder");
            return _client.Request<ApiTradeOrderResponseDTO>("order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "order", order}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }


        /// <summary>
        /// <p>Place an order on a particular market. <p>Do not set any order ID fields when requesting a new order, the platform will generate them.</p>
        /// </summary>
        /// <param name="order">The order request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginOrder(NewStopLimitOrderRequestDTO order, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/newstoplimitorder");
            _client.BeginRequest(callback, state, "order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "order", order}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }

        public ApiTradeOrderResponseDTO EndOrder(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/cancel");
            return _client.Request<ApiTradeOrderResponseDTO>("order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "cancelOrder", cancelOrder}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Cancel an order.
        /// </summary>
        /// <param name="cancelOrder">The cancel order request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginCancelOrder(CancelOrderRequestDTO cancelOrder, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/cancel");
            _client.BeginRequest(callback, state, "order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "cancelOrder", cancelOrder}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public ApiTradeOrderResponseDTO EndCancelOrder(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/updatestoplimitorder");
            return _client.Request<ApiTradeOrderResponseDTO>("order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "order", order}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship).
        /// </summary>
        /// <param name="order">The update order request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginUpdateOrder(UpdateStopLimitOrderRequestDTO order, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/updatestoplimitorder");
            _client.BeginRequest(callback, state, "order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "order", order}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public ApiTradeOrderResponseDTO EndUpdateOrder(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/order/openpositions?TradingAccountId={tradingAccountId}");
            return _client.Request<ListOpenPositionsResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service GetOpenPosition when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListOpenPositions(int tradingAccountId, ApiAsyncCallback<ListOpenPositionsResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/order/openpositions?TradingAccountId={tradingAccountId}");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public ListOpenPositionsResponseDTO EndListOpenPositions(ApiAsyncResult<ListOpenPositionsResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/order/activestoplimitorders?TradingAccountId={tradingAccountId}");
            return _client.Request<ListActiveStopLimitOrderResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service GetActiveStopLimitOrder when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListActiveStopLimitOrders(int tradingAccountId, ApiAsyncCallback<ListActiveStopLimitOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/order/activestoplimitorders?TradingAccountId={tradingAccountId}");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public ListActiveStopLimitOrderResponseDTO EndListActiveStopLimitOrders(ApiAsyncResult<ListActiveStopLimitOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{orderId}/activestoplimitorder");
            return _client.Request<GetActiveStopLimitOrderResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for an active stop limit order with a specified order ID. It returns a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service ListActiveStopLimitOrders for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see the HTTP service GetOrder.
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetActiveStopLimitOrder(string orderId, ApiAsyncCallback<GetActiveStopLimitOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{orderId}/activestoplimitorder");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public GetActiveStopLimitOrderResponseDTO EndGetActiveStopLimitOrder(ApiAsyncResult<GetActiveStopLimitOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{orderId}/openposition");
            return _client.Request<GetOpenPositionResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a trade / open position with a specified order ID. It returns a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service ListOpenPositions for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see the HTTP service GetOrder.
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOpenPosition(string orderId, ApiAsyncCallback<GetOpenPositionResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{orderId}/openposition");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public GetOpenPositionResponseDTO EndGetOpenPosition(ApiAsyncResult<GetOpenPositionResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/order/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}");
            return _client.Request<ListTradeHistoryResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListTradeHistory(int tradingAccountId, int maxResults, ApiAsyncCallback<ListTradeHistoryResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/order/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public ListTradeHistoryResponseDTO EndListTradeHistory(ApiAsyncResult<ListTradeHistoryResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/order/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}");
            return _client.Request<ListStopLimitOrderHistoryResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's stop / limit order history. The result set includes <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b>. </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>
        /// <param name="tradingAccountId">The ID of the trading account to get orders for.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListStopLimitOrderHistory(int tradingAccountId, int maxResults, ApiAsyncCallback<ListStopLimitOrderHistoryResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/order/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public ListStopLimitOrderHistoryResponseDTO EndListStopLimitOrderHistory(ApiAsyncResult<ListStopLimitOrderHistoryResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/{orderId}");
            return _client.Request<GetOrderResponseDTO>("order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for an order by a specific order ID.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered</b>).</p>
        /// </summary>
        /// <param name="orderId">The requested order ID.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOrder(string orderId, ApiAsyncCallback<GetOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/{orderId}");
            _client.BeginRequest(callback, state, "order", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public GetOrderResponseDTO EndGetOrder(ApiAsyncResult<GetOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/newtradeorder");
            return _client.Request<ApiTradeOrderResponseDTO>("order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "trade", trade}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }


        /// <summary>
        /// <p>Place a trade on a particular market.</p> <p>Do not set any order ID fields when requesting a new trade, the platform will generate them.</p>
        /// </summary>
        /// <param name="trade">The trade request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginTrade(NewTradeOrderRequestDTO trade, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/newtradeorder");
            _client.BeginRequest(callback, state, "order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "trade", trade}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }

        public ApiTradeOrderResponseDTO EndTrade(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/updatetradeorder");
            return _client.Request<ApiTradeOrderResponseDTO>("order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "update", update}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }


        /// <summary>
        /// Update a trade (for adding a stop/limit etc).
        /// </summary>
        /// <param name="update">The update trade request.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginUpdateTrade(UpdateTradeOrderRequestDTO update, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/updatetradeorder");
            _client.BeginRequest(callback, state, "order", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "update", update}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }

        public ApiTradeOrderResponseDTO EndUpdateTrade(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/UserAccount/ClientAndTradingAccount");
            return _client.Request<AccountInformationResponseDTO>("useraccount", uriTemplate , "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Returns the User's ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientAndTradingAccount( ApiAsyncCallback<AccountInformationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/UserAccount/ClientAndTradingAccount");
            _client.BeginRequest(callback, state, "useraccount", uriTemplate , "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public AccountInformationResponseDTO EndGetClientAndTradingAccount(ApiAsyncResult<AccountInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/UserAccount/Save");
            return _client.Request<ApiSaveAccountInformationResponseDTO>("useraccount", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "saveAccountInformationRequest", saveAccountInformationRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Saves the users account information.
        /// </summary>
        /// <param name="saveAccountInformationRequest">Saves the users account information.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveAccountInformation(ApiSaveAccountInformationRequestDTO saveAccountInformationRequest, ApiAsyncCallback<ApiSaveAccountInformationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/UserAccount/Save");
            _client.BeginRequest(callback, state, "useraccount", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "saveAccountInformationRequest", saveAccountInformationRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ApiSaveAccountInformationResponseDTO EndSaveAccountInformation(ApiAsyncResult<ApiSaveAccountInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/message/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}");
            return _client.Request<ApiLookupResponseDTO>("message", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "lookupEntityName", lookupEntityName}, 
                { "cultureId", cultureId}
            }, TimeSpan.FromMilliseconds(3600000), "default");
        }


        /// <summary>
        /// Use the message lookup service to get localised textual names for the various status code & Ids returned by the API. For example, a query for OrderStatusReasons will contain text names for all the possible values of OrderStatusReason in the ApiOrderResponseDTO. You should only request the list once per session (for each entity you're interested in).
        /// </summary>
        /// <param name="lookupEntityName">The entity to lookup (eg OrderStatusReason, InstructionStatusReason, OrderApplicability or Culture).</param>
        /// <param name="cultureId">The cultureId used to override the translated text description. (Optional)</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetSystemLookup(string lookupEntityName, int cultureId, ApiAsyncCallback<ApiLookupResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/message/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}");
            _client.BeginRequest(callback, state, "message", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "lookupEntityName", lookupEntityName}, 
                { "cultureId", cultureId}
            }, TimeSpan.FromMilliseconds(3600000), "default");
        }

        public ApiLookupResponseDTO EndGetSystemLookup(ApiAsyncResult<ApiLookupResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/message/translation?clientApplicationId={clientApplicationId}&cultureId={cultureId}&accountOperatorId={accountOperatorId}");
            return _client.Request<ApiClientApplicationMessageTranslationResponseDTO>("message", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "clientApplicationId", clientApplicationId}, 
                { "cultureId", cultureId}, 
                { "accountOperatorId", accountOperatorId}
            }, TimeSpan.FromMilliseconds(3600000), "default");
        }


        /// <summary>
        /// Use the message translation service to get client specific translated textual strings.
        /// </summary>
        /// <param name="clientApplicationId">Client application identifier. (Optional)</param>
        /// <param name="cultureId">CultureId which corresponds to a culture code. (Optional)</param>
        /// <param name="accountOperatorId">Account operator identifier. (Optional)</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientApplicationMessageTranslation(int clientApplicationId, int cultureId, int accountOperatorId, ApiAsyncCallback<ApiClientApplicationMessageTranslationResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/message/translation?clientApplicationId={clientApplicationId}&cultureId={cultureId}&accountOperatorId={accountOperatorId}");
            _client.BeginRequest(callback, state, "message", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "clientApplicationId", clientApplicationId}, 
                { "cultureId", cultureId}, 
                { "accountOperatorId", accountOperatorId}
            }, TimeSpan.FromMilliseconds(3600000), "default");
        }

        public ApiClientApplicationMessageTranslationResponseDTO EndGetClientApplicationMessageTranslation(ApiAsyncResult<ApiClientApplicationMessageTranslationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/");
            return _client.Request<ListWatchlistResponseDTO>("watchlists", uriTemplate , "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Gets all watchlists for the user account. There are no parameters for this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetWatchlists( ApiAsyncCallback<ListWatchlistResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/");
            _client.BeginRequest(callback, state, "watchlists", uriTemplate , "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ListWatchlistResponseDTO EndGetWatchlists(ApiAsyncResult<ListWatchlistResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/Save");
            return _client.Request<ApiSaveWatchlistResponseDTO>("watchlist", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "apiSaveWatchlistRequestDto", apiSaveWatchlistRequestDto}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Save watchlist.
        /// </summary>
        /// <param name="apiSaveWatchlistRequestDto">The watchlist to save.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveWatchlist(ApiSaveWatchlistRequestDTO apiSaveWatchlistRequestDto, ApiAsyncCallback<ApiSaveWatchlistResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/Save");
            _client.BeginRequest(callback, state, "watchlist", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "apiSaveWatchlistRequestDto", apiSaveWatchlistRequestDto}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ApiSaveWatchlistResponseDTO EndSaveWatchlist(ApiAsyncResult<ApiSaveWatchlistResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("/delete");
            return _client.Request<ApiDeleteWatchlistResponseDTO>("watchlist", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "deleteWatchlistRequestDto", deleteWatchlistRequestDto}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Delete a watchlist.
        /// </summary>
        /// <param name="deleteWatchlistRequestDto">The watchlist to delete.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginDeleteWatchlist(ApiDeleteWatchlistRequestDTO deleteWatchlistRequestDto, ApiAsyncCallback<ApiDeleteWatchlistResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("/delete");
            _client.BeginRequest(callback, state, "watchlist", uriTemplate , "POST",
            new Dictionary<string, object>
            {
                { "deleteWatchlistRequestDto", deleteWatchlistRequestDto}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ApiDeleteWatchlistResponseDTO EndDeleteWatchlist(ApiAsyncResult<ApiDeleteWatchlistResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
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
            string uriTemplate = _client.AppendApiKey("?errorCode={errorCode}");
            return _client.Request<ApiErrorResponseDTO>("errors", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "errorCode", errorCode}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Raises an error condition when an unexpected or uncontrolled event occurs.
        /// </summary>
        /// <param name="errorCode">The error code for the condition encountered.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGenerateException(int errorCode, ApiAsyncCallback<ApiErrorResponseDTO> callback, object state)
        {
            string uriTemplate = _client.AppendApiKey("?errorCode={errorCode}");
            _client.BeginRequest(callback, state, "errors", uriTemplate , "GET",
            new Dictionary<string, object>
            {
                { "errorCode", errorCode}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ApiErrorResponseDTO EndGenerateException(ApiAsyncResult<ApiErrorResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        }            
    }
}