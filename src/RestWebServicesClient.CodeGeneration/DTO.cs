using System;
namespace CityIndex.RestWebServices.DTO
{ 
 
 
	#region NewsDTO

    /// <summary>
    /// A news headline
    /// </summary>
	public class NewsDTO
	{
        /// <summary>
        /// The unique identifier for a news story
        /// minimum : 1
        /// maximum : 2147483647
        /// </summary>
		public  Int32 StoryId{get;set;}
        /// <summary>
        /// The News story headline
        /// </summary>
		public  String Headline{get;set;}
        /// <summary>
        /// The date on which the news story was published. Always in UTC
        /// </summary>
		public  String PublishDate{get;set;}
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
        /// minimum : 1000000
        /// maximum : 9999999
        /// </summary>
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// The market name
        /// minLength : 1
        /// maxLength : 120
        /// </summary>
		public  String Name{get;set;}
	}

	#endregion

 
 
	#region IfDoneDTO

    /// <summary>
    /// An IfDone order represents an order which is placed when the corresponding order is filled, e.g attaching a stop/limit to a trade or order
    /// </summary>
	public class IfDoneDTO
	{
        /// <summary>
        /// The price at which the stop order will be filled
        /// </summary>
		public  StopLimitOrderDTO Stop{get;set;}
        /// <summary>
        /// The price at which the limit order will be filled
        /// </summary>
		public  StopLimitOrderDTO Limit{get;set;}
        /// <summary>
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillTime (GTT), GoodTillCancelled (GTC) or GoodTillDay (GTD)
        /// </summary>
		public  String Applicability{get;set;}
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillTime IfDone orders
        /// </summary>
		public  String ExpiryDateTimeUTC{get;set;}
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
        /// format : "wcf-date"
        /// </summary>
		public  DateTime BarDate{get;set;}
        /// <summary>
        /// The price at the open of the price bar interval
        /// </summary>
		public  Decimal Open{get;set;}
        /// <summary>
        /// The highest price occuring during the interval of the price bar
        /// </summary>
		public  Decimal High{get;set;}
        /// <summary>
        /// The lowest price occuring during the interval of the price bar
        /// </summary>
		public  Decimal Low{get;set;}
        /// <summary>
        /// The price at the end of the price bar interval
        /// </summary>
		public  Decimal Close{get;set;}
	}

	#endregion

 
 
	#region OrderDTO

    /// <summary>
    /// 
    /// </summary>
	public class OrderDTO
	{
	}

	#endregion

 
 
	#region StopLimitOrderDTO

    /// <summary>
    /// 
    /// </summary>
	public class StopLimitOrderDTO : OrderDTO
	{
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
        /// format : "wcf-date"
        /// </summary>
		public  DateTime TickDate{get;set;}
        /// <summary>
        /// The mid price
        /// minimum : 0.0
        /// </summary>
		public  Decimal Price{get;set;}
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

 
 
	#region NewTradeOrderRequestDTO

    /// <summary>
    /// Is not officialy part of the API yet, so don't generate documentation TODO: DAVID: omit jschema on the type to prevent meta generation
    /// </summary>
	public class NewTradeOrderRequestDTO
	{
        /// <summary>
        /// A market's unique identifier
        /// minimum : 1000000
        /// maximum : 9999999
        /// </summary>
		public  Int32 MarketId{get;set;}
        /// <summary>
        /// Direction identifier for order/trade, values supported are buy or sell
        /// </summary>
		public  String Direction{get;set;}
        /// <summary>
        /// Size of the order/trade
        /// </summary>
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower of the two
        /// </summary>
		public  Decimal BidPrice{get;set;}
        /// <summary>
        /// Market prices are quote as a pair (buy/sell or bid/offer), the OfferPrice is the higher of the market price pair
        /// </summary>
		public  Decimal OfferPrice{get;set;}
        /// <summary>
        /// Unique identifier for a price tick
        /// </summary>
		public  String AuditId{get;set;}
        /// <summary>
        /// TradingAccount associated with the trade/order request
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

 
 
	#region CreateSessionResponseDTO

    /// <summary>
    /// Response to a CreateSessionRequest
    /// </summary>
	public class CreateSessionResponseDTO
	{
        /// <summary>
        /// Your session token (treat as a random string) <BR /> Session tokens are valid for a set period (7 days) from the time of their creation. <BR /> The period is subject to change, and may vary depending on who you logon as.
        /// minLength : 36
        /// maxLength : 100
        /// </summary>
		public  String Session{get;set;}
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

 
 
	#region NewTradeOrderResponseDTO

    /// <summary>
    /// The response returned from the underlying trading system
    /// </summary>
	public class NewTradeOrderResponseDTO
	{
        /// <summary>
        /// The status of the order (Pending, Accepted, Open, etc)
        /// </summary>
		public  Int32 Status{get;set;}
        /// <summary>
        /// The id corresponding to a more descriptive reason for the order status
        /// </summary>
		public  Int32 StatusReason{get;set;}
        /// <summary>
        /// The unique identifier associated to the order returned from the underlying trading system
        /// </summary>
		public  Int32 OrderId{get;set;}
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

 
 
	#region ErrorResponseDTO

    /// <summary>
    /// This is a description of ErrorResponseDTO
    /// </summary>
	public class ErrorResponseDTO
	{
        /// <summary>
        /// This is a description of the ErrorMessage property
        /// </summary>
		public  String ErrorMessage{get;set;}
        /// <summary>
        /// The error code
        /// </summary>
		public  ErrorCode ErrorCode{get;set;}
	}

	#endregion

 
 
	#region HedgeRequestDTO

    /// <summary>
    /// Represents an incoming message for booking hedge instructions to the IP
    /// </summary>
	public class HedgeRequestDTO
	{
        /// <summary>
        /// Unique orderId as provided by the external provider
        /// </summary>
		public  String Reference{get;set;}
        /// <summary>
        /// External provider (Eg. currenex, ubs, pru bache, etc)
        /// </summary>
		public  String Source{get;set;}
        /// <summary>
        /// The dealer's user name for the given external provider
        /// </summary>
		public  String Trader{get;set;}
        /// <summary>
        /// Direction identifier for hedge, values supported are buy or sell
        /// </summary>
		public  String Direction{get;set;}
        /// <summary>
        /// Size of the hedge trade
        /// </summary>
		public  Decimal Quantity{get;set;}
        /// <summary>
        /// The price at which the hedge trade is to be placed
        /// </summary>
		public  Decimal Price{get;set;}
        /// <summary>
        /// Value date (expressed as UTC)
        /// format : "wcf-date"
        /// </summary>
		public  DateTime ValueDate{get;set;}
        /// <summary>
        /// Additional info (Eg. A/B book)
        /// </summary>
		public  String AdditionalInfo{get;set;}
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
        /// </summary>
		public  Boolean LoggedOut{get;set;}
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
        /// </summary>
		public  Int32[] ClientAccountIds{get;set;}
        /// <summary>
        /// TradingAccountIds that this session is authorized to work with
        /// </summary>
		public  Int32[] TradingAccountIds{get;set;}
        /// <summary>
        /// Whether this session token is still valid
        /// </summary>
		public  Boolean IsValid{get;set;}
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
        /// minLength : 6
        /// maxLength : 20
        /// </summary>
		public  String UserName{get;set;}
        /// <summary>
        /// Password is case sensitive
        /// minLength : 6
        /// maxLength : 20
        /// </summary>
		public  String Password{get;set;}
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


}



