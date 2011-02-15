using Newtonsoft.Json;
using StreamingClient;
using StreamingClient.Websocket;

namespace CIAPI.Streaming.Websocket
{
    internal class StompDtoConverter<T>:IMessageConverter<T>
    {
        public T Convert(object data)
        {
            return JsonConvert.DeserializeObject<T>(((StompMessage) data).Body);
        }
    }
}