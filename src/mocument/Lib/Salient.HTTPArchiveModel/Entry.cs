using System;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object represents an array with all exported HTTP requests. 
    /// Sorting entries by startedDateTime (starting from the oldest) is preferred 
    /// way how to export data since it can make importing faster. 
    /// However the reader application should always make sure the array is 
    /// sorted (if required for the import).
    /// </summary>
    [Serializable]
    public class Entry
    {
        public Entry()
        {
            time = -1;
 
        }

        /// <summary>
        /// pageref [string, unique, optional]
        /// Reference to the parent page. Leave out this field if the application does not 
        /// support grouping by pages.
        /// </summary>
        public string pageref { get; set; }

        /// <summary>
        /// startedDateTime [string]
        /// Date and time stamp of the request start (ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD).
        /// </summary>
        public string startedDateTime { get; set; }

        /// <summary>
        /// time [number] 
        /// Total elapsed time of the request in milliseconds. 
        /// This is the sum of all timings available in the timings object 
        /// (i.e. not including -1 values) 
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// request [object] 
        /// Detailed info about the request.
        /// </summary>
        public Request request { get; set; }

        /// <summary>
        /// response [object] 
        /// Detailed info about the response.
        /// </summary>
        public Response response { get; set; }

        /// <summary>
        /// cache [object]
        /// Info about cache usage.
        /// </summary>
        public Cache cache { get; set; }

        /// <summary>
        /// timings [object]
        /// Detailed timing info about request/response round trip.
        /// </summary>
        public Timings timings { get; set; }

        /// <summary>
        /// serverIPAddress [string, optional] (new in 1.2)
        /// IP address of the server that was connected (result of DNS resolution).
        /// </summary>
        public string serverIPAddress { get; set; }

        /// <summary>
        /// connection [string, optional] (new in 1.2)
        /// Unique ID of the parent TCP/IP connection, can be the client port number. 
        /// Note that a port number doesn't have to be unique identifier in cases where 
        /// the port is shared for more connections. 
        /// If the port isn't available for the application, any other unique connection ID 
        /// can be used instead (e.g. connection index). 
        /// Leave out this field if the application doesn't support this info.
        /// </summary>
        public string connection { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}