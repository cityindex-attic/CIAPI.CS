using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.DTO;

namespace CIAPI.Streaming
{
    public class Client : IStreamingClient
    {
        public Client(Uri streamingUri, Rpc.Client authenticatedClient)
        {
            
        }

        public event EventHandler<MessageEventArgs<string>> MessageRecieved;

        public StreamingListener<TDto, TMessageConverter> Build<TDto, TMessageConverter>(string newsTopic)
            where TDto : class,new()
            where TMessageConverter : IMessageConverter<TDto>, new()
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }
    }
}
