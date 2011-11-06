using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Lightstreamer.DotNet.Client;
using StreamingClient.Lightstreamer;

namespace StreamingClient
{

    public class FaultTolerantLsClientAdapter : IDisposable, IConnectionListener
    {
        private static readonly Object ConnLock = new Object();

        private readonly string _adapterSet;
        public string AdapterSet
        {
            get
            {
                return _adapterSet;
            }
        }

        private readonly string _sessionId;
        private readonly string _userName;

        private readonly string _streamingUri;
        private bool _isPolling;
        private bool _reconnect;

        public bool Connected { get; private set; }

        private int _phase;
        private int _lastDelay = 1;

        internal readonly LSClient Client;
        private readonly Dictionary<string, IStreamingListener> _currentListeners = new Dictionary<string, IStreamingListener>();


        public FaultTolerantLsClientAdapter(string streamingUri, string userName, string sessionId, string adapterSet)
        {
#if !SILVERLIGHT
            //Ensure that at least another 2 concurrent HTTP connections are allowed by the desktop .NET framework
            //(it defaults to 2, which will already be used if there is another LSClient active)
            ServicePointManager.DefaultConnectionLimit += 2;
#endif
            _adapterSet = adapterSet;
            _streamingUri = streamingUri;
            _sessionId = sessionId;
            _userName = userName;
            Client = new LSClient();
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
            gate.WaitOne();
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
            lock (_currentListeners)
            {
                if (_currentListeners.ContainsValue(listener))
                {
                    _currentListeners.Remove(listener.Topic);
                }
                new Thread(listener.Stop).Start();
            }
        }

        public IStreamingListener<TDto> BuildListener<TDto>(string topic) where TDto : class, new()
        {

            if (!_currentListeners.ContainsKey(topic))
            {
                IStreamingListener listener = new ListenerAdapter<TDto>(topic, this);
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
        internal Boolean CheckPhase(int ph)
        {
            lock (ConnLock)
            {
                return ph == _phase;
            }
        }


        #region Methods accessed only by ClientStartStop

        private void OpenConnection(int ph)
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
                    Constraints = { MaxBandwidth = 999999 }
                };

                try
                {
                    Debug.WriteLine("connecting streaming client to adapter " + _adapterSet);
                    Client.OpenConnection(connection, this);
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


        private void CloseConnection(int ph)
        {
            if (!CheckPhase(ph))
            {
                return;
            }

            Client.CloseConnection();
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
            private readonly FaultTolerantLsClientAdapter _adapter;
            private readonly int _phase;

            public ClientStartStop(int ph, FaultTolerantLsClientAdapter adapter)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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