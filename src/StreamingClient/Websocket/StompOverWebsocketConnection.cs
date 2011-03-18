using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StreamingClient.Websocket
{
    public class StompOverWebsocketConnection : IDisposable
    {
        private readonly object _transmissionLock = new object();
        private readonly IWebsocketClient _websocketClient;

        public StompOverWebsocketConnection(Uri host): 
            this(new WebsocketClient(host))
        {
        }

        public StompOverWebsocketConnection(IWebsocketClient websocketClient)
        {
            _websocketClient = websocketClient;
        }

        public void Connect(string userName, string password)
        {
            _websocketClient.Connect();
            TransmitPacket("CONNECT", null, 
                new Dictionary<string, string>
                    {
                        {"login", userName}, 
                        {"passcode", password}
                    });
            var ack = ReceivePacket();
            if (ack.command != "CONNECTED")
            {
                throw new ApplicationException("Could not connect : " + ack);
            }
        }

        public void Dispose()
        {
            TransmitPacket("DISCONNECT", null, null);
            _websocketClient.Close();
        }

        public void Send(string destination, string body, IDictionary headers)
        {
            TransmitPacket("SEND", body, AddHeader(headers, "destination", destination));
        }

        public void Send(string destination, string body)
        {
            Send(destination, body, null);
        }

        public void Begin()
        {
            TransmitPacket("BEGIN", null, null);
        }

        public void Commit()
        {
            TransmitPacket("COMMIT", null, null);
        }

        public void Abort()
        {
            TransmitPacket("ABORT", null, null);
        }

        public void Subscribe(string destination, IDictionary headers)
        {
            TransmitPacket("SUBSCRIBE", null, AddHeader(headers, "destination", destination));
        }

        public void Subscribe(string destination)
        {
            Subscribe(destination, null);
        }

        public void Unsubscribe(string destination, IDictionary headers)
        {
            // James says: if you supplied a consumerID in the message then you will still be subbed
            TransmitPacket("UNSUBSCRIBE", null, AddHeader(headers, "destination", destination));
        }

        public void Unsubscribe(string destination)
        {
            Unsubscribe(destination, null);
        }

        private IDictionary AddHeader(IDictionary headers, string key, string value)
        {
            if (headers == null) headers = new Dictionary<string, string>();
            headers.Remove(key);
            headers.Add(key, value);

            return headers;
        }
        private void TransmitPacket(string command, string body, IDictionary headers)
        {
            lock (_transmissionLock)
            {
                var message = new StringBuilder();
                message.AppendLine(command);
                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                    {
                        var val = headers[key];
                        message.AppendFormat("{0}:{1}\r\n", key, val);
                    }
                }
                message.AppendLine();
                message.AppendLine(body);
                message.Append('\u0000');
                _websocketClient.SendFrame(message.ToString());
            }
        }

        public StompMessage WaitForMessage()
        {
            var packet = ReceivePacket();
            if (packet.command != "MESSAGE") 
                throw new InvalidDataException(string.Format("Expected packet command MESSAGE, but received packet with command: {0}", packet.command));

            return new StompMessage((string)packet.headers["destination"], packet.body, packet.headers);
        }

        private Packet ReceivePacket()
        {
            var response = _websocketClient.ReceiveFrame();
            var stringReader = new StringReader(response);
            Packet packet = new Packet();
            packet.command = stringReader.ReadLine(); // MESSAGE, ERROR or RECEIPT

            //return if command =~ /\A\s*\Z/

            string line;
            while ((line = stringReader.ReadLine()) != "")
            {
                string[] split = line.Split(new char[] { ':' }, 2);
                packet.headers[split[0]] = split[1];
            }

            packet.body = stringReader.ReadToEnd().TrimEnd('\r', '\n');

            return packet;
        }

        private class Packet
        {
            public string command;
            public string body;
            public IDictionary headers = new Hashtable();
        }

    }
}