using System;
using System.Net;
using Lightstreamer.DotNet.Client;
using Common.Logging;

namespace TradingApi.Client.Core.Lightstreamer
{
    public interface ILightstreamerConnection
    {
        bool IsOpen { get; }
        LSClient LSClient { get; }
        string ServerUrl { get; }
        void Open();
        event EventHandler<StatusEventArgs> StatusChanged;
        void OnStatusChanged(StatusEventArgs e);
        void Close();
    }

    public class LightstreamerConnection: IConnectionListener, ILightstreamerConnection
    {
        private readonly string _username;
        private readonly string _session;
        private static ILog _log = LogManager.GetLogger(typeof(LightstreamerConnection));
        public string ServerUrl { get; private set; }
        public string Adapter { get; private set; }
        private readonly LSClient _lsClient = new LSClient();
        public LSClient LSClient
        {
            get { return _lsClient; }
        }

        public LightstreamerConnection(string serverUrl, string username, string session, string adapter)
        {
            _username = username;
            _session = session;
            Adapter = adapter;
            IsOpen = false;
            ServerUrl = serverUrl;

            _log.Debug("LightstreamerConnection .ctor");
        }

        public bool IsOpen { get; private set; }
        public virtual void Open()
        {
            var connectionInfo = new ConnectionInfo
                                     {
                                         pushServerUrl = ServerUrl,
                                         adapter = Adapter,
                                         user = _username,
                                         password = _session
                                     };
            connectionInfo.constraints.maxBandwidth = 999999;
            _lsClient.OpenConnection(connectionInfo, this);

            IsOpen = true;
        }

        public event EventHandler<StatusEventArgs> StatusChanged;

        public void OnStatusChanged(StatusEventArgs e)
        {
            if (StatusChanged != null) StatusChanged(this, e);
        }

        public virtual void Close()
        {
            _lsClient.CloseConnection();
            IsOpen = false;
        }


        #region Implementation of IConnectionListener

        public virtual void OnConnectionEstablished()
        {
            OnStatusChanged(new StatusEventArgs { Status = " Connection established" });
        }

        public virtual void OnSessionStarted()
        {
            OnStatusChanged(new StatusEventArgs { Status = "Session started" });
        }

        public virtual void OnNewBytes(long bytes)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("{0} new bytes recieved", bytes) });
        }

        public virtual void OnDataError(PushServerException e)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Data Error: {0}:{1}\r\n({2}){3}\r\n{4}", e.GetType(), e.Message, e.ErrorCode, e.Data, e.StackTrace) });
        }

        public virtual void OnActivityWarning(bool warningOn)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Activity warning: {0}", warningOn) });
        }

        public virtual void OnClose()
        {
            OnStatusChanged(new StatusEventArgs { Status = "Connection closed" });
        }

        public virtual void OnFailure(PushServerException e)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Failure: {0}:{1}\r\n({2}){3}\r\n{4}", e.GetType(), e.Message, e.ErrorCode, e.Data, e.StackTrace) });
        }

        public virtual void OnFailure(PushConnException e)
        {
            OnStatusChanged(new StatusEventArgs { Status = string.Format("Failure: {0}:{1}\r\n{2}\r\n{3}", e.GetType(), e.Message, e.Data, e.StackTrace) });
        }

        #endregion
    }
}
