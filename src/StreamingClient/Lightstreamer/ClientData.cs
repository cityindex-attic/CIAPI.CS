using Lightstreamer.DotNet.Client;
namespace StreamingClient.Lightstreamer
{
    public class ClientData
    {
        public LSClient client;
        public string dataAdapter;
        public ConnectionInfo connection;
        public bool connected;
    }    
}
