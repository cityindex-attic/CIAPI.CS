namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Web;

    internal class HttpProvider
    {
        private string address;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private string request;
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");

        public HttpProvider(string address)
        {
            this.address = address;
        }

        protected internal virtual bool AddLine(Hashtable parameters, long limit)
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

        public virtual Stream DoPost(Hashtable parameters)
        {
            Stream responseStream;
            this.AddLine(parameters, 0L);
            HttpWebRequest request = this.SendPost();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                responseStream = response.GetResponseStream();
            }
            catch (WebException exception)
            {
                try
                {
                    response.Close();
                }
                catch (Exception)
                {
                }
                throw exception;
            }
            return responseStream;
        }

        private string HashToString(Hashtable parameters)
        {
            StringBuilder builder = new StringBuilder();
            IEnumerator enumerator = new HashSetSupport(parameters.Keys).GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = (string) enumerator.Current;
                string s = (string) parameters[current];
                if (s == null)
                {
                    s = "";
                }
                try
                {
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    UTF8Encoding encoding2 = new UTF8Encoding();
                    s = HttpUtility.UrlEncode(encoding2.GetString(encoding.GetBytes(s)));
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
                builder.Append(s);
            }
            return builder.ToString();
        }

        protected internal virtual HttpWebRequest SendPost()
        {
            streamLogger.Debug("Opening connection to " + this.address);
            Uri requestUri = new Uri(this.address);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
            if (request == null)
            {
                streamLogger.Debug("Failed connection to " + this.address);
                throw new IOException("Connection failed");
            }
            request.Method = "POST";
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = policy;
            request.KeepAlive = false;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2";
            request.UserAgent = "Lightstreamer .NET Client";
            Stream requestStream = request.GetRequestStream();
            if (this.request != null)
            {
                if (streamLogger.IsDebugEnabled)
                {
                    streamLogger.Debug("Posting data: " + this.request);
                }
                byte[] bytes = new ASCIIEncoding().GetBytes(this.request);
                requestStream.Write(bytes, 0, bytes.Length);
            }
            requestStream.Flush();
            requestStream.Close();
            return request;
        }
    }
}

