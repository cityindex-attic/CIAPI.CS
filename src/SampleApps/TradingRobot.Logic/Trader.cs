using CIAPI.DTO;
using CIAPI.Rpc;

namespace TradingRobot.Logic
{
    public abstract class Trader
    {
        private readonly Client _client;

        protected Trader(Client client)
        {
            _client = client;
        }

        public decimal Quantity { get; set; }

        private static int GetTradingAccountId(Client client, int marketId, AccountInformationResponseDTO accountInfo)
        {
            GetMarketInformationResponseDTO marketInfo = client.Market.GetMarketInformation(marketId.ToString());
            bool isCfd = marketInfo.MarketInformation.Name.EndsWith("CFD");
            ApiTradingAccountDTO tradingAccount = isCfd
                                                      ? accountInfo.TradingAccounts[0]
                                                      : accountInfo.TradingAccounts[1];
            return tradingAccount.TradingAccountId;
        }

        public ApiTradeOrderResponseDTO ClosePostion(PriceDTO price)
        {
            return Trade(price, Direction.sell);
        }

        public ApiTradeOrderResponseDTO OpenPosition(PriceDTO price)
        {
            return Trade(price, Direction.buy);
        }


        public ApiTradeOrderResponseDTO Trade(PriceDTO price, Direction direction)
        {
            AccountInformationResponseDTO accountInfo = _client.AccountInformation.GetClientAndTradingAccount();
            int tradingAccountId = GetTradingAccountId(_client, price.MarketId, accountInfo);
            var request = new NewTradeOrderRequestDTO
                              {
                                  MarketId = price.MarketId,
                                  Direction = direction.ToString().ToLower(),
                                  Quantity = Quantity,
                                  BidPrice = price.Bid,
                                  OfferPrice = price.Offer,
                                  AuditId = price.AuditId,
                                  TradingAccountId = tradingAccountId,
                                  AutoRollover = false
                              };

            ApiTradeOrderResponseDTO response = _client.TradesAndOrders.Trade(request);
            return response;
        }

        public abstract void ProcessTick(PriceDTO price);
    }
}