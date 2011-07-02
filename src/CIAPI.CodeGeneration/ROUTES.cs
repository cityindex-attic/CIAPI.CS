using System;
using System.Collections.Generic;
using CityIndex.JsonClient;
using CIAPI.DTO;

namespace CIAPI.Rpc
{
	public partial class Client 
	{ 
 
        /// <summary>
        /// <p>Create a new session. This is how you "log on" to the CIAPI. Post a <a onclick="dojo.hash('#type.ApiLogOnRequestDTO'); return false;" class="json-link" href="#">ApiLogOnRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="UserName">Username is case sensitive</param>
		/// <param name="Password">Password is case sensitive</param>
        /// <returns></returns>
        internal ApiLogOnResponseDTO LogOn(String UserName, String Password)
        {
       
            return Request<ApiLogOnResponseDTO>("session","/", "POST", new Dictionary<string, object>
                                {
									{"UserName",UserName},
									{"Password",Password},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// <p>Create a new session. This is how you "log on" to the CIAPI. Post a <a onclick="dojo.hash('#type.ApiLogOnRequestDTO'); return false;" class="json-link" href="#">ApiLogOnRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="UserName">Username is case sensitive</param>
		/// <param name="Password">Password is case sensitive</param>
        /// <returns></returns>
        internal void BeginLogOn(String UserName, String Password, ApiAsyncCallback<ApiLogOnResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "session","/", "POST",new Dictionary<string, object>
                                {
									{"UserName",UserName},
									{"Password",Password},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        internal ApiLogOnResponseDTO EndLogOn(ApiAsyncResult<ApiLogOnResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Delete a session. This is how you "log off" from the CIAPI.</p>
        /// </summary>		
		/// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
		/// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <returns></returns>
        internal ApiLogOffResponseDTO DeleteSession(String userName, Guid session)
        {
       
            return Request<ApiLogOffResponseDTO>("session","/deleteSession?userName={userName}&session={session}", "POST", new Dictionary<string, object>
                                {
									{"userName",userName},
									{"session",session},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// <p>Delete a session. This is how you "log off" from the CIAPI.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
		/// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <returns></returns>
        internal void BeginDeleteSession(String userName, Guid session, ApiAsyncCallback<ApiLogOffResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "session","/deleteSession?userName={userName}&session={session}", "POST",new Dictionary<string, object>
                                {
									{"userName",userName},
									{"session",session},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        internal ApiLogOffResponseDTO EndDeleteSession(ApiAsyncResult<ApiLogOffResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Get historic price bars in OHLC (open, high, low, close) format, suitable for plotting candlestick chartsReturns price bars in ascending order up to the current time.When there are no prices per a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has "gaps", i.e. the gap between the datetime of each price bar might not be equal to interval x spanSample Urls: /market/1234/history?interval=MINUTE&span=15&pricebars=180/market/735/history?interval=HOUR&span=1&pricebars=240/market/1577/history?interval=DAY&span=1&pricebars=10
        /// </summary>		
		/// <param name="marketId">The marketId</param>
		/// <param name="interval">The pricebar interval</param>
		/// <param name="span">The number of each interval per pricebar.</param>
		/// <param name="priceBars">The total number of pricebars to return</param>
        /// <returns></returns>
        public GetPriceBarResponseDTO GetPriceBars(String marketId, String interval, Int32 span, String priceBars)
        {
       
            return Request<GetPriceBarResponseDTO>("market","/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}", "GET", new Dictionary<string, object>
                                {
									{"marketId",marketId},
									{"interval",interval},
									{"span",span},
									{"priceBars",priceBars},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        /// <summary>
        /// Get historic price bars in OHLC (open, high, low, close) format, suitable for plotting candlestick chartsReturns price bars in ascending order up to the current time.When there are no prices per a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has "gaps", i.e. the gap between the datetime of each price bar might not be equal to interval x spanSample Urls: /market/1234/history?interval=MINUTE&span=15&pricebars=180/market/735/history?interval=HOUR&span=1&pricebars=240/market/1577/history?interval=DAY&span=1&pricebars=10
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="marketId">The marketId</param>
		/// <param name="interval">The pricebar interval</param>
		/// <param name="span">The number of each interval per pricebar.</param>
		/// <param name="priceBars">The total number of pricebars to return</param>
        /// <returns></returns>
        public void BeginGetPriceBars(String marketId, String interval, Int32 span, String priceBars, ApiAsyncCallback<GetPriceBarResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "market","/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}", "GET",new Dictionary<string, object>
                                {
									{"marketId",marketId},
									{"interval",interval},
									{"span",span},
									{"priceBars",priceBars},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        public GetPriceBarResponseDTO EndGetPriceBars(ApiAsyncResult<GetPriceBarResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Get historic price ticks. Returns price ticks in ascending order up to the current time. The length of time between each tick will be different.
        /// </summary>		
		/// <param name="marketId">The marketId</param>
		/// <param name="priceTicks">The total number of price ticks to return</param>
        /// <returns></returns>
        public GetPriceTickResponseDTO GetPriceTicks(String marketId, String priceTicks)
        {
       
            return Request<GetPriceTickResponseDTO>("market","/{marketId}/tickhistory?priceticks={priceTicks}", "GET", new Dictionary<string, object>
                                {
									{"marketId",marketId},
									{"priceTicks",priceTicks},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        /// <summary>
        /// Get historic price ticks. Returns price ticks in ascending order up to the current time. The length of time between each tick will be different.
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="marketId">The marketId</param>
		/// <param name="priceTicks">The total number of price ticks to return</param>
        /// <returns></returns>
        public void BeginGetPriceTicks(String marketId, String priceTicks, ApiAsyncCallback<GetPriceTickResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "market","/{marketId}/tickhistory?priceticks={priceTicks}", "GET",new Dictionary<string, object>
                                {
									{"marketId",marketId},
									{"priceTicks",priceTicks},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        public GetPriceTickResponseDTO EndGetPriceTicks(ApiAsyncResult<GetPriceTickResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Get Market Information for the specified market.</p>
        /// </summary>		
		/// <param name="marketId">The marketId</param>
        /// <returns></returns>
        public GetMarketInformationResponseDTO GetMarketInformation(String marketId)
        {
       
            return Request<GetMarketInformationResponseDTO>("market","/{marketId}/information", "GET", new Dictionary<string, object>
                                {
									{"marketId",marketId},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// <p>Get Market Information for the specified market.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="marketId">The marketId</param>
        /// <returns></returns>
        public void BeginGetMarketInformation(String marketId, ApiAsyncCallback<GetMarketInformationResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "market","/{marketId}/information", "GET",new Dictionary<string, object>
                                {
									{"marketId",marketId},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public GetMarketInformationResponseDTO EndGetMarketInformation(ApiAsyncResult<GetMarketInformationResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
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
        /// <returns></returns>
        public ListMarketInformationSearchResponseDTO ListMarketInformationSearch(Boolean searchByMarketCode, Boolean searchByMarketName, Boolean spreadProductType, Boolean cfdProductType, Boolean binaryProductType, String query, Int32 maxResults)
        {
       
            return Request<ListMarketInformationSearchResponseDTO>("market","/market/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}", "GET", new Dictionary<string, object>
                                {
									{"searchByMarketCode",searchByMarketCode},
									{"searchByMarketName",searchByMarketName},
									{"spreadProductType",spreadProductType},
									{"cfdProductType",cfdProductType},
									{"binaryProductType",binaryProductType},
									{"query",query},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// <p>Queries for market information.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="searchByMarketCode">Should the search be done by market code</param>
		/// <param name="searchByMarketName">Should the search be done by market Name</param>
		/// <param name="spreadProductType">Should the search include spread bet markets</param>
		/// <param name="cfdProductType">Should the search include CFD markets</param>
		/// <param name="binaryProductType">Should the search include binary markets</param>
		/// <param name="query">The text to search for.  Matches part of market name / code from the start.</param>
		/// <param name="maxResults">The maximum number of results to return</param>
        /// <returns></returns>
        public void BeginListMarketInformationSearch(Boolean searchByMarketCode, Boolean searchByMarketName, Boolean spreadProductType, Boolean cfdProductType, Boolean binaryProductType, String query, Int32 maxResults, ApiAsyncCallback<ListMarketInformationSearchResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "market","/market/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}", "GET",new Dictionary<string, object>
                                {
									{"searchByMarketCode",searchByMarketCode},
									{"searchByMarketName",searchByMarketName},
									{"spreadProductType",spreadProductType},
									{"cfdProductType",cfdProductType},
									{"binaryProductType",binaryProductType},
									{"query",query},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public ListMarketInformationSearchResponseDTO EndListMarketInformationSearch(ApiAsyncResult<ListMarketInformationSearchResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for market information.</p>
        /// </summary>		
		/// <param name="MarketIds">The list of market ids</param>
        /// <returns></returns>
        public ListMarketInformationResponseDTO ListMarketInformation(Int32[] MarketIds)
        {
       
            return Request<ListMarketInformationResponseDTO>("market","/market/information", "POST", new Dictionary<string, object>
                                {
									{"MarketIds",MarketIds},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// <p>Queries for market information.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="MarketIds">The list of market ids</param>
        /// <returns></returns>
        public void BeginListMarketInformation(Int32[] MarketIds, ApiAsyncCallback<ListMarketInformationResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "market","/market/information", "POST",new Dictionary<string, object>
                                {
									{"MarketIds",MarketIds},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public ListMarketInformationResponseDTO EndListMarketInformation(ApiAsyncResult<ListMarketInformationResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Get a list of current news headlines
        /// </summary>		
		/// <param name="category">Filter headlines by category</param>
		/// <param name="maxResults">Restrict the number of headlines returned</param>
        /// <returns></returns>
        public ListNewsHeadlinesResponseDTO ListNewsHeadlines(String category, Int32 maxResults)
        {
       
            return Request<ListNewsHeadlinesResponseDTO>("news","?Category={category}&MaxResults={maxResults}", "GET", new Dictionary<string, object>
                                {
									{"category",category},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        /// <summary>
        /// Get a list of current news headlines
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="category">Filter headlines by category</param>
		/// <param name="maxResults">Restrict the number of headlines returned</param>
        /// <returns></returns>
        public void BeginListNewsHeadlines(String category, Int32 maxResults, ApiAsyncCallback<ListNewsHeadlinesResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "news","?Category={category}&MaxResults={maxResults}", "GET",new Dictionary<string, object>
                                {
									{"category",category},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        public ListNewsHeadlinesResponseDTO EndListNewsHeadlines(ApiAsyncResult<ListNewsHeadlinesResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Get the detail of a specific news story
        /// </summary>		
		/// <param name="storyId">The news story Id</param>
        /// <returns></returns>
        public GetNewsDetailResponseDTO GetNewsDetail(String storyId)
        {
       
            return Request<GetNewsDetailResponseDTO>("news","/{storyId}", "GET", new Dictionary<string, object>
                                {
									{"storyId",storyId},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        /// <summary>
        /// Get the detail of a specific news story
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="storyId">The news story Id</param>
        /// <returns></returns>
        public void BeginGetNewsDetail(String storyId, ApiAsyncCallback<GetNewsDetailResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "news","/{storyId}", "GET",new Dictionary<string, object>
                                {
									{"storyId",storyId},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        public GetNewsDetailResponseDTO EndGetNewsDetail(ApiAsyncResult<GetNewsDetailResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Returns a list of CFD markets filtered by market name and/or market code
        /// </summary>		
		/// <param name="searchByMarketName">The characters that the CFD market name should start with</param>
		/// <param name="searchByMarketCode">The characters that the market code should start with (normally this is the RIC code for the market)</param>
		/// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
		/// <param name="maxResults">The maximum number of markets to return.</param>
        /// <returns></returns>
        public ListCfdMarketsResponseDTO ListCfdMarkets(String searchByMarketName, String searchByMarketCode, Int32 clientAccountId, Int32 maxResults)
        {
       
            return Request<ListCfdMarketsResponseDTO>("cfd/markets","?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET", new Dictionary<string, object>
                                {
									{"searchByMarketName",searchByMarketName},
									{"searchByMarketCode",searchByMarketCode},
									{"clientAccountId",clientAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// Returns a list of CFD markets filtered by market name and/or market code
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="searchByMarketName">The characters that the CFD market name should start with</param>
		/// <param name="searchByMarketCode">The characters that the market code should start with (normally this is the RIC code for the market)</param>
		/// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
		/// <param name="maxResults">The maximum number of markets to return.</param>
        /// <returns></returns>
        public void BeginListCfdMarkets(String searchByMarketName, String searchByMarketCode, Int32 clientAccountId, Int32 maxResults, ApiAsyncCallback<ListCfdMarketsResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "cfd/markets","?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET",new Dictionary<string, object>
                                {
									{"searchByMarketName",searchByMarketName},
									{"searchByMarketCode",searchByMarketCode},
									{"clientAccountId",clientAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public ListCfdMarketsResponseDTO EndListCfdMarkets(ApiAsyncResult<ListCfdMarketsResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Returns a list of Spread Betting markets filtered by market name and/or market code
        /// </summary>		
		/// <param name="searchByMarketName">The characters that the Spread market name should start with</param>
		/// <param name="searchByMarketCode">The characters that the Spread market code should start with (normally this is the RIC code for the market)</param>
		/// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
		/// <param name="maxResults">The maximum number of markets to return.</param>
        /// <returns></returns>
        public ListSpreadMarketsResponseDTO ListSpreadMarkets(String searchByMarketName, String searchByMarketCode, Int32 clientAccountId, Int32 maxResults)
        {
       
            return Request<ListSpreadMarketsResponseDTO>("spread/markets","?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET", new Dictionary<string, object>
                                {
									{"searchByMarketName",searchByMarketName},
									{"searchByMarketCode",searchByMarketCode},
									{"clientAccountId",clientAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        /// <summary>
        /// Returns a list of Spread Betting markets filtered by market name and/or market code
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="searchByMarketName">The characters that the Spread market name should start with</param>
		/// <param name="searchByMarketCode">The characters that the Spread market code should start with (normally this is the RIC code for the market)</param>
		/// <param name="clientAccountId">The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)</param>
		/// <param name="maxResults">The maximum number of markets to return.</param>
        /// <returns></returns>
        public void BeginListSpreadMarkets(String searchByMarketName, String searchByMarketCode, Int32 clientAccountId, Int32 maxResults, ApiAsyncCallback<ListSpreadMarketsResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "spread/markets","?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}", "GET",new Dictionary<string, object>
                                {
									{"searchByMarketName",searchByMarketName},
									{"searchByMarketCode",searchByMarketCode},
									{"clientAccountId",clientAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(10000),"data");
        }

        public ListSpreadMarketsResponseDTO EndListSpreadMarkets(ApiAsyncResult<ListSpreadMarketsResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Place an order on a particular market. Post a <a onclick="dojo.hash('#type.NewStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">NewStopLimitOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new order, the platform will generate them.</p>
        /// </summary>		
		/// <param name="OrderId">Order identifier of the order to update</param>
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Currency">Currency to place order in</param>
		/// <param name="AutoRollover">Flag to indicate whether the trade will automatically roll into the              next market when the current market expires</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
		/// <param name="IfDone">List of IfDone Orders which will be filled when             the initial trade/order is triggered</param>
		/// <param name="OcoOrder">Corresponding OcoOrder (One Cancels the Other)</param>
		/// <param name="Applicability">Identifier which relates to the expiry of the             order/trade, i.e. GoodTillDate (GTD),              GoodTillCancelled (GTC) or GoodForDay (GFD)</param>
		/// <param name="ExpiryDateTimeUTC">The associated expiry DateTime for a              pair of GoodTillDate IfDone orders</param>
		/// <param name="Guaranteed">Flag to determine whether an order is guaranteed to trigger and fill at             the associated trigger price</param>
		/// <param name="TriggerPrice">Price at which the order is intended to be triggered</param>
        /// <returns></returns>
        public ApiTradeOrderResponseDTO Order(Int32 OrderId, Int32 MarketId, String Currency, Boolean AutoRollover, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId, ApiIfDoneDTO[] IfDone, NewStopLimitOrderRequestDTO OcoOrder, String Applicability, DateTime? ExpiryDateTimeUTC, Boolean Guaranteed, Decimal TriggerPrice)
        {
       
            return Request<ApiTradeOrderResponseDTO>("order","/newstoplimitorder", "POST", new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
									{"MarketId",MarketId},
									{"Currency",Currency},
									{"AutoRollover",AutoRollover},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
									{"IfDone",IfDone},
									{"OcoOrder",OcoOrder},
									{"Applicability",Applicability},
									{"ExpiryDateTimeUTC",ExpiryDateTimeUTC},
									{"Guaranteed",Guaranteed},
									{"TriggerPrice",TriggerPrice},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        /// <summary>
        /// <p>Place an order on a particular market. Post a <a onclick="dojo.hash('#type.NewStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">NewStopLimitOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new order, the platform will generate them.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="OrderId">Order identifier of the order to update</param>
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Currency">Currency to place order in</param>
		/// <param name="AutoRollover">Flag to indicate whether the trade will automatically roll into the              next market when the current market expires</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
		/// <param name="IfDone">List of IfDone Orders which will be filled when             the initial trade/order is triggered</param>
		/// <param name="OcoOrder">Corresponding OcoOrder (One Cancels the Other)</param>
		/// <param name="Applicability">Identifier which relates to the expiry of the             order/trade, i.e. GoodTillDate (GTD),              GoodTillCancelled (GTC) or GoodForDay (GFD)</param>
		/// <param name="ExpiryDateTimeUTC">The associated expiry DateTime for a              pair of GoodTillDate IfDone orders</param>
		/// <param name="Guaranteed">Flag to determine whether an order is guaranteed to trigger and fill at             the associated trigger price</param>
		/// <param name="TriggerPrice">Price at which the order is intended to be triggered</param>
        /// <returns></returns>
        public void BeginOrder(Int32 OrderId, Int32 MarketId, String Currency, Boolean AutoRollover, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId, ApiIfDoneDTO[] IfDone, NewStopLimitOrderRequestDTO OcoOrder, String Applicability, DateTime ExpiryDateTimeUTC, Boolean Guaranteed, Decimal TriggerPrice, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/newstoplimitorder", "POST",new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
									{"MarketId",MarketId},
									{"Currency",Currency},
									{"AutoRollover",AutoRollover},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
									{"IfDone",IfDone},
									{"OcoOrder",OcoOrder},
									{"Applicability",Applicability},
									{"ExpiryDateTimeUTC",ExpiryDateTimeUTC},
									{"Guaranteed",Guaranteed},
									{"TriggerPrice",TriggerPrice},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        public ApiTradeOrderResponseDTO EndOrder(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Cancel an order. Post a <a onclick="dojo.hash('#type.CancelOrderRequestDTO'); return false;" class="json-link" href="#">CancelOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="OrderId">The order identifier.</param>
		/// <param name="TradingAccountId">TradingAccount associated with the cancel order request.</param>
        /// <returns></returns>
        public ApiTradeOrderResponseDTO CancelOrder(Int32 OrderId, Int32 TradingAccountId)
        {
       
            return Request<ApiTradeOrderResponseDTO>("order","/cancel", "POST", new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
									{"TradingAccountId",TradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Cancel an order. Post a <a onclick="dojo.hash('#type.CancelOrderRequestDTO'); return false;" class="json-link" href="#">CancelOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="OrderId">The order identifier.</param>
		/// <param name="TradingAccountId">TradingAccount associated with the cancel order request.</param>
        /// <returns></returns>
        public void BeginCancelOrder(Int32 OrderId, Int32 TradingAccountId, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/cancel", "POST",new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
									{"TradingAccountId",TradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public ApiTradeOrderResponseDTO EndCancelOrder(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship). Post an <a onclick="dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>		
        /// <returns></returns>
        public ApiTradeOrderResponseDTO UpdateOrder(Int32 OrderId, Int32 MarketId, String Currency, Boolean AutoRollover, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId, ApiIfDoneDTO[] IfDone, NewStopLimitOrderRequestDTO OcoOrder, String Applicability, DateTime ExpiryDateTimeUTC, Boolean Guaranteed, Decimal TriggerPrice)
        {
       
            return Request<ApiTradeOrderResponseDTO>("order","/updatestoplimitorder", "POST", new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
									{"MarketId",MarketId},
									{"Currency",Currency},
									{"AutoRollover",AutoRollover},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
									{"IfDone",IfDone},
									{"OcoOrder",OcoOrder},
									{"Applicability",Applicability},
									{"ExpiryDateTimeUTC",ExpiryDateTimeUTC},
									{"Guaranteed",Guaranteed},
									{"TriggerPrice",TriggerPrice},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Update an order (for adding a stop/limit or attaching an OCO relationship). Post an <a onclick="dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;" class="json-link" href="#">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="callback"></param>
        /// <returns></returns>
        public void BeginUpdateOrder(Int32 OrderId, Int32 MarketId, String Currency, Boolean AutoRollover, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId, ApiIfDoneDTO[] IfDone, NewStopLimitOrderRequestDTO OcoOrder, String Applicability, DateTime ExpiryDateTimeUTC, Boolean Guaranteed, Decimal TriggerPrice, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/updatestoplimitorder", "POST",new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
									{"MarketId",MarketId},
									{"Currency",Currency},
									{"AutoRollover",AutoRollover},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
									{"IfDone",IfDone},
									{"OcoOrder",OcoOrder},
									{"Applicability",Applicability},
									{"ExpiryDateTimeUTC",ExpiryDateTimeUTC},
									{"Guaranteed",Guaranteed},
									{"TriggerPrice",TriggerPrice},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public ApiTradeOrderResponseDTO EndUpdateOrder(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetOpenPosition'); return false;" class="json-link" href="#">GetOpenPosition</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>		
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <returns></returns>
        public ListOpenPositionsResponseDTO ListOpenPositions(Int32 tradingAccountId)
        {
       
            return Request<ListOpenPositionsResponseDTO>("order","/order/openpositions?TradingAccountId={tradingAccountId}", "GET", new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetOpenPosition'); return false;" class="json-link" href="#">GetOpenPosition</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <returns></returns>
        public void BeginListOpenPositions(Int32 tradingAccountId, ApiAsyncCallback<ListOpenPositionsResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/order/openpositions?TradingAccountId={tradingAccountId}", "GET",new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public ListOpenPositionsResponseDTO EndListOpenPositions(ApiAsyncResult<ListOpenPositionsResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetActiveStopLimitOrder'); return false;" class="json-link" href="#">GetActiveStopLimitOrder</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>		
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <returns></returns>
        public ListActiveStopLimitOrderResponseDTO ListActiveStopLimitOrders(Int32 tradingAccountId)
        {
       
            return Request<ListActiveStopLimitOrderResponseDTO>("order","/order/activestoplimitorders?TradingAccountId={tradingAccountId}", "GET", new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick="dojo.hash('#service.GetActiveStopLimitOrder'); return false;" class="json-link" href="#">GetActiveStopLimitOrder</a> when you get updates on the order stream to get the updated data in this format.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
        /// <returns></returns>
        public void BeginListActiveStopLimitOrders(Int32 tradingAccountId, ApiAsyncCallback<ListActiveStopLimitOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/order/activestoplimitorders?TradingAccountId={tradingAccountId}", "GET",new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public ListActiveStopLimitOrderResponseDTO EndListActiveStopLimitOrders(ApiAsyncResult<ListActiveStopLimitOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for a active stop limit order with a specified order id. It will return a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListActiveStopLimitOrders'); return false;" class="json-link" href="#">ListActiveStopLimitOrders</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>		
		/// <param name="orderId">The requested order id.</param>
        /// <returns></returns>
        public GetActiveStopLimitOrderResponseDTO GetActiveStopLimitOrder(String orderId)
        {
       
            return Request<GetActiveStopLimitOrderResponseDTO>("order","/{orderId}/activestoplimitorder", "GET", new Dictionary<string, object>
                                {
									{"orderId",orderId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for a active stop limit order with a specified order id. It will return a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListActiveStopLimitOrders'); return false;" class="json-link" href="#">ListActiveStopLimitOrders</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="orderId">The requested order id.</param>
        /// <returns></returns>
        public void BeginGetActiveStopLimitOrder(String orderId, ApiAsyncCallback<GetActiveStopLimitOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/{orderId}/activestoplimitorder", "GET",new Dictionary<string, object>
                                {
									{"orderId",orderId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public GetActiveStopLimitOrderResponseDTO EndGetActiveStopLimitOrder(ApiAsyncResult<GetActiveStopLimitOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for a trade / open position with a specified order id. It will return a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListOpenPositions'); return false;" class="json-link" href="#">ListOpenPositions</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>		
		/// <param name="orderId">The requested order id.</param>
        /// <returns></returns>
        public GetOpenPositionResponseDTO GetOpenPosition(String orderId)
        {
       
            return Request<GetOpenPositionResponseDTO>("order","/{orderId}/openposition", "GET", new Dictionary<string, object>
                                {
									{"orderId",orderId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for a trade / open position with a specified order id. It will return a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick="dojo.hash('#service.ListOpenPositions'); return false;" class="json-link" href="#">ListOpenPositions</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick="dojo.hash('#service.GetOrder'); return false;" class="json-link" href="#">GetOrder</a><p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="orderId">The requested order id.</param>
        /// <returns></returns>
        public void BeginGetOpenPosition(String orderId, ApiAsyncCallback<GetOpenPositionResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/{orderId}/openposition", "GET",new Dictionary<string, object>
                                {
									{"orderId",orderId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public GetOpenPositionResponseDTO EndGetOpenPosition(ApiAsyncResult<GetOpenPositionResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).</p>
        /// </summary>		
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
		/// <param name="maxResults">The maximum results to return.</param>
        /// <returns></returns>
        public ListTradeHistoryResponseDTO ListTradeHistory(Int32 tradingAccountId, Int32 maxResults)
        {
       
            return Request<ListTradeHistoryResponseDTO>("order","/order/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET", new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
		/// <param name="maxResults">The maximum results to return.</param>
        /// <returns></returns>
        public void BeginListTradeHistory(Int32 tradingAccountId, Int32 maxResults, ApiAsyncCallback<ListTradeHistoryResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/order/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET",new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public ListTradeHistoryResponseDTO EndListTradeHistory(ApiAsyncResult<ListTradeHistoryResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for a specified trading account's stop / limit order history. The result set will include <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b> </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>		
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
		/// <param name="maxResults">the maximum results to return.</param>
        /// <returns></returns>
        public ListStopLimitOrderHistoryResponseDTO ListStopLimitOrderHistory(Int32 tradingAccountId, Int32 maxResults)
        {
       
            return Request<ListStopLimitOrderHistoryResponseDTO>("order","/order/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET", new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for a specified trading account's stop / limit order history. The result set will include <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b> </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="tradingAccountId">The trading account to get orders for.</param>
		/// <param name="maxResults">the maximum results to return.</param>
        /// <returns></returns>
        public void BeginListStopLimitOrderHistory(Int32 tradingAccountId, Int32 maxResults, ApiAsyncCallback<ListStopLimitOrderHistoryResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/order/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}", "GET",new Dictionary<string, object>
                                {
									{"tradingAccountId",tradingAccountId},
									{"maxResults",maxResults},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public ListStopLimitOrderHistoryResponseDTO EndListStopLimitOrderHistory(ApiAsyncResult<ListStopLimitOrderHistoryResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Queries for an order by a specific order id.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered)</b>.</p>
        /// </summary>		
		/// <param name="orderId">The requested order id.</param>
        /// <returns></returns>
        public GetOrderResponseDTO GetOrder(String orderId)
        {
       
            return Request<GetOrderResponseDTO>("order","/{orderId}", "GET", new Dictionary<string, object>
                                {
									{"orderId",orderId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        /// <summary>
        /// <p>Queries for an order by a specific order id.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered)</b>.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="orderId">The requested order id.</param>
        /// <returns></returns>
        public void BeginGetOrder(String orderId, ApiAsyncCallback<GetOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/{orderId}", "GET",new Dictionary<string, object>
                                {
									{"orderId",orderId},
                                }, TimeSpan.FromMilliseconds(0),"");
        }

        public GetOrderResponseDTO EndGetOrder(ApiAsyncResult<GetOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// <p>Place a trade on a particular market. Post a <a onclick="dojo.hash('#type.NewTradeOrderRequestDTO'); return false;" class="json-link" href="#">NewTradeOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new trade, the platform will generate them.</p>
        /// </summary>		
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Currency">Currency to place order in</param>
		/// <param name="AutoRollover">Flag to indicate whether the trade will automatically roll into the              next market when the current market expires</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="QuoteId">Quote Id</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
		/// <param name="IfDone">List of IfDone Orders which will be filled when             the initial trade/order is triggered</param>
		/// <param name="Close">List of existing order id's that require part             or full closure</param>
        /// <returns></returns>
        public ApiTradeOrderResponseDTO Trade(Int32 MarketId, String Currency, Boolean AutoRollover, String Direction, Decimal Quantity, String QuoteId, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId, ApiIfDoneDTO[] IfDone, Int32[] Close)
        {
       
            return Request<ApiTradeOrderResponseDTO>("order","/newtradeorder", "POST", new Dictionary<string, object>
                                {
									{"MarketId",MarketId},
									{"Currency",Currency},
									{"AutoRollover",AutoRollover},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"QuoteId",QuoteId},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
									{"IfDone",IfDone},
									{"Close",Close},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        /// <summary>
        /// <p>Place a trade on a particular market. Post a <a onclick="dojo.hash('#type.NewTradeOrderRequestDTO'); return false;" class="json-link" href="#">NewTradeOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new trade, the platform will generate them.</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Currency">Currency to place order in</param>
		/// <param name="AutoRollover">Flag to indicate whether the trade will automatically roll into the              next market when the current market expires</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="QuoteId">Quote Id</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
		/// <param name="IfDone">List of IfDone Orders which will be filled when             the initial trade/order is triggered</param>
		/// <param name="Close">List of existing order id's that require part             or full closure</param>
        /// <returns></returns>
        public void BeginTrade(Int32 MarketId, String Currency, Boolean AutoRollover, String Direction, Decimal Quantity, String QuoteId, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId, ApiIfDoneDTO[] IfDone, Int32[] Close, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/newtradeorder", "POST",new Dictionary<string, object>
                                {
									{"MarketId",MarketId},
									{"Currency",Currency},
									{"AutoRollover",AutoRollover},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"QuoteId",QuoteId},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
									{"IfDone",IfDone},
									{"Close",Close},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        public ApiTradeOrderResponseDTO EndTrade(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Update a trade (for adding a stop/limit etc). Post an <a onclick="dojo.hash('#type.UpdateTradeOrderRequestDTO'); return false;" class="json-link" href="#">UpdateTradeOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="OrderId">Order identifier of the order to update</param>
        /// <returns></returns>
        public ApiTradeOrderResponseDTO UpdateTrade(Int32 OrderId)
        {
       
            return Request<ApiTradeOrderResponseDTO>("order","/updatetradeorder", "POST", new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        /// <summary>
        /// Update a trade (for adding a stop/limit etc). Post an <a onclick="dojo.hash('#type.UpdateTradeOrderRequestDTO'); return false;" class="json-link" href="#">UpdateTradeOrderRequestDTO</a> to the uri specified below</p>
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="OrderId">Order identifier of the order to update</param>
        /// <returns></returns>
        public void BeginUpdateTrade(Int32 OrderId, ApiAsyncCallback<ApiTradeOrderResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "order","/updatetradeorder", "POST",new Dictionary<string, object>
                                {
									{"OrderId",OrderId},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        public ApiTradeOrderResponseDTO EndUpdateTrade(ApiAsyncResult<ApiTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        ///// <summary>
        ///// Whether a User is allowed to see Charting Data.
        ///// </summary>		
        ///// <param name="id">Whether a User is allowed to see Charting Data.</param>
        ///// <returns></returns>
        //public Boolean GetChartingEnabled(String id)
        //{
       
        //    return Request<Boolean>("useraccount","/UserAccount/{id}/ChartingEnabled", "GET", new Dictionary<string, object>
        //                        {
        //                            {"id",id},
        //                        }, TimeSpan.FromMilliseconds(0),"data");
        //}

        ///// <summary>
        ///// Whether a User is allowed to see Charting Data.
        ///// </summary>		
        ///// <param name="callback"></param>
        ///// <param name="id">Whether a User is allowed to see Charting Data.</param>
        ///// <returns></returns>
        //public void BeginGetChartingEnabled(String id, ApiAsyncCallback<Boolean> callback, object state)
        //{
        //    BeginRequest(callback, state, "useraccount","/UserAccount/{id}/ChartingEnabled", "GET",new Dictionary<string, object>
        //                        {
        //                            {"id",id},
        //                        }, TimeSpan.FromMilliseconds(0),"data");
        //}

        //public Boolean EndGetChartingEnabled(ApiAsyncResult<Boolean> asyncResult)
        //{
        //    return EndRequest(asyncResult);
        //}
        ///// <summary>
        ///// What are the Users Terms and Conditions.
        ///// </summary>		
        ///// <param name="clientaccount">What are the Users Terms and Conditions.</param>
        ///// <returns></returns>
        //public String GetTermsAndConditions(String clientaccount)
        //{
       
        //    return Request<String>("useraccount","/UserAccount/{clientaccount}/TermsAndConditions", "GET", new Dictionary<string, object>
        //                        {
        //                            {"clientaccount",clientaccount},
        //                        }, TimeSpan.FromMilliseconds(0),"data");
        //}

        ///// <summary>
        ///// What are the Users Terms and Conditions.
        ///// </summary>		
        ///// <param name="callback"></param>
        ///// <param name="clientaccount">What are the Users Terms and Conditions.</param>
        ///// <returns></returns>
        //public void BeginGetTermsAndConditions(String clientaccount, ApiAsyncCallback<String> callback, object state)
        //{
        //    BeginRequest(callback, state, "useraccount","/UserAccount/{clientaccount}/TermsAndConditions", "GET",new Dictionary<string, object>
        //                        {
        //                            {"clientaccount",clientaccount},
        //                        }, TimeSpan.FromMilliseconds(0),"data");
        //}

        //public String EndGetTermsAndConditions(ApiAsyncResult<String> asyncResult)
        //{
        //    return EndRequest(asyncResult);
        //}
        /// <summary>
        /// Returns the Users ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>		
        /// <returns></returns>
        public AccountInformationResponseDTO GetClientAndTradingAccount()
        {
       
            return Request<AccountInformationResponseDTO>("useraccount","/UserAccount/ClientAndTradingAccount", "GET", new Dictionary<string, object>
                                {
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// Returns the Users ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.
        /// </summary>		
		/// <param name="callback"></param>
        /// <returns></returns>
        public void BeginGetClientAndTradingAccount( ApiAsyncCallback<AccountInformationResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "useraccount","/UserAccount/ClientAndTradingAccount", "GET",new Dictionary<string, object>
                                {
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public AccountInformationResponseDTO EndGetClientAndTradingAccount(ApiAsyncResult<AccountInformationResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Simulates an error condition.
        /// </summary>		
		/// <param name="errorCode">Simulates an error condition.</param>
        /// <returns></returns>
        public ApiErrorResponseDTO GenerateException(Int32 errorCode)
        {
       
            return Request<ApiErrorResponseDTO>("errors","?errorCode={errorCode}", "GET", new Dictionary<string, object>
                                {
									{"errorCode",errorCode},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// Simulates an error condition.
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="errorCode">Simulates an error condition.</param>
        /// <returns></returns>
        public void BeginGenerateException(Int32 errorCode, ApiAsyncCallback<ApiErrorResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "errors","?errorCode={errorCode}", "GET",new Dictionary<string, object>
                                {
									{"errorCode",errorCode},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public ApiErrorResponseDTO EndGenerateException(ApiAsyncResult<ApiErrorResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
	}
}



