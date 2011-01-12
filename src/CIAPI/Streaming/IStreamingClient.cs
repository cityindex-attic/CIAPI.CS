using System;

namespace CIAPI.Streaming
{
    public interface IStreamingClient
    {
        event EventHandler<MessageEventArgs<string>> MessageRecieved;
    }
}