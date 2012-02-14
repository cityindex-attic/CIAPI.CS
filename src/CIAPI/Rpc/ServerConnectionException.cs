using Salient.JsonClient;

namespace CIAPI.Rpc
{
    public class ServerConnectionException : ApiSerializationException
    {
        public ServerConnectionException(string message, string responseText)
            : base(message, responseText)
        {
        }
    }
}