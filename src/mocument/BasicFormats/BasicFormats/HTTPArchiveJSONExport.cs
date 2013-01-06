using System.Web;

namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    internal class HTTPArchiveJSONExport
    {
        private static int _iMaxBinaryBodyLength;
        private static int _iMaxTextBodyLength;

        private static Hashtable getBodyInfo(Session oS, bool bUseV1dot2Format)
        {
            int num;
            int num2;
            Hashtable hashtable = new Hashtable();
            getDecompressedSize(oS, out num, out num2);
            hashtable.Add("size", num);
            hashtable.Add("compression", num2);
            hashtable.Add("mimeType", oS.oResponse["Content-Type"]);
            if (oS.responseBodyBytes != null)
            {
                string mIMEType = oS.oResponse.MIMEType;
                bool flag = IsMIMETypeTextEquivalent(mIMEType);
                if (((flag && ("text/plain" == mIMEType)) && (oS.responseBodyBytes.Length > 3)) && ((((oS.responseBodyBytes[0] == 0x43) && (oS.responseBodyBytes[1] == 0x57)) && (oS.responseBodyBytes[2] == 0x53)) || (((oS.responseBodyBytes[0] == 70) && (oS.responseBodyBytes[1] == 0x4c)) && (oS.responseBodyBytes[2] == 0x56))))
                {
                    flag = false;
                }
                if (flag)
                {
                    if (oS.responseBodyBytes.Length < _iMaxTextBodyLength)
                    {
                        hashtable.Add("text", oS.GetResponseBodyAsString());
                        return hashtable;
                    }
                    hashtable.Add("comment", "Body length exceeded fiddler.importexport.HTTPArchiveJSON.MaxTextBodyLength, so body was omitted.");
                    return hashtable;
                }
                if (bUseV1dot2Format)
                {
                    if (oS.responseBodyBytes.Length < _iMaxBinaryBodyLength)
                    {
                        hashtable.Add("encoding", "base64");
                        hashtable.Add("text", Convert.ToBase64String(oS.responseBodyBytes));
                        return hashtable;
                    }
                    hashtable.Add("comment", "Body length exceeded fiddler.importexport.HTTPArchiveJSON.MaxBinaryBodyLength, so body was omitted.");
                }
            }
            return hashtable;
        }

        private static ArrayList getCookies(HTTPHeaders oHeaders)
        {
            ArrayList list = new ArrayList();
            if (oHeaders is HTTPRequestHeaders)
            {
                string str = oHeaders["Cookie"];
                if (!string.IsNullOrEmpty(str))
                {
                    foreach (string str2 in str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string sString = str2.Trim();
                        if (sString.Length >= 1)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable.Add("name", Utilities.TrimAfter(sString, '='));
                            hashtable.Add("value", Utilities.TrimBefore(sString, '='));
                            list.Add(hashtable);
                        }
                    }
                }
                return list;
            }
            foreach (HTTPHeaderItem item in oHeaders)
            {
                if (item.Name == "Set-Cookie")
                {
                    Hashtable hashtable2 = new Hashtable();
                    string str4 = item.Value;
                    string str5 = Utilities.TrimAfter(str4, ';');
                    hashtable2.Add("name", Utilities.TrimAfter(str5, '='));
                    hashtable2.Add("value", Utilities.TrimBefore(str5, '='));
                    string str6 = Utilities.TrimBefore(str4, ';');
                    if (!string.IsNullOrEmpty(str6))
                    {
                        DateTime time;
                        if (str6.IndexOf("httpOnly", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            hashtable2.Add("httpOnly", "true");
                        }
                        if (str6.IndexOf("secure", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            hashtable2.Add("_secure", "true");
                        }
                        Regex regex = new Regex("expires\\s?=\\s?[\"]?(?<TokenValue>[^\";]*)");
                        Match match = regex.Match(str6);
                        if ((match.Success && (match.Groups["TokenValue"] != null)) && DateTime.TryParse(match.Groups["TokenValue"].Value, out time))
                        {
                            hashtable2.Add("expires", time.ToString("o"));
                        }
                        regex = new Regex("domain\\s?=\\s?[\"]?(?<TokenValue>[^\";]*)");
                        match = regex.Match(str6);
                        if (match.Success && (match.Groups["TokenValue"] != null))
                        {
                            hashtable2.Add("domain", match.Groups["TokenValue"].Value);
                        }
                        match = new Regex("path\\s?=\\s?[\"]?(?<TokenValue>[^\";]*)").Match(str6);
                        if (match.Success && (match.Groups["TokenValue"] != null))
                        {
                            hashtable2.Add("path", match.Groups["TokenValue"].Value);
                        }
                    }
                    list.Add(hashtable2);
                }
            }
            return list;
        }

        private static void getDecompressedSize(Session oSession, out int iExpandedSize, out int iCompressionSavings)
        {
            if (oSession.responseBodyBytes == null)
            {
                iExpandedSize = iCompressionSavings = 0;
            }
            else
            {
                int length = oSession.responseBodyBytes.Length;
                byte[] arrBody = (byte[]) oSession.responseBodyBytes.Clone();
                try
                {
                    Utilities.utilDecodeHTTPBody(oSession.oResponse.headers, ref arrBody);
                }
                catch (Exception)
                {
                }
                iExpandedSize = arrBody.Length;
                iCompressionSavings = iExpandedSize - length;
            }
        }

        private static ArrayList getHeadersAsArrayList(HTTPHeaders oHeaders)
        {
            ArrayList list = new ArrayList();
            foreach (HTTPHeaderItem item in oHeaders)
            {
                Hashtable hashtable = new Hashtable(2);
                hashtable.Add("name", item.Name);
                hashtable.Add("value", item.Value);
                list.Add(hashtable);
            }
            return list;
        }

        private static int getMilliseconds(Hashtable htTimers, string sMeasure)
        {
            int num;
            object obj2 = htTimers[sMeasure];
            if (obj2 == null)
            {
                return 0;
            }
            if (obj2 is int)
            {
                num = (int) obj2;
            }
            else
            {
                double a = (double) obj2;
                num = (int) Math.Round(a);
            }
            return Math.Max(0, num);
        }

        private static Hashtable getPostData(Session oS)
        {
            Hashtable hashtable = new Hashtable();
            string sString = oS.oRequest["Content-Type"];
            hashtable.Add("mimeType", Utilities.TrimAfter(sString, ';'));
            if (sString.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            {
                hashtable.Add("params", getQueryString("http://fake/path?" + oS.GetRequestBodyAsString()));
                return hashtable;
            }
            hashtable.Add("text", oS.GetRequestBodyAsString());
            return hashtable;
        }

        private static ArrayList getQueryString(string sURI)
        {
            ArrayList list = new ArrayList();
            try
            {
                Uri uri = new Uri(sURI);
                NameValueCollection values = HttpUtility.ParseQueryString(uri.Query);
                foreach (string str in values.AllKeys)
                {
                    foreach (string str2 in values.GetValues(str))
                    {
                        Hashtable hashtable = new Hashtable();
                        hashtable.Add("name", str);
                        hashtable.Add("value", str2);
                        list.Add(hashtable);
                    }
                }
            }
            catch
            {
                return new ArrayList();
            }
            return list;
        }

        private static Hashtable getRequest(Session oS)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("method", oS.oRequest.headers.HTTPMethod);
            hashtable.Add("url", oS.fullUrl);
            hashtable.Add("httpVersion", oS.oRequest.headers.HTTPVersion);
            hashtable.Add("headersSize", oS.oRequest.headers.ByteCount() + 2);
            hashtable.Add("bodySize", oS.requestBodyBytes.Length);
            hashtable.Add("headers", getHeadersAsArrayList(oS.oRequest.headers));
            hashtable.Add("cookies", getCookies(oS.oRequest.headers));
            hashtable.Add("queryString", getQueryString(oS.fullUrl));
            if ((oS.requestBodyBytes != null) && (oS.requestBodyBytes.Length > 0))
            {
                hashtable.Add("postData", getPostData(oS));
            }
            return hashtable;
        }

        private static Hashtable getResponse(Session oS, bool bUseV1dot2Format)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("status", oS.responseCode);
            hashtable.Add("statusText", Utilities.TrimBefore(oS.oResponse.headers.HTTPResponseStatus, ' '));
            hashtable.Add("httpVersion", oS.oResponse.headers.HTTPVersion);
            hashtable.Add("headersSize", oS.oResponse.headers.ByteCount() + 2);
            hashtable.Add("redirectURL", oS.oResponse.headers["Location"]);
            hashtable.Add("bodySize", (oS.responseBodyBytes == null) ? 0 : oS.responseBodyBytes.Length);
            hashtable.Add("headers", getHeadersAsArrayList(oS.oResponse.headers));
            hashtable.Add("cookies", getCookies(oS.oResponse.headers));
            hashtable.Add("content", getBodyInfo(oS, bUseV1dot2Format));
            return hashtable;
        }

        private static Hashtable getTimings(SessionTimers oTimers, bool bUseV1dot2Format)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("blocked", -1);
            hashtable.Add("dns", oTimers.DNSTime);
            hashtable.Add("connect", oTimers.TCPConnectTime + oTimers.HTTPSHandshakeTime);
            if (bUseV1dot2Format)
            {
                hashtable.Add("ssl", oTimers.HTTPSHandshakeTime);
            }
            TimeSpan span = (TimeSpan) (oTimers.ServerGotRequest - oTimers.FiddlerBeginRequest);
            hashtable.Add("send", Math.Max(0.0, Math.Round(span.TotalMilliseconds)));
            TimeSpan span2 = (TimeSpan) (oTimers.ServerBeginResponse - oTimers.ServerGotRequest);
            hashtable.Add("wait", Math.Max(0.0, Math.Round(span2.TotalMilliseconds)));
            TimeSpan span3 = (TimeSpan) (oTimers.ServerDoneResponse - oTimers.ServerBeginResponse);
            hashtable.Add("receive", Math.Max(0.0, Math.Round(span3.TotalMilliseconds)));
            return hashtable;
        }

        private static int getTotalTime(Hashtable htTimers)
        {
            int num = 0;
            num += getMilliseconds(htTimers, "blocked");
            num += getMilliseconds(htTimers, "dns");
            num += getMilliseconds(htTimers, "connect");
            num += getMilliseconds(htTimers, "send");
            num += getMilliseconds(htTimers, "wait");
            return (num + getMilliseconds(htTimers, "receive"));
        }

        private static bool IsMIMETypeTextEquivalent(string sMIME)
        {
            if (sMIME.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (sMIME.StartsWith("application/"))
            {
                if (sMIME.StartsWith("application/javascript", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMIME.StartsWith("application/x-javascript", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMIME.StartsWith("application/ecmascript", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMIME.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMIME.StartsWith("application/xhtml+xml", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMIME.StartsWith("application/xml", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return sMIME.StartsWith("image/svg+xml", StringComparison.OrdinalIgnoreCase);
        }

        internal static bool WriteStream(StreamWriter swOutput, Session[] oSessions, bool bUseV1dot2Format, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications, int iMaxTextBodyLength, int iMaxBinaryBodyLength)
        {
            _iMaxTextBodyLength = iMaxTextBodyLength;
            _iMaxBinaryBodyLength = iMaxBinaryBodyLength;
            Hashtable hashtable = new Hashtable();
            hashtable.Add("version", bUseV1dot2Format ? "1.2" : "1.1");
            hashtable.Add("pages", new ArrayList(0));
            if (bUseV1dot2Format)
            {
                hashtable.Add("comment", "exported @ " + DateTime.Now.ToString());
            }
            Hashtable hashtable2 = new Hashtable();
            hashtable2.Add("name", "Fiddler");
            hashtable2.Add("version", Application.ProductVersion);
            if (bUseV1dot2Format)
            {
                hashtable2.Add("comment", "http://www.fiddler2.com");
            }
            hashtable.Add("creator", hashtable2);
            ArrayList list = new ArrayList();
            int num = 0;
            foreach (Session session in oSessions)
            {
                try
                {
                    if (session.state < SessionStates.Done)
                    {
                        continue;
                    }
                    Hashtable hashtable3 = new Hashtable();
                    hashtable3.Add("startedDateTime", session.Timers.ClientBeginRequest.ToString("o"));
                    hashtable3.Add("request", getRequest(session));
                    hashtable3.Add("response", getResponse(session, bUseV1dot2Format));
                    hashtable3.Add("cache", new Hashtable());
                    Hashtable htTimers = getTimings(session.Timers, bUseV1dot2Format);
                    hashtable3.Add("time", getTotalTime(htTimers));
                    hashtable3.Add("timings", htTimers);
                    if (bUseV1dot2Format)
                    {
                        string str = session["ui-comments"];
                        if (!string.IsNullOrEmpty(str))
                        {
                            hashtable3.Add("comment", session["ui-comments"]);
                        }
                        string hostIP = session.m_hostIP;
                        if (!string.IsNullOrEmpty(str) && !session.isFlagSet(SessionFlags.SentToGateway))
                        {
                            hashtable3.Add("serverIPAddress", session.m_hostIP);
                        }
                        hashtable3.Add("connection", session.clientPort.ToString());
                    }
                    list.Add(hashtable3);
                }
                catch (Exception exception)
                {
                    FiddlerApplication.ReportException(exception, "Failed to Export Session");
                }
                num++;
                if (evtProgressNotifications != null)
                {
                    ProgressCallbackEventArgs e = new ProgressCallbackEventArgs(((float) num) / ((float) oSessions.Length), "Wrote " + num.ToString() + " sessions to HTTPArchive.");
                    evtProgressNotifications(null, e);
                    if (e.Cancel)
                    {
                        return false;
                    }
                }
            }
            hashtable.Add("entries", list);
            Hashtable json = new Hashtable();
            json.Add("log", hashtable);
            swOutput.WriteLine(JSON.JsonEncode(json));
            return true;
        }
    }
}

