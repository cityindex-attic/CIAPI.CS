using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    [TestFixture]
    public class SupportingTests
    {
        [Test]
        public void ReusingHttpWebRequest()
        {
            HttpWebRequest request;
            WebResponse response;

            request = (HttpWebRequest) WebRequest.Create("http://google.com");

            response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    var responseText = reader.ReadToEnd();
                    Assert.IsNotNullOrEmpty(responseText);
                }
            }


            response.Close();

            

            response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    var responseText = reader.ReadToEnd();
                    Assert.IsNotNullOrEmpty(responseText);
                }
            }
        }
    }
}
