using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System;
namespace Salient.ReliableHttpClient.Tests
{
    // have used this code before - works ok 
    // based on http://blog.salamandersoft.co.uk/index.php/2009/10/how-to-mock-httpwebrequest-when-unit-testing/
    // with some changes needed for silverlight
    public class TestWebReponse : WebResponse
    {
        Stream responseStream;

        /// <summary>Initializes a new instance of <see cref="TestWebReponse"/>
        /// with the response stream to return.</summary>
        public TestWebReponse(Stream responseStream)
        {
            this.responseStream = responseStream;
        }

        /// <summary>See <see cref="WebResponse.GetResponseStream"/>.</summary>
        public override Stream GetResponseStream()
        {
            return responseStream;
        }

        public override void Close()
        {
            // noop
        }

        public override long ContentLength
        {
            get { throw new NotImplementedException(); }
        }

        public override string ContentType
        {
            get { throw new NotImplementedException(); }
        }

        public override Uri ResponseUri
        {
            get { throw new NotImplementedException(); }
        }
    }
}
