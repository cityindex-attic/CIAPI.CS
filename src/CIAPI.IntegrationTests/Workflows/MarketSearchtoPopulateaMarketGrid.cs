using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using CIAPI.Tests;
using NUnit.Framework;
using Salient.ReliableHttpClient;

namespace CIAPI.IntegrationTests.Workflows
{
    [TestFixture]
    public class MarketSearchtoPopulateaMarketGrid 
    {

         
        [Test]
        public void DoWork()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, "bogus_app_key");

            var recorder = new Recorder(rpcClient);
            recorder.Start();
            
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);


            var accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            var response = rpcClient.CFDMarkets.ListCfdMarkets("USD", null, accounts.ClientAccountId, 500, false);

            var dto = new ListMarketInformationRequestDTO();
            dto.MarketIds = response.Markets.Select(m => m.MarketId).ToArray();
            var marketInformation = rpcClient.Market.ListMarketInformation(dto);
             rpcClient.LogOut();
            
            
            recorder.Stop();

            var json = rpcClient.Serializer.SerializeObject(recorder.GetRequests());
        }
    }
}
