using System;
using CIAPI.CS.Koans.KoanRunner;
using Salient.JsonClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.CS.Koans
{
    [KoanCategory(Order = 2)]
    public class AboutSessions
    {
        private Client _rpcClient;
        private string USERNAME;
        private string PASSWORD;

        [Koan(Order = 1)]
        public void CreatingASession()
        {
            //Interaction with the API is done via a top level "client" object 
            //that holds details about your connection.
            
            //You need to initialise the client with a valid endpoint
            _rpcClient = new Rpc.Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"));
            
            //And then create a session by creating a username & password
            //You can get test credentials by requesting them at https://ciapipreprod.cityindextest9.co.uk/CIAPI.docs/#content.test-credentials
            USERNAME = "xx189949";
            PASSWORD = "password";

            try
            {
                _rpcClient.LogIn(USERNAME, PASSWORD);
            }
            catch (ApiException apiException)
            {
                KoanAssert.Fail(string.Format("cannot login because {0}", apiException.Message));
            }

            KoanAssert.That(_rpcClient.Session != "", "after logging in, you should have a valid session");
        }

        [Koan(Order = 2)]
        public void EveryRequestUsesYourSession()
        {
            //The rpcClient stores your current session details, and uses it to authenticate
            //every request.
            var headlines = _rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 10);
            KoanAssert.That(headlines.Headlines.Length > 0, "you should have a set of headlines");

            //When your sessionId expires
            _rpcClient.Session = "{an-expired-session-token}";

            //Then future requests will fail.
            try
            {
                var headlines2 = _rpcClient.News.ListNewsHeadlinesWithSource("dj", "AUS", 10);
                KoanAssert.That(false, "the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                KoanAssert.That(e.Message, Is.StringContaining("(401) Unauthorized"), "The error message should contain something about (401) Unauthorized");
            }
        }

        [Koan(Order = 3)]
        public void YouCanForceYourSessionToExpireByLoggingOut()
        {
            _rpcClient.LogIn(USERNAME, PASSWORD);

            KoanAssert.That(_rpcClient.Session, Is.Not.Null, "You should have a valid sessionId after logon");
            var oldSessionId = _rpcClient.Session;

            //Logging out force expires your session token on the server
            _rpcClient.LogOut();

            //So that future requests with your old token will fail.
            try
            {
                _rpcClient.Session = oldSessionId;
                var headlines2 = _rpcClient.News.ListNewsHeadlinesWithSource("dj", "AUS", 4);
                KoanAssert.Fail("the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                KoanAssert.That(e.Message, Is.StringContaining("(401) Unauthorized"), "The error message should contain something about (401) Unauthorized");
            }
        }

        private string FILL_ME_IN = "replace FILL_ME_IN with the correct value";
    }
}
