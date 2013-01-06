using System;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// Name and version info of used browser.
    /// </summary>
    [Serializable]
    public class VersionInfo
    {
        /// <summary>
        /// name [string] 
        /// Name of the browser used to export the log.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// version [string] 
        /// Version of the browser used to export the log.
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}