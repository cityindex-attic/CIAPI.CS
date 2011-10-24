using System;

namespace StreamingClient
{
    public interface IStreamingListener
    {
        void Start(int phase);
        void Stop();
        string Topic { get; }
        string DataAdapter { get; }
        
    }

    public interface IStreamingListener<TDto> : IStreamingListener where TDto : class
    {
        event EventHandler<MessageEventArgs<TDto>> MessageReceived;
    }
}