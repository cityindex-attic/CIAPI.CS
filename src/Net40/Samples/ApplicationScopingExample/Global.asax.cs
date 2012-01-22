using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using CIAPI.Rpc;
using Salient.ReflectiveLoggingAdapter;


namespace ApplicationScopingExample
{
    public class Global : System.Web.HttpApplication
    {
        private static Client _rpcClient;
        private static CapturingLoggerFactoryAdapter _adapter;
        public static CapturingLoggerFactoryAdapter adapter
        {
            get { return _adapter; }
            private set { _adapter = value; }
        }
        
        public static Client RpcClient
        {
            get { return _rpcClient; }
            private set { _rpcClient = value; }
        }

        protected void Application_Start(object sender, EventArgs e)
        {

            _adapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = _adapter;

            RpcClient = new Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"));
            RpcClient.LogIn("XX531040", "password");

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}