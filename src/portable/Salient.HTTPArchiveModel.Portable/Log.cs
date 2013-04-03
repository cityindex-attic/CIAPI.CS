using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object represents the root of exported data.
    /// </summary>
    [DataContract]
    public class Log
    {
        public Log()
        {
            Entries = new List<Entry>();
        }

        /// <summary>
        /// version [string] 
        /// Version number of the format. If empty, string "1.1" is assumed by default.
        /// </summary>
        [DataMember(Name = "version")]
        public virtual String Version { get; set; }

        /// <summary>
        /// creator [object] 
        /// Name and version info of the log creator application
        /// </summary>
        [DataMember(Name = "creator")]
        public virtual VersionInfo Creator { get; set; }

        /// <summary>
        /// browser [object, optional] 
        /// Name and version info of used browser.
        /// </summary>
        [DataMember(Name = "browser")]
        public virtual VersionInfo Browser { get; set; }

        /// <summary>
        /// pages [array, optional]
        /// List of all exported (tracked) pages. Leave out this field if 
        /// the application does not support grouping by pages.
        /// </summary>
        [DataMember(Name = "pages")]
        public virtual List<Page> Pages { get; set; }

        /// <summary>
        /// entries [array]
        /// List of all exported (tracked) requests.
        /// </summary>
        [DataMember(Name = "entries")]
        public virtual List<Entry> Entries { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2) 
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual String Comment { get; set; }
    }
}