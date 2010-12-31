using System;
using System.Collections.Generic;
using CIAPI.DTO;

namespace CIAPI.Core
{
	public partial class ApiContext
	{ 

        /// <summary>
        /// Create a new session. The is how you "log on" to the CIAPI.
        /// </summary>		
		/// <param name="UserName">Username is case sensitive</param>
		/// <param name="Password">Password is case sensitive</param>
        /// <returns></returns>
        public CreateSessionResponseDTO CreateSession(String UserName, String Password)
        {
       
            return Request<CreateSessionResponseDTO>("session","/", "POST", new Dictionary<string, object>
                                {
									{"UserName",UserName},
									{"Password",Password},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// Create a new session. The is how you "log on" to the CIAPI.
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="UserName">Username is case sensitive</param>
		/// <param name="Password">Password is case sensitive</param>
        /// <returns></returns>
        public void BeginCreateSession(ApiAsyncCallback<CreateSessionResponseDTO> callback, object state, String UserName, String Password)
        {
            BeginRequest(callback, state, "session","/", "POST",new Dictionary<string, object>
                                {
									{"UserName",UserName},
									{"Password",Password},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public CreateSessionResponseDTO EndCreateSession(ApiAsyncResult<CreateSessionResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Delete a session. This is how you "log off" from the CIAPI.
        /// </summary>		
		/// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
		/// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <returns></returns>
        public SessionDeletionResponseDTO DeleteSession(String userName, Guid session)
        {
       
            return Request<SessionDeletionResponseDTO>("session","/deleteSession?userName={userName}&session={session}", "POST", new Dictionary<string, object>
                                {
									{"userName",userName},
									{"session",session},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        /// <summary>
        /// Delete a session. This is how you "log off" from the CIAPI.
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="userName">Username is case sensitive. May be set as a service parameter or as a request header.</param>
		/// <param name="session">The session token. May be set as a service parameter or as a request header.</param>
        /// <returns></returns>
        public void BeginDeleteSession(ApiAsyncCallback<SessionDeletionResponseDTO> callback, object state, String userName, Guid session)
        {
            BeginRequest(callback, state, "session","/deleteSession?userName={userName}&session={session}", "POST",new Dictionary<string, object>
                                {
									{"userName",userName},
									{"session",session},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public SessionDeletionResponseDTO EndDeleteSession(ApiAsyncResult<SessionDeletionResponseDTO> asyncResult)
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
        public void BeginGetPriceBars(ApiAsyncCallback<GetPriceBarResponseDTO> callback, object state, String marketId, String interval, Int32 span, String priceBars)
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
        public void BeginGetPriceTicks(ApiAsyncCallback<GetPriceTickResponseDTO> callback, object state, String marketId, String priceTicks)
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
        public void BeginListNewsHeadlines(ApiAsyncCallback<ListNewsHeadlinesResponseDTO> callback, object state, String category, Int32 maxResults)
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
        public void BeginGetNewsDetail(ApiAsyncCallback<GetNewsDetailResponseDTO> callback, object state, String storyId)
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
        public void BeginListCfdMarkets(ApiAsyncCallback<ListCfdMarketsResponseDTO> callback, object state, String searchByMarketName, String searchByMarketCode, Int32 clientAccountId, Int32 maxResults)
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
        public void BeginListSpreadMarkets(ApiAsyncCallback<ListSpreadMarketsResponseDTO> callback, object state, String searchByMarketName, String searchByMarketCode, Int32 clientAccountId, Int32 maxResults)
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
        /// Place an order on a particular market
        /// </summary>		
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
        /// <returns></returns>
        public NewTradeOrderResponseDTO Order(Int32 MarketId, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId)
        {
       
            return Request<NewTradeOrderResponseDTO>("order","/", "POST", new Dictionary<string, object>
                                {
									{"MarketId",MarketId},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        /// <summary>
        /// Place an order on a particular market
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
        /// <returns></returns>
        public void BeginOrder(ApiAsyncCallback<NewTradeOrderResponseDTO> callback, object state, Int32 MarketId, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId)
        {
            BeginRequest(callback, state, "order","/", "POST",new Dictionary<string, object>
                                {
									{"MarketId",MarketId},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        public NewTradeOrderResponseDTO EndOrder(ApiAsyncResult<NewTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Place a trade on a particular market
        /// </summary>		
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
        /// <returns></returns>
        public NewTradeOrderResponseDTO Trade(Int32 MarketId, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId)
        {
       
            return Request<NewTradeOrderResponseDTO>("trade","/", "POST", new Dictionary<string, object>
                                {
									{"MarketId",MarketId},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        /// <summary>
        /// Place a trade on a particular market
        /// </summary>		
		/// <param name="callback"></param>
		/// <param name="MarketId">A market's unique identifier</param>
		/// <param name="Direction">Direction identifier for order/trade, values supported are buy or sell</param>
		/// <param name="Quantity">Size of the order/trade</param>
		/// <param name="BidPrice">Market prices are quoted as a pair (buy/sell or bid/offer),              the BidPrice is the lower of the two</param>
		/// <param name="OfferPrice">Market prices are quote as a pair (buy/sell or bid/offer),             the OfferPrice is the higher of the market price pair</param>
		/// <param name="AuditId">Unique identifier for a price tick</param>
		/// <param name="TradingAccountId">TradingAccount associated with the trade/order request</param>
        /// <returns></returns>
        public void BeginTrade(ApiAsyncCallback<NewTradeOrderResponseDTO> callback, object state, Int32 MarketId, String Direction, Decimal Quantity, Decimal BidPrice, Decimal OfferPrice, String AuditId, Int32 TradingAccountId)
        {
            BeginRequest(callback, state, "trade","/", "POST",new Dictionary<string, object>
                                {
									{"MarketId",MarketId},
									{"Direction",Direction},
									{"Quantity",Quantity},
									{"BidPrice",BidPrice},
									{"OfferPrice",OfferPrice},
									{"AuditId",AuditId},
									{"TradingAccountId",TradingAccountId},
                                }, TimeSpan.FromMilliseconds(0),"trading");
        }

        public NewTradeOrderResponseDTO EndTrade(ApiAsyncResult<NewTradeOrderResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
        /// <summary>
        /// Simulates an error condition.
        /// </summary>		
		/// <param name="errorCode">Simulates an error condition.</param>
        /// <returns></returns>
        public ErrorResponseDTO GenerateException(Int32 errorCode)
        {
       
            return Request<ErrorResponseDTO>("errors","?errorCode={errorCode}", "GET", new Dictionary<string, object>
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
        public void BeginGenerateException(ApiAsyncCallback<ErrorResponseDTO> callback, object state, Int32 errorCode)
        {
            BeginRequest(callback, state, "errors","?errorCode={errorCode}", "GET",new Dictionary<string, object>
                                {
									{"errorCode",errorCode},
                                }, TimeSpan.FromMilliseconds(0),"data");
        }

        public ErrorResponseDTO EndGenerateException(ApiAsyncResult<ErrorResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
	}
}



