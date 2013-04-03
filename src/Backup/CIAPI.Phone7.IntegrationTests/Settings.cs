using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CIAPI.Phone7.IntegrationTests
{
    public class Settings
    {
        public static Uri RpcUri
        {
            get
            {
                return new Uri("https://ciapi.cityindex.com/tradingapi");
            }
        }

        public static Uri StreamingUri
        {
            get
            {
                return new Uri("https://push.cityindex.com");
            }
        }

        public static string RpcUserName
        {
            get
            {
                return "DM715257";
            }
        }

        public static string RpcPassword
        {
            get
            {
                return "password";
            }
        }
    }
}
