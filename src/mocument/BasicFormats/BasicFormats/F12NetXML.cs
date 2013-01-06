namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    [ProfferFormat("IE's F12 NetXML", "Internet Explorer 9 Developer Tools Export Format.")]
    public class F12NetXML : ISessionImporter, IDisposable
    {
        public void Dispose()
        {
        }

        public Session[] ImportSessions(string sFormat, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if (sFormat != "IE's F12 NetXML")
            {
                return null;
            }
            string str = null;
            if ((dictOptions != null) && dictOptions.ContainsKey("Filename"))
            {
                str = dictOptions["Filename"] as string;
            }
            if (string.IsNullOrEmpty(str))
            {
                str = Utilities.ObtainOpenFilename("Import from " + sFormat, "IE's F12 NetXML (*.xml)|*.xml");
            }
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            List<Session> list = null;
            try
            {
                FileStream strmXML = new FileStream(str, FileMode.Open, FileAccess.Read);
                list = HTTPArchiveXML.LoadStream(strmXML, evtProgressNotifications);
                strmXML.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Failed to import...");
                return null;
            }
            return list.ToArray();
        }
    }
}

