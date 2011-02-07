using System;
using System.Linq;
using System.Threading;
using Common.Logging;
using Lightstreamer.DotNet.Client;
using StreamingClient;
using StreamingClient.Lightstreamer;

namespace CIAPI.Streaming
{
    public class LightstreamerListener<TDto> : IStreamingListener<TDto>, IHandyTableListener
        where TDto : class, new()
    {
        private readonly LightstreamerDtoConverter<TDto> _messageConverter = new LightstreamerDtoConverter<TDto>();
        private static readonly ILog _logger = LogManager.GetLogger(typeof (LightstreamerListener<TDto>));
        private readonly string _groupOrItemName;
        private readonly LSClient _lsClient;
        private SubscribedTableKey _subscribedTableKey;
        private readonly string _dataAdapter;

        public LightstreamerListener(string fullTopic, LSClient lsClient)
        {
            _lsClient = lsClient;
            _dataAdapter = fullTopic.Split('.').First();
            _groupOrItemName = fullTopic.Replace(_dataAdapter + ".", "");
        }

        public event EventHandler<MessageEventArgs<TDto>> MessageRecieved;

        /// <summary>
        /// Start listening.  This is syncronous, and blocks until the server subscription has started
        /// </summary>
        public void Start()
        {
            var groupOrItemName = _groupOrItemName.ToUpper();
            var schema = _messageConverter.GetFieldList();
            var dataAdapter = _dataAdapter.ToUpper();
            _logger.DebugFormat("Subscribing to group:{0}, schema {1}, dataAdapter {2}", groupOrItemName, schema, dataAdapter);
            var simpleTableInfo = new SimpleTableInfo(
                groupOrItemName, 
                mode: "RAW", 
                schema: schema, 
                snap: false)
                { DataAdapter = dataAdapter };
            _subscribedTableKey = _lsClient.SubscribeTable(simpleTableInfo, this, false);
            _logger.DebugFormat("Subscribed to table with key: {0}", _subscribedTableKey.KeyValue);
        }

        /// <summary>
        /// Stop listening.  This is syncronous, and blocks until the server has been informed of the unsubscription; or the timeout is reached
        /// </summary>
        public void Stop()
        {
            if (_subscribedTableKey == null) return;

            var message = String.Format("Unsubscribing from table with key: {0}", _subscribedTableKey.KeyValue);
            _logger.DebugFormat(message);
            
            using (var gate = new ManualResetEvent(false))
            {
                
                new Thread(() =>{   
                                    _lsClient.UnsubscribeTable(_subscribedTableKey);
                                    gate.Set();
                                }) 
                                { Name = "Thread for " + message }
                                .Start();
                if (!gate.WaitOne(LightstreamerDefaults.DEFAULT_TIMEOUT_MS+1000))
                {
                    _logger.WarnFormat(string.Format("Giving up after {0}ms attempting to stop listener: {1}." +
                        "Client has stopped listening, but there might be a zombie process left on the server", LightstreamerDefaults.DEFAULT_TIMEOUT_MS, GetType().Name));
                }
            }
           
        }

        void IHandyTableListener.OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            try
            {
                if (MessageRecieved == null) return;

                MessageRecieved(this, new MessageEventArgs<TDto>(_groupOrItemName, _messageConverter.Convert(update)));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                // TODO: lightstreamer swallows errors thrown here - live with it or fix lightstreamer client code
                throw;
            }
        }

        void IHandyTableListener.OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            _logger.DebugFormat("OnRawUpdatesLost fired -> itemPos: {0} ietmName: {1} lostUpdates:{2}", itemPos, itemName, lostUpdates);
            /* do nothing */
        }

        void IHandyTableListener.OnSnapshotEnd(int itemPos, string itemName)
        {
            _logger.DebugFormat("OnSnapshotEnd fired -> itemPos: {0} ietmName: {1}", itemPos, itemName);
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscr(int itemPos, string itemName)
        {
            _logger.DebugFormat("OnUnsubscr fired -> itemPos: {0} ietmName: {1}", itemPos, itemName);
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscrAll()
        {
            _logger.DebugFormat("OnUnsubscrAll fired");
            /* do nothing */
        }
    }
}