using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using Common.Logging;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.TradingController
{
    /// <summary>
    /// A reference implementation to illustrate some of the capabilities of the api client
    /// as well as a guide to some of the common issues encountered while developing against  
    /// the api client.
    /// </summary>
    public class Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Controller));

        private readonly Dictionary<int, PriceDTO> _prices = new Dictionary<int, PriceDTO>();

        private IStreamingListener<NewsDTO> _newsListener;
        private IStreamingListener<PriceDTO> _pricesListener;
        private object _syncObj = new object();

        public Controller(string rpcUri, string streamingUri, string userName, string password)
        {
            RpcUri = rpcUri;
            StreamingUri = streamingUri;
            UserName = userName;
            Password = password;

        }

        
        public IStreamingClient StreamingClient { get; private set; }
        public Client RpcClient { get; private set; }

        #region session and account state

        public AccountInformationResponseDTO ClientAccounts { get; private set; }
        public ApiTradingAccountDTO CFDAccount { get; private set; }
        public ApiTradingAccountDTO SpreadBettingAccount { get; private set; }
        public string SessionId { get; private set; }

        #endregion

        #region surfacing ctor parameters

        public string RpcUri { get; private set; }
        public string StreamingUri { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        #endregion


        #region Streaming Price Quotes

        public event EventHandler<MessageEventArgs<PriceDTO>> PriceUpdate;


        public Dictionary<int, PriceDTO> GetCurrentPrices()
        {
            lock (_syncObj)
            {
                return _prices.ToDictionary(item => item.Key, item => item.Value);
            }
        }

        public void SubscribePrices(params int[] marketIds)
        {
            lock (_syncObj)
            {
                foreach (int marketId in marketIds)
                {
                    if (!_prices.ContainsKey(marketId))
                    {
                        _prices.Add(marketId, null);
                    }
                }
            }


            RestartPriceListener(_prices.Keys.ToArray());
        }

        public void UnsubscribeAllPrices()
        {
            lock (_syncObj)
            {
                _prices.Clear();
            }

            KillPriceListener();
        }

        public void UnsubscribePrices(params int[] marketIds)
        {
            lock (_syncObj)
            {
                foreach (int marketId in marketIds)
                {
                    if (_prices.ContainsKey(marketId))
                    {
                        _prices.Remove(marketId);
                    }
                }
            }
            
            RestartPriceListener(_prices.Keys.ToArray());
        }

        private void OnPriceUpdate(MessageEventArgs<PriceDTO> e)
        {
            EventHandler<MessageEventArgs<PriceDTO>> handler = PriceUpdate;
            if (handler != null) handler(this, e);
        }

        private void PricesListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            lock (_syncObj)
            {
                if (_prices.ContainsKey(e.Data.MarketId))
                {
                    _prices[e.Data.MarketId] = e.Data;
                }
            }
            OnPriceUpdate(e);
        }

        private void RestartPriceListener(params int[] markets)
        {
            if (_pricesListener != null)
            {
                KillPriceListener();
            }
            
            _pricesListener = StreamingClient.BuildPricesListener(markets);
            _pricesListener.MessageReceived += (PricesListenerMessageReceived);
            
        }

        private void KillPriceListener()
        {
            _pricesListener.MessageReceived -= (PricesListenerMessageReceived);
            _pricesListener.Stop();
            _pricesListener = null;
        }

        #endregion

        #region Streaming News Headlines

        public event EventHandler<MessageEventArgs<NewsDTO>> NewsUpdate;

        // only listening to one news topic at a time. right now options are US, UK and ALL.

        public void SetNewsTopic(string topic)
        {
            if (_newsListener != null)
            {
                _newsListener.MessageReceived -= NewsListenerMessageReceived;
                _newsListener.Stop();
                _newsListener = null;
            }

            _newsListener = StreamingClient.BuildNewsHeadlinesListener(topic);
            _newsListener.MessageReceived += NewsListenerMessageReceived;
            
        }


        private void OnNewsMessageReceived(MessageEventArgs<NewsDTO> e)
        {
            EventHandler<MessageEventArgs<NewsDTO>> handler = NewsUpdate;
            if (handler != null) handler(this, e);
        }

        private void NewsListenerMessageReceived(object sender, MessageEventArgs<NewsDTO> e)
        {
            OnNewsMessageReceived(e);
        }

        #endregion

        #region Trading

        public void PlaceTradeAsync(int tradingAccountId, int marketId, TradeDirection direction, decimal quantity, decimal bidPrice, decimal offerPrice, TradeOrderResponseDelegate callback)
        {
            PlaceTradeAsync(tradingAccountId, marketId, direction, quantity, bidPrice, offerPrice, new int[] { }, callback);
        }

        public void PlaceTradeAsync(int tradingAccountId, int marketId, TradeDirection direction, decimal quantity, decimal bidPrice, decimal offerPrice, int[] close, TradeOrderResponseDelegate callback)
        {
            NewTradeOrderRequestDTO orderRequest;
            lock (_syncObj)
            {
                PriceDTO price;

                _prices.TryGetValue(marketId, out price);

                if (price == null)
                {
                    throw new Exception("you must have a price subscription in order to place a trade");
                }

                orderRequest = new NewTradeOrderRequestDTO
                                   {
                                       AuditId = price.AuditId,
                                       MarketId = marketId,
                                       Direction = direction.ToString(),
                                       BidPrice = bidPrice,
                                       OfferPrice = offerPrice,
                                       Quantity = quantity,
                                       Close = close,
                                       TradingAccountId = tradingAccountId
                                   };
            }

            PlaceTradeAsync(orderRequest, callback);


        }


        /// <summary>
        /// An example of one strategy for placing a trade.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public void PlaceTradeAsync(NewTradeOrderRequestDTO request, TradeOrderResponseDelegate callback)
        {


            RpcClient.TradesAndOrders.BeginTrade(request, (ar) =>
                {
                    var result = new ApiResult<ApiTradeOrderResponseDTO>();

                    try
                    {
                        result.Data = RpcClient.TradesAndOrders.EndTrade(ar);

                        // resolve the magic numbers so that we have readable values for status codes
                        RpcClient.MagicNumberResolver.ResolveMagicNumbers(result.Data);

                    }
                    catch (Exception ex)
                    {
                        result.Exception = ex;
                    }


                    // to avoid deadlocks that can occur when an async call is made within the callback of another async callback
                    // we will fire the callback on a new thread.

                    new Thread(() => callback(result)).Start();

                    // NOTE: the deadlock is a result of locking on the request queue and cache in JsonClient and is the price paid
                    // for that functionality. 
                    
                    // TODO: (in JsonClient) find out how to determine if an object is being locked on and throw exception if this is so.

                    

                }, null);

        } 

        #endregion

        /// <summary>
        /// Connect to rpc and push servers, get client account information and preload lookup values
        /// </summary>
        public void Connect()
        {
            RpcClient = new Client(new Uri(RpcUri));
            try
            {
                RpcClient.LogIn(UserName, Password);
                Log.Info("Rpc client logged in");
            }
            catch 
            {
                Log.Info("Rpc client login failed");
                throw;
            }
            

            SessionId = RpcClient.Session;

            Log.Info("Getting client account information");
            try
            {
                ClientAccounts = RpcClient.AccountInformation.GetClientAndTradingAccount();
                Log.Info("Client account information received");
            }
            catch 
            {
                Log.Info("Failure receiving client account information");
                throw;
            }

            CFDAccount = ClientAccounts.TradingAccounts.FirstOrDefault(a => a.TradingAccountType == "CFD");
            SpreadBettingAccount =
                ClientAccounts.TradingAccounts.FirstOrDefault(a => a.TradingAccountType == "Spread Betting");

            StreamingClient = StreamingClientFactory.CreateStreamingClient(new Uri(StreamingUri), UserName, SessionId);

            Log.Info("Connecting streaming client");
            try
            {
                
                Log.Info("Streaming client constructed");
            }
            catch 
            {
                Log.Info("Failure constructing streaming client");    
                throw;
            }

            

            // Making an API call within an async handler produces a deadlock and the most common source of these deadlocks
            // is resolving a magic number on a DTO you have just recieved. So lets eliminate this by preloading all of the lookups.

            Log.Info("Preloading magic number lookups"); 
            RpcClient.MagicNumberResolver.PreloadMagicNumbers();

        }

        public void Disconnect()
        {
            Log.Info("Shutting down"); 
            
            StreamingClient = null;
            RpcClient.LogOut();
            RpcClient = null;
            ClientAccounts = null;
            CFDAccount = null;
            SpreadBettingAccount = null;
            SessionId = null;
        }


    }
}