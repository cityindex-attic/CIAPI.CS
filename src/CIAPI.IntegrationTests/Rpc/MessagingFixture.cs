using System.Diagnostics;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class MessagingFixture : RpcFixtureBase
    {
        [Test]
        public void CanGetLookup()
        {

            var rpcClient = BuildRpcClient();

            const string lookupEntityName = "OrderStatusReason";
            var orderStatus = rpcClient.Messaging.GetSystemLookup(lookupEntityName, 69);

            Assert.IsTrue(orderStatus.ApiLookupDTOList.Length > 0, "no lookup values returned for " + lookupEntityName);

            rpcClient.LogOut();
        }

        [Test]
        public void CanGetTranslationWithInterestingItems()
        {

            var rpcClient = BuildRpcClient();
            ApiClientApplicationMessageTranslationRequestDTO request = new ApiClientApplicationMessageTranslationRequestDTO
                                                                           {
                                                                               AccountOperatorId = 2347,
                                                                               ClientApplicationId = 0,
                                                                               CultureId = 69,
                                                                               InterestedTranslationKeys = new[] { "contactus_customerservicesemail", "contactus_customerservicesphone" }
                                                                           };
            var translation = rpcClient.Messaging.GetClientApplicationMessageTranslationWithInterestingItems(request);

            Assert.IsTrue(translation.TranslationKeyValuePairs.Length > 0, "no lookup translation values returned for " + request.ToStringWithValues());

            rpcClient.LogOut();
        }

        [Test]
        public void CanGetClientApplicationMessageTranslation()
        {
            var rpcClient = BuildRpcClient();

            var translation = rpcClient.Messaging.GetClientApplicationMessageTranslation(0,69,2347);

            Assert.IsTrue(translation.TranslationKeyValuePairs.Length > 0, "no lookup translation values returned");

            rpcClient.LogOut();
        }

        

        [Test]
        public void CanGetListOfCulturesLookup()
        {
            var rpcClient = BuildRpcClient();

            const string lookupEntityName = "culture";
            var language = rpcClient.Messaging.GetSystemLookup(lookupEntityName, 0); //A bit wierd, but you need to specify a dummy culture when asking for a list of cultures returned in...

            Assert.IsTrue(language.ApiCultureLookupDTOList.Length > 0, "no lookup values returned for " + lookupEntityName);

            rpcClient.LogOut();
        }

        [Test]
        public void CanResolveMagicNumber()
        {

            var rpcClient = BuildRpcClient();

            var resolver = new MagicNumberResolver(rpcClient);
            const string lookupEntityName = MagicNumberKeys.ApiOrderResponseDTO_Status;

            string orderStatusReason = resolver.ResolveMagicNumber(lookupEntityName, 1);

            Assert.IsNotNullOrEmpty(orderStatusReason, "could not resolve magic string");

            rpcClient.LogOut();
        }
        
        [Test]
        public void CanResolveDTO()
        {

            var rpcClient = BuildRpcClient();
            

            // this would be the value you get back from the API
            GetOpenPositionResponseDTO source = new GetOpenPositionResponseDTO
            {
                OpenPosition = new ApiOpenPositionDTO { Status = 1 }
            };

            rpcClient.MagicNumberResolver.ResolveMagicNumbers(source);

            Assert.AreEqual("Pending", source.OpenPosition.Status_Resolved, "status reason not resolved");

            rpcClient.LogOut();
        }

        [Test]
        public void CanPreloadMessages()
        {
            var rpcClient = BuildRpcClient();
            rpcClient .MagicNumberResolver.PreloadMagicNumbers();
            rpcClient.LogOut();
        }
        

        [Test]
        public void CanResolveDTOVersion2()
        {

            var rpcClient = BuildRpcClient();


            // this would be the value you get back from the API
            GetOpenPositionResponseDTO dto = new GetOpenPositionResponseDTO
            {
                OpenPosition = new ApiOpenPositionDTO { Status = 1 }
            };

            rpcClient.MagicNumberResolver.ResolveMagicNumbers(dto);
            

            Assert.AreEqual("Pending", dto.OpenPosition.Status_Resolved, "status reason not resolved");

            rpcClient.LogOut();
        }

        [Test]
        public void LookupIsCached()
        {

            var rpcClient = BuildRpcClient();

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