using System;
using CIAPI.DTO;
using CIAPI.Rpc;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;


namespace CIAPI.Tests.Rpc
{
    [TestFixture]
    public class TestServerApiContextTests
    {
        static TestServerApiContextTests()
        {
            LogManager.CreateInnerLogger =
                (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
 
        [Test]
        public void CanLogin()
        {
            // #TODO: get available socket code from cassinidev

            var server = new TestServer(5051, 1024);
            server.Start();

            
            
            server.ProcessRequest += (s, e) =>
                                         {
                                             var dto = new ApiLogOnResponseDTO()
                                                           {
                                                               AllowedAccountOperator = true,
                                                               PasswordChangeRequired = false,
                                                               Session =
                                                                   "86c6b0df-24d4-4b3f-b699-688626817599"
                                                           };
                                             string json = JsonConvert.SerializeObject(dto);
                                             e.Response = server.CreateRpcResponse(json);
                                         };
            try
            {

                var ctx = new Client(new Uri("http://localhost.:" + server.Port), new Uri("http://localhost.:" + server.Port), "foo");

                ctx.LogIn(Settings.RpcUserName, Settings.RpcPassword);

                Assert.IsNotNullOrEmpty(ctx.Session);

            }
            finally
            {
                server.Stop();
            }



            

        }


    }
}