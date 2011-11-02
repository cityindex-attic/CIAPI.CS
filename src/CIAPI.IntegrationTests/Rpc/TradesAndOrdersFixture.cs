using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class TradesAndOrdersFixture : RpcFixtureBase
    {
        [Test]
        public void CanCancelOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            CancelOrderRequestDTO order = new CancelOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.CancelOrder(order);
        }

        [Test]
        public void CanGetActiveStopLimitOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            string orderId = "";
            var response = rpcClient.TradesAndOrders.GetActiveStopLimitOrder(orderId);
        }

        [Test]
        public void CanGetOpenPosition()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            string orderId = "";
            var response = rpcClient.TradesAndOrders.GetOpenPosition(orderId);
        }

        [Test]
        public void CanGetOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            string order = "";
            var response = rpcClient.TradesAndOrders.GetOrder(order);
        }

        [Test]
        public void CanListActiveStopLimitOrders()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int tradingAccountId = 0;
            var response = rpcClient.TradesAndOrders.ListActiveStopLimitOrders(tradingAccountId);
        }

        [Test]
        public void CanListOpenPositions()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int tradingAccountId = 0;
            var response = rpcClient.TradesAndOrders.ListOpenPositions(tradingAccountId);
        }

        [Test]
        public void CanListStopLimitOrderHistory()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int maxResults = 0;
            int tradingAccountId = 0;
            var response = rpcClient.TradesAndOrders.ListStopLimitOrderHistory(tradingAccountId, maxResults);
        }

        [Test]
        public void CanListTradeHistory()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int maxResults = 0;
            int tradingAccountId = 0;
            var response = rpcClient.TradesAndOrders.ListTradeHistory(tradingAccountId, maxResults);
        }

        [Test]
        public void CanOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            NewStopLimitOrderRequestDTO order = new NewStopLimitOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.Order(order);
        }

        [Test]
        public void CanTrade()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            NewTradeOrderRequestDTO trade = new NewTradeOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.Trade(trade);
        }

        [Test]
        public void CanUpdateOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            UpdateStopLimitOrderRequestDTO update = new UpdateStopLimitOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.UpdateOrder(update);
        }

        [Test]
        public void CanUpdateTrade()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            UpdateTradeOrderRequestDTO update = new UpdateTradeOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.UpdateTrade(update);
        }
    }
}