using System;
using System.Collections.Generic;
using System.Text;

using Lightstreamer.DotNet.Client;

namespace DotNetStockListDemo
{

    class StocklistHandyTableListener : IHandyTableListener {
        private int phase;
        private StocklistClient slClient;

        public StocklistHandyTableListener(StocklistClient slClient, int phase) {

            this.phase = phase;
            this.slClient = slClient;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            this.slClient.UpdateReceived(this.phase, itemPos, update);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
        }

        public void OnUnsubscrAll() {
        }
    }

}

