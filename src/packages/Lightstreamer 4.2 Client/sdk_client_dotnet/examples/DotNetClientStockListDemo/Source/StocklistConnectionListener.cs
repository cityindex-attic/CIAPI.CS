using System;
using System.Collections.Generic;
using System.Text;

using Lightstreamer.DotNet.Client;

namespace DotNetStockListDemo
{

    class StocklistConnectionListener : IConnectionListener {
        public const int VOID = -1;
        public const int DISCONNECTED = 0;
        public const int POLLING = 1;
        public const int STREAMING = 2;
        public const int STALLED = 3;

        private StocklistClient slClient;
        private int phase;

        public StocklistConnectionListener(
                StocklistClient slClient,
                int phase)
        {

            this.slClient = slClient;
            this.phase = phase;
        }

        public void OnConnectionEstablished() {
            this.slClient.StatusChanged(this.phase,VOID, "Connected to Lightstreamer Server..." );
        }

        public void OnSessionStarted(bool isPolling) {
            if (isPolling)
            {
                this.slClient.StatusChanged(this.phase, POLLING, "Lightstreamer is pushing (smart polling mode)...");
            }
            else
            {
                this.slClient.StatusChanged(this.phase, STREAMING, "Lightstreamer is pushing (streaming mode)...");
            }
            
        }

        public void OnNewBytes(long b) {}

        public void OnDataError(PushServerException e) {
            this.slClient.StatusChanged(this.phase, VOID, "Data error");
        }

        public void OnActivityWarning(bool warningOn) {
            if (warningOn) {
                this.slClient.StatusChanged(this.phase, STALLED, "Connection stalled");
            }
            else {
                this.slClient.StatusChanged(this.phase, STREAMING, "Lightstreamer is pushing (streaming mode)...");
            }
        }

        private void onDisconnection(String status) {
            this.slClient.StatusChanged(this.phase, DISCONNECTED, status);
            this.slClient.Start(this.phase);
        }

        public void OnClose() {
            this.onDisconnection("Connection closed");
        }

		public void OnEnd(int cause) {
            this.onDisconnection("Connection forcibly closed");
		}

        public void OnFailure(PushServerException e) {
            this.onDisconnection("Server failure");
        }

        public void OnFailure(PushConnException e) {
            this.onDisconnection("Connection failure");
        }
    }

}
