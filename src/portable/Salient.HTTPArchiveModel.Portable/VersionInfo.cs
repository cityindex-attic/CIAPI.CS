using System;
using System.Runtime.Serialization;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// Name and version info of used browser.
    /// </summary>
    [DataContract]
    public class VersionInfo
    {
        /// <summary>
        /// name [string] 
        /// Name of the browser used to export the log.
        /// </summary>
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// version [string] 
        /// Version of the browser used to export the log.
        /// </summary>
        [DataMember(Name = "version")]
        public virtual string Version { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}