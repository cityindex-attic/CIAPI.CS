namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    [ProfferFormat("HTTPArchive v1.1", "A lossy JSON-based HTTP traffic archive format. Standard is documented @ http://groups.google.com/group/http-archive-specification/web/har-1-1-spec"), ProfferFormat("HTTPArchive v1.2", "A lossy JSON-based HTTP traffic archive format. Standard is documented @ http://groups.google.com/group/http-archive-specification/web/har-1-2-spec")]
    public class HTTPArchiveFormatExport : ISessionExporter, IDisposable
    {
        public void Dispose()
        {
        }

        public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if ((sFormat != "HTTPArchive v1.1") && (sFormat != "HTTPArchive v1.2"))
            {
                return false;
            }
            string str = null;
            int iMaxBinaryBodyLength = FiddlerApplication.Prefs.GetInt32Pref("fiddler.importexport.HTTPArchiveJSON.MaxBinaryBodyLength", 0x8000);
            int iMaxTextBodyLength = FiddlerApplication.Prefs.GetInt32Pref("fiddler.importexport.HTTPArchiveJSON.MaxTextBodyLength", 0x16);
            if (dictOptions != null)
            {
                if (dictOptions.ContainsKey("Filename"))
                {
                    str = dictOptions["Filename"] as string;
                }
                if (dictOptions.ContainsKey("MaxTextBodyLength"))
                {
                    iMaxTextBodyLength = (int) dictOptions["MaxTextBodyLength"];
                }
                if (dictOptions.ContainsKey("MaxBinaryBodyLength"))
                {
                    iMaxBinaryBodyLength = (int) dictOptions["MaxBinaryBodyLength"];
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                str = Utilities.ObtainSaveFilename("Export As " + sFormat, "HTTPArchive JSON (*.har)|*.har");
            }
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    StreamWriter swOutput = new StreamWriter(str, false, Encoding.UTF8);
                    HTTPArchiveJSONExport.WriteStream(swOutput, oSessions, sFormat == "HTTPArchive v1.2", evtProgressNotifications, iMaxTextBodyLength, iMaxBinaryBodyLength);
                    swOutput.Close();
                    return true;
                }
                catch (Exception exception)
                {
                    FiddlerApplication.ReportException(exception, "Failed to save HTTPArchive");
                    return false;
                }
            }
            return false;
        }
    }
}

