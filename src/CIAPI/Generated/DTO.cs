using System;

namespace CIAPI.DTO
{
    /// <summary>
    /// A stop or limit order from a historical perspective.
    /// </summary>
    public class ApiStopLimitOrderHistoryDTO
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
    }

    /// <summary>
    /// An IfDone order represents an order which is placed when the corresponding order is filled, e.g attaching a stop/limit to a trade or order
    /// </summary>
    public class ApiIfDoneDTO
    {
        public ApiStopLimitOrderDTO Stop { get; set; }
        public ApiStopLimitOrderDTO Limit { get; set; }
    }

    /// <summary>
    /// Information about a TradingAccount
    /// </summary>
    public class ApiTradingAccountDTO
    {
        public int TradingAccountId { get; set; }
        public string TradingAccountCode { get; set; }
        public string TradingAccountStatus { get; set; }
        public string TradingAccountType { get; set; }
    }

    /// <summary>
    /// The details of a specific price bar, useful for plotting candlestick charts
    /// </summary>
    public class PriceBarDTO
    {
        public DateTime BarDate { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }

    /// <summary>
    /// A Price for a specific Market.
    /// </summary>
    public class PriceDTO
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
    public class ApiTradeHistoryDTO
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
    }

    /// <summary>
    /// A stop or limit order with a limited number of data fields.
    /// </summary>
    public class ApiBasicStopLimitOrderDTO
    {
        public int OrderId { get; set; }
        public decimal TriggerPrice { get; set; }
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// A news headline
    /// </summary>
    public class NewsDTO
    {
        public int StoryId { get; set; }
        public string Headline { get; set; }
        public DateTime PublishDate { get; set; }
    }

    /// <summary>
    /// Api watchlist item
    /// </summary>
    public class ApiClientAccountWatchlistItemDTO
    {
        public int WatchlistId { get; set; }
        public int MarketId { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Generic look up data.
    /// </summary>
    public class ApiLookupDTO
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
    /// A Trade, or order that is currently open.
    /// </summary>
    public class ApiOpenPositionDTO
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
    }

    /// <summary>
    /// market end of day information.
    /// </summary>
    public class ApiMarketEodDTO
    {
        public string MarketEodUnit { get; set; }
        public int? MarketEodAmount { get; set; }
    }

    /// <summary>
    /// Contains market information.
    /// </summary>
    public class ApiMarketInformationDTO
    {
        public int MarketId { get; set; }
        public string Name { get; set; }
        public decimal? MarginFactor { get; set; }
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
    }

    /// <summary>
    /// Represents an order
    /// </summary>
    public class ApiOrderDTO
    {
        public int OrderId { get; set; }
        public int MarketId { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal TradingAccountId { get; set; }
        public int CurrencyId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public ApiIfDoneDTO[] IfDone { get; set; }
        public ApiStopLimitOrderDTO OcoOrder { get; set; }
    }

    /// <summary>
    /// A stop or limit order that is currently active.
    /// </summary>
    public class ApiActiveStopLimitOrderDTO
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
    }

    /// <summary>
    /// basic information about a Market
    /// </summary>
    public class ApiMarketDTO
    {
        public int MarketId { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// A quote for a specific order request
    /// </summary>
    public class QuoteDTO
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
    public class ApiStopLimitOrderDTO : ApiOrderDTO
    {
        public DateTime? ExpiryDateTimeUTC { get; set; }
        public string Applicability { get; set; }
    }

    /// <summary>
    /// An order for a specific Trading Account
    /// </summary>
    public class OrderDTO
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
    /// The mid price at a particular point in time.
    /// </summary>
    public class PriceTickDTO
    {
        public DateTime TickDate { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Client account watchlist
    /// </summary>
    public class ApiClientAccountWatchlistDTO
    {
        public int WatchlistId { get; set; }
        public string WatchlistDescription { get; set; }
        public int DisplayOrder { get; set; }
        public ApiClientAccountWatchlistItemDTO[] Items { get; set; }
    }

    /// <summary>
    /// The current margin for a specific client account
    /// </summary>
    public class ClientAccountMarginDTO
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
    /// Contains details of a specific news story
    /// </summary>
    public class NewsDetailDTO : NewsDTO
    {
        public string Story { get; set; }
    }

    /// <summary>
    /// Represents a trade order
    /// </summary>
    public class ApiTradeOrderDTO : ApiOrderDTO
    {
    }

    /// <summary>
    /// order response
    /// </summary>
    public class ApiOrderResponseDTO
    {
        public int OrderId { get; set; }
        public int StatusReason { get; set; }
        public int Status { get; set; }
        public decimal Price { get; set; }
        public decimal CommissionCharge { get; set; }
        public ApiIfDoneResponseDTO[] IfDone { get; set; }
        public decimal GuaranteedPremium { get; set; }
        public ApiOrderResponseDTO OCO { get; set; }
    }

    /// <summary>
    /// The response from the stop limit order request
    /// </summary>
    public class ApiStopLimitResponseDTO : ApiOrderResponseDTO
    {
    }

    /// <summary>
    /// The response from a GET request for News headlines
    /// </summary>
    public class ListNewsHeadlinesResponseDTO
    {
        public NewsDTO[] Headlines { get; set; }
    }

    /// <summary>
    /// request to create a session (log on).
    /// </summary>
    public class ApiLogOnRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Response containing the order. Only one of the two fields will be populated; this depends upon the type of order (Trade or Stop / Limit).
    /// </summary>
    public class GetOrderResponseDTO
    {
        public ApiTradeOrderDTO TradeOrder { get; set; }
        public ApiStopLimitOrderDTO StopLimitOrder { get; set; }
    }

    /// <summary>
    /// Response containing the active stop limit order.
    /// </summary>
    public class GetActiveStopLimitOrderResponseDTO
    {
        public ApiActiveStopLimitOrderDTO ActiveStopLimitOrder { get; set; }
    }

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
    public class UpdateWatchlistDisplayOrderRequestDTO
    {
        public int[] NewDisplayOrderIdSequence { get; set; }
    }

    /// <summary>
    /// A cancel order request.
    /// </summary>
    public class CancelOrderRequestDTO
    {
        public int OrderId { get; set; }
        public int TradingAccountId { get; set; }
    }

    /// <summary>
    /// Update watchlist with item
    /// </summary>
    public class InsertWatchlistItemRequestDTO
    {
        public int WatchlistDisplayOrderId { get; set; }
        public int MarketId { get; set; }
    }

    /// <summary>
    /// The response from a request for Price Ticks
    /// </summary>
    public class GetPriceTickResponseDTO
    {
        public PriceTickDTO[] PriceTicks { get; set; }
    }

    /// <summary>
    /// request to delete a session (log off)
    /// </summary>
    public class ApiLogOffRequestDTO
    {
        public string UserName { get; set; }
        public string Session { get; set; }
    }

    /// <summary>
    /// Get market information for a list of markets.
    /// </summary>
    public class ListMarketInformationRequestDTO
    {
        public int[] MarketIds { get; set; }
    }

    /// <summary>
    /// The response from the trade request
    /// </summary>
    public class ApiTradeOrderResponseDTO
    {
        public int Status { get; set; }
        public int StatusReason { get; set; }
        public int OrderId { get; set; }
        public ApiOrderResponseDTO[] Orders { get; set; }
        public ApiQuoteResponseDTO Quote { get; set; }
    }

    /// <summary>
    /// quote response.
    /// </summary>
    public class ApiQuoteResponseDTO
    {
        public int QuoteId { get; set; }
        public int Status { get; set; }
        public int StatusReason { get; set; }
    }

    /// <summary>
    /// Response to a CreateSessionRequest
    /// </summary>
    public class ApiLogOnResponseDTO
    {
        public string Session { get; set; }
    }

    /// <summary>
    /// Response from am market information request.
    /// </summary>
    public class ListMarketInformationResponseDTO
    {
        public ApiMarketInformationDTO[] MarketInformation { get; set; }
    }

    /// <summary>
    /// response from an account information query.
    /// </summary>
    public class AccountInformationResponseDTO
    {
        public string LogonUserName { get; set; }
        public int ClientAccountId { get; set; }
        public string ClientAccountCurrency { get; set; }
        public ApiTradingAccountDTO[] TradingAccounts { get; set; }
    }

    /// <summary>
    /// A request for a stop/limit order
    /// </summary>
    public class NewStopLimitOrderRequestDTO
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
    public class UpdateStopLimitOrderRequestDTO : NewStopLimitOrderRequestDTO
    {
    }

    /// <summary>
    /// The response from a GET price bar history request. Contains both an array of finalized price bars, and a partial (not finalized) bar for the current period
    /// </summary>
    public class GetPriceBarResponseDTO
    {
        public PriceBarDTO[] PriceBars { get; set; }
        public PriceBarDTO PartialPriceBar { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListStopLimitOrderHistory query
    /// </summary>
    public class ListStopLimitOrderHistoryResponseDTO
    {
        public ApiStopLimitOrderHistoryDTO[] StopLimitOrderHistory { get; set; }
    }

    /// <summary>
    /// Response from am market information request.
    /// </summary>
    public class GetMarketInformationResponseDTO
    {
        public ApiMarketInformationDTO MarketInformation { get; set; }
    }

    /// <summary>
    /// Message popup response denoting whether the client application should display a popup notification at startup.
    /// </summary>
    public class GetMessagePopupResponseDTO
    {
        public bool AskForClientApproval { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// This is a description of ErrorResponseDTO
    /// </summary>
    public class ApiErrorResponseDTO
    {
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }

    /// <summary>
    /// Response from a market information search request.
    /// </summary>
    public class ListMarketInformationSearchResponseDTO
    {
        public ApiMarketInformationDTO[] MarketInformation { get; set; }
    }

    /// <summary>
    /// Response containing the open position.
    /// </summary>
    public class GetOpenPositionResponseDTO
    {
        public ApiOpenPositionDTO OpenPosition { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListSpreadMarkets query
    /// </summary>
    public class ListSpreadMarketsResponseDTO
    {
        public ApiMarketDTO[] Markets { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListOpenPositions query
    /// </summary>
    public class ListOpenPositionsResponseDTO
    {
        public ApiOpenPositionDTO[] OpenPositions { get; set; }
    }

    /// <summary>
    /// This is a description of the ErrorCode enum
    /// </summary>
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
    }

    /// <summary>
    /// This Dto is not currently used
    /// </summary>
    public class ListOrdersResponseDTO
    {
    }

    /// <summary>
    /// Contains the result of a ListCfdMarkets query
    /// </summary>
    public class ListCfdMarketsResponseDTO
    {
        public ApiMarketDTO[] Markets { get; set; }
    }

    /// <summary>
    /// Response from a session delete request.
    /// </summary>
    public class ApiLogOffResponseDTO
    {
        public bool LoggedOut { get; set; }
    }

    /// <summary>
    /// Gets the client watchlist
    /// </summary>
    public class ListWatchlistResponseDTO
    {
        public int ClientAccountId { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListTradeHistory query
    /// </summary>
    public class ListTradeHistoryResponseDTO
    {
        public ApiTradeHistoryDTO[] TradeHistory { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListActiveStopLimitOrder query
    /// </summary>
    public class ListActiveStopLimitOrderResponseDTO
    {
        public ApiActiveStopLimitOrderDTO[] ActiveStopLimitOrders { get; set; }
    }

    /// <summary>
    /// A request for a trade order
    /// </summary>
    public class NewTradeOrderRequestDTO
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
    /// A request for updating a trade order
    /// </summary>
    public class UpdateTradeOrderRequestDTO : NewTradeOrderRequestDTO
    {
        public int OrderId { get; set; }
    }

    /// <summary>
    /// JSON returned from News Detail GET request
    /// </summary>
    public class GetNewsDetailResponseDTO
    {
        public NewsDetailDTO NewsDetail { get; set; }
    }

    /// <summary>
    /// Gets the lookup entities from trading database given the lookup name and culture id
    /// </summary>
    public class ApiLookupResponseDTO
    {
        public int CultureId { get; set; }
        public string LookupEntityName { get; set; }
        public ApiLookupDTO[] ApiLookupDTOList { get; set; }
    }

    /// <summary>
    /// if done
    /// </summary>
    public class ApiIfDoneResponseDTO
    {
        public ApiOrderResponseDTO Stop { get; set; }
        public ApiOrderResponseDTO Limit { get; set; }
    }

    /// <summary>
    /// system status
    /// </summary>
    public class SystemStatusDTO
    {
        public string StatusMessage { get; set; }
    }

    /// <summary>
    /// system status request.
    /// </summary>
    public class SystemStatusRequestDTO
    {
        public string TestDepth { get; set; }
    }

}
