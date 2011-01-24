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
        private static readonly ILog Logger = LogManager.GetLogger(typeof (LightstreamerListener<TDto>));
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
            var simpleTableInfo = new SimpleTableInfo(
                _groupOrItemName.ToUpper(), 
                mode: "RAW", 
                schema: _messageConverter.GetFieldList(), 
                snap: false)
                { DataAdapter = _dataAdapter.ToUpper() };
            _subscribedTableKey = _lsClient.SubscribeTable(simpleTableInfo, this, false);
        }

        public  void Stop()
        {
             if (_subscribedTableKey!=null)
                _lsClient.UnsubscribeTable(_subscribedTableKey);
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
                Logger.Error(ex);

                // TODO: lightstreamer swallows errors thrown here - live with it or fix lightstreamer client code
                throw;
            }
        }

        void IHandyTableListener.OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            throw new NotImplementedException();
        }

        void IHandyTableListener.OnSnapshotEnd(int itemPos, string itemName)
        {
            throw new NotImplementedException();
        }

        void IHandyTableListener.OnUnsubscr(int itemPos, string itemName)
        {
            throw new NotImplementedException();
        }

        void IHandyTableListener.OnUnsubscrAll()
        {
            throw new NotImplementedException();
        }
    }
}