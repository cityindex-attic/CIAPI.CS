namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    [ProfferFormat("HTTPArchive", "A lossy JSON-based HTTP traffic archive format. Learn more @ http://groups.google.com/group/http-archive-specification")]
    public class HTTPArchiveFormatImport : ISessionImporter, IDisposable
    {
        public void Dispose()
        {
        }

        public Session[] ImportSessions(string sFormat, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if (sFormat == "HTTPArchive")
            {
                string str = null;
                if ((dictOptions != null) && dictOptions.ContainsKey("Filename"))
                {
                    str = dictOptions["Filename"] as string;
                }
                if (string.IsNullOrEmpty(str))
                {
                    str = Utilities.ObtainOpenFilename("Import " + sFormat, "HTTPArchive JSON (*.har)|*.har");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        List<Session> listSessions = new List<Session>();
                        StreamReader oSR = new StreamReader(str, Encoding.UTF8);
                        HTTPArchiveJSONImport.LoadStream(oSR, listSessions, evtProgressNotifications);
                        oSR.Close();
                        return listSessions.ToArray();
                    }
                    catch (Exception exception)
                    {
                        FiddlerApplication.ReportException(exception, "Failed to import HTTPArchive");
                        return null;
                    }
                }
            }
            return null;
        }
    }
}

