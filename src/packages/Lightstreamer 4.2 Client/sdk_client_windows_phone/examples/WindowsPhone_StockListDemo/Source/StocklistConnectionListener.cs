using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Lightstreamer.DotNet.Client;

namespace WindowsPhone7Demo
{
    class LightstreamerConnectionHandler
    {
        public const int DISCONNECTED = 0;
        public const int CONNECTING = 1;
        public const int CONNECTED = 2;
        public const int STREAMING = 3;
        public const int POLLING = 4;
        public const int STALLED = 5;
        public const int ERROR = 6;
    }

    class StocklistConnectionListener : IConnectionListener
    {

        private ILightstreamerListener listener = null;
        private bool isPolling;
        private bool reconnect = false;
        private int phase;

        public StocklistConnectionListener(ILightstreamerListener listener,int phase)
        {
            this.listener = listener;
            this.phase = phase;
        }

        public void AutomaticReconnect()
        {
           listener.OnReconnectRequest(phase);
        }

        public void OnConnectionEstablished()
        {
           listener.OnStatusChange(phase, LightstreamerConnectionHandler.CONNECTED,"Connected to Lightstreamer Server...");
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

        public void OnNewBytes(long b) { }

        public void OnDataError(PushServerException e)
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.ERROR,"Data error");
        }

        public void OnActivityWarning(bool warningOn)
        {
            if (warningOn)
            {
                listener.OnStatusChange(phase, LightstreamerConnectionHandler.STALLED, "Connection stalled");
            }
            else
            {
                OnSessionStarted(this.isPolling);
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
            listener.OnStatusChange(phase,LightstreamerConnectionHandler.DISCONNECTED,"Connection forcibly closed");
            reconnect = true;
        }

        public void OnFailure(PushServerException e)
        {
            listener.OnStatusChange(phase, LightstreamerConnectionHandler.DISCONNECTED,"Server failure" + e.ToString());
            reconnect = true;
        }

        public void OnFailure(PushConnException e)
        {
            listener.OnStatusChange(phase,LightstreamerConnectionHandler.DISCONNECTED, "Connection failure " + e.ToString());
            reconnect = true;
        }
    }
}