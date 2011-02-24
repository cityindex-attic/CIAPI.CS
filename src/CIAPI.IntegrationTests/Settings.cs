using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CIAPI.IntegrationTests
{
    public class Settings
    {
        public static Uri RpcUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["apiRpcUrl"]);
            }
        }

        public static Uri StreamingUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["apiStreamingUrl"]);
            }
        }

        public static string RpcUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["RpcUserName"];
            }
        }

        public static string RpcPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["RpcPassword"];
            }
        }
    }
}
