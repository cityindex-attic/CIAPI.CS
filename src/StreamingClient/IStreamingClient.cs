using System;

namespace StreamingClient
{
    public interface IStreamingClient
    {
        event EventHandler<MessageEventArgs<object>> MessageRecieved;
        event EventHandler<StatusEventArgs> StatusChanged;
        void Connect();
        void Disconnect();
    }
}