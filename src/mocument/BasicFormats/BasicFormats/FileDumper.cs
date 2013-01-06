namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    [ProfferFormat("Raw Files", "Save all downloaded content to a folder.")]
    public class FileDumper : ISessionExporter, IDisposable
    {
        private string _MakeSafeFilename(string sFilename)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (sFilename.IndexOfAny(invalidFileNameChars) < 0)
            {
                return Utilities.TrimAfter(sFilename, 0xff);
            }
            StringBuilder builder = new StringBuilder(sFilename);
            for (int i = 0; i < builder.Length; i++)
            {
                if ((Array.IndexOf<char>(invalidFileNameChars, sFilename[i]) > -1) && (sFilename[i] != '\\'))
                {
                    builder[i] = '-';
                }
            }
            return Utilities.TrimAfter(builder.ToString(), 160);
        }

        public void Dispose()
        {
        }

        public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if (sFormat != "Raw Files")
            {
                return false;
            }
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bool bValue = true;
            bool flag2 = true;
            bool flag3 = false;
            if (dictOptions != null)
            {
                if (dictOptions.ContainsKey("Folder"))
                {
                    folderPath = dictOptions["Folder"] as string;
                    flag3 = true;
                }
                if (dictOptions.ContainsKey("RecreateStructure"))
                {
                    bValue = string.Equals("True", dictOptions["RecreateStructure"] as string, StringComparison.OrdinalIgnoreCase);
                    flag3 = true;
                }
                if (dictOptions.ContainsKey("OpenFolder"))
                {
                    flag2 = string.Equals("True", dictOptions["OpenFolder"] as string, StringComparison.OrdinalIgnoreCase);
                    flag3 = true;
                }
            }
            if (!flag3)
            {
                UIFileExport export = new UIFileExport {
                    txtLocation = { Text = folderPath },
                    cbRecreateFolderStructure = { Checked = FiddlerApplication.Prefs.GetBoolPref("fiddler.exporters.RawFiles.RecreateStructure", true) },
                    cbOpenFolder = { Checked = FiddlerApplication.Prefs.GetBoolPref("fiddler.exporters.RawFiles.OpenFolder", true) }
                };
                this.SetDefaultPath(export.txtLocation, "fiddler.exporters.RawFiles.DefaultPath", folderPath);
                if (export.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
                bValue = export.cbRecreateFolderStructure.Checked;
                flag2 = export.cbOpenFolder.Checked;
                folderPath = export.txtLocation.Text;
                FiddlerApplication.Prefs.SetBoolPref("fiddler.exporters.RawFiles.RecreateStructure", bValue);
                FiddlerApplication.Prefs.SetBoolPref("fiddler.exporters.RawFiles.OpenFolder", flag2);
                FiddlerApplication.Prefs.SetStringPref("fiddler.exporters.RawFiles.DefaultPath", folderPath);
                export.Dispose();
                folderPath = folderPath + @"\Dump-" + DateTime.Now.ToString("MMdd-HH-mm-ss") + @"\";
            }
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception exception)
            {
                FiddlerApplication.ReportException(exception, "Export Failed");
                return false;
            }
            int num = 0;
            foreach (Session session in oSessions)
            {
                try
                {
                    if (session.HTTPMethodIs("CONNECT"))
                    {
                        num++;
                    }
                    else
                    {
                        if ((session.responseBodyBytes != null) && (session.responseBodyBytes.Length > 0))
                        {
                            string str2;
                            if (bValue)
                            {
                                str2 = Utilities.TrimAfter(session.url, '?').Replace('/', '\\');
                                if (str2.EndsWith(@"\"))
                                {
                                    str2 = str2 + session.SuggestedFilename;
                                }
                                if ((str2.Length > 0) && (str2.Length < 260))
                                {
                                    str2 = folderPath + this._MakeSafeFilename(str2);
                                }
                                else
                                {
                                    str2 = folderPath + session.SuggestedFilename;
                                }
                            }
                            else
                            {
                                str2 = folderPath + session.SuggestedFilename;
                            }
                            str2 = Utilities.EnsureUniqueFilename(str2);
                            byte[] responseBodyBytes = session.responseBodyBytes;
                            if (session.oResponse.headers.Exists("Content-Encoding") || session.oResponse.headers.Exists("Transfer-Encoding"))
                            {
                                responseBodyBytes = (byte[]) responseBodyBytes.Clone();
                                Utilities.utilDecodeHTTPBody(session.oResponse.headers, ref responseBodyBytes);
                            }
                            Utilities.WriteArrayToFile(str2, responseBodyBytes);
                        }
                        num++;
                        if (evtProgressNotifications != null)
                        {
                            ProgressCallbackEventArgs e = new ProgressCallbackEventArgs(((float) num) / ((float) oSessions.Length), "Dumped " + num.ToString() + " files to disk.");
                            evtProgressNotifications(null, e);
                            if (e.Cancel)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception exception2)
                {
                    FiddlerApplication.ReportException(exception2, "Failed to generate response file.");
                }
            }
            if (flag2)
            {
                Process.Start("explorer.exe", folderPath);
            }
            return true;
        }

        private void SetDefaultPath(TextBox txtUI, string sPrefName, string sDefaultPath)
        {
            string stringPref = FiddlerApplication.Prefs.GetStringPref(sPrefName, sDefaultPath);
            try
            {
                if (!Directory.Exists(stringPref))
                {
                    stringPref = sDefaultPath;
                }
            }
            catch
            {
                stringPref = sDefaultPath;
            }
            txtUI.Text = stringPref;
        }
    }
}

