using System;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object describes timings for various events (states) fired during the page load.
    /// All times are specified in milliseconds. If a time info is not available appropriate 
    /// field is set to -1.
    /// </summary>
    [DataContract]
    public class PageTimings
    {
        public PageTimings()
        {
            OnContentLoad = -1;
            OnLoad = -1;
        }

        /// <summary>
        /// onContentLoad [number, optional]
        /// Content of the page loaded. Number of milliseconds since page load started 
        /// (page.startedDateTime). Use -1 if the timing does not apply to the current request.
        /// </summary>
        [DataMember(Name = "onContentLoad")]
        public virtual int OnContentLoad { get; set; }

        /// <summary>
        /// onLoad [number,optional]
        /// Page is loaded (onLoad event fired). Number of milliseconds since page load started 
        /// (page.startedDateTime). Use -1 if the timing does not apply to the current request.
        /// </summary>
        [DataMember(Name = "onLoad")]
        public virtual int OnLoad { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}