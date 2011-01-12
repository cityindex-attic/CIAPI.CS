using System;

namespace CIAPI.Streaming
{
    public class StreamingListener<TDto, TMessageConverter>
        where TDto : class,new()
        where TMessageConverter : IMessageConverter<TDto>, new()
    {
        private readonly string _topic;

        public StreamingListener(string topic, IStreamingClient connection)
        {
            _topic = topic;
            connection.MessageRecieved += (s, e) =>
                {
                    if (MessageRecieved == null) return;
                    if (e.Topic != _topic) return;
                    var messageData = new TMessageConverter().Convert(e.Data);
                    MessageRecieved(this,
                                    new MessageEventArgs<TDto>(e.Topic, messageData));
                };
        }

        public event EventHandler<MessageEventArgs<TDto>> MessageRecieved;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}