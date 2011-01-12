using System;
using System.Configuration;

namespace CIAPI.Tests
{
    public static class TestConfig
    {
        public static string RpcUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["apiUrl"];
            }
        }

        public static string StreamingUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["streamingUrl"];
            }
        }

        public static string ApiUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["apiUsername"];
            }
        }
        public static string ApiPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["apiPassword"];
            }
        }
        public static string ApiTestSessionId
        {
            get
            {
                return ConfigurationManager.AppSettings["apiTestSessionId"];
            }
        }


        
    }
}