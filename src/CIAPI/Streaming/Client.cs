using System;
using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming
{
    public class Client : IStreamingClient, IConnectionListener
    {
        private readonly string _sessionId;
        private readonly Uri _streamingUri;
        private readonly string _userName;
        private LSClient _internalClient;

        public Client(Uri streamingUri, string userName, string sessionId)
        {
            _streamingUri = streamingUri;
            _sessionId = sessionId;
            _userName = userName;
        }

        #region IStreamingClient Members

        public event EventHandler<MessageEventArgs<string>> MessageRecieved;

        #endregion

        public event EventHandler<StatusEventArgs> StatusChanged;

        public void Connect()
        {
            _internalClient = new LSClient();

            var connectionInfo = new ConnectionInfo
                {
                    pushServerUrl = _streamingUri.GetLeftPart(UriPartial.Authority),
                    adapter = _streamingUri.PathAndQuery.TrimStart('/'),
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

        public IStreamingListener<NewsDTO> BuildNew(string newsTopic)
        {
            return new StreamingNewsListener(newsTopic, _internalClient);
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

        void IConnectionListener.OnSessionStarted()
        {
            OnStatusChanged(new StatusEventArgs {Status = "Session started"});
        }

        void IConnectionListener.OnNewBytes(long bytes)
        {
            OnStatusChanged(new StatusEventArgs {Status = string.Format("{0} new bytes recieved", bytes)});
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
    }
}