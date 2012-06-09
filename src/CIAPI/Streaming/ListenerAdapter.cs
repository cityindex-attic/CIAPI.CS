using System;
using System.Linq;
using System.Threading;

using Lightstreamer.DotNet.Client;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization;
using StreamingClient.Lightstreamer;

namespace StreamingClient
{
    /// <summary>
    /// the purpose of this adapter is to allow consumer to maintain handlers
    /// for listener events even when the internal listener is reinstantiated due
    /// to fault tolerance events
    /// </summary>
    public sealed class ListenerAdapter<TDto> : IStreamingListener<TDto> where TDto : class, new()
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ListenerAdapter<TDto>));

        private readonly IJsonSerializer _serializer;
        private readonly string _adapterSet;
        private readonly string _channel;
        private readonly string _groupOrItemName;
        private readonly FaultTolerantLsClientAdapter _lsClient;
        private readonly LightstreamerDtoConverter<TDto> _messageConverter ;


        
        public string AdapterSet
        {
            get
            {
                return _adapterSet;
            }
        }
        public string Channel
        {
            get
            {
                return _channel;
            }
        }
        private TableListener<TDto> _listener;
        private SubscribedTableKey _subscribedTableKey;

        public ListenerAdapter(string topic, FaultTolerantLsClientAdapter client, IJsonSerializer serializer)
        {
            _serializer = serializer;
            _messageConverter = new LightstreamerDtoConverter<TDto>(_serializer);

            Topic = topic;
            _lsClient = client;
            _adapterSet = client.AdapterSet;
            _channel = topic.Split('.').First();
            _groupOrItemName = topic.Replace(_channel + ".", "");
        }

        #region IStreamingListenerAdapter<TDto> Members

        void IStreamingListener.Start(int phase)
        {
            string groupOrItemName = _groupOrItemName.ToUpper();
            if (_listener != null)
            {
                _listener.MessageReceived -= ListenerMessageReceived;
                ((IStreamingListener) this).Stop();
            }

            _listener = new TableListener<TDto>(groupOrItemName, phase,_serializer);
            _listener.MessageReceived += ListenerMessageReceived;

            string schema = _messageConverter.GetFieldList();
            string channel = _channel.ToUpper();
            Logger.Debug(string.Format("Subscribing to group:{0}, schema {1}, dataAdapter {2}", groupOrItemName, schema,channel));

            var simpleTableInfo = new SimpleTableInfo(
                groupOrItemName,
                schema: schema,
                mode: "MERGE", snap: true ) //To ensure the last message published prior to this subscription is recieved immediately, mode must be MERGE and snap = true
                {DataAdapter = channel};
            var gate = new ManualResetEvent(false);
            Exception ex = null;
            new Thread(() =>
                           {
                               try
                               {
                                   _subscribedTableKey = _lsClient.Client.SubscribeTable(simpleTableInfo, _listener,
                                                                                         false);
                                   Logger.Debug(string.Format("Subscribed to table with key: {0}", _subscribedTableKey));
                               }
                               catch (Exception exInner)
                               {
                                   ex = exInner;
                               }
                               gate.Set();
                           }).Start();
            if (ex != null)
            {
                Logger.Error(ex);
                throw ex;
            }

            if (!gate.WaitOne(LightstreamerDefaults.DEFAULT_TIMEOUT_MS + 1000))
            {
                Logger.Error(string.Format("Listener taking longer than {0}ms to start: {1}.",
                                           LightstreamerDefaults.DEFAULT_TIMEOUT_MS, GetType().Name));
            }
        }


        void IStreamingListener.Stop()
        {
            if (_subscribedTableKey == null) return;

            string message = String.Format("Unsubscribing from table with key: {0}", _subscribedTableKey);
            Logger.Debug(message);
            new Thread(() =>
                           {
                               try
                               {
                                   _lsClient.Client.UnsubscribeTable(_subscribedTableKey);
                               }
                               catch (Exception ex)
                               {
                                   Logger.Warn(ex);
                               }
                           }) {Name = "Thread for " + message}.Start();
        }

        public event EventHandler<MessageEventArgs<TDto>> MessageReceived;


        public string Topic { get; set; }

        #endregion

        private void ListenerMessageReceived(object sender, MessageEventArgs<TDto> e)
        {
            // if the table listener is out of phase, ignore
            if (!_lsClient.CheckPhase(e.Phase))
            {
                return;
            }

            EventHandler<MessageEventArgs<TDto>> handler = MessageReceived;
            if (handler != null) handler(this, e);
        }

    }
}