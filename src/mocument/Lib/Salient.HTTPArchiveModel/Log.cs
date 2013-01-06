using System;
using System.Collections.Generic;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object represents the root of exported data.
    /// </summary>
    [Serializable]
    public class Log
    {
        public Log()
        {
            entries = new List<Entry>();
        }

        /// <summary>
        /// version [string] 
        /// Version number of the format. If empty, string "1.1" is assumed by default.
        /// </summary>
        public String version { get; set; }

        /// <summary>
        /// creator [object] 
        /// Name and version info of the log creator application
        /// </summary>
        public VersionInfo creator { get; set; }

        /// <summary>
        /// browser [object, optional] 
        /// Name and version info of used browser.
        /// </summary>
        public VersionInfo browser { get; set; }

        /// <summary>
        /// pages [array, optional]
        /// List of all exported (tracked) pages. Leave out this field if 
        /// the application does not support grouping by pages.
        /// </summary>
        public List<Page> pages { get; set; }

        /// <summary>
        /// entries [array]
        /// List of all exported (tracked) requests.
        /// </summary>
        public List<Entry> entries { get;  set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2) 
        /// A comment provided by the user or the application.
        /// </summary>
        public String comment { get; set; }
    }
}