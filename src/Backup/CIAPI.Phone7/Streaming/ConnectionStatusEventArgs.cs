using System;

namespace CIAPI.StreamingClient
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStatusEventArgs:EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public ConnectionStatusEventArgs(string message,ConnectionStatus status)
        {
            Message = message;
            Status = status;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConnectionStatus Status { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0}: {1}", Status, Message);
        }
    }
}