using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    [TestFixture]
    public class ClientFixture
    {
        [Test]
        public void CheckDns()
        {
            Assert.False(UrlIsValid("should.not.exist"));
            //+		[0]	{67.215.77.132}	System.Net.IPAddress

        }

        public static bool UrlIsValid(string smtpHost)
        {
            bool br = false;
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(smtpHost);
                br = true;
            }
            catch (SocketException se)
            {
                br = false;
            }
            return br;
        }
    }
}
