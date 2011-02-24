using System;
using CIAPI.CS.Koans.KoanRunner;
using CityIndex.JsonClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.CS.Koans
{
    [KoanCategory]
    public class AboutSessions
    {
        private Client _rpcClient;
        private string USERNAME;
        private string PASSWORD;

        [Koan]
        public void CreatingASession()
        {
            //Interaction with the API is done via a top level "client" object 
            //that holds details about your connection.
            
            //You need to initialise the client with a valid endpoint
            _rpcClient = new Rpc.Client(new Uri("http://ciapipreprod.cityindextest9.co.uk/TradingApi"));
            
            //And then create a session by creating a username & password
            //You can get test credentials by requesting them at {CIAPI.docs}/#test-credentials.htm
            USERNAME = "DM904310";
            PASSWORD = "password";
            _rpcClient.LogIn(USERNAME, PASSWORD);

            KoanAssert.That(_rpcClient.SessionId != "", "after logging in, you should have a valid session");
        }

        [Koan]
        public void EveryRequestUsesYourSession()
        {
            //The rpcClient stores your current session details, and uses it to authenticate
            //every request.
            var headlines = _rpcClient.ListNewsHeadlines("UK", 10);
            KoanAssert.That(headlines.Headlines.Length > 0, "you should have a set of headlines");

            //When your sessionId expires
            _rpcClient.SessionId = "{an-expired-session-token}";

            //Then future requests will fail.
            try
            {
                var headlines2 = _rpcClient.ListNewsHeadlines("AUS", 10);
                KoanAssert.That(false, "the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                KoanAssert.That(e.Message, Is.StringContaining("401"), "The error message should contain something about (401) Unauthorized");
            }
        }

        [Koan]
        public void YouCanForceYourSessionToExpireByLoggingOut()
        {
            _rpcClient.LogIn(USERNAME, PASSWORD);

            KoanAssert.That(_rpcClient.SessionId, Is.Not.Null, "You should have a valid sessionId after logon");
            var oldSessionId = _rpcClient.SessionId;

            //Logging out force expires your session token on the server
            _rpcClient.LogOut();

            //So that future requests with your old token will fail.
            try
            {
                _rpcClient.SessionId = oldSessionId;
                var headlines2 = _rpcClient.ListNewsHeadlines("AUS", 4);
                KoanAssert.Fail("the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                KoanAssert.That(e.Message, Is.StringContaining("401"), "The error message should contain something about (401) Unauthorized");
            }
        }

    }
}
