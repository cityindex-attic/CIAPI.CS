using System;
using CIAPI.CS.Koans.KoanRunner;
using CIAPI.Rpc;

namespace CIAPI.CS.Koans
{
    [KoanCategory]
    public class AboutSessions
    {
        private Client _rpcClient;

        [Koan]
        public void CreatingAClient()
        {
            //Interaction with the API is done via a top level "client" object 
            //that holds details about your connection.
            
            //See TODO for details of the available endpoints
            _rpcClient = new Rpc.Client(new Uri("http://enter.endpoint.server/TradingApi"));
            _rpcClient.ListNewsHeadlines("UK", 10);
        }

        [Koan]
        public void CreatingASession()
        {
            //To interact with any of the API methods; you need to provide your 
            //username & password to obtain a session
            //You can get test credentials by requesting them at {CIAPI.docs}/#test-credentials.htm
            _rpcClient.LogIn("your_user_name", "your_password");

            Assert.That(_rpcClient.SessionId != "", "after logging in, you should have a valid session");
        }

    }
}
