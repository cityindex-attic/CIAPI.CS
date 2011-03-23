using System;

namespace StreamingClient
{
    public interface IStreamingListener
    {
        void Start();
        void Stop();
        string Topic { get; }
    }

    public interface IStreamingListener<TDto> : IStreamingListener where TDto : class
    {
        event EventHandler<MessageEventArgs<TDto>> MessageReceived;
    }
}