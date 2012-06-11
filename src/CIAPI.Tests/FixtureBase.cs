using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Rpc;
using CIAPI.Serialization;
using CIAPI.Streaming.Testing;
using NUnit.Framework;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.Tests
{
    public abstract class FixtureBase
    {
        

        
        protected Client BuildAuthenticatedTestClient(out TestRequestFactory requestFactory, out TestStreamingClientFactory streamingFactory)
        {
            var client = BuildTestClient(out requestFactory, out streamingFactory);
            client.UserName = TestConfig.ApiUsername;
            client.Session = TestConfig.ApiTestSessionId;
            return client;
        }
        protected Client BuildTestClient(out TestRequestFactory requestFactory, out TestStreamingClientFactory streamingFactory)
        {
            requestFactory = new TestRequestFactory();
            streamingFactory = new TestStreamingClientFactory();

            var rpcClient = new Client(new Uri("http://foo.com"), new Uri("http://foo.com"), "FOOBAR", new Serializer(), requestFactory, streamingFactory);
            return rpcClient;
        }
    }
}
