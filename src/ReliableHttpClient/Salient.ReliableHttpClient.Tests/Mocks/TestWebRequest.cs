using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Salient.ReliableHttpClient.Tests
{
    public class TestWebRequest : WebRequest
    {
        public static TestWebRequest CreateTestRequest(Regex requestUriRegex, string response, TimeSpan latency, Exception requestStreamException, Exception responseStreamException, Exception endGetResponseException)
        {
            var request = new TestWebRequest(response, latency, requestStreamException, responseStreamException, endGetResponseException);
            request.RequestUriRegex = requestUriRegex;
#if !SILVERLIGHT
            request.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(30).TotalMilliseconds);
#else
            //FIXME: Need a way to timeout requests in Silverlight
#endif
            return request;
        }

        private readonly TimeSpan _latency;
        private readonly Exception _requestStreamException;
        private readonly Exception _responseStreamException;
        private readonly Exception _endGetResponseException;

        private readonly MemoryStream _requestStream = new MemoryStream();
        private readonly MemoryStream _responseStream;
        private WebHeaderCollection _headers = new WebHeaderCollection();
        private TestAsyncResult _webResponseAsyncResult;
        private bool _isAborted = false;

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
        public Regex RequestUriRegex { get; set; }
        public override Uri RequestUri
        {
            get { return new Uri("http://TestRequest.com/"); }
        }

        public override string ContentType { get; set; }

        /// <summary>Initializes a new instance of <see cref="TestWebRequest"/>
        /// with the response to return.</summary>
        public TestWebRequest(string response)
            : this(response, TimeSpan.FromMilliseconds(10), null, null, null)
        {

        }

        public TestWebRequest(string response, TimeSpan latency, Exception requestStreamException, Exception responseStreamException, Exception endGetResponseException)
        {
            _responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response));
            _latency = latency;
            _requestStreamException = requestStreamException;
            _responseStreamException = responseStreamException;
            _endGetResponseException = endGetResponseException;
        }

        
        public override IAsyncResult BeginGetResponse(System.AsyncCallback callback, object state)
        {
            if (_responseStreamException != null)
            {
                throw _responseStreamException;
            }
            _webResponseAsyncResult = new TestAsyncResult(callback, state, _latency); //we want to introduce latency on getting the response
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

        public override IAsyncResult BeginGetRequestStream(System.AsyncCallback callback, object state)
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
            var response = ReadFully(_requestStream);
            return System.Text.Encoding.UTF8.GetString(response, 0, response.Length);
        }

        private void ThrowIfAborted()
        {
            if (_isAborted)
            {
                throw new WebException("The request was aborted: The request was canceled", WebExceptionStatus.RequestCanceled);
            }
        }

        private static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
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

    public class TestAsyncResult : IAsyncResult
    {
        private const int DEFAULT_LATENCY_IN_MS = 100;
        private readonly System.AsyncCallback _callback;
        private readonly object _state;
        private readonly ManualResetEvent _waitHandle;
        private readonly Timer _timer;
        public bool IsCompleted { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public bool CompletedSynchronously
        {
            get { return IsCompleted; }
        }

        public TestAsyncResult(System.AsyncCallback callback, object state)
            : this(callback, state, TimeSpan.FromMilliseconds(DEFAULT_LATENCY_IN_MS))
        {
        }

        public TestAsyncResult(System.AsyncCallback callback, object state, TimeSpan latency)
        {
            IsCompleted = false;
            _callback = callback;
            _state = state;
            _waitHandle = new ManualResetEvent(false);
            _timer = new Timer(onTimer => NotifyComplete(), null, latency, TimeSpan.FromMilliseconds(-1));
        }

        public void Abort()
        {
            _timer.Dispose();
            NotifyComplete();
        }

        private void NotifyComplete()
        {
            IsCompleted = true;
            _waitHandle.Set();
            if (_callback != null)
                _callback(this);
        }
    }
}