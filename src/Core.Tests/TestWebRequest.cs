using System;
using System.IO;
using System.Net;
using System.Threading;

namespace CIAPI.Core.Tests
{
    public class TestWebRequest : WebRequest
    {

        private readonly int _latency;
        private readonly Exception _requestStreamException;
        private readonly Exception _responseStreamException;
        private readonly Exception _endGetResponseException;

        private readonly MemoryStream _requestStream = new MemoryStream();
        private readonly MemoryStream _responseStream;
        private WebHeaderCollection _headers = new WebHeaderCollection();
        public override WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

        public override string Method { get; set; }

        public override Uri RequestUri
        {
            get { throw new NotImplementedException(); }
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            using (var wait = new AutoResetEvent(false))
            {
                wait.WaitOne(_latency);
            }
            if (_requestStreamException != null)
            {
                throw _requestStreamException;
            }
            return new TestAsyncResult(callback, state);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
 
            if (_responseStreamException != null)
            {
                throw _responseStreamException;
            }
            return new TestAsyncResult(callback, state);
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return new MemoryStream();
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            using (var wait = new AutoResetEvent(false))
            {
                wait.WaitOne(_latency);
            }
            if (_endGetResponseException != null)
            {
                throw _endGetResponseException;
            }

            return new TestWebReponse(_responseStream);
        }

        public override string ContentType { get; set; }

        /// <summary>Initializes a new instance of <see cref="TestWebRequest"/>
        /// with the response to return.</summary>
        public TestWebRequest(string response)
            : this(response, 10, null, null, null)
        {

        }

        public TestWebRequest(string response, int latency, Exception requestStreamException, Exception responseStreamException, Exception endGetResponseException)
        {

            _responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response));
            _latency = latency;
            _requestStreamException = requestStreamException;
            _responseStreamException = responseStreamException;
            _endGetResponseException = endGetResponseException;
        }

        /// <summary>Returns the request contents as a string.</summary>
        public string ContentAsString()
        {
            var response = ReadFully(_requestStream);
            return System.Text.Encoding.UTF8.GetString(response, 0, response.Length);
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
#if !SILVERLIGHT
        public override long ContentLength { get; set; }
        /// <summary>See <see cref="WebRequest.GetRequestStream"/>.</summary>
        public override Stream GetRequestStream()
        {
            using (var wait = new AutoResetEvent(false))
            {
                wait.WaitOne(_latency);
            }
            if (_requestStreamException != null)
            {
                throw _requestStreamException;
            }

            return _requestStream;
        }

        /// <summary>See <see cref="WebRequest.GetResponse"/>.</summary>
        public override WebResponse GetResponse()
        {
            using (var wait = new AutoResetEvent(false))
            {
                wait.WaitOne(_latency);
            }
            if (_responseStreamException != null)
            {
                throw _responseStreamException;
            }
            return new TestWebReponse(_responseStream);
        }
#endif
    }

    public class TestAsyncResult : IAsyncResult
    {
        public TestAsyncResult(AsyncCallback callback, object state)
        {
            new Timer(o => callback(this), null, 100, -1);
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }
    }
}