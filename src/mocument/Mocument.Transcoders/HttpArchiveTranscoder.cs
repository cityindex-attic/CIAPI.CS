using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Fiddler;
using Mocument.Model;
using Salient.HTTPArchiveModel;

namespace Mocument.Transcoders
{
    public static class HttpArchiveTranscoder
    {
        internal static readonly int MaxBinaryBodyLength =
            FiddlerApplication.Prefs.GetInt32Pref("fiddler.importexport.HTTPArchiveJSON.MaxBinaryBodyLength", 0x8000);







        public static List<Session> Import(Tape tape)
        {
            return tape.log.entries.Select(Import)
                .Where(session => session != null)
                .ToList();
        }

        public static Session Import(Entry entry)
        {
            DateTime now;
            Request htRequest = entry.request;
            byte[] arrRequest = GetRequestFromEntry(htRequest);
            Response htResponse = entry.response;
            byte[] arrResponse = GetResponseFromEntry(htResponse);

            if ((arrRequest == null) || (arrResponse == null))
            {
                throw new Exception("Failed to get session from entry");
            }

            const SessionFlags responseStreamed = SessionFlags.ResponseStreamed;
            var session = new Session(arrRequest, arrResponse, responseStreamed);
            int num = GetTotalSize(htResponse);
            if (num > 0)
            {
                session["X-TRANSFER-SIZE"] = num.ToString(CultureInfo.InvariantCulture);
            }

            session["ui-comments"] = entry.comment;

            if (!DateTime.TryParse(entry.startedDateTime, out now))
            {
                now = DateTime.Now;
            }

            session.Timers.DNSTime = entry.timings.dns;
            session.Timers.TCPConnectTime = entry.timings.connect;
            session.Timers.HTTPSHandshakeTime = entry.timings.ssl;
            session.Timers.ClientConnected = session.Timers.ClientBeginRequest = session.Timers.ClientDoneRequest = now;
            session.Timers.ServerConnected =
                session.Timers.FiddlerBeginRequest =
                now.AddMilliseconds(((entry.timings.blocked + session.Timers.DNSTime) + session.Timers.TCPConnectTime) +
                                    session.Timers.HTTPSHandshakeTime);
            session.Timers.ServerGotRequest = session.Timers.FiddlerBeginRequest.AddMilliseconds(entry.timings.send);
            session.Timers.ServerBeginResponse = now.AddMilliseconds(entry.timings.wait);
            session.Timers.ServerDoneResponse =
                session.Timers.ServerBeginResponse.AddMilliseconds(entry.timings.receive);
            session.Timers.ClientBeginResponse = session.Timers.ClientDoneResponse = session.Timers.ServerDoneResponse;
            return session;
        }

        public static Tape Export(IEnumerable<Session> sessions)
        {
            var tape = new Tape();
            tape.log = new Log
                           {
                               version = "1.2",
                               comment = "exported @ " + DateTime.Now.ToString(CultureInfo.InvariantCulture)
                           };
            tape.log.creator = new VersionInfo()
                                   {
                                       name = "mocument",
                                       version = "//#TODO: get executing assembly version",
                                       comment = "http://mocment.it"
                                   };
            // #FIXME - why the truck is tape.log.Entries null? it is instantiated in the ctor of Log?
            tape.log.entries = new List<Entry>();

            foreach (Session session in sessions)
            {
                if (session.state != SessionStates.Done)
                {
                    continue;
                }

                try
                {
                    var entry = Export(session);

                    tape.log.entries.Add(entry);
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {
                    // #TODO: report?
                    // "Failed to Export Session"
                }
            }

            return tape;
        }

        public static Entry Export(Session session, bool requestOnly = false)
        {
            var entry = new Entry();
            entry.startedDateTime = session.Timers.ClientBeginRequest.ToString("o");
            entry.request = GetRequest(session);
            if (!requestOnly)
            {
                entry.response = GetResponse(session);
            }

            entry.timings = GetTimings(session.Timers);
            entry.comment = session["ui-comments"];

            entry.time = GetTotalTime(entry.timings);


            if (
                !string.IsNullOrEmpty(session["ui-comments"])
                // <-- not sure if this is correct, maybe a typo or missing assignation in BasicFormats?
                && !session.isFlagSet(SessionFlags.SentToGateway))
            {
                entry.serverIPAddress = session.m_hostIP;
            }
            entry.connection = session.clientPort.ToString(CultureInfo.InvariantCulture);
            return entry;
        }

        #region Export



        private static int GetTotalTime(Timings timings)
        {
            int total = 0;
            total += timings.blocked;
            total += timings.dns;
            total += timings.connect;
            total += timings.send;
            total += timings.wait;
            total += timings.receive;
            return total;
        }

        private static Timings GetTimings(SessionTimers timers)
        {
            var timings = new Timings
                              {
                                  blocked = -1,
                                  dns = timers.DNSTime,
                                  connect = timers.TCPConnectTime + timers.HTTPSHandshakeTime,
                                  ssl = timers.HTTPSHandshakeTime
                              };

            TimeSpan span = timers.ServerGotRequest - timers.FiddlerBeginRequest;
            timings.send = (int)Math.Max(0.0, Math.Round(span.TotalMilliseconds));

            TimeSpan span2 = timers.ServerBeginResponse - timers.ServerGotRequest;
            timings.wait = (int)Math.Max(0.0, Math.Round(span2.TotalMilliseconds));

            TimeSpan span3 = timers.ServerDoneResponse - timers.ServerBeginResponse;
            timings.receive = (int)Math.Max(0.0, Math.Round(span3.TotalMilliseconds));
            return timings;
        }

        private static Response GetResponse(Session session)
        {
            var response = new Response();
            response.status = session.responseCode;
            response.statusText = Utilities.TrimBefore(session.oResponse.headers.HTTPResponseStatus, ' ');
            response.httpVersion = session.oResponse.headers.HTTPVersion;
            response.headersSize = session.oResponse.headers.ByteCount() + 2;
            response.redirectURL = session.oResponse["Location"];
            response.bodySize = session.responseBodyBytes.Length;
            response.headers = GetHeaders(session.oResponse.headers);
            response.cookies = GetCookies(session.oResponse.headers);
            response.content = GetBodyInfo(session);

            return response;
        }

        private static Request GetRequest(Session oS)
        {
            var url = new Uri(oS.fullUrl);
            var host = url.Host;
            var path = url.AbsolutePath;

            var request = new Request();
            request.host = host;
            request.path = path;
            request.method = oS.oRequest.headers.HTTPMethod;
            request.url = oS.fullUrl;
            request.httpVersion = oS.oRequest.headers.HTTPVersion;
            request.headersSize = oS.oRequest.headers.ByteCount() + 2;
            request.bodySize = oS.requestBodyBytes.Length;
            request.headers = GetHeaders(oS.oRequest.headers);
            request.cookies = GetCookies(oS.oRequest.headers);
            request.queryString = GetQueryString(oS.fullUrl);

            if ((oS.requestBodyBytes != null) && (oS.requestBodyBytes.Length > 0))
            {
                request.postData = GetPostData(oS);
            }
            return request;
        }

        private static Content GetBodyInfo(Session oS)
        {
            var content = new Content();

            int num;
            int num2;

            GetDecompressedSize(oS, out num, out num2);
            content.size = num;
            content.compression = num2;
            content.mimeType = oS.oResponse["Content-Type"];

            string mImeType = oS.oResponse.MIMEType;
            bool isMimeTypeTextEquivalent = Utility.IsMimeTypeTextEquivalent(mImeType);
            if (((isMimeTypeTextEquivalent && ("text/plain" == mImeType)) && (oS.responseBodyBytes.Length > 3)) &&
                ((((oS.responseBodyBytes[0] == 0x43) && (oS.responseBodyBytes[1] == 0x57)) &&
                  (oS.responseBodyBytes[2] == 0x53)) ||
                 (((oS.responseBodyBytes[0] == 70) && (oS.responseBodyBytes[1] == 0x4c)) &&
                  (oS.responseBodyBytes[2] == 0x56))))
            {
                isMimeTypeTextEquivalent = false;
            }
            if (isMimeTypeTextEquivalent)
            {
                content.text = oS.GetResponseBodyAsString();
                return content;
            }
            if (oS.responseBodyBytes.Length < MaxBinaryBodyLength)
            {
                content.encoding = "base64";
                content.text = Convert.ToBase64String(oS.responseBodyBytes);
                return content;
            }

            content.comment =
                "Body length exceeded Mocument.Transcoders.HttpArchiveTranscoder.MaxBinaryBodyLength , so body was omitted.";
            return content;
        }

        private static void GetDecompressedSize(Session oSession, out int iExpandedSize, out int iCompressionSavings)
        {
            int length = oSession.responseBodyBytes.Length;
            var arrBody = (byte[])oSession.responseBodyBytes.Clone();
            try
            {
                Utilities.utilDecodeHTTPBody(oSession.oResponse.headers, ref arrBody);
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
            }
            iExpandedSize = arrBody.Length;
            iCompressionSavings = iExpandedSize - length;
        }

        private static PostData GetPostData(Session oS)
        {
            var postData = new PostData();
            string contentType = oS.oRequest["Content-Type"];

            postData.mimeType = Utilities.TrimAfter(contentType, ';');

            if (contentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            {
                postData.@params = GetQueryString("http://fake/path?" + oS.GetRequestBodyAsString());
                return postData;
            }
            postData.text = oS.GetRequestBodyAsString();

            return postData;
        }

        private static List<NameValuePair> GetHeaders(HTTPHeaders headers)
        {
            var list = new List<NameValuePair>();

            foreach (HTTPHeaderItem item in headers)
            {
                list.Add(new NameValuePair
                             {
                                 name = item.Name,
                                 value = item.Value
                             });
            }
            return list;
        }

        private static List<Cookie> GetCookies(HTTPHeaders oHeaders)
        {
            var cookies = new List<Cookie>();

            if (oHeaders is HTTPRequestHeaders)
            {
                string cookiesString = oHeaders["Cookie"];
                if (!string.IsNullOrEmpty(cookiesString))
                {
                    cookies.AddRange(
                        from cookieString in cookiesString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        select cookieString.Trim()
                        into trimmedCookieString
                        where trimmedCookieString.Length >= 1
                        select new Cookie
                                   {
                                       name = Utilities.TrimAfter(trimmedCookieString, '='),
                                       value = Utilities.TrimBefore(trimmedCookieString, '=')

                                       // #TODO - fully parse cookie string
                                       //,
                                       //comment = "",
                                       //domain = "",
                                       //expires = "",
                                       //httpOnly = false,
                                       //path = "",
                                       //secure = false
                                   });
                }
            }
            return cookies;
        }

        private static List<NameValuePair> GetQueryString(string sUri)
        {
            var queryString = new List<NameValuePair>();

            try
            {
                var uri = new Uri(sUri);

                NameValueCollection values = HttpUtility.ParseQueryString(uri.Query);

                queryString.AddRange(from key in values.AllKeys
                                     from value in values.GetValues(key)
                                     select new NameValuePair
                                                {
                                                    name = key,
                                                    value = value
                                                });
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
                // swallow and report problem
            }
            return queryString;
        }

        #endregion

        #region Import





        private static int GetTotalSize(Response response)
        {
            if ((response.headersSize >= 0) && (response.bodySize >= 0))
            {
                // #TODO: i don't understand the condition
                return (response.headersSize + response.bodySize);
            }
            return -1;
        }


        private static byte[] GetRequestFromEntry(Request request)
        {
            string b = request.method;
            string httpVersion = request.httpVersion;
            if (string.IsNullOrEmpty(httpVersion))
            {
                httpVersion = "HTTP/0.0";
            }
            string url = request.url;

            string str4 = GetHeaderStringFromParams(request.headers);
            string str5 = string.Empty;
            if (request.postData != null)
            {
                if (!string.IsNullOrEmpty(request.postData.text))
                {
                    str5 = request.postData.text;
                }
                else if (request.postData.@params != null)
                {
                    str5 = GetStringFromParams(request.postData.@params);
                }
            }
            if (string.Equals("CONNECT", b, StringComparison.OrdinalIgnoreCase))
            {
                url = Utilities.TrimBeforeLast(url, '/');
            }

            string s = string.Format("{0} {1} {2}\r\n{3}\r\n{4}", new object[] { b, url, httpVersion, str4, str5 });

            return CONFIG.oHeaderEncoding.GetBytes(s);
        }

        private static string GetStringFromParams(IEnumerable<NameValuePair> parameters)
        {
            var builder = new StringBuilder();
            bool flag = true;
            foreach (NameValuePair hashtable in parameters)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append("&");
                }

                builder.AppendFormat("{0}={1}",
                                     HttpUtility.UrlEncode(hashtable.name, Encoding.UTF8),
                                     HttpUtility.UrlEncode(hashtable.value, Encoding.UTF8));
            }
            return builder.ToString();
        }

        private static string GetHeaderStringFromParams(IEnumerable<NameValuePair> headers)
        {
            var builder = new StringBuilder();
            foreach (NameValuePair hashtable in headers)
            {
                builder.AppendFormat("{0}: {1}\r\n", hashtable.name, hashtable.value);
            }
            return builder.ToString();
        }

        private static byte[] GetResponseFromEntry(Response response)
        {
            string sHeaders = GetHeaderStringFromParams(response.headers);

            byte[] src = GetBodyArrayFromContent(response.content, sHeaders);

            string s = string.Format("{0} {1} {2}\r\n{3}\r\n",
                                     string.IsNullOrEmpty(response.httpVersion) ? "HTTP/0.0" : response.httpVersion,
                                     response.status,
                                     response.statusText,
                                     sHeaders);

            byte[] bytes = CONFIG.oHeaderEncoding.GetBytes(s);
            var dst = new byte[bytes.Length + src.Length];
            Buffer.BlockCopy(bytes, 0, dst, 0, bytes.Length);
            Buffer.BlockCopy(src, 0, dst, bytes.Length, src.Length);
            return dst;
        }

        private static byte[] GetBodyArrayFromContent(Content content, string headers)
        {
            var writeData = new byte[0];
            if (content != null)
            {
                if (content.text == null)
                {
                    return writeData;
                }

                if (content.encoding != null && ("base64" == content.encoding))
                {
                    // ReSharper disable AssignNullToNotNullAttribute
                    return Convert.FromBase64String(content.text);
                    // ReSharper restore AssignNullToNotNullAttribute
                }

                Encoding encoding = Encoding.UTF8;
                if (content.mimeType != null && (content.mimeType.IndexOf("charset", StringComparison.Ordinal) > -1))
                {
                    Match match = new Regex("charset\\s?=\\s?[\"]?(?<TokenValue>[^\";]*)").Match(content.mimeType);
                    if (match.Success && (match.Groups["TokenValue"] != null))
                    {
                        try
                        {
                            encoding = Encoding.GetEncoding(match.Groups["TokenValue"].Value);
                        }
                        // ReSharper disable RedundantCatchClause
                        catch
                        {
                            throw;
                        }
                        // ReSharper restore RedundantCatchClause
                    }
                }
                // ReSharper disable AssignNullToNotNullAttribute
                writeData = encoding.GetBytes(content.text);
                // ReSharper restore AssignNullToNotNullAttribute
                if (headers.Contains("Content-Encoding") && headers.Contains("gzip"))
                {
                    writeData = Utilities.GzipCompress(writeData);
                }
                if (headers.Contains("Content-Encoding") && headers.Contains("deflate"))
                {
                    writeData = Utilities.DeflaterCompress(writeData);
                }
                if (headers.Contains("Transfer-Encoding") && headers.Contains("chunked"))
                {
                    writeData = Utilities.doChunk(writeData, 2);
                }
            }
            return writeData;
        }

        #endregion
    }
}