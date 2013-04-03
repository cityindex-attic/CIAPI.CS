using System;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// A POCO implementation of the HTTP Archive specification v 1.2
    /// http://www.softwareishard.com/blog/har-12-spec/
    /// 
    /// NOTE: property names are intentionally camel-cased to provide
    /// spec compliant round trip JSON serialization.
    /// 
    /// </summary>
    [DataContract]
    public class HTTPArchive
    {
        public HTTPArchive()
        {
            Log = new Log();
        }

        /// <summary>
        /// This object represents the root of exported data.
        /// </summary>
        [DataMember(Name = "log")]
        public virtual Log Log { get; set; }
    }
}