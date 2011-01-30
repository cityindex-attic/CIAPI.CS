using System;

namespace CIAPI.Streaming
{
    public interface IStreamingClient
    {
        event EventHandler<MessageEventArgs<object>> MessageRecieved;
        event EventHandler<StatusEventArgs> StatusChanged;
        void Connect();
        void Disconnect();
    }
}