using Lightstreamer.DotNet.Client;

namespace WindowsPhone7Demo
{
    internal class LightstreamerConnectionHandler
    {
        public const int DISCONNECTED = 0;
        public const int CONNECTING = 1;
        public const int CONNECTED = 2;
        public const int STREAMING = 3;
        public const int POLLING = 4;
        public const int STALLED = 5;
        public const int ERROR = 6;
    }

    internal class StocklistConnectionListener : IConnectionListener
    {
        private readonly ILightstreamerListener listener;
        private readonly int phase;
        private bool isPolling;
        private bool reconnect;

        public StocklistConnectionListener(ILightstreamerListener listener, int phase)
        {
            this.listener = listener;
            this.phase = phase;
        }

        #region IConnectionListener Members

        public void OnConnectionEstablished()
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.CONNECTED, "Connected to Lightstreamer Server...");
        }

        public void OnSessionStarted(bool isPolling)
        {
            string message;
            int status;
            this.isPolling = isPolling;
            message = "Lightstreamer is pushing...";
            if (isPolling)
            {
                status = LightstreamerConnectionHandler.POLLING;
            }
            else
            {
                status = LightstreamerConnectionHandler.STREAMING;
            }

            listener.OnStatusChange(phase, status, message);
        }

        public void OnNewBytes(long b)
        {
        }

        public void OnDataError(PushServerException e)
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.ERROR, "Data error");
        }

        public void OnActivityWarning(bool warningOn)
        {
            if (warningOn)
            {
                listener.OnStatusChange(phase, LightstreamerConnectionHandler.STALLED, "Connection stalled");
            }
            else
            {
                OnSessionStarted(isPolling);
            }
        }

        public void OnClose()
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.DISCONNECTED, "Connection closed");
            if (reconnect)
            {
                AutomaticReconnect();
                reconnect = false;
            }
        }

        public void OnEnd(int cause)
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.DISCONNECTED, "Connection forcibly closed");
            reconnect = true;
        }

        public void OnFailure(PushServerException e)
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.DISCONNECTED, "Server failure" + e);
            reconnect = true;
        }

        public void OnFailure(PushConnException e)
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.DISCONNECTED, "Connection failure " + e);
            reconnect = true;
        }

        #endregion

        public void AutomaticReconnect()
        {
            listener.OnReconnectRequest(phase);
        }
    }
}