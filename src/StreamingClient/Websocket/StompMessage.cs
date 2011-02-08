using System.Collections;

namespace StreamingClient.Websocket
{
    public class StompMessage
    {
        private readonly string body;
        private readonly string destination;
        private readonly IDictionary headers;
 
        public StompMessage(string destination, string body, IDictionary headers)
        {
            this.body = body;
            this.headers = headers;
        }
 
        public string Body
        {
            get { return body; }
        }
 
        public string Destination
        {
            get { return destination; }
        }
 
        public string this[string key]
        {
            get { return (string) headers[key]; }
        }
    }
}