namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    [ProfferFormat("WCAT Script", "WCAT request-replay scripts are loaded by Microsoft's Web Capacity Analysis Tool, available from http://www.iis.net/community/Performance")]
    public class WCATExport : ISessionExporter, IDisposable
    {
        public void Dispose()
        {
        }

        private void EmitRequestHeaderEntry(StringBuilder sb, string headername, string headervalue)
        {
            sb.AppendLine("      setheader");
            sb.AppendLine("      {");
            sb.AppendLine("        name=\"" + headername + "\";");
            sb.AppendLine("        value=\"" + headervalue.Replace("\"", "\\\"") + "\";");
            sb.AppendLine("      }");
        }

        private void EmitScenarioHeader(StringBuilder sb)
        {
            sb.Append("scenario\r\n{\r\n  name    = \"Fiddler-Generated WCAT Script\";\r\n  warmup      = 30;\r\n  duration    = 120;\r\n  cooldown    = 10;\r\n\r\n  default\r\n  {\r\n    version = HTTP11;\r\n    setheader\r\n    {\r\n      name    = \"Connection\";\r\n      value   = \"keep-alive\";\r\n    }\r\n    close = ka;\r\n  }\r\n");
            sb.AppendLine("");
            sb.AppendLine("  transaction                        ");
            sb.AppendLine("  {                                  ");
            sb.AppendLine("    id = \"1\";     ");
            sb.AppendLine("    weight = 1;");
        }

        public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if (sFormat != "WCAT Script")
            {
                return false;
            }
            string str = null;
            if ((dictOptions != null) && dictOptions.ContainsKey("Filename"))
            {
                str = dictOptions["Filename"] as string;
            }
            if (string.IsNullOrEmpty(str))
            {
                str = Utilities.ObtainSaveFilename("Export As " + sFormat, "WCAT Script (*.wcat)|*.wcat");
            }
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    this.EmitScenarioHeader(sb);
                    int num = 0;
                    foreach (Session session in oSessions)
                    {
                        if (session.HTTPMethodIs("GET") || session.HTTPMethodIs("POST"))
                        {
                            sb.AppendLine("    request");
                            sb.AppendLine("    {");
                            if (session.HTTPMethodIs("POST"))
                            {
                                sb.AppendLine("      verb = POST;");
                                sb.AppendFormat("      postdata = \"{0}\";", Encoding.UTF8.GetString(session.requestBodyBytes).Replace("\"", "\\\""));
                            }
                            sb.AppendFormat("      url     = \"{0}\";", session.PathAndQuery.Replace("\"", "\\\""));
                            foreach (HTTPHeaderItem item in session.oRequest.headers)
                            {
                                this.EmitRequestHeaderEntry(sb, item.Name, item.Value);
                            }
                            sb.AppendLine("    }");
                            num++;
                            if (evtProgressNotifications != null)
                            {
                                ProgressCallbackEventArgs e = new ProgressCallbackEventArgs(((float) num) / ((float) oSessions.Length), "Added " + num.ToString() + " sessions to WCAT Script.");
                                evtProgressNotifications(null, e);
                                if (e.Cancel)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    sb.AppendLine("  }\r\n}");
                    File.WriteAllText(str, sb.ToString());
                    return true;
                }
                catch (Exception exception)
                {
                    FiddlerApplication.ReportException(exception, "Failed to save WCAT Script");
                    return false;
                }
            }
            return false;
        }
    }
}

