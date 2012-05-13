using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Salient.ReliableHttpClient.Testing
{
    public class TestWebStream : System.IO.Stream
    {
        public TestWebStream()
        {

        }
        public TestWebStream(Byte[] value)
        {
            _internal = new MemoryStream(value);
        }
        private readonly System.IO.MemoryStream _internal = new System.IO.MemoryStream();
        public override void Flush()
        {
            _internal.Flush();
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return _internal.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _internal.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _internal.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _internal.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return _internal.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _internal.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _internal.CanSeek; }
        }

        public override long Length
        {
            get { return _internal.Length; }
        }

        public override long Position
        {
            get { return _internal.Position; }
            set { _internal.Position = value; }
        }
        public override void Close()
        {
            _content = _internal.ToArray();
            base.Close();
        }

        private Byte[] _content;
        public Byte[] Content
        {
            get
            {

                if (CanRead)
                {
                    return _internal.ToArray();
                }
                else
                {
                    return _content;
                }
            }
        }
    }
    public delegate void TestWebRequestPrepare(TestWebRequest request);

    public class TestWebRequest : WebRequest
    {

        internal TestWebRequestPrepare PrepareResponse;
        internal TestRequestFactory Factory;
        private readonly TimeSpan _latency;
        private readonly Exception _requestStreamException;
        private readonly Exception _responseStreamException;
        private readonly Exception _endGetResponseException;

        private TestWebStream _requestStream = new TestWebStream();
        private TestWebStream _responseStream = new TestWebStream();
        public TestWebStream ResponseStream
        {
            get
            {
                return _responseStream;
            }
            set
            {
                _responseStream = value;
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

            PrepareResponse(this);

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

        public override System.IO.Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return _requestStream;
        }

#if !SILVERLIGHT
        /// <summary>See <see cref="WebRequest.GetRequestStream"/>.</summary>
        public override System.IO.Stream GetRequestStream()
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



    }
    public class TestWebRequestFinder
    {
        private List<RequestInfoBase> _reference;
        public List<RequestInfoBase> Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        public RequestInfoBase FindMatchBySingleHeader(TestWebRequest webRequest, string headerKey)
        {
            string headerValue = webRequest.Headers[headerKey];
            foreach (RequestInfoBase r in _reference)
            {
                if (r.Headers.ContainsKey(headerKey))
                {
                    if ((string)r.Headers[headerKey] == headerValue)
                    {
                        return r;
                    }                    
                }
            }
            return null;
        }
        public RequestInfoBase FindMatchExact(TestWebRequest webRequest)
        {

            foreach (RequestInfoBase r in _reference)
            {
                if (string.Compare(r.Method.ToString(), webRequest.Method, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    continue;
                }

                if (r.Uri.AbsoluteUri != webRequest.RequestUri.AbsoluteUri)
                {
                    continue;
                }

                // #hack - RequestInfoBase requestbody is getting set null while TestWebRequest.RequestBody is returning empty string
                // have to decide which is approppriate and standardize
                if ((r.RequestBody ?? "") != webRequest.RequestBody)
                {
                    continue;
                }

                if (r.Headers != null)
                {
                    if (webRequest.Headers == null)
                    {
                        continue;
                    }

                    //#TODO: compare headers
                }

                return r;


            }
            return null;
        }
    }
}
