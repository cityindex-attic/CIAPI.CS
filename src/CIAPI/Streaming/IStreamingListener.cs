using System;

namespace CIAPI.Streaming
{
    public interface IStreamingListener<TDto> where TDto : class,new()
    {
        event EventHandler<MessageEventArgs<TDto>> MessageRecieved;
        void Start();
        void Stop();
        
    }
}