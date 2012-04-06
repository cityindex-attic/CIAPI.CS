using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Salient.ReliableHttpClient.Testing
{
    public delegate void TestWebRequestPrepare(TestWebRequest request);

    public class TestWebRequest : WebRequest
    {

        internal TestWebRequestPrepare PrepareResponse;
        internal TestRequestFactory Factory;
        private readonly TimeSpan _latency;
        private readonly Exception _requestStreamException;
        private readonly Exception _responseStreamException;
        private readonly Exception _endGetResponseException;

        private readonly MemoryStream _requestStream = new MemoryStream();
        private readonly MemoryStream _responseStream = new MemoryStream();
        public MemoryStream ResponseStream
        {
            get
            {
                return _responseStream;
            }
        }
        private WebHeaderCollection _headers = new WebHeaderCollection();
        private TestAsyncResult _webResponseAsyncResult;
        private bool _isAborted;

        public override WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

#if !SILVERLIGHT
        public override int Timeout { get; set; }
        public override long ContentLength { get; set; }
#endif

        public override string Method { get; set; }
        //public Regex RequestUriRegex { get; set; }

        private readonly Uri _uri;
        public override Uri RequestUri
        {
            get { return _uri; }
        }

        public override string ContentType { get; set; }


        public TestWebRequest(Uri uri)
        {
            _uri = uri;
        }

    

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            PrepareResponse(this);

            if (_responseStreamException != null)
            {
                throw _responseStreamException;
            }
            _webResponseAsyncResult = new TestAsyncResult(callback, state, _latency);
            //we want to introduce latency on getting the response
            return _webResponseAsyncResult;
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            ThrowIfAborted();
            if (_endGetResponseException != null)
            {
                throw _endGetResponseException;
            }
            return new TestWebReponse(_responseStream);
        }

#if !SILVERLIGHT
        /// <summary>See <see cref="WebRequest.GetResponse"/>.</summary>
        public override WebResponse GetResponse()
        {
            using (var wait = new AutoResetEvent(false))
            {
                wait.WaitOne(_latency);
            }
            ThrowIfAborted();
            if (_responseStreamException != null)
            {
                throw _responseStreamException;
            }
            return new TestWebReponse(_responseStream);
        }
#endif

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            if (_requestStreamException != null)
            {
                throw _requestStreamException;
            }
            return new TestAsyncResult(callback, state); //we don't want any latency for the request
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return new MemoryStream();
        }

#if !SILVERLIGHT
        /// <summary>See <see cref="WebRequest.GetRequestStream"/>.</summary>
        public override Stream GetRequestStream()
        {
            if (_requestStreamException != null)
            {
                throw _requestStreamException;
            }

            return _requestStream;
        }
#endif

        public override void Abort()
        {
            _isAborted = true;
            _webResponseAsyncResult.Abort();
        }

        /// <summary>Returns the request contents as a string.</summary>
        public string ContentAsString()
        {
            byte[] response = ReadFully(_requestStream);
            return Encoding.UTF8.GetString(response, 0, response.Length);
        }

        private void ThrowIfAborted()
        {
            if (_isAborted)
            {
                throw new WebException("The request was aborted: The request was canceled",
                                       WebExceptionStatus.RequestCanceled);
            }
        }

        private static byte[] ReadFully(Stream stream)
        {
            var buffer = new byte[32768];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}