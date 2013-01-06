namespace Fiddler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    internal class HTTPArchiveXML
    {
        private static string _getEntityBodyFromNode(XmlReader oEntry)
        {
            while (oEntry.Read())
            {
                string str;
                if (((oEntry.NodeType == XmlNodeType.Element) && ((str = oEntry.Name) != null)) && (str == "text"))
                {
                    return oEntry.ReadElementContentAsString();
                }
            }
            return string.Empty;
        }

        private static string _getHeadersFromEntry(XmlReader oEntry)
        {
            StringBuilder builder = new StringBuilder();
            while (oEntry.Read())
            {
                string str;
                if ((oEntry.NodeType == XmlNodeType.Element) && ((str = oEntry.Name) != null))
                {
                    if (!(str == "name"))
                    {
                        if (str == "value")
                        {
                            goto Label_0052;
                        }
                    }
                    else
                    {
                        builder.Append(oEntry.ReadElementContentAsString());
                        builder.Append(": ");
                    }
                }
                continue;
            Label_0052:
                builder.Append(oEntry.ReadElementContentAsString());
                builder.Append("\r\n");
            }
            return builder.ToString();
        }

        private static byte[] _getRequestFromEntry(XmlReader oEntry)
        {
            string str = null;
            string str2 = "NONE";
            string str3 = "HTTP/0.0";
            string str4 = null;
            string str5 = null;
            while (oEntry.Read())
            {
                string str7;
                if ((oEntry.NodeType == XmlNodeType.Element) && ((str7 = oEntry.Name) != null))
                {
                    if (!(str7 == "url"))
                    {
                        if (str7 == "method")
                        {
                            goto Label_0087;
                        }
                        if (str7 == "httpVersion")
                        {
                            goto Label_009E;
                        }
                        if (str7 == "headers")
                        {
                            goto Label_00B5;
                        }
                        if (str7 == "postData")
                        {
                            goto Label_00C3;
                        }
                    }
                    else
                    {
                        str = oEntry.ReadElementContentAsString();
                    }
                }
                continue;
            Label_0087:
                str2 = oEntry.ReadElementContentAsString();
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = "NONE";
                }
                continue;
            Label_009E:
                str3 = oEntry.ReadElementContentAsString();
                if (string.IsNullOrEmpty(str3))
                {
                    str3 = "HTTP/0.0";
                }
                continue;
            Label_00B5:
                str4 = _getHeadersFromEntry(oEntry.ReadSubtree());
                continue;
            Label_00C3:
                str5 = _getEntityBodyFromNode(oEntry.ReadSubtree());
            }
            string s = string.Format("{0} {1} {2}\r\n{3}\r\n{4}", new object[] { str2, str, str3, str4, str5 });
            return CONFIG.oHeaderEncoding.GetBytes(s);
        }

        private static byte[] _getResponseFromEntry(XmlReader oEntry)
        {
            string a = null;
            string str2 = null;
            string str3 = null;
            string str4 = null;
            string str5 = null;
            while (oEntry.Read())
            {
                string str7;
                if ((oEntry.NodeType == XmlNodeType.Element) && ((str7 = oEntry.Name) != null))
                {
                    if (!(str7 == "status"))
                    {
                        if (str7 == "statusText")
                        {
                            goto Label_007F;
                        }
                        if (str7 == "httpVersion")
                        {
                            goto Label_0088;
                        }
                        if (str7 == "headers")
                        {
                            goto Label_009F;
                        }
                        if (str7 == "content")
                        {
                            goto Label_00AD;
                        }
                    }
                    else
                    {
                        a = oEntry.ReadElementContentAsString();
                    }
                }
                continue;
            Label_007F:
                str2 = oEntry.ReadElementContentAsString();
                continue;
            Label_0088:
                str3 = oEntry.ReadElementContentAsString();
                if (string.IsNullOrEmpty(str3))
                {
                    str3 = "HTTP/0.0";
                }
                continue;
            Label_009F:
                str4 = _getHeadersFromEntry(oEntry.ReadSubtree());
                continue;
            Label_00AD:
                str5 = _getEntityBodyFromNode(oEntry.ReadSubtree());
            }
            if (string.Equals(a, "(cache)", StringComparison.OrdinalIgnoreCase))
            {
                a = "0";
                str2 = "CACHED-" + str2;
            }
            string s = string.Format("{0} {1} {2}\r\n{3}\r\n{4}", new object[] { str3, a, str2, str4, str5 });
            return CONFIG.oHeaderEncoding.GetBytes(s);
        }

        private static Session _getSessionFromEntry(XmlReader oEntry)
        {
            try
            {
                byte[] arrRequest = null;
                byte[] arrResponse = null;
                Hashtable htTimers = null;
                DateTime now = DateTime.Now;
                while (oEntry.Read())
                {
                    string str;
                    if ((oEntry.NodeType == XmlNodeType.Element) && ((str = oEntry.Name) != null))
                    {
                        if (!(str == "request"))
                        {
                            if (str == "response")
                            {
                                goto Label_0074;
                            }
                            if (str == "startedDateTime")
                            {
                                goto Label_0082;
                            }
                            if (str == "timings")
                            {
                                goto Label_0099;
                            }
                        }
                        else
                        {
                            arrRequest = _getRequestFromEntry(oEntry.ReadSubtree());
                        }
                    }
                    continue;
                Label_0074:
                    arrResponse = _getResponseFromEntry(oEntry.ReadSubtree());
                    continue;
                Label_0082:
                    if (!DateTime.TryParse(oEntry.ReadElementContentAsString(), out now))
                    {
                        now = DateTime.Now;
                    }
                    continue;
                Label_0099:
                    htTimers = _getTimings(oEntry.ReadSubtree());
                }
                if ((arrRequest == null) || (arrResponse == null))
                {
                    return null;
                }
                Session session = new Session(arrRequest, arrResponse, SessionFlags.ResponseStreamed);
                if (htTimers != null)
                {
                    session.Timers.DNSTime = getMilliseconds(htTimers, "dns");
                    session.Timers.TCPConnectTime = getMilliseconds(htTimers, "connect");
                    session.Timers.HTTPSHandshakeTime = getMilliseconds(htTimers, "ssl");
                    session.Timers.ClientConnected = session.Timers.ClientBeginRequest = session.Timers.ClientDoneRequest = now;
                    session.Timers.ServerConnected = session.Timers.FiddlerBeginRequest = now.AddMilliseconds((double) (((getMilliseconds(htTimers, "blocked") + session.Timers.DNSTime) + session.Timers.TCPConnectTime) + session.Timers.HTTPSHandshakeTime));
                    session.Timers.ServerGotRequest = session.Timers.FiddlerBeginRequest.AddMilliseconds((double) getMilliseconds(htTimers, "send"));
                    session.Timers.ServerBeginResponse = now.AddMilliseconds((double) getMilliseconds(htTimers, "wait"));
                    session.Timers.ServerDoneResponse = session.Timers.ServerBeginResponse.AddMilliseconds((double) getMilliseconds(htTimers, "receive"));
                    session.Timers.ClientBeginResponse = session.Timers.ClientDoneResponse = session.Timers.ServerDoneResponse;
                }
                return session;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Hashtable _getTimings(XmlReader oEntry)
        {
            Hashtable hashtable = new Hashtable();
            oEntry.ReadStartElement();
            while (oEntry.Read())
            {
                if (oEntry.NodeType == XmlNodeType.Element)
                {
                    double num;
                    string name = oEntry.Name;
                    if (double.TryParse(oEntry.ReadElementContentAsString(), out num))
                    {
                        hashtable.Add(name, num);
                    }
                }
            }
            return hashtable;
        }

        private static int getMilliseconds(Hashtable htTimers, string sMeasure)
        {
            double? nullable = htTimers[sMeasure] as double?;
            if (!nullable.HasValue)
            {
                return 0;
            }
            int num = (int) Math.Round(nullable.Value);
            return Math.Max(0, num);
        }

        internal static List<Session> LoadStream(Stream strmXML, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            List<Session> list = new List<Session>();
            XmlTextReader reader = new XmlTextReader(strmXML);
            int num = 0;
            while (reader.Read())
            {
                string str;
                if (((reader.NodeType == XmlNodeType.Element) && ((str = reader.Name) != null)) && (str == "entry"))
                {
                    Session item = _getSessionFromEntry(reader.ReadSubtree());
                    if (item != null)
                    {
                        list.Add(item);
                        num++;
                        if (evtProgressNotifications != null)
                        {
                            evtProgressNotifications(null, new ProgressCallbackEventArgs(0f, "Imported " + num.ToString() + " sessions."));
                        }
                    }
                }
            }
            return list;
        }
    }
}

