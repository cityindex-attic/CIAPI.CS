using System;
using CityIndex.JsonClient.Converters;
using Newtonsoft.Json;
namespace CIAPI.DTO
{ 
 
 
	#region GatewayOrderDTO

    /// <summary>
    /// Represents an order
    /// </summary>
	public class GatewayOrderDTO
	{
	}

	#endregion

 
 
	#region GatewayStopLimitOrderDTO

    /// <summary>
    /// Represents a stop/limit order
    /// </summary>
	public class GatewayStopLimitOrderDTO : GatewayOrderDTO
	{
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillDate IfDone orders
        /// demoValue : "\\/Date(1290164280000)\\/"
        /// </summary>
		
		public  String ExpiryDateTimeUTC{get;set;}
        /// <summary>
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD)
        /// demoValue : "GTC"
        /// </summary>
		
		public  String Applicability{get;set;}
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
        /// </summary>
		
		public DateTime PublishDate{get;set;}
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

 
 
	#region PriceTickDTO

    /// <summary>
    /// The mid price at a particular point in time.
    /// </summary>
	public class PriceTickDTO
	{
        /// <summary>
        /// The datetime at which a price tick occured. Accurate to the millisecond
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

 
 
	#region OrderDTO

    /// <summary>
    /// An order for a specific Trading Account
    /// </summary>
	public class OrderDTO
	{
        /// <summary>
        /// ???
        /// demoValue : 100
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100
        /// </summary>
		
		public  Int32 ClientAccountId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100
        /// minimum : 0
        /// maximum : 999999999
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : "GBP"
        /// </summary>
		
		public  String CurrencyISO{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1
        /// </summary>
		
		public  Int32 Direction{get;set;}
        /// <summary>
        /// ???
        /// demoValue : true
        /// </summary>
		
		public  Boolean AutoRollover{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal ExecutionPrice{get;set;}
        /// <summary>
        /// The date of the Order. Always expressed in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// </summary>
		
		public  String LastChangedTime{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OpenPrice{get;set;}
        /// <summary>
        /// The date of the Order. Always expressed in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// </summary>
		
		public  String OriginalLastChangedDateTime{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OriginalQuantity{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// ???
        /// demoValue : "TODO"
        /// </summary>
		
		public  String Type{get;set;}
        /// <summary>
        /// ???
        /// demoValue : "96.1575"
        /// </summary>
		
		public  String Status{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1
        /// </summary>
		
		public  Int32 ReasonId{get;set;}
	}

	#endregion

 
 
	#region ClientAccountMarginDTO

    /// <summary>
    /// The current margin for a specific client account
    /// </summary>
	public class ClientAccountMarginDTO
	{
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Cash{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Margin{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal MarginIndicator{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal NetEquity{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal OpenTradeEquity{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal TradeableFunds{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal PendingFunds{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal TradingResource{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100.0
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal TotalMarginRequirement{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 100
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : "GBP"
        /// </summary>
		
		public  String CurrencyISO{get;set;}
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
        /// The price at the open of the price bar interval
        /// demoValue : 1.5
        /// </summary>
		
		public  Decimal Open{get;set;}
        /// <summary>
        /// The highest price occuring during the interval of the price bar
        /// demoValue : 2.343
        /// </summary>
		
		public  Decimal High{get;set;}
        /// <summary>
        /// The lowest price occuring during the interval of the price bar
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

 
 
	#region GatewayIfDoneDTO

    /// <summary>
    /// An IfDone order represents an order which is placed when the corresponding order is filled, e.g attaching a stop/limit to a trade or order
    /// </summary>
	public class GatewayIfDoneDTO
	{
        /// <summary>
        /// The price at which the stop order will be filled
        /// </summary>
		
		public  GatewayStopLimitOrderDTO Stop{get;set;}
        /// <summary>
        /// The price at which the limit order will be filled
        /// </summary>
		
		public  GatewayStopLimitOrderDTO Limit{get;set;}
	}

	#endregion

 
 
	#region ApiTradeHistoryDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ApiTradeHistoryDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String MarketName{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String Direction{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Decimal OriginalQuantity{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Decimal Price{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 TradingAccountId{get;set;}
        /// <summary>
        /// The trade currency
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String LastChangedDateTimeUtc{get;set;}
	}

	#endregion

 
 
	#region ApiActiveStopLimitOrderDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ApiActiveStopLimitOrderDTO
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
        /// TODO
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Decimal TriggerPrice{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Decimal TradingAccountId{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String Type{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String Applicability{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String Status{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO StopOrder{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO LimitOrder{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO OcoOrder{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  String LastChangedDateTimeUTC{get;set;}
	}

	#endregion

 
 
	#region PriceDTO

    /// <summary>
    /// A Price for a specific Market
    /// </summary>
	public class PriceDTO
	{
        /// <summary>
        /// The Market that the Price is related to
        /// demoValue : 1000
        /// minimum : 1
        /// maximum : 9999999
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The date of the Price. Always expressed in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// </summary>
		
		public  String TickDate{get;set;}
        /// <summary>
        /// The current Bid price (price at which the customer can sell)
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Bid{get;set;}
        /// <summary>
        /// The current Offer price (price at which the customer can buy)
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Offer{get;set;}
        /// <summary>
        /// The current mid price
        /// demoValue : 96.1575
        /// minimum : 0.0
        /// maximum : 999999999.0
        /// </summary>
		
		public  Decimal Price{get;set;}
        /// <summary>
        /// The highest price reached for the day
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

 
 
	#region ApiOpenPositionDTO

    /// <summary>
    /// TODO
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
		
		public  String Status{get;set;}
        /// <summary>
        /// The stop order attached to this order.
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO StopOrder{get;set;}
        /// <summary>
        /// The limit order attached to this order.
        /// </summary>
		
		public  ApiBasicStopLimitOrderDTO LimitOrder{get;set;}
        /// <summary>
        /// The date time the order was last changed.
        /// </summary>
		
		public  String LastChangedDateTimeUTC{get;set;}
	}

	#endregion

 
 
	#region ApiBasicStopLimitOrderDTO

    /// <summary>
    /// TODO
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
	}

	#endregion

 
 
	#region MarketDTO

    /// <summary>
    /// Information about a Market
    /// </summary>
	public class MarketDTO
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

 
 
	#region ApiStopLimitOrderHistoryDTO

    /// <summary>
    /// TODO
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
        /// The type of the order stop, limit or trade
        /// </summary>
		
		public  Int32 TypeId{get;set;}
        /// <summary>
        /// When the order applies until. ie good till cancelled (GTC) good for day (GFD) or good till time (GTT).
        /// </summary>
		
		public  Int32 OrderApplicabilityId{get;set;}
        /// <summary>
        /// The trade currency
        /// </summary>
		
		public  String Currency{get;set;}
        /// <summary>
        /// the order status.
        /// </summary>
		
		public  Int32 StatusId{get;set;}
        /// <summary>
        /// The date time the order was last changed.
        /// </summary>
		
		public  String LastChangedDateTimeUtc{get;set;}
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
        /// demoValue : 1
        /// </summary>
		
		public  Int32 QuoteId{get;set;}
        /// <summary>
        /// The Order the Quote is related to
        /// demoValue : 1
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// The Market the Quote is related to
        /// demoValue : 1
        /// </summary>
		
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// ????
        /// demoValue : 1.1
        /// </summary>
		
		public  Decimal BidPrice{get;set;}
        /// <summary>
        /// ????
        /// demoValue : 1.1
        /// </summary>
		
		public  Decimal BidAdjust{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1.1
        /// </summary>
		
		public  Decimal OfferPrice{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1.1
        /// </summary>
		
		public  Decimal OfferAdjust{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1.1
        /// </summary>
		
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1
        /// </summary>
		
		public  Int32 CurrencyId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1
        /// </summary>
		
		public  Int32 StatusId{get;set;}
        /// <summary>
        /// ???
        /// demoValue : 1
        /// </summary>
		
		public  Int32 TypeId{get;set;}
        /// <summary>
        /// The timestamp the quote was requested. Always expressed in UTC
        /// demoValue : "\/Date(1289231327280)\/"
        /// </summary>
		
		public  String RequestDateTime{get;set;}
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

 
 
	#region SessionDeletionResponseDTO

    /// <summary>
    /// 
    /// </summary>
	public class SessionDeletionResponseDTO
	{
        /// <summary>
        /// LogOut status
        /// demoValue : true
        /// </summary>
		
		public  Boolean LoggedOut{get;set;}
	}

	#endregion

 
 
	#region ListOrdersResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ListOrdersResponseDTO
	{
	}

	#endregion

 
 
	#region ListStopLimitOrderHistoryResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ListStopLimitOrderHistoryResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiStopLimitOrderHistoryDTO[] StopLimitOrderHistory{get;set;}
	}

	#endregion

 
 
	#region ApiQuoteResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ApiQuoteResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 QuoteId{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 StatusReason{get;set;}
	}

	#endregion

 
 
	#region ErrorResponseDTO

    /// <summary>
    /// This is a description of ErrorResponseDTO
    /// </summary>
	public class ErrorResponseDTO
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

 
 
	#region ApiOrderResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ApiOrderResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 OrderId{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 StatusReason{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 Status{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Int32 Price{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Decimal CommissionCharge{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiIfDoneResponseDTO[] IfDone{get;set;}
	}

	#endregion

 
 
	#region ApiStopLimitResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ApiStopLimitResponseDTO : ApiOrderResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  Decimal GuaranteedPremium{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiStopLimitResponseDTO OCO{get;set;}
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

 
 
	#region G2SessionValidationResponseDTO

    /// <summary>
    /// 
    /// </summary>
	public class G2SessionValidationResponseDTO
	{
        /// <summary>
        /// ClientAccountIds that this session is authorized to work with
        /// demoValue : [  1,  2,  3,  4]
        /// </summary>
		
		public  Int32[] ClientAccountIds{get;set;}
        /// <summary>
        /// TradingAccountIds that this session is authorized to work with
        /// demoValue : [  1,  2,  3,  4]
        /// </summary>
		
		public  Int32[] TradingAccountIds{get;set;}
        /// <summary>
        /// Whether this session token is still valid
        /// demoValue : true
        /// </summary>
		
		public  Boolean IsValid{get;set;}
	}

	#endregion

 
 
	#region CreateSessionResponseDTO

    /// <summary>
    /// Response to a CreateSessionRequest
    /// </summary>
	public class CreateSessionResponseDTO
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

 
 
	#region LogOnRequestDTO

    /// <summary>
    /// 
    /// </summary>
	public class LogOnRequestDTO
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

 
 
	#region ListSpreadMarketsResponseDTO

    /// <summary>
    /// Contains the result of a ListSpreadMarketsResponseDTO query
    /// </summary>
	public class ListSpreadMarketsResponseDTO
	{
        /// <summary>
        /// A list of Spread Betting markets
        /// </summary>
		
		public  MarketDTO[] Markets{get;set;}
	}

	#endregion

 
 
	#region ErrorCode

    /// <summary>
    /// This is a description of the ErrorCode enum
    /// </summary>
	public enum ErrorCode
	{
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
	}

	#endregion

 
 
	#region ListOpenPositionsResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ListOpenPositionsResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiOpenPositionDTO[] OpenPositions{get;set;}
	}

	#endregion

 
 
	#region ListActiveStopLimitOrderResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ListActiveStopLimitOrderResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiActiveStopLimitOrderDTO[] ActiveStopLimitOrders{get;set;}
	}

	#endregion

 
 
	#region ApiTradeOrderResponseDTO

    /// <summary>
    /// The response returned from the underlying trading system
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
	}

	#endregion

 
 
	#region ListTradeHistoryResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ListTradeHistoryResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiTradeHistoryDTO[] TradeHistory{get;set;}
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
	}

	#endregion

 
 
	#region OcoOrderRequestDTO

    /// <summary>
    /// An trade/order in a 'one cancels the other' relationship
    /// </summary>
	public class OcoOrderRequestDTO : NewTradeOrderRequestDTO
	{
	}

	#endregion

 
 
	#region SessionDeletionRequestDTO

    /// <summary>
    /// 
    /// </summary>
	public class SessionDeletionRequestDTO
	{
	}

	#endregion

 
 
	#region ApiIfDoneResponseDTO

    /// <summary>
    /// TODO
    /// </summary>
	public class ApiIfDoneResponseDTO
	{
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiStopLimitResponseDTO Stop{get;set;}
        /// <summary>
        /// TODO
        /// </summary>
		
		public  ApiStopLimitResponseDTO Limit{get;set;}
	}

	#endregion

 
 
	#region NewStopLimitOrderRequestDTO

    /// <summary>
    /// A request for a stop/limit order
    /// </summary>
	public class NewStopLimitOrderRequestDTO
	{
        /// <summary>
        /// A market's unique identifier
        /// demoValue : 71442
        /// minimum : 1000000
        /// maximum : 9999999
        /// </summary>
		
		public  Int32 MarketId{get;set;}
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
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD)
        /// demoValue : "GTC"
        /// </summary>
		
		public  String Applicability{get;set;}
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillDate IfDone orders
        /// demoValue : "\\/Date(1290164280000)\\/"
        /// </summary>
		
		public  String ExpiryDateTimeUTC{get;set;}
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
		
		public  MarketDTO[] Markets{get;set;}
	}

	#endregion

 
 
	#region CancelOrderRequestDTO

    /// <summary>
    /// A cancel order request
    /// </summary>
	public class CancelOrderRequestDTO
	{
        /// <summary>
        /// The order identifier
        /// </summary>
		
		public  Int32 OrderId{get;set;}
	}

	#endregion


}



