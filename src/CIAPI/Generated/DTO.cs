using System;
using System.Runtime.Serialization;
namespace CIAPI.DTO
{
    /// <summary>
    /// A stop or limit order from a historical perspective.
    /// </summary>
    [DataContract]
    public partial class ApiStopLimitOrderHistoryDTO
    {
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The markets unique identifier.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The market's name.
        /// </summary>
        [DataMember]
        public string MarketName { get; set; }
        /// <summary>
        /// The direction, buy or sell.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// The quantity of the order when it became a trade / was cancelled etc.
        /// </summary>
        [DataMember]
        public decimal OriginalQuantity { get; set; }
        /// <summary>
        /// The price / rate that the order was filled at.
        /// </summary>
        [DataMember]
        public decimal? Price { get; set; }
        /// <summary>
        /// The price / rate that the the order was set to trigger at.
        /// </summary>
        [DataMember]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// The type of the order stop, limit or trade.
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }
        /// <summary>
        /// The duration that the order was applicable, i.e. good till cancelled (GTC), good for day (GFD), or good till time (GTT).
        /// </summary>
        [DataMember]
        public int OrderApplicabilityId { get; set; }
        /// <summary>
        /// The trade currency.
        /// </summary>
        [DataMember]
        public string Currency { get; set; }
        /// <summary>
        /// The order status.
        /// </summary>
        [DataMember]
        public int StatusId { get; set; }
        /// <summary>
        /// The last time that the order changed.
        /// </summary>
        [DataMember]
        public DateTime LastChangedDateTimeUtc { get; set; }
        /// <summary>
        /// The creation date and time of the order.
        /// </summary>
        [DataMember]
        public DateTime CreatedDateTimeUtc { get; set; }
    }

    /// <summary>
    /// Represents an order.
    /// </summary>
    [DataContract]
    public partial class ApiOrderDTO
    {
        /// <summary>
        /// The order identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// A market's unique identifier.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// Direction identifier for trade, values supported are buy or sell.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// Size of the order.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price at which the order was filled.
        /// </summary>
        [DataMember]
        public decimal? Price { get; set; }
        /// <summary>
        /// The ID of the Trading Account associated with the order.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// Currency ID for order (as represented in the trading system).
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }
        /// <summary>
        /// Status ID of order (as represented in the trading system).
        /// </summary>
        [DataMember]
        public int StatusId { get; set; }
        /// <summary>
        /// The type of the order, Trade, stop or limit.
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }
        /// <summary>
        /// List of If/Done Orders which will be filled when the initial order is triggered.
        /// </summary>
        [DataMember]
        public ApiIfDoneDTO[] IfDone { get; set; }
        /// <summary>
        /// Corresponding Oco Order (One Cancels the Other).
        /// </summary>
        [DataMember]
        public ApiStopLimitOrderDTO OcoOrder { get; set; }
    }

    /// <summary>
    /// Represents a stop/limit order.
    /// </summary>
    [DataContract]
    public partial class ApiStopLimitOrderDTO : ApiOrderDTO
    {
        /// <summary>
        /// Price at which the order should be triggered.
        /// </summary>
        [DataMember]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillDate If/Done orders.
        /// </summary>
        [DataMember]
        public DateTime? ExpiryDateTimeUTC { get; set; }
        /// <summary>
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD).
        /// </summary>
        [DataMember]
        public string Applicability { get; set; }
    }

    [DataContract]
    public partial class ApiClientApplicationMessageTranslationDTO
    {
        /// <summary>
        /// Translation key
        /// </summary>
        [DataMember]
        public string Key { get; set; }
        /// <summary>
        /// Translation value
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// A stop or limit order that is currently active.
    /// </summary>
    [DataContract]
    public partial class ApiActiveStopLimitOrderDTO
    {
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The order's parent OrderId.
        /// </summary>
        [DataMember]
        public int? ParentOrderId { get; set; }
        /// <summary>
        /// The markets unique identifier.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The market's name.
        /// </summary>
        [DataMember]
        public string MarketName { get; set; }
        /// <summary>
        /// The direction, buy or sell.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// The quantity of the product.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The marked to market price at which the order will trigger at.
        /// </summary>
        [DataMember]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
        [DataMember]
        public decimal TradingAccountId { get; set; }
        /// <summary>
        /// The type of order, i.e. stop or limit.
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// When the order applies until. i.e. good till cancelled (GTC) good for day (GFD) or good till time (GTT).
        /// </summary>
        [DataMember]
        public int Applicability { get; set; }
        /// <summary>
        /// The associated expiry DateTime.
        /// </summary>
        [DataMember]
        public DateTime? ExpiryDateTimeUTC { get; set; }
        /// <summary>
        /// The trade currency.
        /// </summary>
        [DataMember]
        public string Currency { get; set; }
        /// <summary>
        /// The order status.
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// The if done stop order.
        /// </summary>
        [DataMember]
        public ApiBasicStopLimitOrderDTO StopOrder { get; set; }
        /// <summary>
        /// The if done limit order
        /// </summary>
        [DataMember]
        public ApiBasicStopLimitOrderDTO LimitOrder { get; set; }
        /// <summary>
        /// The order on the other side of a one Cancels the other relationship.
        /// </summary>
        [DataMember]
        public ApiBasicStopLimitOrderDTO OcoOrder { get; set; }
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// </summary>
        [DataMember]
        public DateTime LastChangedDateTimeUTC { get; set; }
        /// <summary>
        /// Plain text StatusReason
        /// </summary>
        [DataMember]
        public string Applicability_Resolved { get; set; }
    }

    /// <summary>
    /// Market end of day (EOD) information.
    /// </summary>
    [DataContract]
    public partial class ApiMarketEodDTO
    {
        /// <summary>
        /// Unit.
        /// </summary>
        [DataMember]
        public string MarketEodUnit { get; set; }
        /// <summary>
        /// End of day amount.
        /// </summary>
        [DataMember]
        public int? MarketEodAmount { get; set; }
    }

    /// <summary>
    /// Generic look up data entities - such as localised textual names.
    /// </summary>
    [DataContract]
    public partial class ApiLookupDTO
    {
        /// <summary>
        /// The lookup ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Lookup items description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// The order to display the items on a user interface.
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Translation text ID.
        /// </summary>
        [DataMember]
        public int? TranslationTextId { get; set; }
        /// <summary>
        /// Translated text.
        /// </summary>
        [DataMember]
        public string TranslationText { get; set; }
        /// <summary>
        /// Is active flag.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// Is allowed flag.
        /// </summary>
        [DataMember]
        public bool IsAllowed { get; set; }
    }

    /// <summary>
    /// Lookup data specific to a Culture
    /// </summary>
    [DataContract]
    public partial class ApiCultureLookupDTO : ApiLookupDTO
    {
        /// <summary>
        /// 2 letter ISO 639 culture code followed by a 2 letter uppercase ISO 3166 culture code
        /// </summary>
        [DataMember]
        public string Code { get; set; }
    }

    /// <summary>
    /// A stop or limit order with a limited number of data fields.
    /// </summary>
    [DataContract]
    public partial class ApiBasicStopLimitOrderDTO
    {
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The order's trigger price.
        /// </summary>
        [DataMember]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// The quantity of the product.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// API watchlist item.
    /// </summary>
    [DataContract]
    public partial class ApiClientAccountWatchlistItemDTO
    {
        /// <summary>
        /// ID of the parent watchlist.
        /// </summary>
        [DataMember]
        public int WatchlistId { get; set; }
        /// <summary>
        /// Watchlist item market ID.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// Watchlist item display order.
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// The current margin and other account balance data for a specific client account used in the ClientAccountMargin stream.
    /// </summary>
    [DataContract]
    public partial class ClientAccountMarginDTO
    {
        /// <summary>
        /// Cash balance expressed in the clients base currency.
        /// </summary>
        [DataMember]
        public decimal Cash { get; set; }
        /// <summary>
        /// The client account's total margin requirement expressed in base currency.
        /// </summary>
        [DataMember]
        public decimal Margin { get; set; }
        /// <summary>
        /// Margin indicator expressed as a percentage.
        /// </summary>
        [DataMember]
        public decimal MarginIndicator { get; set; }
        /// <summary>
        /// Net equity expressed in the clients base currency.
        /// </summary>
        [DataMember]
        public decimal NetEquity { get; set; }
        /// <summary>
        /// Open trade equity (open / unrealised PNL) expressed in the client's base currency.
        /// </summary>
        [DataMember]
        public decimal OpenTradeEquity { get; set; }
        /// <summary>
        /// Tradable funds expressed in the client's base currency.
        /// </summary>
        [DataMember]
        public decimal TradeableFunds { get; set; }
        /// <summary>
        /// N/A
        /// </summary>
        [DataMember]
        public decimal PendingFunds { get; set; }
        /// <summary>
        /// Trading resource expressed in the client's base currency.
        /// </summary>
        [DataMember]
        public decimal TradingResource { get; set; }
        /// <summary>
        /// Total margin requirement expressed in the client's base currency.
        /// </summary>
        [DataMember]
        public decimal TotalMarginRequirement { get; set; }
        /// <summary>
        /// The clients base currency ID.
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }
        /// <summary>
        /// The clients base currency ISO code.
        /// </summary>
        [DataMember]
        public string CurrencyISO { get; set; }
    }

    /// <summary>
    /// A headline for a news story.
    /// </summary>
    [DataContract]
    public partial class NewsDTO
    {
        /// <summary>
        /// The unique identifier for a news story.
        /// </summary>
        [DataMember]
        public int StoryId { get; set; }
        /// <summary>
        /// The news story headline.
        /// </summary>
        [DataMember]
        public string Headline { get; set; }
        /// <summary>
        /// The date on which the news story was published. Always in UTC.
        /// </summary>
        [DataMember]
        public DateTime PublishDate { get; set; }
    }

    /// <summary>
    /// Contains details of a specific news story.
    /// </summary>
    [DataContract]
    public partial class NewsDetailDTO : NewsDTO
    {
        /// <summary>
        /// The detail of the news story. This can contain HTML characters.
        /// </summary>
        [DataMember]
        public string Story { get; set; }
    }

    /// <summary>
    /// Basic information about a market tag.
    /// </summary>
    [DataContract]
    public partial class ApiMarketTagDTO
    {
        /// <summary>
        /// A unique identifier for this market tag.
        /// </summary>
        [DataMember]
        public int MarketTagId { get; set; }
        /// <summary>
        /// The market tag description. Can be localised if required.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Used to determine if the market tag is a primary (1) or secondary (2) tag.
        /// </summary>
        [DataMember]
        public int Type { get; set; }
    }

    /// <summary>
    /// Information about a Trading Account.
    /// </summary>
    [DataContract]
    public partial class ApiTradingAccountDTO
    {
        /// <summary>
        /// Trading Account ID.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// Trading Account Code.
        /// </summary>
        [DataMember]
        public string TradingAccountCode { get; set; }
        /// <summary>
        /// Trading account status with possible values (Open, Closed).
        /// </summary>
        [DataMember]
        public string TradingAccountStatus { get; set; }
        /// <summary>
        /// Trading account type with possible values (Spread, CFD).
        /// </summary>
        [DataMember]
        public string TradingAccountType { get; set; }
    }

    /// <summary>
    /// The current margin requirement and open trade equity (OTE) of an order, used in the TradeMargin stream.
    /// </summary>
    [DataContract]
    public partial class TradeMarginDTO
    {
        /// <summary>
        /// The client account this message relates to.
        /// </summary>
        [DataMember]
        public int ClientAccountId { get; set; }
        /// <summary>
        /// The order direction, 1 == Buy and 0 == Sell.
        /// </summary>
        [DataMember]
        public int DirectionId { get; set; }
        /// <summary>
        /// The margin requirement converted to the correct currency for this order.
        /// </summary>
        [DataMember]
        public decimal MarginRequirementConverted { get; set; }
        /// <summary>
        /// The currency ID of the margin requirement for this order. See the "Currency ID" section of the CIAPI User Guide for a listing of the currency IDs.
        /// </summary>
        [DataMember]
        public int MarginRequirementConvertedCurrencyId { get; set; }
        /// <summary>
        /// The currency ISO code of the margin requirement for this order.
        /// </summary>
        [DataMember]
        public string MarginRequirementConvertedCurrencyISOCode { get; set; }
        /// <summary>
        /// The market ID the order is on.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The market type ID. 1 = Option Market; 2 = Ordinary Market; 4 = Binary Market.
        /// </summary>
        [DataMember]
        public int MarketTypeId { get; set; }
        /// <summary>
        /// The margin multiplier.
        /// </summary>
        [DataMember]
        public decimal Multiplier { get; set; }
        /// <summary>
        /// The Order ID.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The Open Trade Equity converted to the correct currency for this order.
        /// </summary>
        [DataMember]
        public decimal OTEConverted { get; set; }
        /// <summary>
        /// The currency ID of the OTE for this order.
        /// </summary>
        [DataMember]
        public int OTEConvertedCurrencyId { get; set; }
        /// <summary>
        /// The currency ISO code of the OTE for this order.
        /// </summary>
        [DataMember]
        public string OTEConvertedCurrencyISOCode { get; set; }
        /// <summary>
        /// The price the calculation was performed at.
        /// </summary>
        [DataMember]
        public decimal PriceCalculatedAt { get; set; }
        /// <summary>
        /// The price the order was taken at.
        /// </summary>
        [DataMember]
        public decimal PriceTakenAt { get; set; }
        /// <summary>
        /// The quantity of the order.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// A quote for a specific order request.
    /// </summary>
    [DataContract]
    public partial class QuoteDTO
    {
        /// <summary>
        /// The unique ID of the Quote.
        /// </summary>
        [DataMember]
        public int QuoteId { get; set; }
        /// <summary>
        /// The ID of the Order that the Quote is related to.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The Market the Quote is related to.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The Price of the original Order request for a Buy.
        /// </summary>
        [DataMember]
        public decimal BidPrice { get; set; }
        /// <summary>
        /// The amount the bid price will be adjusted to become an order when the customer is buying (BidPrice + BidAdjust = BuyPrice).
        /// </summary>
        [DataMember]
        public decimal BidAdjust { get; set; }
        /// <summary>
        /// The Price of the original Order request for a Sell.
        /// </summary>
        [DataMember]
        public decimal OfferPrice { get; set; }
        /// <summary>
        /// The amount the offer price will be adjusted to become an order when the customer is selling (OfferPrice + OfferAdjust = OfferPrice).
        /// </summary>
        [DataMember]
        public decimal OfferAdjust { get; set; }
        /// <summary>
        /// The Quantity is the number of units for the trade i.e CFD Quantity = Number of CFD's to Buy or Sell , FX Quantity = amount in base currency.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The system internal ID for the ISO Currency. An API call will be available in the near future to look up the equivalent ISO Code.
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }
        /// <summary>
        /// The Status ID of the Quote. An API call will be available in the near future to look up the Status values.
        /// </summary>
        [DataMember]
        public int StatusId { get; set; }
        /// <summary>
        /// The quote type ID.
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }
        /// <summary>
        /// The timestamp the quote was requested. Always expressed in UTC.
        /// </summary>
        [DataMember]
        public DateTime RequestDateTime { get; set; }
    }

    /// <summary>
    /// Represents a trade order.
    /// </summary>
    [DataContract]
    public partial class ApiTradeOrderDTO : ApiOrderDTO
    {
    }

    /// <summary>
    /// An order for a specific Trading Account.
    /// </summary>
    [DataContract]
    public partial class OrderDTO
    {
        /// <summary>
        /// The Order identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The Market identifier.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// Client account ID.
        /// </summary>
        [DataMember]
        public int ClientAccountId { get; set; }
        /// <summary>
        /// Trading account ID.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// Trade currency ID.
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }
        /// <summary>
        /// Trade currency ISO code.
        /// </summary>
        [DataMember]
        public string CurrencyISO { get; set; }
        /// <summary>
        /// Direction of the order (1 == buy, 0 == sell).
        /// </summary>
        [DataMember]
        public int Direction { get; set; }
        /// <summary>
        /// Flag indicating whether the order automatically rolls over.
        /// </summary>
        [DataMember]
        public bool AutoRollover { get; set; }
        /// <summary>
        /// The price the order was executed at.
        /// </summary>
        [DataMember]
        public decimal ExecutionPrice { get; set; }
        /// <summary>
        /// The date and time that the order was last changed. Always expressed in UTC.
        /// </summary>
        [DataMember]
        public DateTime LastChangedTime { get; set; }
        /// <summary>
        /// The open price of the order.
        /// </summary>
        [DataMember]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// The date of the order. Always expressed in UTC.
        /// </summary>
        [DataMember]
        public DateTime OriginalLastChangedDateTime { get; set; }
        /// <summary>
        /// The orders original quantity, before any part / full closures.
        /// </summary>
        [DataMember]
        public decimal OriginalQuantity { get; set; }
        /// <summary>
        /// The position method identifier of the order.
        /// </summary>
        [DataMember]
        public int PositionMethodId { get; set; }
        /// <summary>
        /// The current quantity of the order.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The type of the order (1 = Trade / 2 = Stop / 3 = Limit).
        /// </summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// The order status ID.
        /// </summary>
        [DataMember]
        public string Status { get; set; }
        /// <summary>
        /// The order status reason identifier.
        /// </summary>
        [DataMember]
        public int ReasonId { get; set; }
    }

    /// <summary>
    /// The details of a specific price bar, useful for plotting candlestick charts.
    /// </summary>
    [DataContract]
    public partial class PriceBarDTO
    {
        /// <summary>
        /// The starting date for the price bar interval.
        /// </summary>
        [DataMember]
        public DateTime BarDate { get; set; }
        /// <summary>
        /// The price at the start (open) of the price bar interval.
        /// </summary>
        [DataMember]
        public decimal Open { get; set; }
        /// <summary>
        /// The highest price occurring during the interval of the price bar.
        /// </summary>
        [DataMember]
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price occurring during the interval of the price bar.
        /// </summary>
        [DataMember]
        public decimal Low { get; set; }
        /// <summary>
        /// The price at the end (close) of the price bar interval.
        /// </summary>
        [DataMember]
        public decimal Close { get; set; }
    }

    /// <summary>
    /// Market tag information extended to include a list o child tags.
    /// </summary>
    [DataContract]
    public partial class ApiPrimaryMarketTagDTO : ApiMarketTagDTO
    {
        /// <summary>
        /// The list of child tags associated with this market tag.
        /// </summary>
        [DataMember]
        public ApiMarketTagDTO[] Children { get; set; }
    }

    /// <summary>
    /// Contains market information to be modified and saved.
    /// </summary>
    [DataContract]
    public partial class ApiMarketInformationSaveDTO
    {
        /// <summary>
        /// The ID of the market to be modified.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// Setting to indicate the user's price tolerance for the given market.
        /// </summary>
        [DataMember]
        public decimal? PriceTolerance { get; set; }
        /// <summary>
        /// Flag to indicate if the price tolerance value has changed.
        /// </summary>
        [DataMember]
        public bool PriceToleranceIsDirty { get; set; }
        /// <summary>
        /// The user's margin factor for the given market.
        /// </summary>
        [DataMember]
        public decimal? MarginFactor { get; set; }
        /// <summary>
        /// Flag to indicate if the margin factor value has changed.
        /// </summary>
        [DataMember]
        public bool MarginFactorIsDirty { get; set; }
    }

    /// <summary>
    /// Basic information about a Market.
    /// </summary>
    [DataContract]
    public partial class ApiMarketDTO
    {
        /// <summary>
        /// A market's unique identifier.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The market name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }

    /// <summary>
    /// A Trade from a historical perspective.
    /// </summary>
    [DataContract]
    public partial class ApiTradeHistoryDTO
    {
        /// <summary>
        /// The order ID.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The orders that are being closed / part closed by this order.
        /// </summary>
        [DataMember]
        public int[] OpeningOrderIds { get; set; }
        /// <summary>
        /// The market ID.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The name of the market.
        /// </summary>
        [DataMember]
        public string MarketName { get; set; }
        /// <summary>
        /// The direction of the trade.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// The original quantity of the trade, before part closures.
        /// </summary>
        [DataMember]
        public decimal OriginalQuantity { get; set; }
        /// <summary>
        /// The current quantity of the trade.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The open price of the trade.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
        /// <summary>
        /// The Trading Account ID that the order is on.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// The trade currency.
        /// </summary>
        [DataMember]
        public string Currency { get; set; }
        /// <summary>
        /// The realised profit and loss.
        /// </summary>
        [DataMember]
        public decimal? RealisedPnl { get; set; }
        /// <summary>
        /// The realised Pnl currency.
        /// </summary>
        [DataMember]
        public string RealisedPnlCurrency { get; set; }
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// </summary>
        [DataMember]
        public DateTime LastChangedDateTimeUtc { get; set; }
        /// <summary>
        /// The time the order was executed.
        /// </summary>
        [DataMember]
        public DateTime ExecutedDateTimeUtc { get; set; }
    }

    /// <summary>
    /// Client account watchlist.
    /// </summary>
    [DataContract]
    public partial class ApiClientAccountWatchlistDTO
    {
        /// <summary>
        /// The ID of the Watchlist.
        /// </summary>
        [DataMember]
        public int WatchlistId { get; set; }
        /// <summary>
        /// Watchlist description.
        /// </summary>
        [DataMember]
        public string WatchlistDescription { get; set; }
        /// <summary>
        /// Watchlist display order.
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Watchlist items.
        /// </summary>
        [DataMember]
        public ApiClientAccountWatchlistItemDTO[] Items { get; set; }
    }

    /// <summary>
    /// Contains market information.
    /// </summary>
    [DataContract]
    public partial class ApiMarketInformationDTO
    {
        /// <summary>
        /// Market ID.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The market name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Margin factor, expressed as points or as a percentage.
        /// </summary>
        [DataMember]
        public decimal? MarginFactor { get; set; }
        /// <summary>
        /// The minimum margin factor.
        /// </summary>
        [DataMember]
        public decimal? MinMarginFactor { get; set; }
        /// <summary>
        /// The maximum margin factor.
        /// </summary>
        [DataMember]
        public decimal? MaxMarginFactor { get; set; }
        /// <summary>
        /// The margin factor units.
        /// </summary>
        [DataMember]
        public int MarginFactorUnits { get; set; }
        /// <summary>
        /// The minimum distance from the current price you can place an order.
        /// </summary>
        [DataMember]
        public decimal? MinDistance { get; set; }
        /// <summary>
        /// The minimum quantity that can be traded over the web.
        /// </summary>
        [DataMember]
        public decimal? WebMinSize { get; set; }
        /// <summary>
        /// The max size of an order.
        /// </summary>
        [DataMember]
        public decimal? MaxSize { get; set; }
        /// <summary>
        /// Flag indicating whether the market is a 24 hour market.
        /// </summary>
        [DataMember]
        public bool Market24H { get; set; }
        /// <summary>
        /// The number of decimal places in the market's price.
        /// </summary>
        [DataMember]
        public int? PriceDecimalPlaces { get; set; }
        /// <summary>
        /// Default quote length.
        /// </summary>
        [DataMember]
        public int? DefaultQuoteLength { get; set; }
        /// <summary>
        /// Flag indicating whether you can trade this market on the web.
        /// </summary>
        [DataMember]
        public bool TradeOnWeb { get; set; }
        /// <summary>
        /// New sell orders will be rejected. Orders resulting in a short open position will be red carded.
        /// </summary>
        [DataMember]
        public bool LimitUp { get; set; }
        /// <summary>
        /// New buy orders will be rejected. Orders resulting in a long open position will be red carded.
        /// </summary>
        [DataMember]
        public bool LimitDown { get; set; }
        /// <summary>
        /// Cannot open a short position. Equivalent to limit up.
        /// </summary>
        [DataMember]
        public bool LongPositionOnly { get; set; }
        /// <summary>
        /// Can only close open positions. Equivalent to both Limit up and Limit down.
        /// </summary>
        [DataMember]
        public bool CloseOnly { get; set; }
        /// <summary>
        /// List of market end of day DTOs.
        /// </summary>
        [DataMember]
        public ApiMarketEodDTO[] MarketEod { get; set; }
        /// <summary>
        /// Setting to indicate the user's price tolerance for the given market.
        /// </summary>
        [DataMember]
        public decimal? PriceTolerance { get; set; }
        /// <summary>
        /// Multiplier used to calculate the significance of the price tolerance to the appropriate decimal place.
        /// </summary>
        [DataMember]
        public int ConvertPriceToPipsMultiplier { get; set; }
        /// <summary>
        /// The Id of the type of the market setting, ie Spread, CFD.
        /// </summary>
        [DataMember]
        public int MarketSettingsTypeId { get; set; }
        /// <summary>
        /// The type of the market setting, ie Spread, CFD.
        /// </summary>
        [DataMember]
        public string MarketSettingsType { get; set; }
        /// <summary>
        /// A short summary of the market name used when presenting the market name on mobile clients.
        /// </summary>
        [DataMember]
        public string MobileShortName { get; set; }
    }

    /// <summary>
    /// A Price for a specific Market.
    /// </summary>
    [DataContract]
    public partial class PriceDTO
    {
        /// <summary>
        /// The Market that the Price is related to.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The date of the Price. Always expressed in UTC.
        /// </summary>
        [DataMember]
        public DateTime TickDate { get; set; }
        /// <summary>
        /// The current Bid price (price at which the customer can sell).
        /// </summary>
        [DataMember]
        public decimal Bid { get; set; }
        /// <summary>
        /// The current Offer price (price at which the customer can buy, sometimes referred to as Ask price).
        /// </summary>
        [DataMember]
        public decimal Offer { get; set; }
        /// <summary>
        /// The current mid price.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
        /// <summary>
        /// The highest price reached for the day.
        /// </summary>
        [DataMember]
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price reached for the day.
        /// </summary>
        [DataMember]
        public decimal Low { get; set; }
        /// <summary>
        /// The change since the last price (always positive). See Direction for direction of the change.
        /// </summary>
        [DataMember]
        public decimal Change { get; set; }
        /// <summary>
        /// The direction of movement since the last price. 1 == up, 0 == down.
        /// </summary>
        [DataMember]
        public int Direction { get; set; }
        /// <summary>
        /// A unique ID for this price. Treat as a unique, but random string.
        /// </summary>
        [DataMember]
        public string AuditId { get; set; }
    }

    /// <summary>
    /// The mid price at the point in time of the price tick.
    /// </summary>
    [DataContract]
    public partial class PriceTickDTO
    {
        /// <summary>
        /// The date time at which a price tick occurred. Accurate to the millisecond.
        /// </summary>
        [DataMember]
        public DateTime TickDate { get; set; }
        /// <summary>
        /// The mid price.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Contains the If/Done stop and limit orders. An If/Done order is comprised of two separate orders linked togehter and requested as a single order. When the first order is executed, the second order becomes an active order. For example, attaching a stop/limit to a trade or order.
    /// </summary>
    [DataContract]
    public partial class ApiIfDoneDTO
    {
        /// <summary>
        /// The price at which the stop order will be filled.
        /// </summary>
        [DataMember]
        public ApiStopLimitOrderDTO Stop { get; set; }
        /// <summary>
        /// The price at which the limit order will be filled.
        /// </summary>
        [DataMember]
        public ApiStopLimitOrderDTO Limit { get; set; }
    }

    /// <summary>
    /// A Trade, or order that is currently open.
    /// </summary>
    [DataContract]
    public partial class ApiOpenPositionDTO
    {
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The markets unique identifier.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// The market's name.
        /// </summary>
        [DataMember]
        public string MarketName { get; set; }
        /// <summary>
        /// The direction, buy or sell.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// The quantity of the order.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price / rate that the trade was opened at.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
        /// <summary>
        /// The trading account that the order is on.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// The trade currency.
        /// </summary>
        [DataMember]
        public string Currency { get; set; }
        /// <summary>
        /// The order status.
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// The stop order attached to this order.
        /// </summary>
        [DataMember]
        public ApiBasicStopLimitOrderDTO StopOrder { get; set; }
        /// <summary>
        /// The limit order attached to this order.
        /// </summary>
        [DataMember]
        public ApiBasicStopLimitOrderDTO LimitOrder { get; set; }
        /// <summary>
        /// The last time that the order changed. Note - Does not include things such as the current market price.
        /// </summary>
        [DataMember]
        public DateTime LastChangedDateTimeUTC { get; set; }
        /// <summary>
        /// Plain text StatusReason
        /// </summary>
        [DataMember]
        public string Status_Resolved { get; set; }
    }

    /// <summary>
    /// The response from the News Detail GET request.
    /// </summary>
    [DataContract]
    public partial class GetNewsDetailResponseDTO
    {
        /// <summary>
        /// The details of the news item.
        /// </summary>
        [DataMember]
        public NewsDetailDTO NewsDetail { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListStopLimitOrderHistory query.
    /// </summary>
    [DataContract]
    public partial class ListStopLimitOrderHistoryResponseDTO
    {
        /// <summary>
        /// A list of historical stop / limit orders.
        /// </summary>
        [DataMember]
        public ApiStopLimitOrderHistoryDTO[] StopLimitOrderHistory { get; set; }
    }

    /// <summary>
    /// Response from a market information request.
    /// </summary>
    [DataContract]
    public partial class ListMarketInformationResponseDTO
    {
        /// <summary>
        /// The list of market information for each requested market.
        /// </summary>
        [DataMember]
        public ApiMarketInformationDTO[] MarketInformation { get; set; }
    }

    /// <summary>
    /// Gives a list of client application specific message translations
    /// </summary>
    [DataContract]
    public partial class ApiClientApplicationMessageTranslationResponseDTO
    {
        /// <summary>
        /// List of message translations (key/value pairs)
        /// </summary>
        [DataMember]
        public ApiClientApplicationMessageTranslationDTO[] TranslationKeyValuePairs { get; set; }
    }

    /// <summary>
    /// Contains the response of a ListActiveStopLimitOrder query.
    /// </summary>
    [DataContract]
    public partial class ListActiveStopLimitOrderResponseDTO
    {
        /// <summary>
        /// The requested list of active stop / limit orders.
        /// </summary>
        [DataMember]
        public ApiActiveStopLimitOrderDTO[] ActiveStopLimitOrders { get; set; }
    }

    /// <summary>
    /// An (empty) response to indicate that the save market information operation has completed.
    /// </summary>
    [DataContract]
    public partial class ApiSaveMarketInformationResponseDTO
    {
    }

    /// <summary>
    /// Response from an account information query.
    /// </summary>
    [DataContract]
    public partial class AccountInformationResponseDTO
    {
        /// <summary>
        /// Logon user name.
        /// </summary>
        [DataMember]
        public string LogonUserName { get; set; }
        /// <summary>
        /// Client account ID.
        /// </summary>
        [DataMember]
        public int ClientAccountId { get; set; }
        /// <summary>
        /// Base currency of the client account.
        /// </summary>
        [DataMember]
        public string ClientAccountCurrency { get; set; }
        /// <summary>
        /// Account Operator ID.
        /// </summary>
        [DataMember]
        public int AccountOperatorId { get; set; }
        /// <summary>
        /// A list of trading accounts.
        /// </summary>
        [DataMember]
        public ApiTradingAccountDTO[] TradingAccounts { get; set; }
        /// <summary>
        /// The user's personal email address.
        /// </summary>
        [DataMember]
        public string PersonalEmailAddress { get; set; }
        /// <summary>
        /// Does the user have more than one email address configured?
        /// </summary>
        [DataMember]
        public bool HasMultipleEmailAddresses { get; set; }
    }

    /// <summary>
    /// The response from a GET request for News headlines.
    /// </summary>
    [DataContract]
    public partial class ListNewsHeadlinesResponseDTO
    {
        /// <summary>
        /// A list of News headlines.
        /// </summary>
        [DataMember]
        public NewsDTO[] Headlines { get; set; }
    }

    /// <summary>
    /// Response containing the active stop limit order.
    /// </summary>
    [DataContract]
    public partial class GetActiveStopLimitOrderResponseDTO
    {
        /// <summary>
        /// The active stop limit order. If it is null then the active stop limit order does not exist.
        /// </summary>
        [DataMember]
        public ApiActiveStopLimitOrderDTO ActiveStopLimitOrder { get; set; }
    }

    /// <summary>
    /// A request for a stop/limit order.
    /// </summary>
    [DataContract]
    public partial class NewStopLimitOrderRequestDTO
    {
        /// <summary>
        /// The identifier of the order to update.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// The unique identifier for the market.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// Currency to place order in.
        /// </summary>
        [DataMember]
        public string Currency { get; set; }
        /// <summary>
        /// Flag to indicate whether the trade will automatically roll into the next market when the current market expires.
        /// </summary>
        [DataMember]
        public bool AutoRollover { get; set; }
        /// <summary>
        /// Direction identifier for order/trade, values supported are buy or sell.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// Size of the order/trade.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower of the two.
        /// </summary>
        [DataMember]
        public decimal BidPrice { get; set; }
        /// <summary>
        /// Market prices are quoted as a pair (buy/sell or bid/offer), the OfferPrice is the higher of the market price pair.
        /// </summary>
        [DataMember]
        public decimal OfferPrice { get; set; }
        /// <summary>
        /// Unique identifier for a price tick.
        /// </summary>
        [DataMember]
        public string AuditId { get; set; }
        /// <summary>
        /// The ID of the TradingAccount associated with the trade/order request.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// List of If/Done Orders that will be filled when the initial trade/order is triggered.
        /// </summary>
        [DataMember]
        public ApiIfDoneDTO[] IfDone { get; set; }
        /// <summary>
        /// Corresponding OCO Order (One Cancels the Other) if one has been defined.
        /// </summary>
        [DataMember]
        public NewStopLimitOrderRequestDTO OcoOrder { get; set; }
        /// <summary>
        /// Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD).
        /// </summary>
        [DataMember]
        public string Applicability { get; set; }
        /// <summary>
        /// The associated expiry DateTime for a pair of GoodTillDate IfDone orders.
        /// </summary>
        [DataMember]
        public DateTime? ExpiryDateTimeUTC { get; set; }
        /// <summary>
        /// Flag to determine whether an order is guaranteed to trigger and fill at the associated trigger price.
        /// </summary>
        [DataMember]
        public bool Guaranteed { get; set; }
        /// <summary>
        /// Price at which the order is intended to be triggered.
        /// </summary>
        [DataMember]
        public decimal TriggerPrice { get; set; }
    }

    /// <summary>
    /// A request to update a stop/limit order.
    /// </summary>
    [DataContract]
    public partial class UpdateStopLimitOrderRequestDTO : NewStopLimitOrderRequestDTO
    {
    }

    /// <summary>
    /// Response containing the system status.
    /// </summary>
    [DataContract]
    public partial class SystemStatusDTO
    {
        /// <summary>
        /// The status message.
        /// </summary>
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public partial class ApiSaveAccountInformationResponseDTO
    {
    }

    /// <summary>
    /// Response to an order request.
    /// </summary>
    [DataContract]
    public partial class ApiOrderResponseDTO
    {
        /// <summary>
        /// Order ID.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// Order status reason ID.
        /// </summary>
        [DataMember]
        public int StatusReason { get; set; }
        /// <summary>
        /// Order status ID.
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// Order fill price.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
        /// <summary>
        /// Commission charge.
        /// </summary>
        [DataMember]
        public decimal CommissionCharge { get; set; }
        /// <summary>
        /// List of If/Done orders.
        /// </summary>
        [DataMember]
        public ApiIfDoneResponseDTO[] IfDone { get; set; }
        /// <summary>
        /// Premium for guaranteed orders.
        /// </summary>
        [DataMember]
        public decimal GuaranteedPremium { get; set; }
        /// <summary>
        /// An order in an OCO relationship with this order.
        /// </summary>
        [DataMember]
        public ApiOrderResponseDTO OCO { get; set; }
        /// <summary>
        /// Plain text StatusReason
        /// </summary>
        [DataMember]
        public string StatusReason_Resolved { get; set; }
        /// <summary>
        /// Plain text StatusReason
        /// </summary>
        [DataMember]
        public string Status_Resolved { get; set; }
    }

    /// <summary>
    /// The response from the stop limit order request
    /// </summary>
    [DataContract]
    public partial class ApiStopLimitResponseDTO : ApiOrderResponseDTO
    {
    }

    /// <summary>
    /// Contains the response of a ListCfdMarkets query.
    /// </summary>
    [DataContract]
    public partial class ListCfdMarketsResponseDTO
    {
        /// <summary>
        /// A list of CFD markets.
        /// </summary>
        [DataMember]
        public ApiMarketDTO[] Markets { get; set; }
    }

    /// <summary>
    /// Update watchlist with item request.
    /// </summary>
    [DataContract]
    public partial class InsertWatchlistItemRequestDTO
    {
        /// <summary>
        /// The watchlist display order ID to add the item.
        /// </summary>
        [DataMember]
        public int ParentWatchlistDisplayOrderId { get; set; }
        /// <summary>
        /// The market item to add into the watchlist.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
    }

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
    [DataContract]
    public partial class SaveMarketInformationRequestDTO
    {
        /// <summary>
        /// The list of market information objects to be saved.
        /// </summary>
        [DataMember]
        public ApiMarketInformationSaveDTO[] MarketInformation { get; set; }
        /// <summary>
        /// The trading account on which the market information objects should be saved.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
    }

    /// <summary>
    /// The response from the trade request.
    /// </summary>
    [DataContract]
    public partial class ApiTradeOrderResponseDTO
    {
        /// <summary>
        /// The status of the order (Pending, Accepted, Open, etc.)
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// The ID corresponding to a more descriptive reason for the order status.
        /// </summary>
        [DataMember]
        public int StatusReason { get; set; }
        /// <summary>
        /// The unique identifier associated to the order returned from the underlying trading system.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// List of orders with their associated response.
        /// </summary>
        [DataMember]
        public ApiOrderResponseDTO[] Orders { get; set; }
        /// <summary>
        /// Quote response.
        /// </summary>
        [DataMember]
        public ApiQuoteResponseDTO Quote { get; set; }
        /// <summary>
        /// Plain text StatusReason
        /// </summary>
        [DataMember]
        public string StatusReason_Resolved { get; set; }
        /// <summary>
        /// Plain text StatusReason
        /// </summary>
        [DataMember]
        public string Status_Resolved { get; set; }
    }

    /// <summary>
    /// A request for a trade order.
    /// </summary>
    [DataContract]
    public partial class NewTradeOrderRequestDTO
    {
        /// <summary>
        /// The unique identifier for a market.
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
        /// <summary>
        /// Currency to place order in.
        /// </summary>
        [DataMember]
        public string Currency { get; set; }
        /// <summary>
        /// Flag to indicate whether the trade will automatically roll into the next market interval when the current market interval expires.
        /// </summary>
        [DataMember]
        public bool AutoRollover { get; set; }
        /// <summary>
        /// Direction identifier for order/trade, values supported are buy or sell.
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        /// <summary>
        /// Size of the order/trade.
        /// </summary>
        [DataMember]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The quote identifier.
        /// </summary>
        [DataMember]
        public int? QuoteId { get; set; }
        /// <summary>
        /// Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower value of the pair.
        /// </summary>
        [DataMember]
        public decimal BidPrice { get; set; }
        /// <summary>
        /// Market prices are quote as a pair (buy/sell or bid/offer), the OfferPrice is the higher value of the pair.
        /// </summary>
        [DataMember]
        public decimal OfferPrice { get; set; }
        /// <summary>
        /// Unique identifier for a price tick.
        /// </summary>
        [DataMember]
        public string AuditId { get; set; }
        /// <summary>
        /// The ID of the TradingAccount associated with the trade/order request.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
        /// <summary>
        /// List of If/Done Orders that will be filled when the initial trade/order is triggered.
        /// </summary>
        [DataMember]
        public ApiIfDoneDTO[] IfDone { get; set; }
        /// <summary>
        /// List of existing order ID's that require part or full closure.
        /// </summary>
        [DataMember]
        public int[] Close { get; set; }
    }

    /// <summary>
    /// A request to update a trade order.
    /// </summary>
    [DataContract]
    public partial class UpdateTradeOrderRequestDTO : NewTradeOrderRequestDTO
    {
        /// <summary>
        /// The identifier of the order to update.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
    }

    /// <summary>
    /// The response from a request for historical Price Ticks.
    /// </summary>
    [DataContract]
    public partial class GetPriceTickResponseDTO
    {
        /// <summary>
        /// An array of price ticks, sorted in ascending order by PriceTick.TickDate.
        /// </summary>
        [DataMember]
        public PriceTickDTO[] PriceTicks { get; set; }
    }

    /// <summary>
    /// Response from a market information request.
    /// </summary>
    [DataContract]
    public partial class GetMarketInformationResponseDTO
    {
        /// <summary>
        /// The requested market information.
        /// </summary>
        [DataMember]
        public ApiMarketInformationDTO MarketInformation { get; set; }
    }

    /// <summary>
    /// Request to delete a session (log off).
    /// </summary>
    [DataContract]
    public partial class ApiLogOffRequestDTO
    {
        /// <summary>
        /// User name of the session to delete (log off). This is case sensitive.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Session identifier (session token) to delete (log off).
        /// </summary>
        [DataMember]
        public string Session { get; set; }
    }

    /// <summary>
    /// This is a description of the ErrorCode enum.
    /// </summary>
    [DataContract]
    public enum ErrorCode
    {
        /// <summary>
        /// No error has occured.
        /// </summary>
        [DataMember]
        NoError = 0,
        /// <summary>
        /// The server understood the request, but is refusing to fulfill it.
        /// </summary>
        [DataMember]
        Forbidden = 403,
        /// <summary>
        /// An unexpected condition was encountered by the server preventing it from fulfilling the request.
        /// </summary>
        [DataMember]
        InternalServerError = 500,
        /// <summary>
        /// Server could not understand request due to an invalid parameter type.
        /// </summary>
        [DataMember]
        InvalidParameterType = 4000,
        /// <summary>
        /// Server could not understand request due to a missing parameter.
        /// </summary>
        [DataMember]
        ParameterMissing = 4001,
        /// <summary>
        /// Server could not understand request due to an invalid parameter value.
        /// </summary>
        [DataMember]
        InvalidParameterValue = 4002,
        /// <summary>
        /// Server could not understand request due to an invalid JSON request.
        /// </summary>
        [DataMember]
        InvalidJsonRequest = 4003,
        /// <summary>
        /// Server could not understand request due to an invalid JSON case format.
        /// </summary>
        [DataMember]
        InvalidJsonRequestCaseFormat = 4004,
        /// <summary>
        /// The credentials used to authenticate are invalid. Either the username, password or both are incorrect.
        /// </summary>
        [DataMember]
        InvalidCredentials = 4010,
        /// <summary>
        /// The session credentials supplied are invalid.
        /// </summary>
        [DataMember]
        InvalidSession = 4011,
        /// <summary>
        /// There is no data available.
        /// </summary>
        [DataMember]
        NoDataAvailable = 5001,
        /// <summary>
        /// Request has been throttled.
        /// </summary>
        [DataMember]
        Throttling = 5002,
    }

    /// <summary>
    /// Request to create a session (log on).
    /// </summary>
    [DataContract]
    public partial class ApiLogOnRequestDTO
    {
        /// <summary>
        /// Username is case sensitive.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Password is case sensitive.
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// A unique key use to identify the client application.
        /// </summary>
        [DataMember]
        public string AppKey { get; set; }
        /// <summary>
        /// The version of he client application.
        /// </summary>
        [DataMember]
        public string AppVersion { get; set; }
        /// <summary>
        /// Any comments the client applications what to associate with this session. Optional.
        /// </summary>
        [DataMember]
        public string AppComments { get; set; }
    }

    /// <summary>
    /// Delete watchlist item
    /// </summary>
    [DataContract]
    public partial class DeleteWatchlistItemRequestDTO
    {
        /// <summary>
        /// The watchlist display order id to delete the item from
        /// </summary>
        [DataMember]
        public int ParentWatchlistDisplayOrderId { get; set; }
        /// <summary>
        /// The market item to delete
        /// </summary>
        [DataMember]
        public int MarketId { get; set; }
    }

    /// <summary>
    /// Response from a session delete (Log Out) request.
    /// </summary>
    [DataContract]
    public partial class ApiLogOffResponseDTO
    {
        /// <summary>
        /// Flag indicating the Log Out status.
        /// </summary>
        [DataMember]
        public bool LoggedOut { get; set; }
    }

    /// <summary>
    /// Response from a market search with tags request.
    /// </summary>
    [DataContract]
    public partial class MarketInformationSearchWithTagsResponseDTO
    {
        /// <summary>
        /// The requested list of market information.
        /// </summary>
        [DataMember]
        public ApiMarketDTO[] Markets { get; set; }
        /// <summary>
        /// The requested list of market tags.
        /// </summary>
        [DataMember]
        public ApiMarketTagDTO[] Tags { get; set; }
    }

    /// <summary>
    /// System status request.
    /// </summary>
    [DataContract]
    public partial class SystemStatusRequestDTO
    {
        /// <summary>
        /// Depth to test.
        /// </summary>
        [DataMember]
        public string TestDepth { get; set; }
    }

    /// <summary>
    /// Response containing the order. Only one of the two fields will be populated depending upon the type of order (Trade or Stop / Limit).
    /// </summary>
    [DataContract]
    public partial class GetOrderResponseDTO
    {
        /// <summary>
        /// The details of the order if it is a trade / open position.
        /// </summary>
        [DataMember]
        public ApiTradeOrderDTO TradeOrder { get; set; }
        /// <summary>
        /// The details of the order if it is a stop limit order.
        /// </summary>
        [DataMember]
        public ApiStopLimitOrderDTO StopLimitOrder { get; set; }
    }

    /// <summary>
    /// The response containing version information and upgrade url of the client application.
    /// </summary>
    [DataContract]
    public partial class GetVersionInformationResponseDTO
    {
        /// <summary>
        /// The minimum version of the client application that can be used. If the installed version is less than this it shouldn't be used.
        /// </summary>
        [DataMember]
        public string MinimumRequiredVersion { get; set; }
        /// <summary>
        /// The latest version of the client application that can be used.
        /// </summary>
        [DataMember]
        public string LatestVersion { get; set; }
        /// <summary>
        /// The url of the upgrade.
        /// </summary>
        [DataMember]
        public string UpgradeUrl { get; set; }
    }

    /// <summary>
    /// Request to change account information.
    /// </summary>
    [DataContract]
    public partial class ApiSaveAccountInformationRequestDTO
    {
        /// <summary>
        /// The personal email address for the user.
        /// </summary>
        [DataMember]
        public string PersonalEmailAddress { get; set; }
        /// <summary>
        /// Setting to indicate if the personal email value has changed.
        /// </summary>
        [DataMember]
        public bool PersonalEmailAddressIsDirty { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListTradeHistory query.
    /// </summary>
    [DataContract]
    public partial class ListTradeHistoryResponseDTO
    {
        /// <summary>
        /// A list of historical trades.
        /// </summary>
        [DataMember]
        public ApiTradeHistoryDTO[] TradeHistory { get; set; }
    }

    /// <summary>
    /// An (empty) response to indicate the save watchlist operation has completed.
    /// </summary>
    [DataContract]
    public partial class ApiSaveWatchlistResponseDTO
    {
    }

    /// <summary>
    /// Response from a request to delete a watchlist.
    /// </summary>
    [DataContract]
    public partial class ApiDeleteWatchlistResponseDTO
    {
        /// <summary>
        /// Flag confirming whether the watchlist was deleted.
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
    }

    /// <summary>
    /// Request to update the display order of items in a watchlist.
    /// </summary>
    [DataContract]
    public partial class UpdateWatchlistDisplayOrderRequestDTO
    {
        /// <summary>
        /// Represents the new client watchlist displayOrderId list sequence.
        /// </summary>
        [DataMember]
        public int[] NewDisplayOrderIdSequence { get; set; }
    }

    /// <summary>
    /// Get market information request for a list of markets.
    /// </summary>
    [DataContract]
    public partial class ListMarketInformationRequestDTO
    {
        /// <summary>
        /// The list of market IDs to get information for.
        /// </summary>
        [DataMember]
        public int[] MarketIds { get; set; }
    }

    /// <summary>
    /// Request to change a user's password.
    /// </summary>
    [DataContract]
    public partial class ApiChangePasswordRequestDTO
    {
        /// <summary>
        /// The username of the user whose password is to be changed (case sensitive).
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// The user's existing password (case sensitive).
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// The user's new password (case sensitive).
        /// </summary>
        [DataMember]
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// Quote response.
    /// </summary>
    [DataContract]
    public partial class ApiQuoteResponseDTO
    {
        /// <summary>
        /// Quote ID.
        /// </summary>
        [DataMember]
        public int QuoteId { get; set; }
        /// <summary>
        /// Quote status.
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// Quote status reason.
        /// </summary>
        [DataMember]
        public int StatusReason { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListSpreadMarkets query.
    /// </summary>
    [DataContract]
    public partial class ListSpreadMarketsResponseDTO
    {
        /// <summary>
        /// A list of Spread Betting markets.
        /// </summary>
        [DataMember]
        public ApiMarketDTO[] Markets { get; set; }
    }

    /// <summary>
    /// Gets the lookup entities from trading database given the lookup name and culture ID.
    /// </summary>
    [DataContract]
    public partial class ApiLookupResponseDTO
    {
        /// <summary>
        /// The requested culture ID.
        /// </summary>
        [DataMember]
        public int CultureId { get; set; }
        /// <summary>
        /// The requested lookup name.
        /// </summary>
        [DataMember]
        public string LookupEntityName { get; set; }
        /// <summary>
        /// List of lookup entities from the database.
        /// </summary>
        [DataMember]
        public ApiLookupDTO[] ApiLookupDTOList { get; set; }
        /// <summary>
        /// TODO: document me!
        /// </summary>
        [DataMember]
        public ApiCultureLookupDTO[] ApiCultureLookupDTOList { get; set; }
    }

    /// <summary>
    /// Response to an If/Done order request.
    /// </summary>
    [DataContract]
    public partial class ApiIfDoneResponseDTO
    {
        /// <summary>
        /// The Stop order reponse.
        /// </summary>
        [DataMember]
        public ApiOrderResponseDTO Stop { get; set; }
        /// <summary>
        /// The Limit order response.
        /// </summary>
        [DataMember]
        public ApiOrderResponseDTO Limit { get; set; }
    }

    /// <summary>
    /// Request to delete a watchlist.
    /// </summary>
    [DataContract]
    public partial class ApiDeleteWatchlistRequestDTO
    {
        /// <summary>
        /// The ID of the watchlist to delete.
        /// </summary>
        [DataMember]
        public int WatchlistId { get; set; }
    }

    /// <summary>
    /// Message popup response denoting whether the client application should display a popup notification at startup.
    /// </summary>
    [DataContract]
    public partial class GetMessagePopupResponseDTO
    {
        /// <summary>
        /// Flag indicating if the client application asks for client approval.
        /// </summary>
        [DataMember]
        public bool AskForClientApproval { get; set; }
        /// <summary>
        /// The message to display to the client.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }

    /// <summary>
    /// Response to a CreateSessionRequest (Log On).
    /// </summary>
    [DataContract]
    public partial class ApiLogOnResponseDTO
    {
        /// <summary>
        /// Your session token (treat as a random string). <BR /> Session tokens are valid for a set period from the time of their creation. <BR /> The period is subject to change, and may vary depending on who you logon as.
        /// </summary>
        [DataMember]
        public string Session { get; set; }
        /// <summary>
        /// Flag used it indicate if a password change is needed.
        /// </summary>
        [DataMember]
        public bool PasswordChangeRequired { get; set; }
        /// <summary>
        /// Flag used it indicate if the account operator to which this use is associated is allowed to access the application.
        /// </summary>
        [DataMember]
        public bool AllowedAccountOperator { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListOpenPositions query.
    /// </summary>
    [DataContract]
    public partial class ListOpenPositionsResponseDTO
    {
        /// <summary>
        /// A list of trades / open positions.
        /// </summary>
        [DataMember]
        public ApiOpenPositionDTO[] OpenPositions { get; set; }
    }

    /// <summary>
    /// The response from a price bar history GET request. Contains both an array of finalized price bars, and a partial (not finalized) bar for the current period.
    /// </summary>
    [DataContract]
    public partial class GetPriceBarResponseDTO
    {
        /// <summary>
        /// An array of finalized price bars, sorted in ascending order based on PriceBar.BarDate
        /// </summary>
        [DataMember]
        public PriceBarDTO[] PriceBars { get; set; }
        /// <summary>
        /// The (non-finalized) price bar data for the current period (i.e, the period that hasn't yet completed).
        /// </summary>
        [DataMember]
        public PriceBarDTO PartialPriceBar { get; set; }
    }

    /// <summary>
    /// Response containing the open position information.
    /// </summary>
    [DataContract]
    public partial class GetOpenPositionResponseDTO
    {
        /// <summary>
        /// The open position information. If it is null then the open position does not exist.
        /// </summary>
        [DataMember]
        public ApiOpenPositionDTO OpenPosition { get; set; }
    }

    /// <summary>
    /// Response to a change password request.
    /// </summary>
    [DataContract]
    public partial class ApiChangePasswordResponseDTO
    {
        /// <summary>
        /// Was the password change request successful?
        /// </summary>
        [DataMember]
        public bool IsPasswordChanged { get; set; }
    }

    /// <summary>
    /// A cancel order request.
    /// </summary>
    [DataContract]
    public partial class CancelOrderRequestDTO
    {
        /// <summary>
        /// The order identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }
        /// <summary>
        /// ID of the trading account associated with the cancel order request.
        /// </summary>
        [DataMember]
        public int TradingAccountId { get; set; }
    }

    /// <summary>
    /// Response to a client watchlist GET request.
    /// </summary>
    [DataContract]
    public partial class ListWatchlistResponseDTO
    {
        /// <summary>
        /// Client account ID.
        /// </summary>
        [DataMember]
        public int ClientAccountId { get; set; }
        /// <summary>
        /// List of client account watchlists.
        /// </summary>
        [DataMember]
        public ApiClientAccountWatchlistDTO[] ClientAccountWatchlists { get; set; }
    }

    /// <summary>
    /// Response from a market information search request.
    /// </summary>
    [DataContract]
    public partial class ListMarketInformationSearchResponseDTO
    {
        /// <summary>
        /// The requested list of market information.
        /// </summary>
        [DataMember]
        public ApiMarketInformationDTO[] MarketInformation { get; set; }
    }

    /// <summary>
    /// Response from a market search with tags request.
    /// </summary>
    [DataContract]
    public partial class MarketInformationTagLookupResponseDTO
    {
        /// <summary>
        /// The requested list of market tags.
        /// </summary>
        [DataMember]
        public ApiPrimaryMarketTagDTO[] Tags { get; set; }
    }

    /// <summary>
    /// The response to an error condition.
    /// </summary>
    [DataContract]
    public partial class ApiErrorResponseDTO
    {
        /// <summary>
        /// The intended HTTP status code. This will be the same value as the actual HTTP status code unless the QueryString contains only200=true. This is useful for JavaScript clients who can only read responses with status code 200.
        /// </summary>
        [DataMember]
        public int HttpStatus { get; set; }
        /// <summary>
        /// This is a description of the ErrorMessage property.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// The error code.
        /// </summary>
        [DataMember]
        public int ErrorCode { get; set; }
    }

    /// <summary>
    /// Simulated order response
    /// </summary>
    [DataContract]
    public partial class ApiSimulateOrderResponseDTO
    {
        /// <summary>
        /// Simulated order status reason id.
        /// </summary>
        [DataMember]
        public int StatusReason { get; set; }
        /// <summary>
        /// Simulated order status id.
        /// </summary>
        [DataMember]
        public int Status { get; set; }
    }

    /// <summary>
    /// Request to save a watchlist.
    /// </summary>
    [DataContract]
    public partial class ApiSaveWatchlistRequestDTO
    {
        /// <summary>
        /// The watchlist to save. This will update an existing watchlist; or when the watchlistId is omitted or 0 is supplied, it will create a new watchlist.
        /// </summary>
        [DataMember]
        public ApiClientAccountWatchlistDTO Watchlist { get; set; }
    }

}
