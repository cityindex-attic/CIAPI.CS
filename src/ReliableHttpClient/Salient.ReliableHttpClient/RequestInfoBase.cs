using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Salient.ReliableHttpClient
{
    public class RequestInfoBase
    {
        public RequestInfoBase()
        {
            UserAgent = "Salient.ReliableHttpClient";
            Method = RequestMethod.GET;
            RequestContentType = ContentType.JSON;
            ResponseContentType = ContentType.JSON;
            State = RequestItemState.New;
            CacheDuration = TimeSpan.Zero;
            CacheExpiration = DateTimeOffset.MinValue;
            Id = Guid.NewGuid();

            Parameters = new Dictionary<string, object>();
            _headers = new Dictionary<string, object>();
        }

        public string UriTemplate { get; set; }
        public string Target { get; set; }
        public int AllowedRetries { get; set; }
        public string RequestBody { get; set; }
        public RequestMethod Method { get; set; }

        public int Timeout { get; set; }
        public TimeSpan CacheDuration { get; set; }
        public ContentType RequestContentType { get; set; }
        public ContentType ResponseContentType { get; set; }
        public string UserAgent { get; set; }
        public Uri Uri { get; set; }
        public DateTimeOffset CacheExpiration { get; set; }
        public Guid Id { get; set; }


        public int AttemptedRetries { get; set; }
        public DateTimeOffset Issued { get; set; }
        public DateTimeOffset Completed { get; set; }
        public int Index { get; set; }
        public RequestItemState State { get; set; }
        public string ResponseText { get; set; }

        public Dictionary<string, object> GetHeaders()
        {
            lock (_headers)
            {
                return new Dictionary<string, object>(_headers);
            }
        }
        internal Dictionary<string, object> _headers;

        public Dictionary<string, object> Parameters { get; set; }
        public ReliableHttpException Exception { get; set; }

        internal static WebRequest CreateRequest(RequestInfoBase info, IRequestFactory requestFactory)
        {
            WebRequest Request = requestFactory.Create(info.Uri.AbsoluteUri);

            Request.Method = info.Method.ToString();
            if ((info.Method == RequestMethod.POST || info.Method == RequestMethod.PUT))
            {
                switch (info.RequestContentType)
                {
                    case ContentType.JSON:
                        Request.ContentType = "application/json";
                        break;
                    case ContentType.FORM:
                        Request.ContentType = "application/x-www-form-urlencoded";
                        break;
                    case ContentType.XML:
                        Request.ContentType = "application/xml";
                        break;
                    case ContentType.TEXT:
                        Request.ContentType = "text/plain";
                        break;
                }
            }



            if (Request is HttpWebRequest)
            {
                var request = ((HttpWebRequest)Request);

                switch (info.ResponseContentType)
                {
                    case ContentType.JSON:
                        request.Accept = "text/plain, text/json, application/json";
                        break;
                    case ContentType.FORM:
                        request.Accept = "text/plain, text/json, application/json, text/xml, application/xml";
                        break;
                    case ContentType.XML:
                        request.Accept = "text/xml, application/xml";
                        break;
                    case ContentType.TEXT:
                        request.Accept = "text/plain";
                        break;
                }

                request.UserAgent = info.UserAgent;
                var headers = info.GetHeaders();
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers[header.Key] = header.Value.ToString();

                    }
                }
            }


            if ((info.Method == RequestMethod.POST || info.Method == RequestMethod.PUT))
            {
                if (string.IsNullOrEmpty(info.RequestBody))
                {
#if !WINDOWS_PHONE
                    Request.ContentLength = 0;
#endif
                }
                else
                {
                    // set request stream
                    var gate = new AutoResetEvent(false);
                    byte[] bodyValue = Encoding.UTF8.GetBytes(info.RequestBody);
                    Exception exception = null;
                    Request.BeginGetRequestStream(ac =>
                    {
                        try
                        {
                            using (
                                Stream requestStream =
                                    Request.EndGetRequestStream(ac))
                            {
                                requestStream.Write(bodyValue, 0, bodyValue.Length);
                                requestStream.Flush();
                            }
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                        finally
                        {
                            gate.Set();
                        }
                    }, null);

                    gate.WaitOne();
                    // #FIXME: logic to catch stalls conflicts with throttle
                    //if (!gate.WaitOne(10000))
                    //{
                    //    throw new Exception("timed out setting request body");
                    //}
                    if (exception != null)
                    {
                        throw exception;
                    }
                }

            }
            return Request;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // #TODO: obfuscate anything that looks like a password in Uri and parameters

            sb.AppendLine(string.Format("Item: #{0} {3} {2} [{1}]", Index, Id, Uri, Method));
            sb.AppendLine(string.Format("Target/UriTemplate: {0} {1}", Target, UriTemplate));
            sb.AppendLine(string.Format("State: {0}", State));

            sb.AppendLine("Headers:");
            var headers = GetHeaders();
            if (headers == null || headers.Count == 0)
            {
                sb.AppendLine("\tNONE");
            }
            else
            {
                foreach (var item in headers)
                {
                    sb.AppendLine(string.Format("\t{0} = {1}", item.Key, item.Value));
                }
            }


            sb.AppendLine("Parameters:");

            if (Parameters == null || Parameters.Count == 0)
            {
                sb.AppendLine("\tNONE");
            }
            else
            {
                foreach (var item in Parameters)
                {
                    sb.AppendLine(string.Format("\t{0} = {1}", item.Key, item.Value));
                }
            }

            sb.AppendLine(string.Format("Content Type: request - {0}, response - {1}", RequestContentType,
                                        ResponseContentType));
            sb.AppendLine(string.Format("Body: {0}", RequestBody));
            sb.AppendLine(string.Format("Timeout: {0}", Timeout));
            sb.AppendLine(string.Format("UserAgent: {0}", UserAgent));
            sb.AppendLine(string.Format("ResponseText: {0}", ResponseText));

            sb.AppendLine(string.Format("Latency: {0} ({1} {2})", Completed.Subtract(Issued).Duration(), Issued,
                                        Completed));
            sb.AppendLine(string.Format("Retries: {0}/{1}", AttemptedRetries, AllowedRetries));
            sb.AppendLine(string.Format("Caching: duration - {0}, expires - {1}", CacheDuration, CacheExpiration));

            if (Exception != null)
            {
                sb.AppendLine(string.Format("Exception: {0}", Exception));
            }


            return sb.ToString();
        }

        public RequestInfoBase Copy()
        {
            RequestInfoBase result = new RequestInfoBase()
                                   {
                                       AllowedRetries = this.AllowedRetries,
                                       AttemptedRetries = this.AttemptedRetries,
                                       CacheDuration = this.CacheDuration,
                                       CacheExpiration = this.CacheExpiration,
                                       Completed = this.Completed,
                                       Id = this.Id,
                                       Index = this.Index,
                                       Issued = this.Issued,
                                       Method = this.Method,

                                       RequestBody = this.RequestBody,
                                       RequestContentType = this.RequestContentType,
                                       ResponseContentType = this.ResponseContentType,
                                       ResponseText = this.ResponseText,
                                       State = this.State,
                                       Target = this.Target,
                                       Timeout = this.Timeout,
                                       Uri = this.Uri,
                                       UriTemplate = this.UriTemplate,
                                       UserAgent = this.UserAgent
                                   };

            if (Parameters != null)
            {
                result.Parameters = new Dictionary<string, object>(this.Parameters);
            }

            if (Exception != null)
            {
                result.Exception = ReliableHttpException.Create(this.Exception);
            }

            var headers = GetHeaders();
            if (headers != null)
            {
                result._headers = headers;
            }
            return result;
        }
    }
}