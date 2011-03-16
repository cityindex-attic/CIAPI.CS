using System;

namespace StreamingClient.Websocket
{
    public class StompListener<T>: IStreamingListener<T> where T : class
    {
        private readonly string _topic;
        private readonly StompOverWebsocketClient _stompClient;

        internal StompListener(string topic, StompOverWebsocketClient stompClient)
        {
            _topic = topic;
            _stompClient = stompClient;
            _stompClient.MessageRecieved+=OnMessageRecieved;
        }

        private void OnMessageRecieved(object sender, MessageEventArgs<object> e)
        {
            if (e.Topic == _topic)
            {
                if (MessageRecieved!=null)
                {
                    T dto = new StompDtoConverter<T>().Convert(e.Data);
                    MessageRecieved(this, new MessageEventArgs<T>(e.Topic, dto));
                }
            }
        }

        public void Start()
        {
            _stompClient.Subscribe(_topic);
        }

        public void Stop()
        {
            _stompClient.Unsubscribe(_topic);
        }

        public event EventHandler<MessageEventArgs<T>> MessageRecieved;
    }
}