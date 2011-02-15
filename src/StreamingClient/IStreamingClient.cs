using System;

namespace StreamingClient
{
    public interface IStreamingClient
    {
        event EventHandler<MessageEventArgs<object>> MessageRecieved;
        event EventHandler<StatusEventArgs> StatusChanged;
        void Connect();
        void Disconnect();
        IStreamingListener<TDto> BuildListener<TDto>(string topic) where TDto : class, new();
    }
}