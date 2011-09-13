using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Lightstreamer.DotNet.Client;

namespace WindowsPhone7Demo
{
    class StocklistHandyTableListener : IHandyTableListener
    {
        private ILightstreamerListener listener = null;
        private const int lockt = 15000;
        private int phase;

        public StocklistHandyTableListener(ILightstreamerListener listener, int phase)
        {
            this.listener = listener;
            this.phase = phase;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            //listener.OnItemUpdate(phase, itemPos, itemName, update);
            Debug.WriteLine(update.ToString());
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            //listener.OnLostUpdate(phase, itemPos, itemName, lostUpdates);
            Debug.WriteLine(lostUpdates.ToString());
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
        }

        public void OnUnsubscrAll()
        {
        }
    }
}