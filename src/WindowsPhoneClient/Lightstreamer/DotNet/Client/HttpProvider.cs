namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Security;
    using System.Text;
    using System.Threading;

    internal class HttpProvider
    {
        private string address;
        private CookieContainer cookies;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private string request;
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");

        public HttpProvider(string address, CookieContainer cookies)
        {
            this.cookies = cookies;
            this.address = address;
        }

        protected internal virtual bool AddLine(IDictionary parameters, long limit)
        {
            string newRequest;
            string paramsStr = this.HashToString(parameters);
            if (this.request == null)
            {
                newRequest = paramsStr;
            }
            else
            {
                newRequest = this.request + "\r\n" + paramsStr;
            }
            if ((limit > 0L) && (newRequest.Length > limit))
            {
                return false;
            }
            this.request = newRequest;
            return true;
        }

        protected internal static HttpWebRequest CreateWebRequest(string address, CookieContainer cookies)
        {
            Uri url = new Uri(address);
            HttpWebRequest connection = (HttpWebRequest) WebRequest.Create(url);
            if (connection == null)
            {
                streamLogger.Debug("Failed connection to " + address);
                throw new IOException("Connection failed");
            }
            connection.CookieContainer = cookies;
            connection.AllowReadStreamBuffering = false;
            return connection;
        }

        internal virtual Stream DoHTTP(bool isPost)
        {
            HttpWebRequest connection;
            Stream stream;
            if (isPost)
            {
                connection = this.SendPost();
            }
            else
            {
                connection = this.SendGet();
            }
            WebResponse response = null;
            try
            {
                MyRequestState state = new MyRequestState {
                    request = connection
                };
                if (connection.BeginGetResponse(new AsyncCallback(HttpProvider.ReadCallback), state) == null)
                {
                    throw new IOException("Response gathering failed unexpectedly (1)");
                }
                state.allDone.WaitOne();
                if (state.ioException != null)
                {
                    throw state.ioException;
                }
                if (state.webException != null)
                {
                    throw state.webException;
                }
                if (state.response == null)
                {
                    throw new IOException("Response gathering failed unexpectedly (2)");
                }
                response = state.response;
                stream = response.GetResponseStream();
            }
            catch (Exception e)
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
                throw e;
            }
            return stream;
        }

        public virtual Stream DoHTTP(IDictionary parameters, bool isPost)
        {
            this.AddLine(parameters, 0L);
            return this.DoHTTP(isPost);
        }

        private string HashToString(IDictionary parameters)
        {
            StringBuilder output = new StringBuilder();
            IEnumerator i = parameters.Keys.GetEnumerator();
            while (i.MoveNext())
            {
                string name = (string) i.Current;
                string val = (string) parameters[name];
                if (val == null)
                {
                    val = "";
                }
                try
                {
                    val = Uri.EscapeDataString(val);
                }
                catch (Exception e)
                {
                    protLogger.Debug("Error sending command", e);
                    throw new IOException("Encoding error");
                }
                if (output.Length != 0)
                {
                    output.Append("&");
                }
                output.Append(name);
                output.Append("=");
                output.Append(val);
            }
            return output.ToString();
        }

        private static void ReadCallback(IAsyncResult asynchronousResult)
        {
            MyRequestState state = (MyRequestState) asynchronousResult.AsyncState;
            try
            {
                WebResponse postResponse = state.request.EndGetResponse(asynchronousResult);
                state.response = postResponse;
            }
            catch (WebException e)
            {
                state.webException = e;
            }
            catch (IOException e)
            {
                state.ioException = e;
            }
            catch (SecurityException e)
            {
                state.webException = new WebException("Security exception", e);
            }
            catch (Exception e)
            {
                state.ioException = new IOException("Unexpected exception", e);
            }
            finally
            {
                state.allDone.Set();
            }
        }

        protected internal virtual HttpWebRequest SendGet()
        {
            streamLogger.Debug("Opening connection to " + this.address);
            string queryString = this.address;
            if (this.request != null)
            {
                queryString = this.address + "?" + this.request;
            }
            HttpWebRequest connection = CreateWebRequest(queryString, this.cookies);
            connection.Method = "GET";
            return connection;
        }

        protected internal virtual HttpWebRequest SendPost()
        {
            streamLogger.Debug("Opening connection to " + this.address);
            HttpWebRequest connection = CreateWebRequest(this.address, this.cookies);
            connection.Method = "POST";
            connection.ContentType = "application/x-www-form-urlencoded";
            MyRequestState state = new MyRequestState {
                request = connection
            };
            if (connection.BeginGetRequestStream(new AsyncCallback(HttpProvider.WriteCallback), state) == null)
            {
                throw new IOException("Request submission failed unexpectedly (1)");
            }
            state.allDone.WaitOne();
            if (state.ioException != null)
            {
                throw state.ioException;
            }
            if (state.webException != null)
            {
                throw state.webException;
            }
            if (state.stream == null)
            {
                throw new IOException("Request submission failed unexpectedly (2)");
            }
            Stream output = state.stream;
            if (this.request != null)
            {
                if (streamLogger.IsDebugEnabled)
                {
                    streamLogger.Debug("Posting data: " + this.request);
                }
                byte[] byte1 = new UTF8Encoding().GetBytes(this.request);
                output.Write(byte1, 0, byte1.Length);
            }
            output.Flush();
            output.Close();
            return connection;
        }

        private static void WriteCallback(IAsyncResult asynchronousResult)
        {
            MyRequestState state = (MyRequestState) asynchronousResult.AsyncState;
            try
            {
                Stream postStream = state.request.EndGetRequestStream(asynchronousResult);
                state.stream = postStream;
            }
            catch (WebException e)
            {
                state.webException = e;
            }
            catch (IOException e)
            {
                state.ioException = e;
            }
            catch (SecurityException e)
            {
                state.webException = new WebException("Security exception", e);
            }
            catch (Exception e)
            {
                state.ioException = new IOException("Unexpected exception", e);
            }
            finally
            {
                state.allDone.Set();
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

