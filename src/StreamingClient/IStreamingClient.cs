using System;

namespace StreamingClient
{
    public interface IStreamingClient
    {
        event EventHandler<MessageEventArgs<object>> MessageReceived;
        event EventHandler<StatusEventArgs> StatusChanged;
        void Connect();
        void Disconnect();
    }
}