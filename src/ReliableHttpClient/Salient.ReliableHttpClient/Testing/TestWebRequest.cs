using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Salient.ReliableHttpClient.Testing
{
    public delegate void TestWebRequestPrepare(TestWebRequest request);

    [Serializable]
    public class TestWebRequest : WebRequest, IDisposable
    {
        private TestAsyncResult _requestStreamAsyncResult;
        internal TestWebRequestPrepare PrepareResponse;
        public TimeSpan Latency { get; set; }

        public Exception ResponseStreamException { get; set; }
        public Exception RequestStreamException { get; set; }
        public Exception EndGetResponseException { get; set; }

        private TestWebStream _requestStream = new TestWebStream();
        private TestWebStream _responseStream = new TestWebStream();


        public TestWebStream ResponseStream
        {
            get { return _responseStream; }
            set { _responseStream = value; }
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

            if (ResponseStreamException != null)
            {
                throw ResponseStreamException;
            }
            _webResponseAsyncResult = new TestAsyncResult(callback, state, Latency);
            //we want to introduce latency on getting the response
            return _webResponseAsyncResult;
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            ThrowIfAborted();
            if (EndGetResponseException != null)
            {
                throw EndGetResponseException;
            }
            return new TestWebReponse(_responseStream);
        }

#if !SILVERLIGHT
        /// <summary>See <see cref="WebRequest.GetResponse"/>.</summary>
        public override WebResponse GetResponse()
        {
            PrepareResponse(this);

            using (var wait = new AutoResetEvent(false))
            {
                wait.WaitOne(Latency);
            }
            ThrowIfAborted();
            if (ResponseStreamException != null)
            {
                throw ResponseStreamException;
            }
            return new TestWebReponse(_responseStream);
        }
#endif


        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            if (RequestStreamException != null)
            {
                throw RequestStreamException;
            }
            _requestStreamAsyncResult = new TestAsyncResult(callback, state);
            return _requestStreamAsyncResult; //we don't want any latency for the request
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return _requestStream;
        }

#if !SILVERLIGHT
        /// <summary>See <see cref="WebRequest.GetRequestStream"/>.</summary>
        public override Stream GetRequestStream()
        {
            if (RequestStreamException != null)
            {
                throw RequestStreamException;
            }

            return _requestStream;
        }
#endif

        public override void Abort()
        {
            _isAborted = true;
            _webResponseAsyncResult.Abort();
        }


        public string RequestBody
        {
            get
            {
                if (_requestStream != null)
                {
                    byte[] requestStreamContent = _requestStream.Content;
                    return Encoding.UTF8.GetString(requestStreamContent, 0, requestStreamContent.Length);
                }
                return "";
            }
            set { _requestStream = new TestWebStream(Encoding.UTF8.GetBytes(value)); }
        }


        private void ThrowIfAborted()
        {
            if (_isAborted)
            {
                throw new WebException("The request was aborted: The request was canceled",
                                       WebExceptionStatus.RequestCanceled);
            }
        }

        public static byte[] ReadFully(Stream stream)
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


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetResponseStream(string responseText)
        {
            ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(responseText));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Warning	18	CA1001 : Microsoft.Design : Implement IDisposable on 'TestWebRequest' because it creates members of the following IDisposable types: 
                // 'TestWebStream', 'TestAsyncResult'. 
                if (_requestStream != null)
                {
                    _requestStream.Dispose();
                }
                if (_responseStream != null)
                {
                    _responseStream.Dispose();
                }
                if (_webResponseAsyncResult != null)
                {
                    _webResponseAsyncResult.Dispose();
                }
                if (_requestStreamAsyncResult != null)
                {
                    _requestStreamAsyncResult.Dispose();
                }
            }
        }
    }
}