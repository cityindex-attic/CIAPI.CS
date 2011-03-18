using System;

namespace StreamingClient
{
    public interface IStreamingListener
    {
        void Start();
        void Stop();
    }

    public interface IStreamingListener<TDto> : IStreamingListener where TDto : class
    {
        event EventHandler<MessageEventArgs<TDto>> MessageReceived;
    }
}