namespace CIAPI.StreamingClient
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Disconnected = 0,
        /// <summary>
        /// 
        /// </summary>
        Connecting = 1,
        /// <summary>
        /// 
        /// </summary>
        Connected = 2,
        /// <summary>
        /// 
        /// </summary>
        Streaming = 3,
        /// <summary>
        /// 
        /// </summary>
        Polling = 4,
        /// <summary>
        /// 
        /// </summary>
        Stalled = 5,
        /// <summary>
        /// 
        /// </summary>
        Error = 6
    }
}