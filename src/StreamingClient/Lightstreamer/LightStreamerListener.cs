using System;
using System.Linq;
using System.Threading;
using Common.Logging;
using Lightstreamer.DotNet.Client;


namespace StreamingClient.Lightstreamer
{
    public class LightstreamerListener<TDto> : IStreamingListener<TDto>, IHandyTableListener
        where TDto : class, new()
    {
 


        private readonly LightstreamerDtoConverter<TDto> _messageConverter = new LightstreamerDtoConverter<TDto>();
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LightstreamerListener<TDto>));
        private readonly string _groupOrItemName;
        private readonly string _fullTopic;
        private readonly LSClient _lsClient;
        private SubscribedTableKey _subscribedTableKey;
        private readonly string _dataAdapter;

        public LightstreamerListener(string fullTopic, LSClient lsClient)
        {
            _fullTopic = fullTopic;
            _lsClient = lsClient;
            _dataAdapter = fullTopic.Split('.').First();
            _groupOrItemName = fullTopic.Replace(_dataAdapter + ".", "");
        }

        public event EventHandler<MessageEventArgs<TDto>> MessageReceived;

        /// <summary>
        /// Start listening.  This is syncronous, and blocks until the server subscription has started
        /// </summary>
        public void Start()
        {
#if SILVERLIGHT
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                throw new Exception("You cannot call this method from the UI thread.  Call this from a background thread");
            }
#endif
            var groupOrItemName = _groupOrItemName.ToUpper();
            var schema = _messageConverter.GetFieldList();
            var dataAdapter = _dataAdapter.ToUpper();
            _logger.DebugFormat("Subscribing to group:{0}, schema {1}, dataAdapter {2}", groupOrItemName, schema, dataAdapter);
            var simpleTableInfo = new SimpleTableInfo(
                groupOrItemName,
                mode: "RAW",
                schema: schema,
                snap: false) { DataAdapter = dataAdapter };
            _subscribedTableKey = _lsClient.SubscribeTable(simpleTableInfo, this, false);
            _logger.DebugFormat("Subscribed to table with key: {0}", _subscribedTableKey);
        }

        /// <summary>
        /// Stop listening.  This is syncronous, and blocks until the server has been informed of the unsubscription; or the timeout is reached
        /// </summary>
        public void Stop()
        {
#if SILVERLIGHT
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                throw new Exception("You cannot call this method from the UI thread.  Call this from a background thread");
            }
#endif
            if (_subscribedTableKey == null) return;

            var message = String.Format("Unsubscribing from table with key: {0}", _subscribedTableKey);
            _logger.DebugFormat(message);


            // DAVID: apparently putting the waithandle in a using is causing part 2
            // FIXED: OjbectDisposedException:  Part two occurs due to the waithandle being in a using. something about this particular
            // implementation of the pattern disposes of the gate before set is called.

            var gate = new ManualResetEvent(false); 
            // don't worry about disposing this after we are done. since a using is breaking everything, any attempts
            // of cleaning it up are going to run into the same problem. let GC take care of it.


            new Thread(() =>
                           {
                               try
                               {
                                   _lsClient.UnsubscribeTable(_subscribedTableKey);
                               }
                               catch (Exception ex)
                               {
                                   // FIXME: this is part one of the SL hang issue. -- right now, a session not found (SYNC ERROR) is being thrown up and since you 
                                   // are in a spawned thread you get a zombie. The exception appears to be valid as when the rpc client tries to log out
                                   // it gets back a LoggedOut: false respose.

                                   // break and step through PushServerTranslator.DoControlRequest - > PushServerTranslator.CheckAnswer AFTER it has started streaming
                                   // to view this behavior
                               }
                               gate.Set();
                           }) {Name = "Thread for " + message}
                .Start();
            if (!gate.WaitOne(LightstreamerDefaults.DEFAULT_TIMEOUT_MS + 1000))
            {
                _logger.WarnFormat(string.Format("Giving up after {0}ms attempting to stop listener: {1}." +
                                                 "Client has stopped listening, but there might be a zombie process left on the server",
                                                 LightstreamerDefaults.DEFAULT_TIMEOUT_MS, GetType().Name));
            }
        }

        public string Topic
        {
            get { return _fullTopic; }
        }

        void IHandyTableListener.OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            try
            {
                if (MessageReceived == null) return;

                MessageReceived(this, new MessageEventArgs<TDto>(_groupOrItemName, _messageConverter.Convert(update)));
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