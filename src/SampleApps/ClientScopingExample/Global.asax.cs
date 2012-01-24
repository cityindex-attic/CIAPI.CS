using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using CityIndex.JsonClient;
using CityIndex.ReflectiveLoggingAdapter;
using Client = CIAPI.Rpc.Client;

namespace ClientScopingExample
{
    public class Global : System.Web.HttpApplication
    {
        private static Client _rpcClient;
        private static string _rpcStatus;
        public static Client rpcClient
        {
            get
            {
                return _rpcClient;
            }
        }
        public static string rpcStatus
        {
            get
            {
                return _rpcStatus;
            }

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // set up a capturing logger so that we can output snapshots of activity upon page render
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                {
                    return new CapturingAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
                };
             
            try
            {
                _rpcClient = new Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"));
                var response = _rpcClient.LogIn("XX070608", "password");
                if (response.PasswordChangeRequired)
                {
                    // internal logic to notify admin of password change required
                }

            }
            catch (ApiException ex)
            {
                _rpcStatus = ex.ToString();

            }
            catch (Exception ex)
            {
                _rpcStatus = ex.ToString();

            }
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