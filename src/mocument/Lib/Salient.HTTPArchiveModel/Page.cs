using System;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// An exported page
    /// </summary>
    [Serializable]
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
        public string startedDateTime { get; set; }

        /// <summary>
        /// id [string] 
        /// Unique identifier of a page within the . Entries use it to refer the parent page.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// title [string]
        /// Page title.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// pageTimings[object]
        /// Detailed timing info about page load.
        /// </summary>
        public PageTimings pageTimings { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}