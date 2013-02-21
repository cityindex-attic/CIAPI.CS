using System;

namespace CIAPI.StreamingClient
{

 
    
    /// <summary>
    /// 
    /// </summary>
    public interface IStreamingListener :IDisposable
    {
        // TODO: these need to be hidden
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phase"></param>
        void Start(int phase);
        /// <summary>
        /// 
        /// </summary>
        void Stop();


        /// <summary>
        /// 
        /// </summary>
        string Topic { get; }
        /// <summary>
        /// 
        /// </summary>
        string Adapter { get; }
        
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IStreamingListener<TDto> : IStreamingListener where TDto : class
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageEventArgs<TDto>> MessageReceived;
    }
}