using System;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object describes timings for various events (states) fired during the page load.
    /// All times are specified in milliseconds. If a time info is not available appropriate 
    /// field is set to -1.
    /// </summary>
    [Serializable]
    public class PageTimings
    {
        public PageTimings()
        {
            onContentLoad = -1;
            onLoad = -1;
        }

        /// <summary>
        /// onContentLoad [number, optional]
        /// Content of the page loaded. Number of milliseconds since page load started 
        /// (page.startedDateTime). Use -1 if the timing does not apply to the current request.
        /// </summary>
        public int onContentLoad { get; set; }

        /// <summary>
        /// onLoad [number,optional]
        /// Page is loaded (onLoad event fired). Number of milliseconds since page load started 
        /// (page.startedDateTime). Use -1 if the timing does not apply to the current request.
        /// </summary>
        public int onLoad { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}