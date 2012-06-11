using System;

namespace CIAPI.StreamingClient
{

 
    
    public interface IStreamingListener :IDisposable
    {
        // TODO: these need to be hidden
        void Start(int phase);
        void Stop();


        string Topic { get; }
        string Adapter { get; }
        
        
    }

    public interface IStreamingListener<TDto> : IStreamingListener where TDto : class
    {
        event EventHandler<MessageEventArgs<TDto>> MessageReceived;
    }
}