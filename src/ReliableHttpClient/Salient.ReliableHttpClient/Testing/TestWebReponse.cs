using System;
using System.IO;
using System.Net;

namespace Salient.ReliableHttpClient.Testing
{
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