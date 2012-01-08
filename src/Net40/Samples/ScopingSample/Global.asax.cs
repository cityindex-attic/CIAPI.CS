using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.SessionState;
using CIAPI.Rpc;
using Common.Logging;
using Common.Logging.Simple;

namespace ScopingSample
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Maintains a list of logged events
        /// </summary>
        public static CapturingLoggerFactoryAdapter LogAdapter { get; private set; }

        /// <summary>
        /// This client, as a static member of this type, will persist in the AppDomain for all sessions and requests
        /// for the life of the app domain. This client would be useful for requests that are not client account specific
        /// and will take advantage of a persistent cache and throttle 
        /// </summary>
        public static Client RpcClient { get; private set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            LogAdapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = LogAdapter;
            RpcClient = new Client(new Uri(WebConfigurationManager.AppSettings["apiRpcUrl"]));
            RpcClient.LogIn(WebConfigurationManager.AppSettings["RpcUserName"], WebConfigurationManager.AppSettings["RpcPassword"]);
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