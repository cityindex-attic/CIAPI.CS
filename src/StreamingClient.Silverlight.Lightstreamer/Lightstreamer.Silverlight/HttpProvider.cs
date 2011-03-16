
using StreamingClient.Lightstreamer;

namespace Lightstreamer.DotNet.Client
{
    using Common.Logging;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Security;
    using System.Text;
    using System.Threading;
#if SILVERLIGHT && !WINDOWS_PHONE
    using System.Windows.Browser;
#endif

    internal class HttpProvider
    {
        private string address;
        private static ILog protLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.protocol));
        private string request;
        private static ILog streamLogger = LogManager.GetLogger(typeof(com.lightstreamer.ls_client.stream));

        public HttpProvider(string address)
        {
            this.address = address;
        }

        protected internal virtual bool AddLine(IDictionary parameters, long limit)
        {
            string str2;
            string str = this.HashToString(parameters);
            if (this.request == null)
            {
                str2 = str;
            }
            else
            {
                str2 = this.request + "\r\n" + str;
            }
            if ((limit > 0L) && (str2.Length > limit))
            {
                return false;
            }
            this.request = str2;
            return true;
        }

        internal virtual Stream DoHTTP(bool isPost)
        {
            HttpWebRequest request;
            Stream responseStream;
            if (isPost)
            {
                request = this.SendPost();
            }
            else
            {
                request = this.SendGet();
            }
            WebResponse response = null;
            try
            {
                MyRequestState state = new MyRequestState {
                    request = request
                };
                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(HttpProvider.ReadCallback), state);
                state.allDone.WaitOne();
                
                if (state.ioException != null)
                {
                    throw state.ioException;
                }
                if (state.webException != null)
                {
                    throw state.webException;
                }
                response = state.response;
                responseStream = response.GetResponseStream();
            }
            catch (Exception exception)
            {
                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                throw exception;
            }
            return responseStream;
        }

        public virtual Stream DoHTTP(IDictionary parameters, bool isPost)
        {
            this.AddLine(parameters, 0L);
            return this.DoHTTP(isPost);
        }

        private string HashToString(IDictionary parameters)
        {
            StringBuilder builder = new StringBuilder();
            IEnumerator enumerator = parameters.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = (string) enumerator.Current;
                string str2 = (string) parameters[current];
                if (str2 == null)
                {
                    str2 = "";
                }
                try
                {
#if SILVERLIGHT && !WINDOWS_PHONE
                    str2 = System.Windows.Browser.HttpUtility.UrlEncode(str2);
#else
                    str2 = System.Net.HttpUtility.UrlEncode(str2);
#endif
                }
                catch (Exception exception)
                {
                    protLogger.Debug("Error sending command", exception);
                    throw new IOException("Encoding error");
                }
                if (builder.Length != 0)
                {
                    builder.Append("&");
                }
                builder.Append(current);
                builder.Append("=");
                builder.Append(str2);
            }
            return builder.ToString();
        }

        private static void ReadCallback(IAsyncResult asynchronousResult)
        {
            MyRequestState asyncState = (MyRequestState) asynchronousResult.AsyncState;
            try
            {
                WebResponse response = asyncState.request.EndGetResponse(asynchronousResult);
                asyncState.response = response;
                asyncState.allDone.Set();
            }
            catch (WebException exception)
            {
                asyncState.webException = exception;
                asyncState.allDone.Set();
            }
            catch (IOException exception2)
            {
                asyncState.ioException = exception2;
                asyncState.allDone.Set();
            }
            catch (SecurityException exception3)
            {
                asyncState.webException = new WebException("Security exception", exception3);
                asyncState.allDone.Set();
            }
            catch (Exception exception4)
            {
                asyncState.ioException = new IOException("Unexpected exception", exception4);
                asyncState.allDone.Set();
            }
        }

        protected internal virtual HttpWebRequest SendGet()
        {
            streamLogger.Debug("Opening connection to " + this.address);
            string address = this.address;
            if (this.request != null)
            {
                address = this.address + "?" + this.request;
            }
            Uri uri = new Uri(address);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (request == null)
            {
                streamLogger.Debug("Failed connection to " + this.address);
                throw new IOException("Connection failed");
            }
            request.Method = "GET";
            request.AllowReadStreamBuffering = false;
            return request;
        }

        protected internal virtual HttpWebRequest SendPost()
        {
            streamLogger.Debug("Opening connection to " + this.address);
            Uri uri = new Uri(this.address);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (request == null)
            {
                streamLogger.Debug("Failed connection to " + this.address);
                throw new IOException("Connection failed");
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            MyRequestState state = new MyRequestState {
                request = request
            };

            IAsyncResult result = request.BeginGetRequestStream(new AsyncCallback(HttpProvider.WriteCallback), state);
            //BEGIN_CI_EDIT
            if (!state.allDone.WaitOne(LightstreamerDefaults.DEFAULT_TIMEOUT_MS))
            {
                state.ioException = new IOException(string.Format("Timed out after {0}ms waiting for request.BeginGetRequestStream for {1}", LightstreamerDefaults.DEFAULT_TIMEOUT_MS, address)); 
                streamLogger.Warn(state.ioException.Message);
            }
            //END_CI_EDIT

            if (state.ioException != null)
            {
                throw state.ioException;
            }
            if (state.webException != null)
            {
                throw state.webException;
            }
            Stream stream = state.stream;
            if (this.request != null)
            {
                if (streamLogger.IsDebugEnabled)
                {
                    streamLogger.Debug("Posting data: " + this.request);
                }
                byte[] bytes = new UTF8Encoding().GetBytes(this.request);
                stream.Write(bytes, 0, bytes.Length);
            }
            stream.Flush();
            stream.Close();
            return request;
        }

        private static void WriteCallback(IAsyncResult asynchronousResult)
        {
            MyRequestState asyncState = (MyRequestState)asynchronousResult.AsyncState;
            try
            {
                Stream stream = asyncState.request.EndGetRequestStream(asynchronousResult);
                asyncState.stream = stream;
                asyncState.allDone.Set();
            }
            catch (WebException exception)
            {
                asyncState.webException = exception;
                asyncState.allDone.Set();
            }
            catch (IOException exception2)
            {
                asyncState.ioException = exception2;
                asyncState.allDone.Set();
            }
            catch (SecurityException exception3)
            {
                asyncState.webException = new WebException("Security exception", exception3);
                asyncState.allDone.Set();
            }
            catch (Exception exception4)
            {
                asyncState.ioException = new IOException("Unexpected exception", exception4);
                asyncState.allDone.Set();
            }
        }

        private class MyRequestState
        {
            internal ManualResetEvent allDone = new ManualResetEvent(false);
            internal IOException ioException;
            internal HttpWebRequest request;
            internal WebResponse response;
            internal Stream stream;
            internal WebException webException;
        }
    }
}

