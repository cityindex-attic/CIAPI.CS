using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using Lightstreamer.DotNet.Client;

// This is the class handling the Lightstreamer Client,
// the entry point for Lightstreamer update events.

namespace WindowsPhone7Demo
{
    public class LightstreamerClient
    {
        public string Username;
        public string SessionId;

        private string[] items;
        private string[] fields;



        private LSClient client;

        public LightstreamerClient(string[] items, string[] fields)
        {
            this.items = items;
            this.fields = fields;
            client = new LSClient();

        }

        public void Stop()
        {
            client.CloseConnection();
        }

        public void Start(string pushServerUrl, int phase, ILightstreamerListener listener)
        {
            StocklistConnectionListener ls = new StocklistConnectionListener(listener, phase);

            ConnectionInfo connInfo = new ConnectionInfo();
            connInfo.PushServerUrl = pushServerUrl;
            connInfo.Adapter = "CITYINDEXSTREAMING";
            connInfo.User = Username;
            connInfo.Password = SessionId;
            connInfo.Constraints.MaxBandwidth = 999999;
            client.OpenConnection(connInfo, ls);

        }

        public void Subscribe(int phase, ILightstreamerListener listener)
        {

            StocklistHandyTableListener hl = new StocklistHandyTableListener(listener, phase);


            var simpleTableInfo = new SimpleTableInfo("PRICE.400535967 81136 400509294 400535971 80902 400509295 400193864 400525367 80926 400498641 400193866 91047 400194551 121766 400172033 139144", mode: "RAW", schema: "AuditId Bid Change Direction High Low MarketId Offer Price TickDate", snap: false) { DataAdapter = "PRICES" };
            client.SubscribeTable(simpleTableInfo, hl, false);
        }



    }
}


