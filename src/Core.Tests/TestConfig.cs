using System;
using System.Configuration;

namespace CIAPI.Core.Tests
{
    public static class TestConfig
    {
        public static string ApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["apiUrl"];
            }
        }
        public static string BasicAuthUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["basicAuthUsername"];
            }
        }
        public static string BasicAuthPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["basicAuthPassword"];
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
        public static Guid ApiTestSessionId
        {
            get
            {
                return new Guid(ConfigurationManager.AppSettings["apiTestSessionId"]);
            }
        }
        
    }
}