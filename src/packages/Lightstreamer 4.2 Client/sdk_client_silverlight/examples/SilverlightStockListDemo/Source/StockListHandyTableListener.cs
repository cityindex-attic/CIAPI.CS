using System;
using System.Windows.Controls;
using System.Threading;

using Lightstreamer.DotNet.Client;
using System.Collections.Generic;

namespace SilverlightDemo
{

    class StocklistHandyTableListener : IHandyTableListener
    {
        private Page page;
        private int phase;
        private List<Stock> gridModel;

        public StocklistHandyTableListener(Page page, int phase, List<Stock> gridModel)
        {
            this.phase = phase;
            this.page = page;
            this.gridModel = gridModel;
        }

        public void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            Stock stock = gridModel[itemPos - 1];
            stock.StockName = update.GetNewValue("stock_name");
            stock.LastPrice = update.GetNewValue("last_price");
            stock.Time = update.GetNewValue("time");
            stock.PctChange = update.GetNewValue("pct_change");
            stock.BidQuantity = update.GetNewValue("bid_quantity");
            stock.Bid = update.GetNewValue("bid");
            stock.Ask = update.GetNewValue("ask");
            stock.AskQuantity = update.GetNewValue("ask_quantity");
            stock.Min = update.GetNewValue("min");
            stock.Max = update.GetNewValue("max");
            stock.RefPrice = update.GetNewValue("ref_price");
            stock.OpenPrice = update.GetNewValue("open_price");

            this.page.setData(this.phase, stock);
        }

        public void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            this.page.StatusChanged(this.phase,"Lost " + lostUpdates + " updates for " + itemPos,null);
        }

        public void OnSnapshotEnd(int itemPos, string itemName)
        {
            this.page.StatusChanged(this.phase, "End of snapshot for " + itemPos, null);
        }

        public void OnUnsubscr(int itemPos, string itemName)
        {
        }

        public void OnUnsubscrAll()
        {
            this.page.StatusChanged(this.phase,  "Unsubscr table", null);
        }
    }

}

