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
            connInfo.Adapter = "DEMO";
            client.OpenConnection(connInfo, ls);

        }

        public void Subscribe(int phase, ILightstreamerListener listener) {

            StocklistHandyTableListener hl = new StocklistHandyTableListener(listener, phase);

            SimpleTableInfo tableInfo = new ExtendedTableInfo(
                items, "MERGE", fields, true);
            tableInfo.DataAdapter = "QUOTE_ADAPTER";
            client.SubscribeTable(tableInfo, hl, false);
        }



    }
}


