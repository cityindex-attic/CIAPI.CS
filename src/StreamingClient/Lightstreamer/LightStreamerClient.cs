using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Lightstreamer.DotNet.Client;

namespace StreamingClient.Lightstreamer
{

    public abstract class LightstreamerClient : IStreamingClient, IConnectionListener
    {
        private readonly string _sessionId;
        private readonly Uri _streamingUri;
        private readonly string _userName;

        private Dictionary<string, ClientData> _clients = new Dictionary<string, ClientData>();
        private Dictionary<string, IStreamingListener> _currentListeners = new Dictionary<string, IStreamingListener>();
        static LightstreamerClient()
        {
            LSClient.SetLoggerProvider(new LSLoggerProvider());
        }
        protected LightstreamerClient(Uri streamingUri, string userName, string sessionId)
        {
            
            _streamingUri = streamingUri;
            _sessionId = sessionId;
            _userName = userName;
        }

        public event EventHandler<MessageEventArgs<object>> MessageReceived;
        public event EventHandler<StatusEventArgs> StatusChanged;

        protected abstract string[] GetAdapterList();

        public void Connect()
        {
            var adapterList = GetAdapterList();

            foreach (string adapter in adapterList)
            {
                var connectionInfo = new ConnectionInfo
                {
                    PushServerUrl = _streamingUri.ToString().TrimEnd('/'),
                    Adapter = adapter,
                    User = _userName,
                    Password = _sessionId,
                    Constraints = { MaxBandwidth = 999999 }
                };
                var client = new LSClient();
                
                _clients.Add(adapter, new ClientData() { client = client, connection = connectionInfo, dataAdapter = adapter });
            }

        }

        public void Disconnect()
        {
            var adapterList = GetAdapterList();
            foreach (var item in _currentListeners)
            {
                try
                {
                    Debug.WriteLine("stopping listener on " + item.Key );
                    item.Value.Stop();
                    Debug.WriteLine("stopped listener on " + item.Key);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("error stopping listener on " + item.Key + "/n" + ex);
                }
            }
            foreach (string adapter in adapterList)
            {
                try
                {
                    Debug.WriteLine("disconnecting client on adapter " + adapter);
                    _clients[adapter].client.CloseConnection();
                    Debug.WriteLine("disconnected client on adapter " + adapter);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("error disconnecting client on " + adapter + "/n" + ex);
                }
            }
            _clients.Clear();
        }

        public void OnStatusChanged(StatusEventArgs e)
        {
            if (StatusChanged != null) StatusChanged(this, e);
        }

        #region Implementation of IConnectionListener

        void IConnectionListener.OnConnectionEstablished()
        {
            OnStatusChanged(new StatusEventArgs { Status = " Connection established" });
        }

        void IConnectionListener.OnNewBytes(long bytes)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("{0} new bytes received", bytes) });
        }

        void IConnectionListener.OnSessionStarted(bool isPolling)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Session started (isPolling: {0})", isPolling) });
        }

        void IConnectionListener.OnDataError(PushServerException e)
        {
            OnStatusChanged(new StatusEventArgs
                {
                    Status =
                                string.Format("Data Error: {0}:{1}\r\n({2}){3}\r\n{4}", e.GetType(), e.Message,
                                              e.ErrorCode, e.Data, e.StackTrace)
                });
        }

        void IConnectionListener.OnEnd(int cause)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Connection ended: cause {0}", cause) });
        }

        void IConnectionListener.OnActivityWarning(bool warningOn)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Activity warning: {0}", warningOn) });
        }

        void IConnectionListener.OnClose()
        {
            OnStatusChanged(new StatusEventArgs { Status = "Connection closed" });
        }

        void IConnectionListener.OnFailure(PushServerException e)
        {
            OnStatusChanged(new StatusEventArgs
                {
                    Status =
                                string.Format("Failure: {0}:{1}\r\n({2}){3}\r\n{4}", e.GetType(), e.Message, e.ErrorCode,
                                              e.Data, e.StackTrace)
                });
        }

        void IConnectionListener.OnFailure(PushConnException e)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Failure: {0}:{1}\r\n{2}\r\n{3}", e.GetType(), e.Message, e.Data, e.StackTrace) });
        }

        #endregion

        public IStreamingListener<TDto> BuildListener<TDto>(string adapter, string topic/*, Regex topicMask*/) where TDto : class, new()
        {
            if (!_currentListeners.ContainsKey(topic))
            {
                var listener = new LightstreamerListener<TDto>(topic, _clients[adapter].client);
                _currentListeners.Add(topic, listener);
            }


            try
            {
                var clientInfo = _clients[adapter];
                if (!clientInfo.connected)
                {
                    Debug.WriteLine("connecting streaming client to adapter " + clientInfo.dataAdapter);
                    clientInfo.client.OpenConnection(clientInfo.connection, this);
                    Debug.WriteLine("connected streaming client to adapter " + clientInfo.dataAdapter);
                    clientInfo.connected = true;
                }
                
            }
            catch (PushUserException ex)
            {
                if (ex.Message == "Requested Adapter Set not available")
                {
                    throw new Exception(string.Format("Data adapter {0} is not available", adapter), ex);
                }
                throw;
            }

            return (IStreamingListener<TDto>)_currentListeners[topic];
        }
    }
}