using System;
using System.Web;
using CIAPI.DTO;

using Salient.ReflectiveLoggingAdapter;
using Client = CIAPI.Rpc.Client;

namespace ClientScopingExample
{
    public class Global : HttpApplication
    {
        private static string _rpcStatus;
        private const string AppKey = "testkey-for-ClientScopingExample";

        /// <summary>
        /// Use the GlobalRpcClient to perform requests on behalf of ALL users taking advantage of the cache and throttle.
        /// </summary>
        public static Client GlobalRpcClient { get; private set; }

        public static string GlobalRpcStatus
        {
            get { return _rpcStatus; }
        }

        

        protected void Application_Start(object sender, EventArgs e)
        {
            // set up a capturing logger so that we can output snapshots of activity upon page render
            LogManager.CreateInnerLogger =
                (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                    {
                        return new CapturingAppender(logName, logLevel, showLevel, showDateTime, showLogName,
                                                     dateTimeFormat);
                    };
            try
            {
                GlobalRpcClient = new Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"), AppKey);
                ApiLogOnResponseDTO response = GlobalRpcClient.LogIn("XX070608", "password");
                if (response.PasswordChangeRequired)
                {
                    _rpcStatus = "admin must change password for global account";
                }
                else
                {
                    _rpcStatus = "OK";
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