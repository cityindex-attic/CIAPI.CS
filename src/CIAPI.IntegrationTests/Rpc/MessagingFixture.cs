using System.Diagnostics;
using CIAPI.DTO;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class MessagingFixture
    {
        [Test]
        public void CanGetLookup()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            const string lookupEntityName = "OrderStatusReason";
            var orderStatus = rpcClient.Messaging.GetSystemLookup(lookupEntityName, 69);

            Assert.IsTrue(orderStatus.ApiLookupDTOList.Length > 0, "no lookup values returned for " + lookupEntityName);

            rpcClient.LogOut();
        }

        [Test]
        public void CanResolveMagicNumber()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            var resolver = new MagicNumberResolver(rpcClient);
            const string lookupEntityName = MagicNumberKeys.ApiOrderResponseDTO_Status;

            string orderStatusReason = resolver.ResolveMagicNumber(lookupEntityName, 1);

            Assert.IsNotNullOrEmpty(orderStatusReason, "could not resolve magic string");

            rpcClient.LogOut();
        }
        [Test]
        public void CanResolveDTO()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            var resolver = new MagicNumberResolver(rpcClient);

            // this would be the value you get back from the API
            GetOpenPositionResponseDTO source = new GetOpenPositionResponseDTO
            {
                OpenPosition = new ApiOpenPositionDTO { Status = 1 }
            };

            source.OpenPosition.ResolveMagicNumbers(resolver);


            Assert.AreEqual("Pending", source.OpenPosition.Status_Resolved, "status reason not resolved");

            rpcClient.LogOut();
        }
        [Test]
        public void LookupIsCached()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            var sw = new Stopwatch();

            for (int i = 0; i < 1000; i++)
            {
                var orderStatus = rpcClient.Messaging.GetSystemLookup("OrderStatusReason", 69);
                if (!sw.IsRunning)
                {
                    sw.Start();
                }
                Assert.IsTrue(orderStatus.ApiLookupDTOList.Length > 0, "no lookup values returned for OrderStatusReason");
            }

            sw.Stop();
            rpcClient.LogOut();

            Assert.IsTrue(sw.ElapsedMilliseconds < 10000, "took too long - not caching");
        }
    }
}