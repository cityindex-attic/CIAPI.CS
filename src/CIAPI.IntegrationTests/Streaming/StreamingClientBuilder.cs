using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Streaming;
using CIAPI.Streaming.Lightstreamer;

namespace CIAPI.IntegrationTests.Streaming
{
    public class StreamingClientBuilder
    {
        public static LightStreamerClient BuildStreamingClient(string userName = "0x234", string password = "password")
        {
            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
            
            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);
           
            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            return new LightStreamerClient(streamingUri, userName, authenticatedClient.SessionId);
        }
    }
}
