using System;
using System.IO;
using System.Net;

namespace Salient.ReliableHttpClient.Testing
{
    public class TestWebReponse : WebResponse
    {
        TestWebStream responseStream;

        /// <summary>Initializes a new instance of <see cref="TestWebReponse"/>
        /// with the response stream to return.</summary>
        public TestWebReponse(TestWebStream responseStream)
        {
            this.responseStream = responseStream;
            _contentLength = responseStream.Length;
            

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

        private long _contentLength;
        public override long ContentLength
        {
            get{return _contentLength;}
            
        }

        private string _contentType;
        public override string ContentType
        {
            get{return _contentType;}
            
        }

        private readonly Uri _responseUri;
        public override Uri ResponseUri
        {
            get{return _responseUri;}
        }
    }
}