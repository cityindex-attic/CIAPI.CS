using System;
using System.Linq;
using Common.Logging;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
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

        public  void Stop()
        {
            if (_subscribedTableKey == null) return;

            _logger.DebugFormat("Unsubscribing from table with key: {0}", _subscribedTableKey.KeyValue);
            try
            {
                _lsClient.UnsubscribeTable(_subscribedTableKey);
            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("Exception occurred when stopping listener:", exception);
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
            /* do nothing */
        }

        void IHandyTableListener.OnSnapshotEnd(int itemPos, string itemName)
        {
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscr(int itemPos, string itemName)
        {
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscrAll()
        {
            /* do nothing */
        }
    }
}