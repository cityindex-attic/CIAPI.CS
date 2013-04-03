using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Salient.HTTPArchiveModel;

namespace Salient.ReliableHttpClient.Portable.Tests
{
    [TestFixture]
    public class HttpClientFixture
    {
        [Test]
        public void CanMakeSimpleGetRequest()
        {
            var client = new HttpClient();
            var result = client.Request(new RequestData()
                {
                    Request = new Request()
                        {
                            Url = "http://google.com",
                            Method = "get",
                            PostData = new PostData()
                        }
                }, "1");

            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data.Response.Content.Text);
        }
    }
}
