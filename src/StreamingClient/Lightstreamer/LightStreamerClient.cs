using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Lightstreamer.DotNet.Client;

namespace StreamingClient.Lightstreamer
{
    public abstract class LightstreamerClient : IStreamingClient, IConnectionListener
    {
        private readonly string _sessionId;
        private readonly Uri _streamingUri;
        private readonly string _userName;
        private LSClient _internalClient;
        private Dictionary<string, IStreamingListener> _currentListeners = new Dictionary<string, IStreamingListener>();

        protected LightstreamerClient(Uri streamingUri, string userName, string sessionId)
        {
            _streamingUri = streamingUri;
            _sessionId = sessionId;
            _userName = userName;
        }

        public event EventHandler<MessageEventArgs<object>> MessageReceived;
        public event EventHandler<StatusEventArgs> StatusChanged;

        public void Connect()
        {
            _internalClient = new LSClient();

            var connectionInfo = new ConnectionInfo
                {
                    pushServerUrl = _streamingUri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.UriEscaped),
                    adapter = _streamingUri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped).TrimStart('/'),
                    user = _userName,
                    password = _sessionId,
                    constraints = {maxBandwidth = 999999}
                };

            _internalClient.OpenConnection(connectionInfo, this);
        }

        public void Disconnect()
        {
            _internalClient.CloseConnection();
        }

        public void OnStatusChanged(StatusEventArgs e)
        {
            if (StatusChanged != null) StatusChanged(this, e);
        }

        #region Implementation of IConnectionListener

        void IConnectionListener.OnConnectionEstablished()
        {
            OnStatusChanged(new StatusEventArgs {Status = " Connection established"});
        }

        void IConnectionListener.OnNewBytes(long bytes)
        {
            OnStatusChanged(new StatusEventArgs {Status = string.Format("{0} new bytes received", bytes)});
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
            OnStatusChanged(new StatusEventArgs {Status = string.Format("Activity warning: {0}", warningOn)});
        }

        void IConnectionListener.OnClose()
        {
            OnStatusChanged(new StatusEventArgs {Status = "Connection closed"});
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
            OnStatusChanged(new StatusEventArgs
                {Status = string.Format("Failure: {0}:{1}\r\n{2}\r\n{3}", e.GetType(), e.Message, e.Data, e.StackTrace)});
        }

        #endregion

        public IStreamingListener<TDto> BuildListener<TDto>(string topic) where TDto : class, new()
        {
            if (!_currentListeners.ContainsKey(topic))
            {
                _currentListeners.Add(topic, new LightstreamerListener<TDto>(topic, _internalClient));
            }
           
            return (IStreamingListener<TDto>) _currentListeners[topic];
        }

        

 

    }
}