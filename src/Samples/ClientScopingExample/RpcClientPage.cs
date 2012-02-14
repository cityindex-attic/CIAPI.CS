using System;
using System.Web.UI;
using CIAPI.Rpc;

namespace ClientScopingExample
{
    public abstract class RpcClientPage : Page
    {
        private string _rpcStatus;

        protected bool LoggedIn
        {
            get { return !string.IsNullOrEmpty(SessionClient.Session); }
        }


        public string RpcStatus
        {
            get { return _rpcStatus; }
        }

        /// <summary>
        /// Use SessionClient for requests specific to each user.
        /// 
        /// </summary>
        public Client SessionClient
        {
            get
            {
                var sessionClient = (Client) Session["client"];
                if (sessionClient == null)
                {
                    sessionClient = new Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"));
                    Session["client"] = sessionClient;
                }

                return sessionClient;
            }
            set { Session["client"] = value; }
        }

        protected abstract void PageLoad(object sender, EventArgs e);

        protected void Page_Load(object sender, EventArgs e)
        {
            PageLoad(sender, e);
        }
    }
}