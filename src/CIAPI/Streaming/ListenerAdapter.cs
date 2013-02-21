using System;
using System.Linq;
using System.Threading;

using Lightstreamer.DotNet.Client;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization;
using CIAPI.StreamingClient.Lightstreamer;

// ReSharper disable CheckNamespace
namespace CIAPI.StreamingClient
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// the purpose of this adapter is to allow consumer to maintain handlers
    /// for listener events even when the internal listener is reinstantiated due
    /// to fault tolerance events
    /// </summary>
    public sealed class ListenerAdapter<TDto> : IStreamingListener<TDto> where TDto : class, new()
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ListenerAdapter<TDto>));

        private readonly IJsonSerializer _serializer;
        private readonly string _adapterSet;
        private readonly string _channel;
        private readonly string _groupOrItemName;
        private readonly IFaultTolerantLsClientAdapter _lsClient;
        private readonly LightstreamerDtoConverter<TDto> _messageConverter;



        /// <summary>
        /// 
        /// </summary>
        public string AdapterSet
        {
            get
            {
                return _adapterSet;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Adapter
        {
            get
            {
                return _channel;
            }
        }
        private ITableListener<TDto> _listener;
        private SubscribedTableKey _subscribedTableKey;
        private string _mode;  //To ensure the last message published prior to this subscription is recieved immediately, mode must be MERGE and snap = true
        private bool _snapshot;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="mode"></param>
        /// <param name="snapshot"></param>
        /// <param name="client"></param>
        /// <param name="serializer"></param>
        public ListenerAdapter(string topic, string mode, bool snapshot, IFaultTolerantLsClientAdapter client, IJsonSerializer serializer)
        {
            _serializer = serializer;
            _messageConverter = new LightstreamerDtoConverter<TDto>(_serializer);

            Topic = topic;
            _lsClient = client;
            _adapterSet = client.AdapterSet;
            _channel = topic.Split('.').First();
            _groupOrItemName = topic.Replace(_channel + ".", "");
            _mode = mode;
            _snapshot = snapshot;
        }

        #region IStreamingListenerAdapter<TDto> Members

        void IStreamingListener.Start(int phase)
        {
            string groupOrItemName = _groupOrItemName.ToUpper();
            if (_listener != null)
            {
                _listener.MessageReceived -= ListenerMessageReceived;
                ((IStreamingListener)this).Stop();
            }

            string schema = _messageConverter.GetFieldList();
            string channel = _channel.ToUpper();

            _listener = new TableListener<TDto>(_adapterSet.ToUpper(), channel, groupOrItemName, phase, _serializer);
            _listener.MessageReceived += ListenerMessageReceived;


            Logger.Debug(string.Format("Subscribing to group:{0}, schema {1}, dataAdapter {2}, mode {3}, snapshot {4}", groupOrItemName, schema, channel, _mode.ToUpper(), _snapshot));

            var simpleTableInfo = new SimpleTableInfo(
                groupOrItemName,
                schema: schema,
                mode: _mode.ToUpper(), snap: _snapshot) 
                { DataAdapter = channel };
            var gate = new ManualResetEvent(false);
            Exception ex = null;
            new Thread(() =>
                           {
                               try
                               {
                                   _subscribedTableKey = _lsClient.SubscribeTable(simpleTableInfo, _listener, false);
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
                                   _lsClient.UnsubscribeTable(_subscribedTableKey);
                               }
                               catch (Exception ex)
                               {
                                   Logger.Warn(ex);
                               }
                           }) { Name = "Thread for " + message }.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MessageEventArgs<TDto>> MessageReceived;


        /// <summary>
        /// 
        /// </summary>
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
            if (handler != null)
            {
                handler(this, e);
            }
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // noop placeholder
            }
        }
    }
}