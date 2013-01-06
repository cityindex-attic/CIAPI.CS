using System;
using System.Collections.Generic;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object describes posted data, if any (embedded in request object).
    /// </summary>
    [Serializable]
    public class PostData
    {
        /// <summary>
        /// mimeType [string]
        /// Mime type of posted data.
        /// </summary>
        public string mimeType { get; set; }

        /// <summary>
        /// params [array]
        /// List of posted parameters (in case of URL encoded parameters).
        /// </summary>
        public List<NameValuePair> @params { get; set; }

        /// <summary>
        /// text [string]
        /// Plain text posted data
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}