using System;
using System.Threading;
using Common.Logging;
using StreamingClient;
using StreamingClient.Websocket;

namespace CIAPI.Streaming.Websocket
{
    public class StompOverWebsocketClient: IStreamingClient
    {
        private readonly Uri _host;
        private readonly string _userName;
        private readonly string _sessionId;
        private StompOverWebsocketConnection _stompConnection;
        public event EventHandler<MessageEventArgs<object>> MessageRecieved;
        public event EventHandler<StatusEventArgs> StatusChanged;
        private Thread _messageListeningThread;
        private readonly ILog _logger = LogManager.GetCurrentClassLogger();

        internal StompOverWebsocketClient(Uri host, string userName, string sessionId)
        {
            _host = host;
            _userName = userName;
            _sessionId = sessionId;
        }

        public void Connect()
        {
            _stompConnection = new StompOverWebsocketConnection(_host);
            _stompConnection.Connect(_userName, _sessionId);
            _messageListeningThread = new Thread(ListenForMessages) {IsBackground = true};
            _messageListeningThread.Start();
        }

        private void ListenForMessages()
        {
            while (true)
            {
                try
                {
                    var msg = _stompConnection.WaitForMessage();
                    if (MessageRecieved != null)
                    {
                        _logger.DebugFormat("Recieved STOMP message on topic: {0}, {1}", msg.Destination, msg.Body);
                        MessageRecieved(this, new MessageEventArgs<object>(msg.Destination, msg));
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }
        }

        public void Disconnect()
        {
            _messageListeningThread.Abort();
            _stompConnection.Dispose();
            _stompConnection = null;
        }

        public IStreamingListener<TDto> BuildListener<TDto>(string topic) where TDto : class, new()
        {
            var listener = new StompListener<TDto>(topic, this);
            return listener;
        }

        internal void Subscribe(string topic)
        {
            _stompConnection.Subscribe(topic);
        }

        internal void Unsubscribe(string topic)
        {
            _stompConnection.Unsubscribe(topic);
        }
    }
}
