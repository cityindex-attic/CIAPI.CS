using System;
using System.Collections.Specialized;
using System.IO;
using CIAPI.DTO;
using CIAPI.Rpc;
using FiddlerSessionParser;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;

namespace CIAPI.Tests.TestingInfrastructure.Fiddler
{
    [TestFixture]
    public class FiddlerRequestEngineFixture
    {
        protected const string AppKey = "testkey-for-CIAPI.IntegrationTests";

        static FiddlerRequestEngineFixture()
        {
            LogManager.CreateInnerLogger =
                (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

        }

        [Test]
        public void CanUseFiddlerSessionsToServeRpcCalls()
        {
            var parser = new Parser();

            // parse saved sessions into collection of session objects
            string path = @"..\..\..\TestingInfrastructure\Fiddler\5_Full.txt";
            var sessions = parser.ParseFile(path);


            var engine = new FiddlerRequestEngine(sessions);


            var server = new TestServer(true);

            string recordedUrl = "https://ciapi.cityindex.com/tradingapi";

         


            server.ProcessRequest += (s, e) =>
                                         {
                                             string requestMethod = e.Request.Method;
                                             string requestUrl = recordedUrl + e.Request.Url;

                                             var session = engine.FindSession(requestMethod, requestUrl);

                                             if (session == null)
                                             {
                                                 

                                                 e.Response = new ServerBase.ResponseInfo
                                                 {
                                                     Status = "404 Not Found"
                                                 };
                                             }
                                             else
                                             {
                                                e.Response = new ServerBase.ResponseInfo
                                                              {
                                                                  Headers = new NameValueCollection(session.Response.Headers),
                                                                  Status = session.Response.StatusCode + " " + session.Response.Status,
                                                                  Body = session.Response.Body
                                                              };    
                                                 }
                                            
                                         };


            server.Start();




            var rpcClient = new Client(new Uri("http://localhost.:" + server.Port), new Uri("http://localhost.:" + server.Port), AppKey);

            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
            rpcClient.LogOut();
            rpcClient.Dispose();

            server.Stop();

        }
    }
}