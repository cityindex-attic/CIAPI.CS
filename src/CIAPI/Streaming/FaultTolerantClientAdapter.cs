using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Lightstreamer.DotNet.Client;
using Salient.ReliableHttpClient.Serialization;
using CIAPI.StreamingClient.Lightstreamer;

// ReSharper disable CheckNamespace
namespace CIAPI.StreamingClient
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFaultTolerantLsClientAdapter : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        string AdapterSet { get; }
        /// <summary>
        /// 
        /// </summary>
        bool Connected { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ph"></param>
        /// <returns></returns>
        Boolean CheckPhase(int ph);
        ///<summary>
        ///</summary>
        int ListenerCount { get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ConnectionStatusEventArgs> StatusUpdate;

        /// <summary>
        /// Allows consumer to stop and remove a listener from this client.
        /// </summary>
        void TearDownListener(IStreamingListener listener);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="mode"></param>
        /// <param name="snapshot"></param>
        /// <typeparam name="TDto"></typeparam>
        /// <returns></returns>
        IStreamingListener<TDto> BuildListener<TDto>(string topic, string mode, bool snapshot) where TDto : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simpleTableInfo"></param>
        /// <param name="listener"></param>
        /// <param name="b"></param>
        /// <typeparam name="TDto"></typeparam>
        /// <returns></returns>
        SubscribedTableKey SubscribeTable<TDto>(SimpleTableInfo simpleTableInfo, ITableListener<TDto> listener, bool b) where TDto : class, new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscribedTableKey"></param>
        void UnsubscribeTable(SubscribedTableKey subscribedTableKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phase"></param>
        void OpenConnection(int phase);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phase"></param>
        void CloseConnection(int phase);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class FaultTolerantLsClientAdapter : IConnectionListener, IFaultTolerantLsClientAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="simpleTableInfo"></param>
        /// <param name="listener"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public SubscribedTableKey SubscribeTable<TDto>(SimpleTableInfo simpleTableInfo, ITableListener<TDto> listener, bool b) where TDto : class, new()
        {

            SubscribedTableKey key = _client.SubscribeTable(simpleTableInfo, listener, b);
            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscribedTableKey"></param>
        public void UnsubscribeTable(SubscribedTableKey subscribedTableKey)
        {
            _client.UnsubscribeTable(subscribedTableKey);
        }


        private readonly IJsonSerializer _serializer;

        private static readonly Object ConnLock = new Object();

        private readonly string _adapterSet;
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

        private readonly string _sessionId;
        private readonly string _userName;
        private bool _usePolling = false;
        private readonly string _streamingUri;
        private bool _isPolling;
        private bool _reconnect;

        /// <summary>
        /// 
        /// </summary>
        public bool Connected { get; private set; }

        private int _phase;
        private int _lastDelay = 1;

        private readonly LSClient _client;
        private readonly Dictionary<string, IStreamingListener> _currentListeners = new Dictionary<string, IStreamingListener>();
        ///<summary>
        ///</summary>
        public int ListenerCount
        {
            get
            {
                return _currentListeners.Count;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="sessionId"></param>
        /// <param name="adapterSet"></param>
        /// <param name="usePolling"></param>
        /// <param name="serializer"></param>
        public FaultTolerantLsClientAdapter(string streamingUri, string userName, string sessionId, string adapterSet, bool usePolling, IJsonSerializer serializer)
        {
#if !SILVERLIGHT
            //Ensure that at least another 2 concurrent HTTP connections are allowed by the desktop .NET framework
            //(it defaults to 2, which will already be used if there is another LSClient active)
            ServicePointManager.DefaultConnectionLimit += 2;
#endif
            _usePolling = usePolling;
            _serializer = serializer;
            _adapterSet = adapterSet;
            _streamingUri = streamingUri;
            _sessionId = sessionId;
            _userName = userName;
            _client = new LSClient();

        }


        internal void Start()
        {
            ClientStartStop execute;
            lock (ConnLock)
            {
                _phase++;
                execute = new ClientStartStop(_phase, this);
            }

            Debug.WriteLine("About to Start Lightstreamer Client");
            var gate = new ManualResetEvent(false);
            Exception ex = null;
            new Thread(() =>
                           {
                               try
                               {
                                   execute.DoStart();
                               }
                               catch (Exception ex1)
                               {

                                   ex = ex1;
                               }
                               gate.Set();
                           })
            {
                Name = "LightStreamerStartThread"
            }.Start();
            if(!gate.WaitOne(LightstreamerDefaults.DEFAULT_TIMEOUT_MS+1000))
            {
                throw new Exception("Timeout starting lightstreamer thread");
            }
            if (ex != null)
            {
                throw ex;
            }
        }


        internal void Stop()
        {
            ClientStartStop execute;

            lock (ConnLock)
            {
                _phase++;
                execute = new ClientStartStop(_phase, this);
            }

            Debug.WriteLine("About to Stop Lightstreamer Client");
            var gate = new ManualResetEvent(false);
            var th = new Thread(() =>
                                    {
                                        execute.DoStop();
                                        gate.Set();
                                    })
            {
                Name = "LightStreamerStopThread"
            };
            th.Start();
            gate.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ConnectionStatusEventArgs> StatusUpdate;

        #region IConnectionListener Members

        void IConnectionListener.OnConnectionEstablished()
        {
            OnStatusUpdate(_phase, ConnectionStatus.Connected, "Connected to Lightstreamer Server...");
        }

        void IConnectionListener.OnSessionStarted(bool isPolling)
        {
            string message;
            ConnectionStatus status;
            this._isPolling = isPolling;
            message = "Lightstreamer is pushing...";
            if (isPolling)
            {
                status = ConnectionStatus.Polling;
            }
            else
            {
                status = ConnectionStatus.Streaming;
            }

            OnStatusUpdate(_phase, status, message);
        }

        void IConnectionListener.OnNewBytes(long bytes)
        {
            // noop
        }

        void IConnectionListener.OnDataError(PushServerException e)
        {
            OnStatusUpdate(_phase, ConnectionStatus.Error, "Data error");
        }

        void IConnectionListener.OnActivityWarning(bool warningOn)
        {
            if (warningOn)
            {
                OnStatusUpdate(_phase, ConnectionStatus.Stalled, "Connection stalled");
            }
            else
            {
                ((IConnectionListener)this).OnSessionStarted(_isPolling);
            }
        }


        void IConnectionListener.OnClose()
        {
            OnStatusUpdate(_phase, ConnectionStatus.Disconnected, "Connection closed");
            if (_reconnect)
            {
                if (!CheckPhase(_phase))
                {
                    return;
                }
                Debug.WriteLine("Reconnecting....");
                Start();
                _reconnect = false;
            }
        }

        void IConnectionListener.OnEnd(int cause)
        {
            OnStatusUpdate(_phase, ConnectionStatus.Disconnected, "Connection forcibly closed");
            _reconnect = true;
        }

        void IConnectionListener.OnFailure(PushServerException e)
        {
            OnStatusUpdate(_phase, ConnectionStatus.Disconnected, "Server failure" + e);
            _reconnect = true;
        }

        void IConnectionListener.OnFailure(PushConnException e)
        {
            OnStatusUpdate(_phase, ConnectionStatus.Disconnected, "Connection failure " + e);
            _reconnect = true;
        }

        #endregion

        /// <summary>
        /// Allows consumer to stop and remove a listener from this client.
        /// </summary>
        public void TearDownListener(IStreamingListener listener)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            lock (_currentListeners)
            {
                if (_currentListeners.ContainsValue(listener))
                {
                    _currentListeners.Remove(listener.Topic);
                }
                new Thread(listener.Stop).Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="mode"></param>
        /// <param name="snapshot"></param>
        /// <typeparam name="TDto"></typeparam>
        /// <returns></returns>
        public IStreamingListener<TDto> BuildListener<TDto>(string topic, string mode, bool snapshot) where TDto : class, new()
        {

            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (!_currentListeners.ContainsKey(topic))
            {
                IStreamingListener listener = new ListenerAdapter<TDto>(topic, mode, snapshot, this, _serializer);
                _currentListeners.Add(topic, listener);
                new Thread(() => listener.Start(_phase)).Start();
            }

            return (IStreamingListener<TDto>)_currentListeners[topic];
        }


        private void OnStatusUpdate(int ph, ConnectionStatus status, string message)
        {
            if (!CheckPhase(ph))
            {
                return;
            }

            EventHandler<ConnectionStatusEventArgs> handler = StatusUpdate;

            if (handler != null)
            {
                var args = new ConnectionStatusEventArgs(message, status);
                handler(this, args);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="ee"></param>
        private void PauseAndRetryStartClient(int ph, Exception ee)
        {

            // #TODO: push to logger
            Debug.WriteLine("Lightstreamer Client, unable to start: " + ee);

            _lastDelay *= 2;
            // Probably a connection issue, ask myself to respawn
            for (int i = _lastDelay; i > 0; i--)
            {
                if (!CheckPhase(ph))
                {
                    return;
                }

                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    OnStatusUpdate(ph, ConnectionStatus.Connecting,
                                   "Network unavailble, next check in " + i + " seconds");
                }
                else
                {
                    OnStatusUpdate(ph, ConnectionStatus.Connecting, "Connection failed, retrying in " + i + " seconds");
                }

                Thread.Sleep(1000);
            }


            Debug.WriteLine("Trying to respawn Lightstreamer Client");

            if (!CheckPhase(_phase))
            {
                return;
            }

            Debug.WriteLine("OnReconnectRequest called");

            Start();


        }

        /// <summary>
        /// helps insure that operations are only performed on active connection/listener
        /// </summary>
        /// <param name="ph"></param>
        /// <returns></returns>
        public Boolean CheckPhase(int ph)
        {
            lock (ConnLock)
            {
                return ph == _phase;
            }
        }


        #region Methods accessed only by ClientStartStop

        void IFaultTolerantLsClientAdapter.OpenConnection(int ph)
        {

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                PauseAndRetryStartClient(ph, null);
                return;
            }

            try
            {
                if (!CheckPhase(ph))
                {
                    return;
                }

                OnStatusUpdate(ph, ConnectionStatus.Connecting, "Connecting to " + _streamingUri);


                var connection = new ConnectionInfo
                {
                    PushServerUrl = _streamingUri.TrimEnd('/'),
                    Adapter = _adapterSet,
                    User = _userName,
                    Password = _sessionId,
                    Constraints = { MaxBandwidth = 999999 },
                    Polling = _usePolling //,PollingIdleMillis = 250,PollingMillis = 250
                };

                try
                {
                    //#TODO find out why it is still trying to connect after shutdown
                    Debug.WriteLine("connecting streaming client to adapter " + _adapterSet);
                    _client.OpenConnection(connection, this);
                    Debug.WriteLine("connected streaming client to adapter " + _adapterSet);
                    Connected = true;

                }
                catch (PushUserException ex)
                {
                    if (ex.Message == "Requested Adapter Set not available")
                    {
                        throw new Exception(string.Format("Adapter set {0} is not available", _adapterSet), ex);
                    }
                    throw;
                }

                // rebuild up any existing listeners
                foreach (KeyValuePair<string, IStreamingListener> kvp in _currentListeners)
                {
                    kvp.Value.Start(_phase);
                }

                Connected = true;
                Debug.WriteLine("Lightstreamer Client Started");
                _lastDelay = 1;

            }

            catch (PushConnException pce)
            {
                PauseAndRetryStartClient(ph, pce);
            }
            catch (SubscrException se)
            {
                PauseAndRetryStartClient(ph, se);
            }


        }


        void IFaultTolerantLsClientAdapter.CloseConnection(int ph)
        {
            if (!CheckPhase(ph))
            {
                return;
            }

            _client.CloseConnection();
            Connected = false;
            OnStatusUpdate(ph, ConnectionStatus.Disconnected, "Disconnected");
            Debug.WriteLine("Lightstreamer Client Stopped");
        }

        #endregion

        /// <summary>
        /// nested class to provide access to private members
        /// </summary>
        private class ClientStartStop
        {
            private readonly IFaultTolerantLsClientAdapter _adapter;
            private readonly int _phase;

            public ClientStartStop(int ph, IFaultTolerantLsClientAdapter adapter)
            {
                _adapter = adapter;
                _phase = ph;
            }

            public void DoStart()
            {
                _adapter.OpenConnection(_phase);
            }

            public void DoStop()
            {
                _adapter.CloseConnection(_phase);
            }
        }

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _disposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
#if !SILVERLIGHT
                // relinquish allocated connection limits.
                ServicePointManager.DefaultConnectionLimit -= 2;
#endif
            }
        }
    }
}