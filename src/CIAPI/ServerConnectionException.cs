

using System;
using Salient.ReliableHttpClient;

namespace CIAPI.Rpc
{
    public class ServerConnectionException : ReliableHttpException
    {
        public ServerConnectionException(string message, string responseText)
            : base(message)
        {
            ResponseText = responseText;
        }

        
    }
}