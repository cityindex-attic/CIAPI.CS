using System;
using System.Collections.Generic;
using CIAPI.DTO;
using CityIndex.JsonClient;
using StreamingClient;
using TradingApi.Client.Core.Lightstreamer;

namespace CIAPI.Rpc

{
    public partial class RpcClient : IStreamingClient
    {
        private readonly Uri _streamingUri;
        private readonly IStreamingConnectionFactory _streamingConnectionFactory;

        public RpcClient(Uri uri, Uri streamingUri, IRequestCache cache, IRequestFactory requestFactory,IStreamingConnectionFactory streamingConnectionFactory,
                         Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
            : base(uri, cache, requestFactory, throttleScopes, retryCount)
        {
            _streamingConnectionFactory = streamingConnectionFactory;
            _streamingUri = streamingUri;
        }

        public RpcClient(Uri rpcUri, Uri streamingUri)
            : base(rpcUri)
        {
            _streamingUri = streamingUri;
            _streamingConnectionFactory = new CityIndexStreamingConnectionFactory();
        }

        /// <summary>
        /// TODO: must null this field on logout - not done here yet due to laziness in updating phone7 and silverlight platform code
        /// </summary>
        private ILightstreamerConnection _streamingConnection;

        public ILightstreamerConnection StreamingConnection
        {
            get
            {
                EnsureClientIsAuthenticated();

                if (_streamingConnection == null)
                {
                    _streamingConnection = _streamingConnectionFactory.Create(_streamingUri, UserName, SessionId);
                }

                return _streamingConnection;
            }
        }


        public IStreamingSubscription<NewsDTO> CreateNewsSubscription(string path)
        {
            return new NewsHeadlineSubscription(path, StreamingConnection);
        }


        private void EnsureClientIsAuthenticated()
        {
            if (string.IsNullOrEmpty(SessionId))
            {
                throw new InvalidOperationException("ApiClient must be authenticated.");
            }
        }
    }
}