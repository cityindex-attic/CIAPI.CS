using System;

namespace CIAPI.Core
{
    public class ApiContext
    {
        public ApiContext(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; protected set;}

        public void Logon()
        {
            if (String.IsNullOrEmpty(ApiKey)) throw new ArgumentException("Api key must be specified before attempting to logon");
            //TODO: Redirect to logon site to authenticate credentials");
        }
    }
}
