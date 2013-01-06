using System;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object describes various phases within request-response round trip. 
    /// All times are specified in milliseconds.
    /// </summary>
     [Serializable]
    public class Timings
    {
        public Timings()
        {
            blocked = -1;
            dns = -1;
            connect = -1;
            ssl = -1;
        }

        /// <summary>
        /// blocked [number, optional]
        /// Time spent in a queue waiting for a network connection. Use -1 if the 
        /// timing does not apply to the current request.
        /// </summary>
        public int blocked { get; set; }

        /// <summary>
        /// dns [number, optional]
        /// DNS resolution time. The time required to resolve a host name. Use -1 if the 
        /// timing does not apply to the current request.
        /// </summary>
        public int dns { get; set; }

        /// <summary>
        /// connect [number, optional]
        /// Time required to create TCP connection. Use -1 if the timing does not apply to 
        /// the current request.
        /// </summary>
        public int connect { get; set; }

        /// <summary>
        /// send [number]
        /// Time required to send HTTP request to the server.
        /// </summary>
        public int send { get; set; }

        /// <summary>
        /// wait [number]
        /// Waiting for a response from the server.
        /// </summary>
        public int wait { get; set; }

        /// <summary>
        /// receive [number]
        /// Time required to read entire response from the server (or cache).
        /// </summary>
        public int receive { get; set; }


        /// <summary>
        /// ssl [number, optional] (new in 1.2)
        /// Time required for SSL/TLS negotiation. If this field is defined then the time is 
        /// also included in the connect field (to ensure backward compatibility with HAR 1.1). 
        /// Use -1 if the timing does not apply to the current request.
        /// </summary>
        public int ssl { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}