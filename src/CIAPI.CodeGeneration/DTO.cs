using System;
using CityIndex.JsonClient.Converters;
using Newtonsoft.Json;
namespace CIAPI.DTO
{ 
 
 
	#region ApiStopLimitOrderHistoryDTO

    /// <summary>
    /// A stop or limit order from a historical perspective.
    /// </summary>
	public class ApiStopLimitOrderHistoryDTO
	{
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The markets unique identifier.
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The market's name.
        /// </summary>
		
		public  String MarketName{get;set;}
        /// <summary>
        /// The direction, buy or sell.
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// The quantity of the order when it became a trade / was cancelled etc.
        /// </summary>
		
		public  Decimal OriginalQuantity{get;set;}
        /// <summary>
        /// The price / rate that the order was filled at.
        /// </summary>
		
		public  String Price{get;set;}
        /// <summary>
        /// The price / rate that the the order was set to trigger at.
        /// </summary>
		
		public  Decimal TriggerPrice{get;set;}
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// The type of the order stop, limit or trade.
        /// </summary>
		
		public  Int32 TypeId{get;set;}
        /// <summary>
        /// When the order applies until. i.e. good till cancelled (GTC) good for day (GFD) or good till time (GTT).
        /// </summary>
		
		public  Int32 OrderApplicabilityId{get;set;}
        /// <summary>
        /// The trade currency.
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// the order status.
        /// </summary>
		
		public  Int32 StatusId{get;set;}
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime LastChangedDateTimeUtc{get;set;}
	}

	#endregion

 
 
	#region ApiIfDoneDTO

    /// <summary>
    /// An IfDone order represents an order which is placed when the corresponding order is filled, e.g attaching a stop/limit to a trade or order
    /// </summary>
	public class ApiIfDoneDTO
	{
        /// <summary>
        /// The price at which the stop order will be filled
        /// </summary>
		
		public  ApiStopLimitOrderDTO Stop{get;set;}
        /// <summary>
        /// The price at which the limit order will be filled
        /// </summary>
		
		public  ApiStopLimitOrderDTO Limit{get;set;}
	}

	#endregion

 
 
	#region ApiTradingAccountDTO

    /// <summary>
    /// Information about a TradingAccount
    /// </summary>
	public class ApiTradingAccountDTO
	{
        /// <summary>
        /// Trading Account Id
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// Trading Account Code
        /// </summary>
		
		public  String TradingAccountCode{get;set;}
        /// <summary>
        /// Trading Account Status
        /// </summary>
		
		public  String TradingAccountStatus{get;set;}
        /// <summary>
        /// Trading Account Type
        /// </summary>
		
		public  String TradingAccountType{get;set;}
	}

	#endregion

 
 
	#region PriceBarDTO

    /// <summary>
    /// The details of a specific price bar, useful for plotting candlestick charts
    /// </summary>
	public class PriceBarDTO
	{
        /// <summary>
        /// The date of the start of the price bar interval
        /// demoValue : "\/Date(1287136540715)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime BarDate{get;set;}
        /// <summary>
        /// For the equities model of charting, this is the price at the start of the price bar interval.
        /// demoValue : 1.5
        /// </summary>
		
		public  Decimal Open{get;set;}
        /// <summary>
        /// The highest price occurring during the interval of the price bar
        /// demoValue : 2.343
        /// </summary>
		
		public  Decimal High{get;set;}
        /// <summary>
        /// The lowest price occurring during the interval of the price bar
        /// demoValue : 1.3423
        /// </summary>
		
		public  Decimal Low{get;set;}
        /// <summary>
        /// The price at the end of the price bar interval
        /// demoValue : 2.42
        /// </summary>
		
		public  Decimal Close{get;set;}
	}

	#endregion

 
 
	#region PriceDTO

    /// <summary>
    /// A Price for a specific Market.
    /// </summary>
	public class PriceDTO
	{
        /// <summary>
        /// The Market that the Price is related to.
        /// demoValue : 1000
        /// minimum : 1
        /// maximum : 9999999
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The date of the Price. Always expressed in UTC.
        /// demoValue : "\/Date(1289231327280)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime TickDate{get;set;}
        /// <summary>
        /// The current Bid price (price at which the customer can sell).
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Bid{get;set;}
        /// <summary>
        /// The current Offer price (price at which the customer can buy, some times referred to as Ask price).
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Offer{get;set;}
        /// <summary>
        /// The current mid price.
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Price{get;set;}
        /// <summary>
        /// The highest price reached for the day.
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal High{get;set;}
        /// <summary>
        /// The lowest price reached for the day
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Low{get;set;}
        /// <summary>
        /// The change since the last price (always positive. See Direction for direction)
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Change{get;set;}
        /// <summary>
        /// The direction of movement since the last price. 1 == up, 0 == down
        /// demoValue : 1
        /// minimum : 0
        /// maximum : 1
        /// </summary>
		
		public  Int32 Direction{get;set;}
        /// <summary>
        /// A unique id for this price. Treat as a unique, but random string
        /// demoValue : "o892nkl8hopin"
        /// </summary>
		
		public  String AuditId{get;set;}
	}

	#endregion

 
 
	#region ApiTradeHistoryDTO

    /// <summary>
    /// A Trade from a historical perspective.
    /// </summary>
	public class ApiTradeHistoryDTO
	{
        /// <summary>
        /// The order id.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The market id.
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The name of the market.
        /// </summary>
		
		public  String MarketName{get;set;}
        /// <summary>
        /// The direction of the trade.
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// The original quantity of the trade, before part closures.
        /// </summary>
		
		public  Decimal OriginalQuantity{get;set;}
        /// <summary>
        /// The open price of the trade.
        /// </summary>
		
		public  Decimal Price{get;set;}
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// The trade currency.
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime LastChangedDateTimeUtc{get;set;}
	}

	#endregion

 
 
	#region ApiBasicStopLimitOrderDTO

    /// <summary>
    /// A stop or limit order with a limited number of data fields.
    /// </summary>
	public class ApiBasicStopLimitOrderDTO
	{
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The order's trigger price.
        /// </summary>
		
		public  Decimal TriggerPrice{get;set;}
        /// <summary>
        /// The quantity of the product.
        /// </summary>
		
		public  Decimal Quantity{get;set;}
	}

	#endregion

 
 
	#region NewsDTO

    /// <summary>
    /// A news headline
    /// </summary>
	public class NewsDTO
	{
        /// <summary>
        /// The unique identifier for a news story
        /// demoValue : 12654
        /// minimum : 1
        /// maximum : 2147483647
        /// </summary>
		
		public  Int32 StoryId{get;set;}
        /// <summary>
        /// The News story headline
        /// demoValue : "Barron's(8/29) Speaking Of Dividends: The Big Cheese: Kraft Foods Slices Costs And Serves A Payout Hike"
        /// </summary>
		
		public  String Headline{get;set;}
        /// <summary>
        /// The date on which the news story was published. Always in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime PublishDate{get;set;}
	}

	#endregion

 
 
	#region ApiClientAccountWatchlistItemDTO

    /// <summary>
    /// Api watchlist item
    /// </summary>
	public class ApiClientAccountWatchlistItemDTO
	{
        /// <summary>
        /// Parent watchlist id
        /// </summary>
		
		public  Int32 WatchlistId{get;set;}
        /// <summary>
        /// Watchlist item market id
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// Watchlist item display order
        /// </summary>
		
		public  Int32 DisplayOrder{get;set;}
	}

	#endregion

 
 
	#region ApiLookupDTO

    /// <summary>
    /// Generic look up data.
    /// </summary>
	public class ApiLookupDTO
	{
        /// <summary>
        /// lookups id.
        /// </summary>
		
		public  Int32 Id{get;set;}
        /// <summary>
        /// lookup items description.
        /// </summary>
		
		public  String Description{get;set;}
        /// <summary>
        /// order the items should be displayed on a user interface.
        /// </summary>
		
		public  Int32 DisplayOrder{get;set;}
        /// <summary>
        /// translation text id.
        /// </summary>
		
		public  String TranslationTextId{get;set;}
        /// <summary>
        /// translated text.
        /// </summary>
		
		public  String TranslationText{get;set;}
        /// <summary>
        /// is active.
        /// </summary>
		
		public  Boolean IsActive{get;set;}
        /// <summary>
        /// is allowed.
        /// </summary>
		
		public  Boolean IsAllowed{get;set;}
	}

	#endregion

 
 
	#region ApiOpenPositionDTO

    /// <summary>
    /// A Trade, or order that is currently open.
    /// </summary>
	public class ApiOpenPositionDTO
	{
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The markets unique identifier.
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The market's name.
        /// </summary>
		
		public  String MarketName{get;set;}
        /// <summary>
        /// The direction, buy or sell.
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// The quantity of the order.
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// The price / rate that the trade was opened at.
        /// </summary>
		
		public  Decimal Price{get;set;}
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// The trade currency.
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// The order status.
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// The stop order attached to this order.
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO StopOrder{get;set;}
        /// <summary>
        /// The limit order attached to this order.
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO LimitOrder{get;set;}
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime LastChangedDateTimeUTC{get;set;}
	}

	#endregion

 
 
	#region ApiMarketEodDTO

    /// <summary>
    /// market end of day information.
    /// </summary>
	public class ApiMarketEodDTO
	{
        /// <summary>
        /// Unit
        /// </summary>
		
		public  String MarketEodUnit{get;set;}
        /// <summary>
        /// End of day amount.
        /// </summary>
		
		public  String MarketEodAmount{get;set;}
	}

	#endregion

 
 
	#region ApiMarketInformationDTO

    /// <summary>
    /// Contains market information.
    /// </summary>
	public class ApiMarketInformationDTO
	{
        /// <summary>
        /// market id.
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The market name
        /// </summary>
		
		public  String Name{get;set;}
        /// <summary>
        /// Margin factor, expressed as points or a percentage.
        /// </summary>
		
		public  String MarginFactor{get;set;}
        /// <summary>
        /// The minimum distance from the current price you can place an order.
        /// </summary>
		
		public  String MinDistance{get;set;}
        /// <summary>
        /// The minimum quantity that can be traded over the web.
        /// </summary>
		
		public  String WebMinSize{get;set;}
        /// <summary>
        /// The max size of an order.
        /// </summary>
		
		public  String MaxSize{get;set;}
        /// <summary>
        /// Is the market a 24 hour market.
        /// </summary>
		
		public  Boolean Market24H{get;set;}
        /// <summary>
        /// the number of decimal places in the market's price.
        /// </summary>
		
		public  String PriceDecimalPlaces{get;set;}
        /// <summary>
        /// default quote length.
        /// </summary>
		
		public  String DefaultQuoteLength{get;set;}
        /// <summary>
        /// Can you trade this market on the web.
        /// </summary>
		
		public  Boolean TradeOnWeb{get;set;}
        /// <summary>
        /// New sell orders will be rejected. Orders resulting in a short open position will be red carded.
        /// </summary>
		
		public  Boolean LimitUp{get;set;}
        /// <summary>
        /// New buy orders will be rejected. Orders resulting in a long open position will be red carded.
        /// </summary>
		
		public  Boolean LimitDown{get;set;}
        /// <summary>
        /// Cannot open a short position. Equivalent to limit up.
        /// </summary>
		
		public  Boolean LongPositionOnly{get;set;}
        /// <summary>
        /// Can only close open positions. Equivalent to both Limit up and Limit down.
        /// </summary>
		
		public  Boolean CloseOnly{get;set;}
        /// <summary>
        /// list of market end of day dtos.
        /// </summary>
		
		public  ApiMarketEodDTO[] MarketEod{get;set;}
	}

	#endregion

 
 
	#region ApiOrderDTO

    /// <summary>
    /// Represents an order
    /// </summary>
	public class ApiOrderDTO
	{
        /// <summary>
        /// The order identifier
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// A market's unique identifier
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// Direction identifier for trade, values supported are buy or sell
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// Size of the order
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// The price at which the order is to be filled
        /// </summary>
		
		public  String Price{get;set;}
        /// <summary>
        /// TradingAccount associated with the order
        /// </summary>
		
		public  Decimal TradingAccountId{get;set;}
        /// <summary>
        /// Currency id for order (as represented in the trading system)
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// Status id of order (as represented in the trading system)
        /// </summary>
		
		public  Int32 StatusId{get;set;}
        /// <summary>
        /// The type of the order, Trade, stop or limit.
        /// </summary>
		
		public  Int32 TypeId{get;set;}
        /// <summary>
        /// List of IfDone Orders which will be filled when the initial order is triggered
        /// </summary>
		
		public  ApiIfDoneDTO[] IfDone{get;set;}
        /// <summary>
        /// Corresponding OcoOrder (One Cancels the Other)
        /// </summary>
		
		public  ApiStopLimitOrderDTO OcoOrder{get;set;}
	}

	#endregion

 
 
	#region ApiActiveStopLimitOrderDTO

    /// <summary>
    /// A stop or limit order that is currently active.
    /// </summary>
	public class ApiActiveStopLimitOrderDTO
	{
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The order's parent OrderId.
        /// </summary>
		
		public  String ParentOrderId{get;set;}
        /// <summary>
        /// The markets unique identifier.
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The market's name.
        /// </summary>
		
		public  String MarketName{get;set;}
        /// <summary>
        /// The direction, buy or sell.
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// The quantity of the product.
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// The marked to market price at which the order will trigger at.
        /// </summary>
		
		public  Decimal TriggerPrice{get;set;}
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
		
		public  Decimal TradingAccountId{get;set;}
        /// <summary>
        /// The type of order, i.e. stop or limit.
        /// </summary>
		
		public  Int32 Type{get;set;}
        /// <summary>
        /// When the order applies until. i.e. good till cancelled (GTC) good for day (GFD) or good till time (GTT).
        /// </summary>
		
		public  Int32 Applicability{get;set;}
        /// <summary>
        /// The associated expiry DateTime.
        /// demoValue : "\\/Date(1290164280000)\\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime ExpiryDateTimeUTC{get;set;}
        /// <summary>
        /// The trade currency.
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// The order status.
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// The if done stop order.
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO StopOrder{get;set;}
        /// <summary>
        /// The if done limit order
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO LimitOrder{get;set;}
        /// <summary>
        /// The order on the other side of a one Cancels the other relationship.
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO OcoOrder{get;set;}
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime LastChangedDateTimeUTC{get;set;}
	}

	#endregion

 
 
	#region ApiMarketDTO

    /// <summary>
    /// basic information about a Market
    /// </summary>
	public class ApiMarketDTO
	{
        /// <summary>
        /// A market's unique identifier
        /// demoValue : 254527845
        /// minimum : 1000000
        /// maximum : 9999999
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The market name
        /// demoValue : "Vodaphone CFD"
        /// minLength : 1
        /// maxLength : 120
        /// </summary>
		
		public  String Name{get;set;}
	}

	#endregion

 
 
	#region QuoteDTO

    /// <summary>
    /// A quote for a specific order request
    /// </summary>
	public class QuoteDTO
	{
        /// <summary>
        /// The uniqueId of the Quote
        /// demoValue : 54198759874
        /// </summary>
		
		public  Int32 QuoteId{get;set;}
        /// <summary>
        /// The Order the Quote is related to
        /// demoValue : 8458418478
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The Market the Quote is related to
        /// demoValue : 425748
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The Price of the original Order request for a Buy
        /// demoValue : 1.12345
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal BidPrice{get;set;}
        /// <summary>
        /// The amount the bid price will be adjusted to become an order when the customer is buying (BidPrice + BidAdjust = BuyPrice)
        /// demoValue : 1.12345
        /// minimum : -999999999.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal BidAdjust{get;set;}
        /// <summary>
        /// The Price of the original Order request for a Sell
        /// demoValue : 1.12345
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OfferPrice{get;set;}
        /// <summary>
        /// The amount the offer price will be adjusted to become an order when the customer is selling (OfferPrice + OfferAdjust = OfferPrice)
        /// demoValue : 1.12345
        /// minimum : -999999999.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OfferAdjust{get;set;}
        /// <summary>
        /// The Quantity is the number of units for the trade i.e CFD Quantity = Number of CFD's to Buy or Sell , FX Quantity = amount in base currency.
        /// demoValue : 1.1
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// The system internal Id for the ISO Currency the equivalent ISO Code can be found using the API Call TODO Fill when the API call is available
        /// demoValue : 1
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// The Status id of the Quote. The list of different Status values can be found using the API call TODO Fill when call avaliable
        /// demoValue : 1
        /// </summary>
		
		public  Int32 StatusId{get;set;}
        /// <summary>
        /// The quote type id.
        /// demoValue : 1
        /// </summary>
		
		public  Int32 TypeId{get;set;}
        /// <summary>
        /// The timestamp the quote was requested. Always expressed in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime RequestDateTime{get;set;}
	}

	#endregion

 
 
	#region ApiStopLimitOrderDTO

    /// <summary>
    /// Represents a stop/limit order
    /// </summary>
	public class ApiStopLimitOrderDTO : ApiOrderDTO
	{
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillDate IfDone orders
        /// demoValue : "\\/Date(1290164280000)\\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime ExpiryDateTimeUTC{get;set;}
        /// <summary>
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD)
        /// demoValue : "GTC"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime Applicability{get;set;}
	}

	#endregion

 
 
	#region OrderDTO

    /// <summary>
    /// An order for a specific Trading Account
    /// </summary>
	public class OrderDTO
	{
        /// <summary>
        /// Order id
        /// demoValue : 100
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// Market id.
        /// demoValue : 100
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// Client account id.
        /// demoValue : 100
        /// </summary>
		
		public  Int32 ClientAccountId{get;set;}
        /// <summary>
        /// Trading account id.
        /// demoValue : 100
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// Trade currency id.
        /// demoValue : 100
        /// minimum : 0
        /// maximum : 999999999
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// Trade currency ISO code.
        /// demoValue : "GBP"
        /// </summary>
		
		public  String CurrencyISO{get;set;}
        /// <summary>
        /// direction of the order.
        /// demoValue : 1
        /// </summary>
		
		public  Int32 Direction{get;set;}
        /// <summary>
        /// Does the order automatically roll over.
        /// demoValue : true
        /// </summary>
		
		public  Boolean AutoRollover{get;set;}
        /// <summary>
        /// the price the order was executed at.
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal ExecutionPrice{get;set;}
        /// <summary>
        /// The date time that the order was last changed. Always expressed in UTC.
        /// demoValue : "\/Date(1289231327280)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime LastChangedTime{get;set;}
        /// <summary>
        /// the open price of the order.
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OpenPrice{get;set;}
        /// <summary>
        /// The date of the Order. Always expressed in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime OriginalLastChangedDateTime{get;set;}
        /// <summary>
        /// The orders original quantity, before any part / full closures.
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OriginalQuantity{get;set;}
        /// <summary>
        /// The position method of the order.
        /// </summary>
		
		public  Int32 PositionMethodId{get;set;}
        /// <summary>
        /// The current quantity of the order.
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// the type of the order (1 = Trade / 2 = Stop / 3 = Limit)
        /// demoValue : "1"
        /// </summary>
		
		public  String Type{get;set;}
        /// <summary>
        /// The order status id.
        /// demoValue : "1"
        /// </summary>
		
		public  String Status{get;set;}
        /// <summary>
        /// the order status reason is.
        /// demoValue : 1
        /// </summary>
		
		public  Int32 ReasonId{get;set;}
	}

	#endregion

 
 
	#region PriceTickDTO

    /// <summary>
    /// The mid price at a particular point in time.
    /// </summary>
	public class PriceTickDTO
	{
        /// <summary>
        /// The datetime at which a price tick occurred. Accurate to the millisecond
        /// demoValue : "\/Date(1287136540715)\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime TickDate{get;set;}
        /// <summary>
        /// The mid price
        /// demoValue : 1.5457
        /// minimum : 0.0
        /// </summary>
		
		public  Decimal Price{get;set;}
	}

	#endregion

 
 
	#region ApiClientAccountWatchlistDTO

    /// <summary>
    /// Client account watchlist
    /// </summary>
	public class ApiClientAccountWatchlistDTO
	{
        /// <summary>
        /// Watchlist item id
        /// </summary>
		
		public  Int32 WatchlistId{get;set;}
        /// <summary>
        /// Watchlist description
        /// </summary>
		
		public  String WatchlistDescription{get;set;}
        /// <summary>
        /// Watchlist display order
        /// </summary>
		
		public  Int32 DisplayOrder{get;set;}
        /// <summary>
        /// Watchlist items
        /// </summary>
		
		public  ApiClientAccountWatchlistItemDTO[] Items{get;set;}
	}

	#endregion

 
 
	#region ClientAccountMarginDTO

    /// <summary>
    /// The current margin for a specific client account
    /// </summary>
	public class ClientAccountMarginDTO
	{
        /// <summary>
        /// cash balance expressed in the clients base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Cash{get;set;}
        /// <summary>
        /// The client account's total margin requirement expressed in base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Margin{get;set;}
        /// <summary>
        /// Margin indicator expressed as a percentage.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal MarginIndicator{get;set;}
        /// <summary>
        /// Net equity expressed in the clients base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal NetEquity{get;set;}
        /// <summary>
        /// open trade equity (open / unrealised PNL) expressed in the client's base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OpenTradeEquity{get;set;}
        /// <summary>
        /// tradable funds expressed in the client's base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal TradeableFunds{get;set;}
        /// <summary>
        /// N/A
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal PendingFunds{get;set;}
        /// <summary>
        /// trading resource expressed in the client's base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal TradingResource{get;set;}
        /// <summary>
        /// total margin requirement expressed in the client's base currency.
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal TotalMarginRequirement{get;set;}
        /// <summary>
        /// The clients base currency id.
        /// demoValue : 100
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// The clients base currency iso code.
        /// demoValue : "GBP"
        /// </summary>
		
		public  String CurrencyISO{get;set;}
	}

	#endregion

 
 
	#region NewsDetailDTO

    /// <summary>
    /// Contains details of a specific news story
    /// </summary>
	public class NewsDetailDTO : NewsDTO
	{
        /// <summary>
        /// The detail of the story. This can contain HTML characters.
        /// demoValue : "<pre>    (Expect lots of HTML here)     By Shirley A. Lazo </pre><p>    (END) Dow Jones Newswires</p><p>   August 27, 2005 00:01 ET (04:01 GMT)</p>"
        /// minLength : 0
        /// maxLength : 2147483647
        /// </summary>
		
		public  String Story{get;set;}
	}

	#endregion

 
 
	#region ApiTradeOrderDTO

    /// <summary>
    /// Represents a trade order
    /// </summary>
	public class ApiTradeOrderDTO : ApiOrderDTO
	{
	}

	#endregion

 
 
	#region ApiOrderResponseDTO

    /// <summary>
    /// order response
    /// </summary>
	public class ApiOrderResponseDTO
	{
        /// <summary>
        /// order id.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// order status reason id.
        /// </summary>
		
		public  Int32 StatusReason{get;set;}
        /// <summary>
        /// order status id.
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// price.
        /// </summary>
		
		public  Decimal Price{get;set;}
        /// <summary>
        /// commission charge.
        /// </summary>
		
		public  Decimal CommissionCharge{get;set;}
        /// <summary>
        /// list of if done orders.
        /// </summary>
		
		public  ApiIfDoneResponseDTO[] IfDone{get;set;}
        /// <summary>
        /// premium for guaranteed orders.
        /// </summary>
		
		public  Decimal GuaranteedPremium{get;set;}
        /// <summary>
        /// an order in an OCO relationship with this order.
        /// </summary>
		
		public  ApiOrderResponseDTO OCO{get;set;}
	}

	#endregion

 
 
	#region ApiStopLimitResponseDTO

    /// <summary>
    /// The response from the stop limit order request
    /// </summary>
	public class ApiStopLimitResponseDTO : ApiOrderResponseDTO
	{
	}

	#endregion

 
 
	#region ListNewsHeadlinesResponseDTO

    /// <summary>
    /// The response from a GET request for News headlines
    /// </summary>
	public class ListNewsHeadlinesResponseDTO
	{
        /// <summary>
        /// A list of News headlines
        /// </summary>
		
		public  NewsDTO[] Headlines{get;set;}
	}

	#endregion

 
 
	#region ApiLogOnRequestDTO

    /// <summary>
    /// request to create a session (log on).
    /// </summary>
	public class ApiLogOnRequestDTO
	{
        /// <summary>
        /// Username is case sensitive
        /// demoValue : "CC735158"
        /// minLength : 6
        /// maxLength : 20
        /// </summary>
		
		public  String UserName{get;set;}
        /// <summary>
        /// Password is case sensitive
        /// demoValue : "password"
        /// minLength : 6
        /// maxLength : 20
        /// </summary>
		
		public  String Password{get;set;}
	}

	#endregion

 
 
	#region GetOrderResponseDTO

    /// <summary>
    /// Response containing the order. Only one of the two fields will be populated; this depends upon the type of order (Trade or Stop / Limit).
    /// </summary>
	public class GetOrderResponseDTO
	{
        /// <summary>
        /// The details of the order if it's a trade / open position.
        /// </summary>
		
		public  ApiTradeOrderDTO TradeOrder{get;set;}
        /// <summary>
        /// The details of the order if it's a stop limit order.
        /// </summary>
		
		public  ApiStopLimitOrderDTO StopLimitOrder{get;set;}
	}

	#endregion

 
 
	#region GetActiveStopLimitOrderResponseDTO

    /// <summary>
    /// Response containing the active stop limit order.
    /// </summary>
	public class GetActiveStopLimitOrderResponseDTO
	{
        /// <summary>
        /// The active stop limit order. If it is null then the active stop limit order does not exist.
        /// </summary>
		
		public  ApiActiveStopLimitOrderDTO ActiveStopLimitOrder{get;set;}
	}

	#endregion

 
 
	#region UpdateWatchlistDisplayOrderRequestDTO

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
	public class UpdateWatchlistDisplayOrderRequestDTO
	{
        /// <summary>
        /// Represents the new client watchlist displayOrderId list sequence
        /// </summary>
		
		public  Int32[] NewDisplayOrderIdSequence{get;set;}
	}

	#endregion

 
 
	#region CancelOrderRequestDTO

    /// <summary>
    /// A cancel order request.
    /// </summary>
	public class CancelOrderRequestDTO
	{
        /// <summary>
        /// The order identifier.
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// TradingAccount associated with the cancel order request.
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
	}

	#endregion

 
 
	#region InsertWatchlistItemRequestDTO

    /// <summary>
    /// Update watchlist with item
    /// </summary>
	public class InsertWatchlistItemRequestDTO
	{
        /// <summary>
        /// The watchlist display order id to add the item
        /// </summary>
		
		public  Int32 WatchlistDisplayOrderId{get;set;}
        /// <summary>
        /// The market item to add
        /// </summary>
		
		public  Int32 MarketId{get;set;}
	}

	#endregion

 
 
	#region GetPriceTickResponseDTO

    /// <summary>
    /// The response from a request for Price Ticks
    /// </summary>
	public class GetPriceTickResponseDTO
	{
        /// <summary>
        /// An array of price ticks, sorted in ascending order by PriceTick.TickDate
        /// </summary>
		
		public  PriceTickDTO[] PriceTicks{get;set;}
	}

	#endregion

 
 
	#region ApiLogOffRequestDTO

    /// <summary>
    /// request to delete a session (log off)
    /// </summary>
	public class ApiLogOffRequestDTO
	{
        /// <summary>
        /// user name of the session to delete (log off).
        /// </summary>
		
		public  String UserName{get;set;}
        /// <summary>
        /// session identifier of the session to delete (log off).
        /// </summary>
		
		public  String Session{get;set;}
	}

	#endregion

 
 
	#region ListMarketInformationRequestDTO

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
	public class ListMarketInformationRequestDTO
	{
        /// <summary>
        /// The list of market ids
        /// </summary>
		
		public  Int32[] MarketIds{get;set;}
	}

	#endregion

 
 
	#region ApiTradeOrderResponseDTO

    /// <summary>
    /// The response from the trade request
    /// </summary>
	public class ApiTradeOrderResponseDTO
	{
        /// <summary>
        /// The status of the order (Pending, Accepted, Open, etc)
        /// demoValue : 1
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// The id corresponding to a more descriptive reason for the order status
        /// demoValue : 1
        /// </summary>
		
		public  Int32 StatusReason{get;set;}
        /// <summary>
        /// The unique identifier associated to the order returned from the underlying trading system
        /// demoValue : 1
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// List of orders with their associated response
        /// </summary>
		
		public  ApiOrderResponseDTO[] Orders{get;set;}
        /// <summary>
        /// Quote response
        /// </summary>
		
		public  ApiQuoteResponseDTO Quote{get;set;}
	}

	#endregion

 
 
	#region ApiQuoteResponseDTO

    /// <summary>
    /// quote response.
    /// </summary>
	public class ApiQuoteResponseDTO
	{
        /// <summary>
        /// quote id.
        /// </summary>
		
		public  Int32 QuoteId{get;set;}
        /// <summary>
        /// quote status.
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// quote status reason.
        /// </summary>
		
		public  Int32 StatusReason{get;set;}
	}

	#endregion

 
 
	#region ApiLogOnResponseDTO

    /// <summary>
    /// Response to a CreateSessionRequest
    /// </summary>
	public class ApiLogOnResponseDTO
	{
        /// <summary>
        /// Your session token (treat as a random string) <BR /> Session tokens are valid for a set period from the time of their creation. <BR /> The period is subject to change, and may vary depending on who you logon as.
        /// demoValue : "D2FF3E4D-01EA-4741-86F0-437C919B5559"
        /// minLength : 36
        /// maxLength : 100
        /// </summary>
		
		public  String Session{get;set;}
	}

	#endregion

 
 
	#region ListMarketInformationResponseDTO

    /// <summary>
    /// Response from am market information request.
    /// </summary>
	public class ListMarketInformationResponseDTO
	{
        /// <summary>
        /// The requested list of market information.
        /// </summary>
		
		public  ApiMarketInformationDTO[] MarketInformation{get;set;}
	}

	#endregion

 
 
	#region AccountInformationResponseDTO

    /// <summary>
    /// response from an account information query.
    /// </summary>
	public class AccountInformationResponseDTO
	{
        /// <summary>
        /// logon user name.
        /// </summary>
		
		public  String LogonUserName{get;set;}
        /// <summary>
        /// client account id.
        /// </summary>
		
		public  Int32 ClientAccountId{get;set;}
        /// <summary>
        /// Base currency of the client account.
        /// </summary>
		
		public  String ClientAccountCurrency{get;set;}
        /// <summary>
        /// a list of trading accounts.
        /// </summary>
		
		public  ApiTradingAccountDTO[] TradingAccounts{get;set;}
	}

	#endregion

 
 
	#region NewStopLimitOrderRequestDTO

    /// <summary>
    /// A request for a stop/limit order
    /// </summary>
	public class NewStopLimitOrderRequestDTO
	{
        /// <summary>
        /// Order identifier of the order to update
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// A market's unique identifier
        /// demoValue : 71442
        /// minimum : 1000000
        /// maximum : 9999999
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// Currency to place order in
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// Flag to indicate whether the trade will automatically roll into the next market when the current market expires
        /// </summary>
		
		public  Boolean AutoRollover{get;set;}
        /// <summary>
        /// Direction identifier for order/trade, values supported are buy or sell
        /// demoValue : "buy"
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// Size of the order/trade
        /// demoValue : 1.0
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower of the two
        /// demoValue : 100.0
        /// </summary>
		
		public  Decimal BidPrice{get;set;}
        /// <summary>
        /// Market prices are quote as a pair (buy/sell or bid/offer), the OfferPrice is the higher of the market price pair
        /// demoValue : 110.0
        /// </summary>
		
		public  Decimal OfferPrice{get;set;}
        /// <summary>
        /// Unique identifier for a price tick
        /// demoValue : "5998CBE8-3594-4232-A57E-09EC3A4E7AA8"
        /// </summary>
		
		public  String AuditId{get;set;}
        /// <summary>
        /// TradingAccount associated with the trade/order request
        /// demoValue : 1
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// List of IfDone Orders which will be filled when the initial trade/order is triggered
        /// </summary>
		
		public  ApiIfDoneDTO[] IfDone{get;set;}
        /// <summary>
        /// Corresponding OcoOrder (One Cancels the Other)
        /// </summary>
		
		public  NewStopLimitOrderRequestDTO OcoOrder{get;set;}
        /// <summary>
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD)
        /// demoValue : "GTC"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime Applicability{get;set;}
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillDate IfDone orders
        /// demoValue : "\\/Date(1290164280000)\\/"
        /// format : "wcf-date"
        /// </summary>
		
		public  DateTime ExpiryDateTimeUTC{get;set;}
        /// <summary>
        /// Flag to determine whether an order is guaranteed to trigger and fill at the associated trigger price
        /// </summary>
		
		public  Boolean Guaranteed{get;set;}
        /// <summary>
        /// Price at which the order is intended to be triggered
        /// </summary>
		
		public  Decimal TriggerPrice{get;set;}
	}

	#endregion

 
 
	#region UpdateStopLimitOrderRequestDTO

    /// <summary>
    /// A request for updating a stop/limit order
    /// </summary>
	public class UpdateStopLimitOrderRequestDTO : NewStopLimitOrderRequestDTO
	{
	}

	#endregion

 
 
	#region GetPriceBarResponseDTO

    /// <summary>
    /// The response from a GET price bar history request. Contains both an array of finalized price bars, and a partial (not finalized) bar for the current period
    /// </summary>
	public class GetPriceBarResponseDTO
	{
        /// <summary>
        /// An array of finalized price bars, sorted in ascending order based on PriceBar.BarDate
        /// </summary>
		
		public  PriceBarDTO[] PriceBars{get;set;}
        /// <summary>
        /// The (non-finalized) price bar data for the current period (i.e, the period that hasn't yet completed)
        /// </summary>
		
		public  PriceBarDTO PartialPriceBar{get;set;}
	}

	#endregion

 
 
	#region ListStopLimitOrderHistoryResponseDTO

    /// <summary>
    /// Contains the result of a ListStopLimitOrderHistory query
    /// </summary>
	public class ListStopLimitOrderHistoryResponseDTO
	{
        /// <summary>
        /// A list of historical stop / limit orders.
        /// </summary>
		
		public  ApiStopLimitOrderHistoryDTO[] StopLimitOrderHistory{get;set;}
	}

	#endregion

 
 
	#region GetMarketInformationResponseDTO

    /// <summary>
    /// Response from am market information request.
    /// </summary>
	public class GetMarketInformationResponseDTO
	{
        /// <summary>
        /// The requested market information.
        /// </summary>
		
		public  ApiMarketInformationDTO MarketInformation{get;set;}
	}

	#endregion

 
 
	#region GetMessagePopupResponseDTO

    /// <summary>
    /// Message popup response denoting whether the client application should display a popup notification at startup.
    /// </summary>
	public class GetMessagePopupResponseDTO
	{
        /// <summary>
        /// Should the client application ask for client approval.
        /// </summary>
		
		public  Boolean AskForClientApproval{get;set;}
        /// <summary>
        /// The message to display to the client.
        /// </summary>
		
		public  String Message{get;set;}
	}

	#endregion

 
 
	#region ApiErrorResponseDTO

    /// <summary>
    /// This is a description of ErrorResponseDTO
    /// </summary>
	public class ApiErrorResponseDTO
	{
        /// <summary>
        /// This is a description of the ErrorMessage property
        /// demoValue : "sample value"
        /// </summary>
		
		public  String ErrorMessage{get;set;}
        /// <summary>
        /// The error code
        /// </summary>
		
		public  ErrorCode ErrorCode{get;set;}
	}

	#endregion

 
 
	#region ListMarketInformationSearchResponseDTO

    /// <summary>
    /// Response from a market information search request.
    /// </summary>
	public class ListMarketInformationSearchResponseDTO
	{
        /// <summary>
        /// The requested list of market information.
        /// </summary>
		
		public  ApiMarketInformationDTO[] MarketInformation{get;set;}
	}

	#endregion

 
 
	#region GetOpenPositionResponseDTO

    /// <summary>
    /// Response containing the open position.
    /// </summary>
	public class GetOpenPositionResponseDTO
	{
        /// <summary>
        /// The open position. If it is null then the open position does not exist.
        /// </summary>
		
		public  ApiOpenPositionDTO OpenPosition{get;set;}
	}

	#endregion

 
 
	#region ListSpreadMarketsResponseDTO

    /// <summary>
    /// Contains the result of a ListSpreadMarkets query
    /// </summary>
	public class ListSpreadMarketsResponseDTO
	{
        /// <summary>
        /// A list of Spread Betting markets
        /// </summary>
		
		public  ApiMarketDTO[] Markets{get;set;}
	}

	#endregion

 
 
	#region ListOpenPositionsResponseDTO

    /// <summary>
    /// Contains the result of a ListOpenPositions query
    /// </summary>
	public class ListOpenPositionsResponseDTO
	{
        /// <summary>
        /// A list of trades / open positions.
        /// </summary>
		
		public  ApiOpenPositionDTO[] OpenPositions{get;set;}
	}

	#endregion

 
 
	#region ErrorCode

    /// <summary>
    /// This is a description of the ErrorCode enum
    /// </summary>
	public enum ErrorCode
	{
        /// <summary>
        /// No error has occured
        /// </summary>
		NoError=0,
        /// <summary>
        /// This is a description of Forbidden
        /// </summary>
		Forbidden=403,
        /// <summary>
        /// This is a description of InternalServerError
        /// </summary>
		InternalServerError=500,
        /// <summary>
        /// This is a description of InvalidParameterType
        /// </summary>
		InvalidParameterType=4000,
        /// <summary>
        /// This is a description of ParameterMissing
        /// </summary>
		ParameterMissing=4001,
        /// <summary>
        /// This is a description of InvalidParameterValue
        /// </summary>
		InvalidParameterValue=4002,
        /// <summary>
        /// This is a description of InvalidJsonRequest
        /// </summary>
		InvalidJsonRequest=4003,
        /// <summary>
        /// This is a description of InvalidJsonRequestCaseFormat
        /// </summary>
		InvalidJsonRequestCaseFormat=4004,
        /// <summary>
        /// The credentials used to authenticate are invalid.  Either the username, password or both are incorrect.
        /// </summary>
		InvalidCredentials=4010,
        /// <summary>
        /// The session credentials supplied are invalid
        /// </summary>
		InvalidSession=4011,
        /// <summary>
        /// There is no data available
        /// </summary>
		NoDataAvailable=5001,
	}

	#endregion

 
 
	#region ListOrdersResponseDTO

    /// <summary>
    /// This Dto is not currently used
    /// </summary>
	public class ListOrdersResponseDTO
	{
	}

	#endregion

 
 
	#region ListCfdMarketsResponseDTO

    /// <summary>
    /// Contains the result of a ListCfdMarkets query
    /// </summary>
	public class ListCfdMarketsResponseDTO
	{
        /// <summary>
        /// A list of CFD markets
        /// </summary>
		
		public  ApiMarketDTO[] Markets{get;set;}
	}

	#endregion

 
 
	#region ApiLogOffResponseDTO

    /// <summary>
    /// Response from a session delete request.
    /// </summary>
	public class ApiLogOffResponseDTO
	{
        /// <summary>
        /// LogOut status
        /// demoValue : true
        /// </summary>
		
		public  Boolean LoggedOut{get;set;}
	}

	#endregion

 
 
	#region ListWatchlistResponseDTO

    /// <summary>
    /// Gets the client watchlist
    /// </summary>
	public class ListWatchlistResponseDTO
	{
        /// <summary>
        /// Client account id
        /// </summary>
		
		public  Int32 ClientAccountId{get;set;}
	}

	#endregion

 
 
	#region ListTradeHistoryResponseDTO

    /// <summary>
    /// Contains the result of a ListTradeHistory query
    /// </summary>
	public class ListTradeHistoryResponseDTO
	{
        /// <summary>
        /// A list of historical trades.
        /// </summary>
		
		public  ApiTradeHistoryDTO[] TradeHistory{get;set;}
	}

	#endregion

 
 
	#region ListActiveStopLimitOrderResponseDTO

    /// <summary>
    /// Contains the result of a ListActiveStopLimitOrder query
    /// </summary>
	public class ListActiveStopLimitOrderResponseDTO
	{
        /// <summary>
        /// The requested list of active stop / limit orders.
        /// </summary>
		
		public  ApiActiveStopLimitOrderDTO[] ActiveStopLimitOrders{get;set;}
	}

	#endregion

 
 
	#region NewTradeOrderRequestDTO

    /// <summary>
    /// A request for a trade order
    /// </summary>
	public class NewTradeOrderRequestDTO
	{
        /// <summary>
        /// A market's unique identifier
        /// demoValue : 71442
        /// minimum : 1000000
        /// maximum : 9999999
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// Currency to place order in
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// Flag to indicate whether the trade will automatically roll into the next market when the current market expires
        /// </summary>
		
		public  Boolean AutoRollover{get;set;}
        /// <summary>
        /// Direction identifier for order/trade, values supported are buy or sell
        /// demoValue : "buy"
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// Size of the order/trade
        /// demoValue : 1.0
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// Quote Id
        /// </summary>
		
		public  String QuoteId{get;set;}
        /// <summary>
        /// Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower of the two
        /// demoValue : 100.0
        /// </summary>
		
		public  Decimal BidPrice{get;set;}
        /// <summary>
        /// Market prices are quote as a pair (buy/sell or bid/offer), the OfferPrice is the higher of the market price pair
        /// demoValue : 110.0
        /// </summary>
		
		public  Decimal OfferPrice{get;set;}
        /// <summary>
        /// Unique identifier for a price tick
        /// demoValue : "5998CBE8-3594-4232-A57E-09EC3A4E7AA8"
        /// </summary>
		
		public  String AuditId{get;set;}
        /// <summary>
        /// TradingAccount associated with the trade/order request
        /// demoValue : 1
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// List of IfDone Orders which will be filled when the initial trade/order is triggered
        /// </summary>
		
		public  ApiIfDoneDTO[] IfDone{get;set;}
        /// <summary>
        /// List of existing order id's that require part or full closure
        /// </summary>
		
		public  Int32[] Close{get;set;}
	}

	#endregion

 
 
	#region UpdateTradeOrderRequestDTO

    /// <summary>
    /// A request for updating a trade order
    /// </summary>
	public class UpdateTradeOrderRequestDTO : NewTradeOrderRequestDTO
	{
        /// <summary>
        /// Order identifier of the order to update
        /// </summary>
		
		public  Int32 OrderId{get;set;}
	}

	#endregion

 
 
	#region GetNewsDetailResponseDTO

    /// <summary>
    /// JSON returned from News Detail GET request
    /// </summary>
	public class GetNewsDetailResponseDTO
	{
        /// <summary>
        /// The details of the news item
        /// </summary>
		
		public  NewsDetailDTO NewsDetail{get;set;}
	}

	#endregion

 
 
	#region ApiLookupResponseDTO

    /// <summary>
    /// Gets the lookup entities from trading database given the lookup name and culture id
    /// </summary>
	public class ApiLookupResponseDTO
	{
        /// <summary>
        /// The culture id requested
        /// </summary>
		
		public  Int32 CultureId{get;set;}
        /// <summary>
        /// The lookup name requested
        /// </summary>
		
		public  String LookupEntityName{get;set;}
        /// <summary>
        /// List of lookup entities from the database
        /// </summary>
		
		public  ApiLookupDTO[] ApiLookupDTOList{get;set;}
	}

	#endregion

 
 
	#region ApiIfDoneResponseDTO

    /// <summary>
    /// if done
    /// </summary>
	public class ApiIfDoneResponseDTO
	{
        /// <summary>
        /// Stop
        /// </summary>
		
		public  ApiOrderResponseDTO Stop{get;set;}
        /// <summary>
        /// Limit
        /// </summary>
		
		public  ApiOrderResponseDTO Limit{get;set;}
	}

	#endregion

 
 
	#region SystemStatusDTO

    /// <summary>
    /// system status
    /// </summary>
	public class SystemStatusDTO
	{
        /// <summary>
        /// a status message
        /// </summary>
		
		public  String StatusMessage{get;set;}
	}

	#endregion

 
 
	#region SystemStatusRequestDTO

    /// <summary>
    /// system status request.
    /// </summary>
	public class SystemStatusRequestDTO
	{
        /// <summary>
        /// depth to test.
        /// </summary>
		
		public  String TestDepth{get;set;}
	}

	#endregion


}



