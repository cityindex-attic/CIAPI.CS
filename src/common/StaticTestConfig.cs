using System;


namespace CIAPI.Tests
{
    public static class StaticTestConfig
    {
        public static string RpcUrl
        {
            get
            {
                //https://ciapipreprod.cityindextest9.co.uk/TradingApi
                // https://ciapiqat.cityindextest9.co.uk/TradingApi
                return "https://ciapi.cityindex.com/tradingapi";
            }
        }

        public static string StreamingUrl
        {
            get
            {
                //https://pushpreprod.cityindextest9.co.uk
                return "https://push.cityindex.com";
            }
        }

        public static string ApiUsername
        {
            get
            {
                //xx189949
                return "DM715257";
            }
        }
        public static string ApiPassword
        {
            get
            {
                return "password";
            }
        }
        public static string ApiTestSessionId
        {
            get
            {
                return "D2FF3E4D-01EA-4741-86F0-437C919B5559";
            }
        }

        public static string AppKey
        {
            get
            {
                return "D2FF3E4D-01EA-4741-86F0-437C919B5559";
            }
        }
        
    }
}