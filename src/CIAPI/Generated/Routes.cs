using System;
using System.Collections.Generic;
using CityIndex.JsonClient;
using CIAPI.DTO;
namespace CIAPI.Rpc
{
    public partial class Client
    {

      public _Authentication Authentication{get; private set;}
      public _PriceHistory PriceHistory{get; private set;}
      public _Market Market{get; private set;}
      public _News News{get; private set;}
      public _CFDMarkets CFDMarkets{get; private set;}
      public _SpreadMarkets SpreadMarkets{get; private set;}
      public _TradesAndOrders TradesAndOrders{get; private set;}
      public _AccountInformation AccountInformation{get; private set;}
      public _Messaging Messaging{get; private set;}
      public _Watchlist Watchlist{get; private set;}
      public _ExceptionHandling ExceptionHandling{get; private set;}
        public Client(Uri uri)
            : base(uri, new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(), new ErrorResponseDTOJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"),new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "default")) )
        {

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. Market = new _Market(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ExceptionHandling = new _ExceptionHandling(this);
        }
        public Client(Uri uri, IRequestController requestController)
            : base(uri, requestController)
        {

            this. Authentication = new _Authentication(this);
            this. PriceHistory = new _PriceHistory(this);
            this. Market = new _Market(this);
            this. News = new _News(this);
            this. CFDMarkets = new _CFDMarkets(this);
            this. SpreadMarkets = new _SpreadMarkets(this);
            this. TradesAndOrders = new _TradesAndOrders(this);
            this. AccountInformation = new _AccountInformation(this);
            this. Messaging = new _Messaging(this);
            this. Watchlist = new _Watchlist(this);
            this. ExceptionHandling = new _ExceptionHandling(this);
        }            

        public class _Authentication
        {
            private Client _client;
            public _Authentication(Client client){ this._client = client;}

        // ***********************************
        // LogOn
        // ***********************************


        /// <summary>
        /// <p>Create a new session. This is how you "log on" to the CIAPI. Post a <a onclick="dojo.hash('#type.ApiLogOnRequestDTO'); return false;" class="json-link" href="#">ApiLogOnRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="apiLogOnRequest"> [DESCRIPTION MISSING]</param>
        internal virtual ApiLogOnResponseDTO LogOn(ApiLogOnRequestDTO apiLogOnRequest)
        {
            return _client.Request<ApiLogOnResponseDTO>("session", "/", "POST",
            new Dictionary<string, object>
            {
                { "apiLogOnRequest", apiLogOnRequest}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// <p>Create a new session. This is how you "log on" to the CIAPI. Post a <a onclick="dojo.hash('#type.ApiLogOnRequestDTO'); return false;" class="json-link" href="#">ApiLogOnRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="apiLogOnRequest"> [DESCRIPTION MISSING]</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal virtual void BeginLogOn(ApiLogOnRequestDTO apiLogOnRequest, ApiAsyncCallback<ApiLogOnResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "session", "/", "POST",
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
            return _client.Request<ApiLogOffResponseDTO>("session", "/deleteSession?userName={userName}&session={session}", "POST",
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
            _client.BeginRequest(callback, state, "session", "/deleteSession?userName={userName}&session={session}", "POST",
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


        }            
        public class _PriceHistory
        {
            private Client _client;
            public _PriceHistory(Client client){ this._client = client;}

        // ***********************************
        // GetPriceBars
        // ***********************************


        /// <summary>
        /// Get historic price bars in OHLC (open, high, low, close) format, suitable for plotting candlestick chartsReturns price bars in ascending order up to the current time.When there are no prices per a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has "gaps", i.e. the gap between the datetime of each price bar might not be equal to interval x spanSample Urls: /market/1234/history?interval=MINUTE&span=15&pricebars=180/market/735/history?interval=HOUR&span=1&pricebars=240/market/1577/history?interval=DAY&span=1&pricebars=10
        /// </summary>
        /// <param name="marketId">The marketId</param>
        /// <param name="interval">The pricebar interval</param>
        /// <param name="span">The number of each interval per pricebar.</param>
        /// <param name="priceBars">The total number of pricebars to return</param>
        public virtual GetPriceBarResponseDTO GetPriceBars(string marketId, string interval, int span, string priceBars)
        {
            return _client.Request<GetPriceBarResponseDTO>("market", "/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}", "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "priceBars", priceBars}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Get historic price bars in OHLC (open, high, low, close) format, suitable for plotting candlestick chartsReturns price bars in ascending order up to the current time.When there are no prices per a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has "gaps", i.e. the gap between the datetime of each price bar might not be equal to interval x spanSample Urls: /market/1234/history?interval=MINUTE&span=15&pricebars=180/market/735/history?interval=HOUR&span=1&pricebars=240/market/1577/history?interval=DAY&span=1&pricebars=10
        /// </summary>
        /// <param name="marketId">The marketId</param>
        /// <param name="interval">The pricebar interval</param>
        /// <param name="span">The number of each interval per pricebar.</param>
        /// <param name="priceBars">The total number of pricebars to return</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceBars(string marketId, string interval, int span, string priceBars, ApiAsyncCallback<GetPriceBarResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "market", "/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}", "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "interval", interval}, 
                { "span", span}, 
                { "priceBars", priceBars}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }

        public GetPriceBarResponseDTO EndGetPriceBars(ApiAsyncResult<GetPriceBarResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // GetPriceTicks
        // ***********************************


        /// <summary>
        /// Get historic price ticks. Returns price ticks in ascending order up to the current time. The length of time between each tick will be different.
        /// </summary>
        /// <param name="marketId">The marketId</param>
        /// <param name="priceTicks">The total number of price ticks to return</param>
        public virtual GetPriceTickResponseDTO GetPriceTicks(string marketId, string priceTicks)
        {
            return _client.Request<GetPriceTickResponseDTO>("market", "/{marketId}/tickhistory?priceticks={priceTicks}", "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "priceTicks", priceTicks}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Get historic price ticks. Returns price ticks in ascending order up to the current time. The length of time between each tick will be different.
        /// </summary>
        /// <param name="marketId">The marketId</param>
        /// <param name="priceTicks">The total number of price ticks to return</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetPriceTicks(string marketId, string priceTicks, ApiAsyncCallback<GetPriceTickResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "market", "/{marketId}/tickhistory?priceticks={priceTicks}", "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}, 
                { "priceTicks", priceTicks}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }

        public GetPriceTickResponseDTO EndGetPriceTicks(ApiAsyncResult<GetPriceTickResponseDTO> asyncResult)
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
        /// <p>Get Market Information for the specified market.</p>
        /// </summary>
        /// <param name="marketId">The marketId</param>
        public virtual GetMarketInformationResponseDTO GetMarketInformation(string marketId)
        {
            return _client.Request<GetMarketInformationResponseDTO>("market", "/{marketId}/information", "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// <p>Get Market Information for the specified market.</p>
        /// </summary>
        /// <param name="marketId">The marketId</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetMarketInformation(string marketId, ApiAsyncCallback<GetMarketInformationResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "market", "/{marketId}/information", "GET",
            new Dictionary<string, object>
            {
                { "marketId", marketId}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public GetMarketInformationResponseDTO EndGetMarketInformation(ApiAsyncResult<GetMarketInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // ListMarketInformationSearch
        // ***********************************


        /// <summary>
        /// <p>Queries for market information.</p>
        /// </summary>
        /// <param name="searchByMarketCode">Should the search be done by market code</param>
        /// <param name="searchByMarketName">Should the search be done by market Name</param>
        /// <param name="spreadProductType">Should the search include spread bet markets</param>
        /// <param name="cfdProductType">Should the search include CFD markets</param>
        /// <param name="binaryProductType">Should the search include binary markets</param>
        /// <param name="query">The text to search for.  Matches part of market name / code from the start.</param>
        /// <param name="maxResults">The maximum number of results to return</param>
        public virtual ListMarketInformationSearchResponseDTO ListMarketInformationSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults)
        {
            return _client.Request<ListMarketInformationSearchResponseDTO>("market", "/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketCode", searchByMarketCode}, 
                { "searchByMarketName", searchByMarketName}, 
                { "spreadProductType", spreadProductType}, 
                { "cfdProductType", cfdProductType}, 
                { "binaryProductType", binaryProductType}, 
                { "query", query}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// <p>Queries for market information.</p>
        /// </summary>
        /// <param name="searchByMarketCode">Should the search be done by market code</param>
        /// <param name="searchByMarketName">Should the search be done by market Name</param>
        /// <param name="spreadProductType">Should the search include spread bet markets</param>
        /// <param name="cfdProductType">Should the search include CFD markets</param>
        /// <param name="binaryProductType">Should the search include binary markets</param>
        /// <param name="query">The text to search for.  Matches part of market name / code from the start.</param>
        /// <param name="maxResults">The maximum number of results to return</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListMarketInformationSearch(bool searchByMarketCode, bool searchByMarketName, bool spreadProductType, bool cfdProductType, bool binaryProductType, string query, int maxResults, ApiAsyncCallback<ListMarketInformationSearchResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "market", "/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketCode", searchByMarketCode}, 
                { "searchByMarketName", searchByMarketName}, 
                { "spreadProductType", spreadProductType}, 
                { "cfdProductType", cfdProductType}, 
                { "binaryProductType", binaryProductType}, 
                { "query", query}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public ListMarketInformationSearchResponseDTO EndListMarketInformationSearch(ApiAsyncResult<ListMarketInformationSearchResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // ListMarketInformation
        // ***********************************


        /// <summary>
        /// <p>Get Market Information for the specified list of markets. Post a <a onclick="dojo.hash('#type.ListMarketInformationRequestDTO'); return false;" class="json-link" href="#">ListMarketInformationRequestDTO</a> to the uri specified below.</p>
        /// </summary>
        public virtual ListMarketInformationResponseDTO ListMarketInformation()
        {
            return _client.Request<ListMarketInformationResponseDTO>("market", "/market/information", "POST",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// <p>Get Market Information for the specified list of markets. Post a <a onclick="dojo.hash('#type.ListMarketInformationRequestDTO'); return false;" class="json-link" href="#">ListMarketInformationRequestDTO</a> to the uri specified below.</p>
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListMarketInformation( ApiAsyncCallback<ListMarketInformationResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "market", "/market/information", "POST",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
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
        /// <param name="listMarketInformationRequestSaveDTO">The market information request</param>
        public virtual ApiSaveMarketInformationResponseDTO SaveMarketInformation(SaveMarketInformationRequestDTO listMarketInformationRequestSaveDTO)
        {
            return _client.Request<ApiSaveMarketInformationResponseDTO>("market", "/market/information/save", "POST",
            new Dictionary<string, object>
            {
                { "listMarketInformationRequestSaveDTO", listMarketInformationRequestSaveDTO}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Save Market Information for the specified list of markets.
        /// </summary>
        /// <param name="listMarketInformationRequestSaveDTO">The market information request</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveMarketInformation(SaveMarketInformationRequestDTO listMarketInformationRequestSaveDTO, ApiAsyncCallback<ApiSaveMarketInformationResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "market", "/market/information/save", "POST",
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
        public class _News
        {
            private Client _client;
            public _News(Client client){ this._client = client;}

        // ***********************************
        // ListNewsHeadlines
        // ***********************************


        /// <summary>
        /// Get a list of current news headlines
        /// </summary>
        /// <param name="category">Filter headlines by category</param>
        /// <param name="maxResults">Restrict the number of headlines returned</param>
        public virtual ListNewsHeadlinesResponseDTO ListNewsHeadlines(string category, int maxResults)
        {
            return _client.Request<ListNewsHeadlinesResponseDTO>("news", "?Category={category}&MaxResults={maxResults}&Source={source}", "GET",
            new Dictionary<string, object>
            {
                { "category", category}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Get a list of current news headlines
        /// </summary>
        /// <param name="category">Filter headlines by category</param>
        /// <param name="maxResults">Restrict the number of headlines returned</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListNewsHeadlines(string category, int maxResults, ApiAsyncCallback<ListNewsHeadlinesResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "news", "?Category={category}&MaxResults={maxResults}&Source={source}", "GET",
            new Dictionary<string, object>
            {
                { "category", category}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }

        public ListNewsHeadlinesResponseDTO EndListNewsHeadlines(ApiAsyncResult<ListNewsHeadlinesResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // GetNewsDetail
        // ***********************************


        /// <summary>
        /// Get the detail of a specific news story
        /// </summary>
        /// <param name="storyId">The news story Id</param>
        public virtual GetNewsDetailResponseDTO GetNewsDetail(string storyId)
        {
            return _client.Request<GetNewsDetailResponseDTO>("news", "/{storyId}?Source={source}", "GET",
            new Dictionary<string, object>
            {
                { "storyId", storyId}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Get the detail of a specific news story
        /// </summary>
        /// <param name="storyId">The news story Id</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetNewsDetail(string storyId, ApiAsyncCallback<GetNewsDetailResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "news", "/{storyId}?Source={source}", "GET",
            new Dictionary<string, object>
            {
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
        /// Returns a list of CFD markets filtered by market name and/or market code
        /// </summary>
        /// <param name="searchByMarketName">The characters that the CFD market name should start with</param>
        /// <param name="searchByMarketCode">The characters that the market code should start with (normally this is the RIC code for the market)</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        public virtual ListCfdMarketsResponseDTO ListCfdMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults)
        {
            return _client.Request<ListCfdMarketsResponseDTO>("cfd/markets", "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Returns a list of CFD markets filtered by market name and/or market code
        /// </summary>
        /// <param name="searchByMarketName">The characters that the CFD market name should start with</param>
        /// <param name="searchByMarketCode">The characters that the market code should start with (normally this is the RIC code for the market)</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListCfdMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, ApiAsyncCallback<ListCfdMarketsResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "cfd/markets", "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}
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
        /// Returns a list of Spread Betting markets filtered by market name and/or market code
        /// </summary>
        /// <param name="searchByMarketName">The characters that the Spread market name should start with</param>
        /// <param name="searchByMarketCode">The characters that the Spread market code should start with (normally this is the RIC code for the market)</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        public virtual ListSpreadMarketsResponseDTO ListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults)
        {
            return _client.Request<ListSpreadMarketsResponseDTO>("spread/markets", "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }


        /// <summary>
        /// Returns a list of Spread Betting markets filtered by market name and/or market code
        /// </summary>
        /// <param name="searchByMarketName">The characters that the Spread market name should start with</param>
        /// <param name="searchByMarketCode">The characters that the Spread market code should start with (normally this is the RIC code for the market)</param>
        /// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
        /// <param name="maxResults">The maximum number of markets to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListSpreadMarkets(string searchByMarketName, string searchByMarketCode, int clientAccountId, int maxResults, ApiAsyncCallback<ListSpreadMarketsResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "spread/markets", "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "searchByMarketName", searchByMarketName}, 
                { "searchByMarketCode", searchByMarketCode}, 
                { "clientAccountId", clientAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(10000), "data");
        }

        public ListSpreadMarketsResponseDTO EndListSpreadMarkets(ApiAsyncResult<ListSpreadMarketsResponseDTO> asyncResult)
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
        /// <p>Place an order on a particular market. Post a <a onclick="dojo.hash('#type.NewStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">NewStopLimitOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new order, the platform will generate them.</p>
        /// </summary>
        /// <param name="order">The order request</param>
        public virtual ApiTradeOrderResponseDTO Order(NewStopLimitOrderRequestDTO order)
        {
            return _client.Request<ApiTradeOrderResponseDTO>("order", "/newstoplimitorder", "POST",
            new Dictionary<string, object>
            {
                { "order", order}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }


        /// <summary>
        /// <p>Place an order on a particular market. Post a <a onclick="dojo.hash('#type.NewStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">NewStopLimitOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new order, the platform will generate them.</p>
        /// </summary>
        /// <param name="order">The order request</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginOrder(NewStopLimitOrderRequestDTO order, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/newstoplimitorder", "POST",
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
        /// <p>Cancel an order. Post a <a onclick="dojo.hash('#type.CancelOrderRequestDTO'); return false;" class="json-link" href="#">CancelOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="cancelOrder">The cancel order request</param>
        public virtual ApiTradeOrderResponseDTO CancelOrder(CancelOrderRequestDTO cancelOrder)
        {
            return _client.Request<ApiTradeOrderResponseDTO>("order", "/cancel", "POST",
            new Dictionary<string, object>
            {
                { "cancelOrder", cancelOrder}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Cancel an order. Post a <a onclick="dojo.hash('#type.CancelOrderRequestDTO'); return false;" class="json-link" href="#">CancelOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="cancelOrder">The cancel order request</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginCancelOrder(CancelOrderRequestDTO cancelOrder, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/cancel", "POST",
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
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship). Post an <a onclick="dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="order"><p>Update an order (for adding a stop/limit or attaching an OCO relationship).  Post an <a onclick="dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p></param>
        public virtual ApiTradeOrderResponseDTO UpdateOrder(UpdateStopLimitOrderRequestDTO order)
        {
            return _client.Request<ApiTradeOrderResponseDTO>("order", "/updatestoplimitorder", "POST",
            new Dictionary<string, object>
            {
                { "order", order}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship). Post an <a onclick="dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="order"><p>Update an order (for adding a stop/limit or attaching an OCO relationship).  Post an <a onclick="dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginUpdateOrder(UpdateStopLimitOrderRequestDTO order, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/updatestoplimitorder", "POST",
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
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetOpenPosition'); return false;" class="json-link" href="#">GetOpenPosition</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        public virtual ListOpenPositionsResponseDTO ListOpenPositions(int tradingAccountId)
        {
            return _client.Request<ListOpenPositionsResponseDTO>("order", "/openpositions?TradingAccountId={tradingAccountId}", "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetOpenPosition'); return false;" class="json-link" href="#">GetOpenPosition</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListOpenPositions(int tradingAccountId, ApiAsyncCallback<ListOpenPositionsResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/openpositions?TradingAccountId={tradingAccountId}", "GET",
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
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetActiveStopLimitOrder'); return false;" class="json-link" href="#">GetActiveStopLimitOrder</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        public virtual ListActiveStopLimitOrderResponseDTO ListActiveStopLimitOrders(int tradingAccountId)
        {
            return _client.Request<ListActiveStopLimitOrderResponseDTO>("order", "/activestoplimitorders?TradingAccountId={tradingAccountId}", "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetActiveStopLimitOrder'); return false;" class="json-link" href="#">GetActiveStopLimitOrder</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListActiveStopLimitOrders(int tradingAccountId, ApiAsyncCallback<ListActiveStopLimitOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/activestoplimitorders?TradingAccountId={tradingAccountId}", "GET",
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
        /// <p>Queries for a active stop limit order with a specified order id. It will return a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListActiveStopLimitOrders'); return false;" class="json-link" href="#">ListActiveStopLimitOrders</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>
        /// <param name="orderId">The requested order id.</param>
        public virtual GetActiveStopLimitOrderResponseDTO GetActiveStopLimitOrder(string orderId)
        {
            return _client.Request<GetActiveStopLimitOrderResponseDTO>("order", "/{orderId}/activestoplimitorder", "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a active stop limit order with a specified order id. It will return a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListActiveStopLimitOrders'); return false;" class="json-link" href="#">ListActiveStopLimitOrders</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>
        /// <param name="orderId">The requested order id.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetActiveStopLimitOrder(string orderId, ApiAsyncCallback<GetActiveStopLimitOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/{orderId}/activestoplimitorder", "GET",
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
        /// <p>Queries for a trade / open position with a specified order id. It will return a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListOpenPositions'); return false;" class="json-link" href="#">ListOpenPositions</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>
        /// <param name="orderId">The requested order id.</param>
        public virtual GetOpenPositionResponseDTO GetOpenPosition(string orderId)
        {
            return _client.Request<GetOpenPositionResponseDTO>("order", "/{orderId}/openposition", "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a trade / open position with a specified order id. It will return a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListOpenPositions'); return false;" class="json-link" href="#">ListOpenPositions</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>
        /// <param name="orderId">The requested order id.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOpenPosition(string orderId, ApiAsyncCallback<GetOpenPositionResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/{orderId}/openposition", "GET",
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
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <param name="maxResults">The maximum results to return.</param>
        public virtual ListTradeHistoryResponseDTO ListTradeHistory(int tradingAccountId, int maxResults)
        {
            return _client.Request<ListTradeHistoryResponseDTO>("order", "/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <param name="maxResults">The maximum results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListTradeHistory(int tradingAccountId, int maxResults, ApiAsyncCallback<ListTradeHistoryResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET",
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
        /// <p>Queries for a specified trading account's stop / limit order history. The result set will include <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b> </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <param name="maxResults">the maximum results to return.</param>
        public virtual ListStopLimitOrderHistoryResponseDTO ListStopLimitOrderHistory(int tradingAccountId, int maxResults)
        {
            return _client.Request<ListStopLimitOrderHistoryResponseDTO>("order", "/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET",
            new Dictionary<string, object>
            {
                { "tradingAccountId", tradingAccountId}, 
                { "maxResults", maxResults}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for a specified trading account's stop / limit order history. The result set will include <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b> </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>
        /// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <param name="maxResults">the maximum results to return.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginListStopLimitOrderHistory(int tradingAccountId, int maxResults, ApiAsyncCallback<ListStopLimitOrderHistoryResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET",
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
        /// <p>Queries for an order by a specific order id.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered)</b>.</p>
        /// </summary>
        /// <param name="orderId">The requested order id.</param>
        public virtual GetOrderResponseDTO GetOrder(string orderId)
        {
            return _client.Request<GetOrderResponseDTO>("order", "/{orderId}", "GET",
            new Dictionary<string, object>
            {
                { "orderId", orderId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// <p>Queries for an order by a specific order id.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered)</b>.</p>
        /// </summary>
        /// <param name="orderId">The requested order id.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetOrder(string orderId, ApiAsyncCallback<GetOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/{orderId}", "GET",
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
        /// <p>Place a trade on a particular market. Post a <a onclick="dojo.hash('#type.NewTradeOrderRequestDTO'); return false;" class="json-link" href="#">NewTradeOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new trade, the platform will generate them.</p>
        /// </summary>
        /// <param name="trade">The trade request</param>
        public virtual ApiTradeOrderResponseDTO Trade(NewTradeOrderRequestDTO trade)
        {
            return _client.Request<ApiTradeOrderResponseDTO>("order", "/newtradeorder", "POST",
            new Dictionary<string, object>
            {
                { "trade", trade}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }


        /// <summary>
        /// <p>Place a trade on a particular market. Post a <a onclick="dojo.hash('#type.NewTradeOrderRequestDTO'); return false;" class="json-link" href="#">NewTradeOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new trade, the platform will generate them.</p>
        /// </summary>
        /// <param name="trade">The trade request</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginTrade(NewTradeOrderRequestDTO trade, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/newtradeorder", "POST",
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
        /// Update a trade (for adding a stop/limit etc). Post an <a onclick="dojo.hash('#type.UpdateTradeOrderRequestDTO'); return false;" class="json-link" href="#">UpdateTradeOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="update">The trade request</param>
        public virtual ApiTradeOrderResponseDTO UpdateTrade(UpdateTradeOrderRequestDTO update)
        {
            return _client.Request<ApiTradeOrderResponseDTO>("order", "/updatetradeorder", "POST",
            new Dictionary<string, object>
            {
                { "update", update}
            }, TimeSpan.FromMilliseconds(0), "trading");
        }


        /// <summary>
        /// Update a trade (for adding a stop/limit etc). Post an <a onclick="dojo.hash('#type.UpdateTradeOrderRequestDTO'); return false;" class="json-link" href="#">UpdateTradeOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>
        /// <param name="update">The trade request</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginUpdateTrade(UpdateTradeOrderRequestDTO update, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "order", "/updatetradeorder", "POST",
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
        /// Returns the Users ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>
        public virtual AccountInformationResponseDTO GetClientAndTradingAccount()
        {
            return _client.Request<AccountInformationResponseDTO>("useraccount", "/UserAccount/ClientAndTradingAccount", "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Returns the Users ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientAndTradingAccount( ApiAsyncCallback<AccountInformationResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "useraccount", "/UserAccount/ClientAndTradingAccount", "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }

        public AccountInformationResponseDTO EndGetClientAndTradingAccount(ApiAsyncResult<AccountInformationResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        }            
        public class _Messaging
        {
            private Client _client;
            public _Messaging(Client client){ this._client = client;}

        // ***********************************
        // GetMessage
        // ***********************************


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="id"> [DESCRIPTION MISSING]</param>
        /// <param name="language"> [DESCRIPTION MISSING]</param>
        /// <param name="category"> [DESCRIPTION MISSING]</param>
        public virtual string GetMessage(string id, string language, string category)
        {
            return _client.Request<string>("message", "/Message/{id}?language={language}&category={category}", "GET",
            new Dictionary<string, object>
            {
                { "id", id}, 
                { "language", language}, 
                { "category", category}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="id"> [DESCRIPTION MISSING]</param>
        /// <param name="language"> [DESCRIPTION MISSING]</param>
        /// <param name="category"> [DESCRIPTION MISSING]</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetMessage(string id, string language, string category, ApiAsyncCallback<string> callback, object state)
        {
            _client.BeginRequest(callback, state, "message", "/Message/{id}?language={language}&category={category}", "GET",
            new Dictionary<string, object>
            {
                { "id", id}, 
                { "language", language}, 
                { "category", category}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public string EndGetMessage(ApiAsyncResult<string> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // GetMessagePopup
        // ***********************************


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="language"> [DESCRIPTION MISSING]</param>
        /// <param name="clientAccountId"> [DESCRIPTION MISSING]</param>
        public virtual GetMessagePopupResponseDTO GetMessagePopup(string language, int clientAccountId)
        {
            return _client.Request<GetMessagePopupResponseDTO>("message", "/message/popup?language={language}&ClientAccountId={clientAccountId}", "GET",
            new Dictionary<string, object>
            {
                { "language", language}, 
                { "clientAccountId", clientAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="language"> [DESCRIPTION MISSING]</param>
        /// <param name="clientAccountId"> [DESCRIPTION MISSING]</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetMessagePopup(string language, int clientAccountId, ApiAsyncCallback<GetMessagePopupResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "message", "/message/popup?language={language}&ClientAccountId={clientAccountId}", "GET",
            new Dictionary<string, object>
            {
                { "language", language}, 
                { "clientAccountId", clientAccountId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public GetMessagePopupResponseDTO EndGetMessagePopup(ApiAsyncResult<GetMessagePopupResponseDTO> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // AcceptOrRejectMessagePopupResponse
        // ***********************************


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="clientAccountId"> [DESCRIPTION MISSING]</param>
        /// <param name="accepted"> [DESCRIPTION MISSING]</param>
        public virtual NullType AcceptOrRejectMessagePopupResponse(int clientAccountId, bool accepted)
        {
            return _client.Request<NullType>("message", "/message/popupchoice?ClientAccountId={clientAccountId}&Accepted={accepted}", "GET",
            new Dictionary<string, object>
            {
                { "clientAccountId", clientAccountId}, 
                { "accepted", accepted}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        ///  [DESCRIPTION MISSING]
        /// </summary>
        /// <param name="clientAccountId"> [DESCRIPTION MISSING]</param>
        /// <param name="accepted"> [DESCRIPTION MISSING]</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginAcceptOrRejectMessagePopupResponse(int clientAccountId, bool accepted, ApiAsyncCallback<NullType> callback, object state)
        {
            _client.BeginRequest(callback, state, "message", "/message/popupchoice?ClientAccountId={clientAccountId}&Accepted={accepted}", "GET",
            new Dictionary<string, object>
            {
                { "clientAccountId", clientAccountId}, 
                { "accepted", accepted}
            }, TimeSpan.FromMilliseconds(0), "default");
        }

        public NullType EndAcceptOrRejectMessagePopupResponse(ApiAsyncResult<NullType> asyncResult)
        {
            return _client.EndRequest(asyncResult);
        }


        // ***********************************
        // GetSystemLookup
        // ***********************************


        /// <summary>
        /// Use the message lookup service to get localised textual names for the various status code & Ids returned by the API. For example, a query for OrderStatusReasons will contain text names for all the possible values of OrderStatusReason in the ApiOrderResponseDTO. You should only request the list once per session (for each entity you're interested in).
        /// </summary>
        /// <param name="lookupEntityName">The entity to lookup (eg OrderStatusReason, InstructionStatusReason, OrderApplicability or Culture)</param>
        /// <param name="cultureId">The cultureId used to override the translated text description. (optional)</param>
        public virtual ApiLookupResponseDTO GetSystemLookup(string lookupEntityName, int cultureId)
        {
            return _client.Request<ApiLookupResponseDTO>("message", "/message/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}", "GET",
            new Dictionary<string, object>
            {
                { "lookupEntityName", lookupEntityName}, 
                { "cultureId", cultureId}
            }, TimeSpan.FromMilliseconds(3600), "default");
        }


        /// <summary>
        /// Use the message lookup service to get localised textual names for the various status code & Ids returned by the API. For example, a query for OrderStatusReasons will contain text names for all the possible values of OrderStatusReason in the ApiOrderResponseDTO. You should only request the list once per session (for each entity you're interested in).
        /// </summary>
        /// <param name="lookupEntityName">The entity to lookup (eg OrderStatusReason, InstructionStatusReason, OrderApplicability or Culture)</param>
        /// <param name="cultureId">The cultureId used to override the translated text description. (optional)</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetSystemLookup(string lookupEntityName, int cultureId, ApiAsyncCallback<ApiLookupResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "message", "/message/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}", "GET",
            new Dictionary<string, object>
            {
                { "lookupEntityName", lookupEntityName}, 
                { "cultureId", cultureId}
            }, TimeSpan.FromMilliseconds(3600), "default");
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
        /// <param name="clientApplicationId">Client application identifier. (optional)</param>
        /// <param name="cultureId">CultureId which corresponds to a culture code. (optional)</param>
        /// <param name="accountOperatorId">Account operator identifier. (optional)</param>
        public virtual ApiClientApplicationMessageTranslationResponseDTO GetClientApplicationMessageTranslation(int clientApplicationId, int cultureId, int accountOperatorId)
        {
            return _client.Request<ApiClientApplicationMessageTranslationResponseDTO>("message", "/message/translation?clientApplicationId={clientApplicationId}&cultureId={cultureId}&accountOperatorId={accountOperatorId}", "GET",
            new Dictionary<string, object>
            {
                { "clientApplicationId", clientApplicationId}, 
                { "cultureId", cultureId}, 
                { "accountOperatorId", accountOperatorId}
            }, TimeSpan.FromMilliseconds(0), "default");
        }


        /// <summary>
        /// Use the message translation service to get client specific translated textual strings.
        /// </summary>
        /// <param name="clientApplicationId">Client application identifier. (optional)</param>
        /// <param name="cultureId">CultureId which corresponds to a culture code. (optional)</param>
        /// <param name="accountOperatorId">Account operator identifier. (optional)</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetClientApplicationMessageTranslation(int clientApplicationId, int cultureId, int accountOperatorId, ApiAsyncCallback<ApiClientApplicationMessageTranslationResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "message", "/message/translation?clientApplicationId={clientApplicationId}&cultureId={cultureId}&accountOperatorId={accountOperatorId}", "GET",
            new Dictionary<string, object>
            {
                { "clientApplicationId", clientApplicationId}, 
                { "cultureId", cultureId}, 
                { "accountOperatorId", accountOperatorId}
            }, TimeSpan.FromMilliseconds(0), "default");
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
        /// Get client watchlist
        /// </summary>
        public virtual ListWatchlistResponseDTO GetWatchlists()
        {
            return _client.Request<ListWatchlistResponseDTO>("watchlists", "/", "GET",
            new Dictionary<string, object>
            {

            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Get client watchlist
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGetWatchlists( ApiAsyncCallback<ListWatchlistResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "watchlists", "/", "GET",
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
        /// Save watchlist
        /// </summary>
        /// <param name="apiSaveWatchlistRequestDto">Save watchlist</param>
        public virtual ApiSaveWatchlistResponseDTO SaveWatchlist(ApiSaveWatchlistRequestDTO apiSaveWatchlistRequestDto)
        {
            return _client.Request<ApiSaveWatchlistResponseDTO>("watchlists", "/Save", "POST",
            new Dictionary<string, object>
            {
                { "apiSaveWatchlistRequestDto", apiSaveWatchlistRequestDto}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Save watchlist
        /// </summary>
        /// <param name="apiSaveWatchlistRequestDto">Save watchlist</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginSaveWatchlist(ApiSaveWatchlistRequestDTO apiSaveWatchlistRequestDto, ApiAsyncCallback<ApiSaveWatchlistResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "watchlists", "/Save", "POST",
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
        /// Delete a watchlist
        /// </summary>
        /// <param name="deleteWatchlistRequestDto">Delete a watchlist</param>
        public virtual ApiDeleteWatchlistResponseDTO DeleteWatchlist(ApiDeleteWatchlistRequestDTO deleteWatchlistRequestDto)
        {
            return _client.Request<ApiDeleteWatchlistResponseDTO>("watchlists", "/delete", "POST",
            new Dictionary<string, object>
            {
                { "deleteWatchlistRequestDto", deleteWatchlistRequestDto}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Delete a watchlist
        /// </summary>
        /// <param name="deleteWatchlistRequestDto">Delete a watchlist</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginDeleteWatchlist(ApiDeleteWatchlistRequestDTO deleteWatchlistRequestDto, ApiAsyncCallback<ApiDeleteWatchlistResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "watchlists", "/delete", "POST",
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
        /// Simulates an error condition.
        /// </summary>
        /// <param name="errorCode">Simulates an error condition.</param>
        public virtual ApiErrorResponseDTO GenerateException(int errorCode)
        {
            return _client.Request<ApiErrorResponseDTO>("errors", "?errorCode={errorCode}", "GET",
            new Dictionary<string, object>
            {
                { "errorCode", errorCode}
            }, TimeSpan.FromMilliseconds(0), "data");
        }


        /// <summary>
        /// Simulates an error condition.
        /// </summary>
        /// <param name="errorCode">Simulates an error condition.</param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public virtual void BeginGenerateException(int errorCode, ApiAsyncCallback<ApiErrorResponseDTO> callback, object state)
        {
            _client.BeginRequest(callback, state, "errors", "?errorCode={errorCode}", "GET",
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