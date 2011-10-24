using System;
namespace  CIAPI.DTO
{
    /// <summary>
    /// A stop or limit order that is currently active.
    /// </summary>
    [Serializable]
    public partial class ApiActiveStopLimitOrderDTO
    {
        public int OrderId { get; set; }
        public int? ParentOrderId { get; set; }
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public decimal TriggerPrice { get; set; }
        public decimal TradingAccountId { get; set; }
        public int Type { get; set; }
        public int Applicability { get; set; }
        public DateTime? ExpiryDateTimeUTC { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public ApiBasicStopLimitOrderDTO StopOrder { get; set; }
        public ApiBasicStopLimitOrderDTO LimitOrder { get; set; }
        public ApiBasicStopLimitOrderDTO OcoOrder { get; set; }
        public DateTime LastChangedDateTimeUTC { get; set; }
        public string Applicability_Resolved { get; set; }
    }

    /// <summary>
    /// Contains market information.
    /// </summary>
    [Serializable]
    public partial class ApiMarketInformationDTO
    {
        public int MarketId { get; set; }
        public string Name { get; set; }
        public decimal? MarginFactor { get; set; }
        public decimal? MinMarginFactor { get; set; }
        public decimal? MaxMarginFactor { get; set; }
        public int MarginFactorUnits { get; set; }
        public decimal? MinDistance { get; set; }
        public decimal? WebMinSize { get; set; }
        public decimal? MaxSize { get; set; }
        public bool Market24H { get; set; }
        public int? PriceDecimalPlaces { get; set; }
        public int? DefaultQuoteLength { get; set; }
        public bool TradeOnWeb { get; set; }
        public bool LimitUp { get; set; }
        public bool LimitDown { get; set; }
        public bool LongPositionOnly { get; set; }
        public bool CloseOnly { get; set; }
        public ApiMarketEodDTO[] MarketEod { get; set; }
        public decimal? PriceTolerance { get; set; }
        public int ConvertPriceToPipsMultiplier { get; set; }
    }

    /// <summary>
    /// A Price for a specific Market.
    /// </summary>
    [Serializable]
    public partial class PriceDTO
    {
        public int MarketId { get; set; }
        public DateTime TickDate { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public decimal Price { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Change { get; set; }
        public int Direction { get; set; }
        public string AuditId { get; set; }
    }

    /// <summary>
    /// A Trade from a historical perspective.
    /// </summary>
    [Serializable]
    public partial class ApiTradeHistoryDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public string Direction { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal Price { get; set; }
        public int TradingAccountId { get; set; }
        public string Currency { get; set; }
        public DateTime LastChangedDateTimeUtc { get; set; }
        public DateTime ExecutedDateTimeUtc { get; set; }
    }

    /// <summary>
    /// Generic look up data.
    /// </summary>
    [Serializable]
    public partial class ApiLookupDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int? TranslationTextId { get; set; }
        public string TranslationText { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowed { get; set; }
    }

    /// <summary>
    /// A news headline
    /// </summary>
    [Serializable]
    public partial class NewsDTO
    {
        public int StoryId { get; set; }
        public string Headline { get; set; }
        public DateTime PublishDate { get; set; }
    }

    /// <summary>
    /// Contains market information.
    /// </summary>
    [Serializable]
    public partial class ApiMarketInformationSaveDTO
    {
        public int MarketId { get; set; }
        public decimal? PriceTolerance { get; set; }
        public bool PriceToleranceIsDirty { get; set; }
        public decimal? MarginFactor { get; set; }
        public bool MarginFactorIsDirty { get; set; }
    }

    [Serializable]
    public partial class ApiClientApplicationMessageTranslationDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// A stop or limit order with a limited number of data fields.
    /// </summary>
    [Serializable]
    public partial class ApiBasicStopLimitOrderDTO
    {
        public int OrderId { get; set; }
        public decimal TriggerPrice { get; set; }
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// A Trade, or order that is currently open.
    /// </summary>
    [Serializable]
    public partial class ApiOpenPositionDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int TradingAccountId { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public ApiBasicStopLimitOrderDTO StopOrder { get; set; }
        public ApiBasicStopLimitOrderDTO LimitOrder { get; set; }
        public DateTime LastChangedDateTimeUTC { get; set; }
        public string Status_Resolved { get; set; }
    }

    /// <summary>
    /// The details of a specific price bar, useful for plotting candlestick charts
    /// </summary>
    [Serializable]
    public partial class PriceBarDTO
    {
        public DateTime BarDate { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }

    /// <summary>
    /// A stop or limit order from a historical perspective.
    /// </summary>
    [Serializable]
    public partial class ApiStopLimitOrderHistoryDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public string Direction { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal TriggerPrice { get; set; }
        public int TradingAccountId { get; set; }
        public int TypeId { get; set; }
        public int OrderApplicabilityId { get; set; }
        public string Currency { get; set; }
        public int StatusId { get; set; }
        public DateTime LastChangedDateTimeUtc { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
    }

    /// <summary>
    /// Represents an order
    /// </summary>
    [Serializable]
    public partial class ApiOrderDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Price { get; set; }
        public int TradingAccountId { get; set; }
        public int CurrencyId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public ApiIfDoneDTO[] IfDone { get; set; }
        public ApiStopLimitOrderDTO OcoOrder { get; set; }
    }

    /// <summary>
    /// Represents a trade order
    /// </summary>
    [Serializable]
    public partial class ApiTradeOrderDTO : ApiOrderDTO
    {
    }

    /// <summary>
    /// market end of day information.
    /// </summary>
    [Serializable]
    public partial class ApiMarketEodDTO
    {
        public string MarketEodUnit { get; set; }
        public int? MarketEodAmount { get; set; }
    }

    /// <summary>
    /// Api watchlist item
    /// </summary>
    [Serializable]
    public partial class ApiClientAccountWatchlistItemDTO
    {
        public int WatchlistId { get; set; }
        public int MarketId { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// A quote for a specific order request
    /// </summary>
    [Serializable]
    public partial class QuoteDTO
    {
        public int QuoteId { get; set; }
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidAdjust { get; set; }
        public decimal OfferPrice { get; set; }
        public decimal OfferAdjust { get; set; }
        public decimal Quantity { get; set; }
        public int CurrencyId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public DateTime RequestDateTime { get; set; }
    }

    /// <summary>
    /// Represents a stop/limit order
    /// </summary>
    [Serializable]
    public partial class ApiStopLimitOrderDTO : ApiOrderDTO
    {
        public decimal TriggerPrice { get; set; }
        public DateTime? ExpiryDateTimeUTC { get; set; }
        public string Applicability { get; set; }
    }

    /// <summary>
    /// Information about a TradingAccount
    /// </summary>
    [Serializable]
    public partial class ApiTradingAccountDTO
    {
        public int TradingAccountId { get; set; }
        public string TradingAccountCode { get; set; }
        public string TradingAccountStatus { get; set; }
        public string TradingAccountType { get; set; }
    }

    /// <summary>
    /// An order for a specific Trading Account
    /// </summary>
    [Serializable]
    public partial class OrderDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public int ClientAccountId { get; set; }
        public int TradingAccountId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyISO { get; set; }
        public int Direction { get; set; }
        public bool AutoRollover { get; set; }
        public decimal ExecutionPrice { get; set; }
        public DateTime LastChangedTime { get; set; }
        public decimal OpenPrice { get; set; }
        public DateTime OriginalLastChangedDateTime { get; set; }
        public decimal OriginalQuantity { get; set; }
        public int PositionMethodId { get; set; }
        public decimal Quantity { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int ReasonId { get; set; }
    }

    /// <summary>
    /// The current margin for a specific client account
    /// </summary>
    [Serializable]
    public partial class ClientAccountMarginDTO
    {
        public decimal Cash { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginIndicator { get; set; }
        public decimal NetEquity { get; set; }
        public decimal OpenTradeEquity { get; set; }
        public decimal TradeableFunds { get; set; }
        public decimal PendingFunds { get; set; }
        public decimal TradingResource { get; set; }
        public decimal TotalMarginRequirement { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyISO { get; set; }
    }

    /// <summary>
    /// basic information about a Market
    /// </summary>
    [Serializable]
    public partial class ApiMarketDTO
    {
        public int MarketId { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Client account watchlist
    /// </summary>
    [Serializable]
    public partial class ApiClientAccountWatchlistDTO
    {
        public int WatchlistId { get; set; }
        public string WatchlistDescription { get; set; }
        public int DisplayOrder { get; set; }
        public ApiClientAccountWatchlistItemDTO[] Items { get; set; }
    }

    /// <summary>
    /// Contains details of a specific news story
    /// </summary>
    [Serializable]
    public partial class NewsDetailDTO : NewsDTO
    {
        public string Story { get; set; }
    }

    /// <summary>
    /// The mid price at a particular point in time.
    /// </summary>
    [Serializable]
    public partial class PriceTickDTO
    {
        public DateTime TickDate { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// An IfDone order represents an order which is placed when the corresponding order is filled, e.g attaching a stop/limit to a trade or order
    /// </summary>
    [Serializable]
    public partial class ApiIfDoneDTO
    {
        public ApiStopLimitOrderDTO Stop { get; set; }
        public ApiStopLimitOrderDTO Limit { get; set; }
    }

    /// <summary>
    /// system status request.
    /// </summary>
    [Serializable]
    public partial class SystemStatusRequestDTO
    {
        public string TestDepth { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListSpreadMarkets query
    /// </summary>
    [Serializable]
    public partial class ListSpreadMarketsResponseDTO
    {
        public ApiMarketDTO[] Markets { get; set; }
    }

    /// <summary>
    /// request to create a session (log on).
    /// </summary>
    [Serializable]
    public partial class ApiLogOnRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListOpenPositions query
    /// </summary>
    [Serializable]
    public partial class ListOpenPositionsResponseDTO
    {
        public ApiOpenPositionDTO[] OpenPositions { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListActiveStopLimitOrder query
    /// </summary>
    [Serializable]
    public partial class ListActiveStopLimitOrderResponseDTO
    {
        public ApiActiveStopLimitOrderDTO[] ActiveStopLimitOrders { get; set; }
    }

    /// <summary>
    /// JSON returned from News Detail GET request
    /// </summary>
    [Serializable]
    public partial class GetNewsDetailResponseDTO
    {
        public NewsDetailDTO NewsDetail { get; set; }
    }

    /// <summary>
    /// Response from a session delete request.
    /// </summary>
    [Serializable]
    public partial class ApiLogOffResponseDTO
    {
        public bool LoggedOut { get; set; }
    }

    /// <summary>
    /// The response from a GET price bar history request. Contains both an array of finalized price bars, and a partial (not finalized) bar for the current period
    /// </summary>
    [Serializable]
    public partial class GetPriceBarResponseDTO
    {
        public PriceBarDTO[] PriceBars { get; set; }
        public PriceBarDTO PartialPriceBar { get; set; }
    }

    /// <summary>
    /// Response from am market information request.
    /// </summary>
    [Serializable]
    public partial class ListMarketInformationResponseDTO
    {
        public ApiMarketInformationDTO[] MarketInformation { get; set; }
    }

    [Serializable]
    public partial class ApiSaveMarketInformationResponseDTO
    {
    }

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
    [Serializable]
    public partial class UpdateWatchlistDisplayOrderRequestDTO
    {
        public int[] NewDisplayOrderIdSequence { get; set; }
    }

    /// <summary>
    /// Gets the client watchlist
    /// </summary>
    [Serializable]
    public partial class ListWatchlistResponseDTO
    {
        public int ClientAccountId { get; set; }
        public ApiClientAccountWatchlistDTO[] ClientAccountWatchlists { get; set; }
    }

    /// <summary>
    /// Gives a list of client application specific message translations
    /// </summary>
    [Serializable]
    public partial class ApiClientApplicationMessageTranslationResponseDTO
    {
        public ApiClientApplicationMessageTranslationDTO[] TranslationKeyValuePairs { get; set; }
    }

    /// <summary>
    /// response from an account information query.
    /// </summary>
    [Serializable]
    public partial class AccountInformationResponseDTO
    {
        public string LogonUserName { get; set; }
        public int ClientAccountId { get; set; }
        public string ClientAccountCurrency { get; set; }
        public ApiTradingAccountDTO[] TradingAccounts { get; set; }
    }

    /// <summary>
    /// A request for a stop/limit order
    /// </summary>
    [Serializable]
    public partial class NewStopLimitOrderRequestDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string Currency { get; set; }
        public bool AutoRollover { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public decimal BidPrice { get; set; }
        public decimal OfferPrice { get; set; }
        public string AuditId { get; set; }
        public int TradingAccountId { get; set; }
        public ApiIfDoneDTO[] IfDone { get; set; }
        public NewStopLimitOrderRequestDTO OcoOrder { get; set; }
        public string Applicability { get; set; }
        public DateTime? ExpiryDateTimeUTC { get; set; }
        public bool Guaranteed { get; set; }
        public decimal TriggerPrice { get; set; }
    }

    /// <summary>
    /// A request for updating a stop/limit order
    /// </summary>
    [Serializable]
    public partial class UpdateStopLimitOrderRequestDTO : NewStopLimitOrderRequestDTO
    {
    }

    /// <summary>
    /// Message popup response denoting whether the client application should display a popup notification at startup.
    /// </summary>
    [Serializable]
    public partial class GetMessagePopupResponseDTO
    {
        public bool AskForClientApproval { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Gets the lookup entities from trading database given the lookup name and culture id
    /// </summary>
    [Serializable]
    public partial class ApiLookupResponseDTO
    {
        public int CultureId { get; set; }
        public string LookupEntityName { get; set; }
        public ApiLookupDTO[] ApiLookupDTOList { get; set; }
    }

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
    [Serializable]
    public partial class SaveMarketInformationRequestDTO
    {
        public ApiMarketInformationSaveDTO[] MarketInformation { get; set; }
        public int TradingAccountId { get; set; }
    }

    /// <summary>
    /// Response containing the order. Only one of the two fields will be populated; this depends upon the type of order (Trade or Stop / Limit).
    /// </summary>
    [Serializable]
    public partial class GetOrderResponseDTO
    {
        public ApiTradeOrderDTO TradeOrder { get; set; }
        public ApiStopLimitOrderDTO StopLimitOrder { get; set; }
    }

    /// <summary>
    /// Response containing the open position.
    /// </summary>
    [Serializable]
    public partial class GetOpenPositionResponseDTO
    {
        public ApiOpenPositionDTO OpenPosition { get; set; }
    }

    /// <summary>
    /// if done
    /// </summary>
    [Serializable]
    public partial class ApiIfDoneResponseDTO
    {
        public ApiOrderResponseDTO Stop { get; set; }
        public ApiOrderResponseDTO Limit { get; set; }
    }

    /// <summary>
    /// request to save a watchlist.
    /// </summary>
    [Serializable]
    public partial class ApiSaveWatchlistRequestDTO
    {
        public ApiClientAccountWatchlistDTO Watchlist { get; set; }
    }

    /// <summary>
    /// The response from the trade request
    /// </summary>
    [Serializable]
    public partial class ApiTradeOrderResponseDTO
    {
        public int Status { get; set; }
        public int StatusReason { get; set; }
        public int OrderId { get; set; }
        public ApiOrderResponseDTO[] Orders { get; set; }
        public ApiQuoteResponseDTO Quote { get; set; }
        public string StatusReason_Resolved { get; set; }
        public string Status_Resolved { get; set; }
    }

    /// <summary>
    /// The response from a GET request for News headlines
    /// </summary>
    [Serializable]
    public partial class ListNewsHeadlinesResponseDTO
    {
        public NewsDTO[] Headlines { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListStopLimitOrderHistory query
    /// </summary>
    [Serializable]
    public partial class ListStopLimitOrderHistoryResponseDTO
    {
        public ApiStopLimitOrderHistoryDTO[] StopLimitOrderHistory { get; set; }
    }

    /// <summary>
    /// A cancel order request.
    /// </summary>
    [Serializable]
    public partial class CancelOrderRequestDTO
    {
        public int OrderId { get; set; }
        public int TradingAccountId { get; set; }
    }

    /// <summary>
    /// quote response.
    /// </summary>
    [Serializable]
    public partial class ApiQuoteResponseDTO
    {
        public int QuoteId { get; set; }
        public int Status { get; set; }
        public int StatusReason { get; set; }
    }

    /// <summary>
    /// Response to a CreateSessionRequest
    /// </summary>
    [Serializable]
    public partial class ApiLogOnResponseDTO
    {
        public string Session { get; set; }
    }

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
    [Serializable]
    public partial class ListMarketInformationRequestDTO
    {
        public int[] MarketIds { get; set; }
    }

    /// <summary>
    /// This is a description of the ErrorCode enum
    /// </summary>
    [Serializable]
    public enum ErrorCode
    {
        NoError = 0,
        Forbidden = 403,
        InternalServerError = 500,
        InvalidParameterType = 4000,
        ParameterMissing = 4001,
        InvalidParameterValue = 4002,
        InvalidJsonRequest = 4003,
        InvalidJsonRequestCaseFormat = 4004,
        InvalidCredentials = 4010,
        InvalidSession = 4011,
        NoDataAvailable = 5001,
        Throttling = 5002,
    }

    /// <summary>
    /// order response
    /// </summary>
    [Serializable]
    public partial class ApiOrderResponseDTO
    {
        public int OrderId { get; set; }
        public int StatusReason { get; set; }
        public int Status { get; set; }
        public decimal Price { get; set; }
        public decimal CommissionCharge { get; set; }
        public ApiIfDoneResponseDTO[] IfDone { get; set; }
        public decimal GuaranteedPremium { get; set; }
        public ApiOrderResponseDTO OCO { get; set; }
        public string StatusReason_Resolved { get; set; }
        public string Status_Resolved { get; set; }
    }

    /// <summary>
    /// Response containing the active stop limit order.
    /// </summary>
    [Serializable]
    public partial class GetActiveStopLimitOrderResponseDTO
    {
        public ApiActiveStopLimitOrderDTO ActiveStopLimitOrder { get; set; }
    }

    /// <summary>
    /// Update watchlist with item
    /// </summary>
    [Serializable]
    public partial class InsertWatchlistItemRequestDTO
    {
        public int ParentWatchlistDisplayOrderId { get; set; }
        public int MarketId { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListTradeHistory query
    /// </summary>
    [Serializable]
    public partial class ListTradeHistoryResponseDTO
    {
        public ApiTradeHistoryDTO[] TradeHistory { get; set; }
    }

    /// <summary>
    /// Response from am market information request.
    /// </summary>
    [Serializable]
    public partial class GetMarketInformationResponseDTO
    {
        public ApiMarketInformationDTO MarketInformation { get; set; }
    }

    [Serializable]
    public partial class ApiSaveWatchlistResponseDTO
    {
    }

    [Serializable]
    public partial class ApiDeleteWatchlistResponseDTO
    {
        public bool Deleted { get; set; }
    }

    /// <summary>
    /// A request for a trade order
    /// </summary>
    [Serializable]
    public partial class NewTradeOrderRequestDTO
    {
        public int MarketId { get; set; }
        public string Currency { get; set; }
        public bool AutoRollover { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public int? QuoteId { get; set; }
        public decimal BidPrice { get; set; }
        public decimal OfferPrice { get; set; }
        public string AuditId { get; set; }
        public int TradingAccountId { get; set; }
        public ApiIfDoneDTO[] IfDone { get; set; }
        public int[] Close { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListCfdMarkets query
    /// </summary>
    [Serializable]
    public partial class ListCfdMarketsResponseDTO
    {
        public ApiMarketDTO[] Markets { get; set; }
    }

    /// <summary>
    /// This Dto is not currently used
    /// </summary>
    [Serializable]
    public partial class ListOrdersResponseDTO
    {
    }

    /// <summary>
    /// This is a description of ErrorResponseDTO
    /// </summary>
    [Serializable]
    public partial class ApiErrorResponseDTO
    {
        public int HttpStatus { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }

    /// <summary>
    /// system status
    /// </summary>
    [Serializable]
    public partial class SystemStatusDTO
    {
        public string StatusMessage { get; set; }
    }

    /// <summary>
    /// A request for updating a trade order
    /// </summary>
    [Serializable]
    public partial class UpdateTradeOrderRequestDTO : NewTradeOrderRequestDTO
    {
        public int OrderId { get; set; }
    }

    /// <summary>
    /// The response from the stop limit order request
    /// </summary>
    [Serializable]
    public partial class ApiStopLimitResponseDTO : ApiOrderResponseDTO
    {
    }

    /// <summary>
    /// request to delete a watchlist.
    /// </summary>
    [Serializable]
    public partial class ApiDeleteWatchlistRequestDTO
    {
        public int WatchlistId { get; set; }
    }

    /// <summary>
    /// request to delete a session (log off)
    /// </summary>
    [Serializable]
    public partial class ApiLogOffRequestDTO
    {
        public string UserName { get; set; }
        public string Session { get; set; }
    }

    /// <summary>
    /// The response from a request for Price Ticks
    /// </summary>
    [Serializable]
    public partial class GetPriceTickResponseDTO
    {
        public PriceTickDTO[] PriceTicks { get; set; }
    }

    /// <summary>
    /// Response from a market information search request.
    /// </summary>
    [Serializable]
    public partial class ListMarketInformationSearchResponseDTO
    {
        public ApiMarketInformationDTO[] MarketInformation { get; set; }
    }

}
