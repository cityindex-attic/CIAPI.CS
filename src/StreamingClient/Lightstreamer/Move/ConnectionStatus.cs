namespace StreamingClient
{
    public enum ConnectionStatus
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Streaming = 3,
        Polling = 4,
        Stalled = 5,
        Error = 6
    }
}