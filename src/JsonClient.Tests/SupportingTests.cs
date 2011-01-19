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
        /// <summary>
        /// proves that closing the response of a request does not enable reuse.
        /// </summary>
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


            Assert.Throws<ArgumentException>(() =>
                {

                    response = request.GetResponse();
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var responseText = reader.ReadToEnd();
                            Assert.IsNotNullOrEmpty(responseText);
                        }
                    }

                });
        }
    }
}
