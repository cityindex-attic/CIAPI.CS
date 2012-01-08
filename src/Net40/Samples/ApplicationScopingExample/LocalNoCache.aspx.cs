using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIAPI.DTO;
using CIAPI.Rpc;
using Common.Logging;
using Common.Logging.Simple;

namespace ApplicationScopingExample
{
    public partial class LocalNoCache : System.Web.UI.Page
    {
        public Client _rpcClient;

        public Client RpcClient
        {
            get { return _rpcClient; }
            private set { _rpcClient = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            RpcClient = new Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"));
            RpcClient.LogIn("XX531040", "password");
        }

        private void ListLogItems()
        {
            ListBox2.Items.Clear();
            foreach (var evt in Global.adapter.LoggerEvents)
            {
                ListBox2.Items.Add(evt.RenderedMessage);
            }
        }
        private void ListNewsHeadlines()
        {
            ListBox1.Items.Clear();
            ListNewsHeadlinesResponseDTO results = RpcClient.News.ListNewsHeadlinesWithSource("DJ", "UK", 100);
            foreach (var headline in results.Headlines)
            {
                ListBox1.Items.Add(headline.Headline);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ListNewsHeadlines();
            ListLogItems();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            
            Global.adapter.Clear();
            ListLogItems();
        }
    }
}