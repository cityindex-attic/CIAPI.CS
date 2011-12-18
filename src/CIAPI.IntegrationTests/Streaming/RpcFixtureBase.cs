using CIAPI.Rpc;
using CIAPI.Streaming;

namespace CIAPI.IntegrationTests.Streaming
{
    public class RpcFixtureBase
    {
        public Client BuildRpcClient(string apiKey = null)
        {
            var rpcClient = new Client(Settings.RpcUri)
                                {
                                    ApiKey = apiKey
                                };
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            return rpcClient;
        }


        public IStreamingClient BuildStreamingClient()
        {
            var authenticatedClient = new CIAPI.Rpc.Client(Settings.RpcUri);
            authenticatedClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            return StreamingClientFactory.CreateStreamingClient(Settings.StreamingUri, Settings.RpcUserName, authenticatedClient.Session);
        }
    }
}