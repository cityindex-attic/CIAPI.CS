using Newtonsoft.Json;

namespace StreamingClient.Websocket
{
    internal class StompDtoConverter<T>:IMessageConverter<T>
    {
        public T Convert(object data)
        {
            return JsonConvert.DeserializeObject<T>(((StompMessage) data).Body);
        }
    }
}