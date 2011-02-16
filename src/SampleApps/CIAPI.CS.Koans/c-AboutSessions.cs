using System;
using CIAPI.CS.Koans.KoanRunner;
using CityIndex.JsonClient;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.CS.Koans
{
    [KoanCategory]
    public class AboutSessions
    {
        private Client _rpcClient;

        [Koan]
        public void CreatingASession()
        {
            //Interaction with the API is done via a top level "client" object 
            //that holds details about your connection.
            
            //You need to initialise the client with a valid endpoint
            _rpcClient = new Rpc.Client(new Uri("http://enter.endpoint.server/TradingApi"));
            
            //And then create a session by creating a username & password
            //You can get test credentials by requesting them at {CIAPI.docs}/#test-credentials.htm
            _rpcClient.LogIn("your_user_name", "your_password");

            Assert.That(_rpcClient.SessionId != "", "after logging in, you should have a valid session");
        }

        [Koan]
        public void EveryRequestUsesYourSession()
        {
            //The rpcClient stores your current session details, and uses it to authenticate
            //every request.
            var headlines = _rpcClient.ListNewsHeadlines("UK", 10);
            Assert.That(headlines.Headlines.Length > 0, "you should have a set of headlines");

            //When your sessionId expires
            _rpcClient.SessionId = "{an-expired-session-token}";

            //Then future requests will fail.
            try
            {
                var headlines2 = _rpcClient.ListNewsHeadlines("AUS", 10);
                Assert.That(false, "the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                //TODO:  Check for a specific type of exception, rather than exception text?
                Assert.That(e.Message.Contains("???"), "The error message should be a (401) Unauthorized");
            }
        }

        [Koan]
        public void YouCanForceYourSessionToExpireByLoggingOut()
        {
            _rpcClient.LogIn("your_user_name", "password");
            var headlines = _rpcClient.ListNewsHeadlines("UK", 3);
            Assert.That(headlines.Headlines.Length > 0, "you should have a set of headlines");

            //Logging out force expires your session token on the server
            _rpcClient.LogOut();

            //So that future requests with your old token will fail.
            try
            {
                var headlines2 = _rpcClient.ListNewsHeadlines("AUS", 4);
                Assert.That(false, "the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                Assert.That(e.Message.Contains("???"), "The error message should be a (401) Unauthorized");
            }
        }

    }
}
