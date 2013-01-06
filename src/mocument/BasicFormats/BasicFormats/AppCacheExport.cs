namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    [ProfferFormat("HTML5 AppCache Manifest", "HTML5 allows creation of Application Caches based on a manifest. See http://diveintohtml5.org/offline.html")]
    public class AppCacheExport : ISessionExporter, IDisposable
    {
        public void Dispose()
        {
        }

        public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if (sFormat != "HTML5 AppCache Manifest")
            {
                return false;
            }
            bool flag = false;
            string str = null;
            if (string.IsNullOrEmpty(str))
            {
                AppCacheOptions options = new AppCacheOptions();
                List<string> list = new List<string>();
                options.lvItems.BeginUpdate();
                foreach (Session session in oSessions)
                {
                    if ((!session.HTTPMethodIs("CONNECT") && (session.responseCode >= 200)) && ((session.responseCode <= 0x18f) && !list.Contains(session.fullUrl)))
                    {
                        list.Add(session.fullUrl);
                        string text = (session.oResponse.headers != null) ? Utilities.TrimAfter(session.oResponse.headers["Content-Type"], ";") : string.Empty;
                        ListViewItem item = options.lvItems.Items.Add(session.fullUrl);
                        item.SubItems.Add((session.responseBodyBytes != null) ? session.responseBodyBytes.Length.ToString() : "0");
                        item.SubItems.Add(text);
                        if (session.HTTPMethodIs("POST"))
                        {
                            item.Checked = true;
                        }
                        if (text.IndexOf("script", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            item.Group = options.lvItems.Groups["lvgScript"];
                        }
                        else if (text.IndexOf("image/", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            item.Group = options.lvItems.Groups["lvgImages"];
                        }
                        else if (text.IndexOf("html", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            item.Group = options.lvItems.Groups["lvgMarkup"];
                        }
                        else if (text.IndexOf("css", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            item.Group = options.lvItems.Groups["lvgCSS"];
                        }
                        else
                        {
                            item.Group = options.lvItems.Groups["lvgOther"];
                        }
                        item.Tag = session;
                    }
                }
                options.lvItems.EndUpdate();
                if (options.lvItems.Items.Count > 0)
                {
                    options.lvItems.FocusedItem = options.lvItems.Items[0];
                }
                if (DialogResult.OK != options.ShowDialog(FiddlerApplication.UI))
                {
                    return flag;
                }
                str = Utilities.ObtainSaveFilename("Export As " + sFormat, "AppCache Manifest (*.appcache)|*.appcache");
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        List<string> list2 = new List<string>();
                        List<string> list3 = new List<string>();
                        string str3 = options.txtBase.Text.Trim();
                        if (str3.Length == 0)
                        {
                            str3 = null;
                        }
                        for (int i = 0; i < options.lvItems.Items.Count; i++)
                        {
                            string str4 = options.lvItems.Items[i].Text;
                            if (((str3 != null) && (str4.Length > str3.Length)) && str4.StartsWith(str3))
                            {
                                str4 = str4.Substring(str3.Length);
                            }
                            if (options.lvItems.Items[i].Checked)
                            {
                                list3.Add(str4);
                            }
                            else
                            {
                                list2.Add(str4);
                            }
                        }
                        StringBuilder builder = new StringBuilder();
                        builder.AppendFormat("CACHE MANIFEST\r\n# Generated: {0}\r\n\r\n", DateTime.Now.ToString());
                        if (str3 != null)
                        {
                            builder.AppendFormat("# Deploy so that URLs are relative to: {0}\r\n\r\n", str3);
                        }
                        if (list2.Count > 0)
                        {
                            builder.Append("CACHE:\r\n");
                            builder.Append(string.Join("\r\n", list2.ToArray()));
                            builder.Append("\r\n");
                        }
                        if (options.cbNetworkFallback.Checked || (list3.Count > 0))
                        {
                            builder.Append("\r\nNETWORK:\r\n");
                            if (options.cbNetworkFallback.Checked)
                            {
                                builder.Append("*\r\n");
                            }
                            builder.Append(string.Join("\r\n", list3.ToArray()));
                        }
                        File.WriteAllText(str, builder.ToString());
                        Process.Start("notepad.exe", str);
                        return true;
                    }
                    catch (Exception exception)
                    {
                        FiddlerApplication.ReportException(exception, "Failed to save MeddlerScript");
                        return false;
                    }
                }
                options.Dispose();
            }
            return flag;
        }
    }
}

