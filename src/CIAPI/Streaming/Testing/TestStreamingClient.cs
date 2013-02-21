using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CIAPI.DTO;
using CIAPI.StreamingClient;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming.Testing
{
    /// <summary>
    /// 
    /// </summary>
    public class TestStreamingClient : IStreamingClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestStreamingClient));
        private IJsonSerializer _serializer;
        private Uri _streamingUri;
        private string _userName;
        private string _session;
        private TestStreamingClientFactory _testStreamingClientFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="serializer"></param>
        /// <param name="testStreamingClientFactory"></param>
        public TestStreamingClient(Uri streamingUri, string userName, string session, IJsonSerializer serializer, TestStreamingClientFactory testStreamingClientFactory)
        {
            _testStreamingClientFactory = testStreamingClientFactory;
            _session = session;
            _userName = userName;
            _streamingUri = streamingUri;
            _serializer = serializer;
            _adapters = new Dictionary<string, Dictionary<string, IStreamingListener>>();
            Log.Debug("TestStreamingClient created for " + string.Format("{1} {2} {0}", streamingUri, userName, _session));
        }

        private readonly Dictionary<string, Dictionary<string, IStreamingListener>> _adapters;


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StatusEventArgs> StatusChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnStatusChanged(object sender, StatusEventArgs e)
        {
            EventHandler<StatusEventArgs> handler = StatusChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAdapter"></param>
        /// <param name="topic"></param>
        /// <typeparam name="TDto"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IStreamingListener<TDto> BuildListener<TDto>(string dataAdapter, string topic)
                where TDto : class, new()
        {
            Dictionary<string, IStreamingListener> adp;
            if (!_adapters.ContainsKey(dataAdapter))
            {
#if WINDOWS_PHONE
                    if(_adapters.Count==5)
                    {
                        throw new Exception("Max concurrent lightstreamer adapters for WP7.1 is 5");
                    }
#endif

                adp = new Dictionary<string, IStreamingListener>();
                _adapters.Add(dataAdapter, adp);

            }
            else
            {
                adp = _adapters[dataAdapter];
            }

            var listener = new TestStreamingListener<TDto>(dataAdapter, topic);

            // #TODO: need to send 'connection established' status update
            adp.Add(topic, listener);

            // hookup listener events;
            listener.Start(0);
            return listener;
        }

        /// <summary>
        /// Allows consumer to stop and remove a listener from this client.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void TearDownListener(IStreamingListener listener)
        {
            listener.Stop();

            ((TestStreamingListener) listener).TearMeDown();

            if (_adapters.ContainsKey(listener.Adapter))
            {

                var adapter = _adapters[listener.Adapter];
                adapter.Remove(listener.Topic);
                if (adapter.Count == 0)
                {
                    _adapters.Remove(listener.Adapter);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            var topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category.ToString());
            IStreamingListener<NewsDTO> listener = BuildListener<NewsDTO>("CITYINDEXSTREAMING", topic);

            // hook up the create message event to the factory and enclose the unhook for use later.
            // this way we don't have to have 6 different unhook methods (one for each dto) on the factory

            ((TestStreamingListener<NewsDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreateNewsMessage;

            ((TestStreamingListener)listener).TearMeDown = () =>
                {
                    ((TestStreamingListener<NewsDTO>)listener).CreateMessage -= _testStreamingClientFactory.OnCreateNewsMessage;
                };

            return listener;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketIds"></param>
        /// <returns></returns>
        public IStreamingListener<PriceDTO> BuildPricesListener(int[] marketIds)
        {
            var topic = string.Join(" ", marketIds.Select(t => Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", t.ToString())).ToArray());
            IStreamingListener<PriceDTO> listener = BuildListener<PriceDTO>("CITYINDEXSTREAMING", topic);
            ((TestStreamingListener<PriceDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreatePriceMessage;
            return listener;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            string topic = "QUOTES";
            IStreamingListener<QuoteDTO> listener = BuildListener<QuoteDTO>("STREAMINGTRADINGACCOUNT", topic);
            ((TestStreamingListener<QuoteDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreateQuoteMessage;
            return listener;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            string topic = "CLIENTACCOUNTMARGIN";
            IStreamingListener<ClientAccountMarginDTO> listener = BuildListener<ClientAccountMarginDTO>("STREAMINGCLIENTACCOUNT", topic);
            ((TestStreamingListener<ClientAccountMarginDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreateClientAccountMarginMessage;
            return listener;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<OrderDTO> BuildOrdersListener()
        {
            string topic = "ORDERS";
            IStreamingListener<OrderDTO> listener = BuildListener<OrderDTO>("STREAMINGCLIENTACCOUNT", topic);
            ((TestStreamingListener<OrderDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreateOrderMessage;
            return listener;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountOperatorId"></param>
        /// <returns></returns>
        public IStreamingListener<PriceDTO> BuildDefaultPricesListener(int accountOperatorId)
        {
            IStreamingListener<PriceDTO> listener = BuildListener<PriceDTO>("CITYINDEXSTREAMINGDEFAULTPRICES", "PRICES.AC" + accountOperatorId);
            ((TestStreamingListener<PriceDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreatePriceMessage;
            return listener;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<TradeMarginDTO> BuildTradeMarginListener()
        {
            IStreamingListener<TradeMarginDTO> listener = BuildListener<TradeMarginDTO>("STREAMINGCLIENTACCOUNT", "TRADEMARGIN.ALL");
            ((TestStreamingListener<TradeMarginDTO>)listener).CreateMessage += _testStreamingClientFactory.OnCreateTradeMarginMessage;

            return listener;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var kvp in _adapters)
                {
                    foreach (var kvp2 in kvp.Value)
                    {
                        kvp2.Value.Dispose();
                    }
                }
            }
        }

    }
}