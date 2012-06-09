using System;
using System.IO;
using System.Net;

namespace Salient.ReliableHttpClient.Testing
{
    
    [Serializable]
    public class TestWebReponse : WebResponse
    {
        TestWebStream responseStream;

        /// <summary>Initializes a new instance of <see cref="TestWebReponse"/>
        /// with the response stream to return.</summary>
        public TestWebReponse(TestWebStream responseStream)
        {
            this.responseStream = responseStream;
            _contentLength = responseStream.Length;

            _headers = new WebHeaderCollection();
        }

        private WebHeaderCollection _headers;
        public override WebHeaderCollection Headers
        {
            get
            {
                return _headers;
            }
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
            get { return _contentLength; }
            
        }
        public void SetContentLength(long contentLength)
        {
            _contentLength = contentLength;
        }

        private Uri _responseUri;

        private string _contentType;
        public void SetContentType(string contentType)
        {
            _contentType = contentType;
        }
        public override string ContentType
        {
            get
            {
                return _contentType;
            }
            
        }


        public override Uri ResponseUri
        {
            get { return _responseUri; }
            
        }
        public void SetResponseUri(Uri uri)
        {
            _responseUri = uri;
        }
    }
}