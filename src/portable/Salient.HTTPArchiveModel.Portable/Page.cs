using System;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// An exported page
    /// </summary>
    [DataContract]
    public class Page
    {
        public Page()
        {
       
        }

        /// <summary>
        /// startedDateTime [string]
        /// Date and time stamp for the beginning of the page load 
        /// (ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD, e.g. 2009-07-24T19:20:30.45+01:00).
        /// </summary>
        [DataMember(Name = "startedDateTime")]
        public virtual string StartedDateTime { get; set; }

        /// <summary>
        /// id [string] 
        /// Unique identifier of a page within the . Entries use it to refer the parent page.
        /// </summary>
        [DataMember(Name = "id")]
        public virtual string Id { get; set; }

        /// <summary>
        /// title [string]
        /// Page title.
        /// </summary>
        [DataMember(Name = "title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// pageTimings[object]
        /// Detailed timing info about page load.
        /// </summary>
        [DataMember(Name = "pageTimings")]
        public virtual PageTimings PageTimings { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}