using System;
using System.Collections;
using System.IO;
using System.Text;

namespace StreamingClient.Websocket
{
    public class StompOverWebsocketConnection : IDisposable
    {
        private readonly object transmissionLock = new object();
        private readonly WebsocketClient websocket;

        public delegate void MessageDelegate(string destination, string body, IDictionary headers);

        public StompOverWebsocketConnection(Uri host, string login, string passcode)
        {
            websocket = new WebsocketClient(host);
            websocket.Connect();

            Transmit("CONNECT", null, null, "login", login, "passcode", passcode);
            Packet ack = Receive();
            if (ack.command != "CONNECTED")
            {
                throw new ApplicationException("Could not connect : " + ack);
            }
        }

        public void Dispose()
        {
            Transmit("DISCONNECT", null, null);
            websocket.Close();
        }

        public void Send(string destination, string body, IDictionary headers)
        {
            Transmit("SEND", body, headers, "destination", destination);
        }

        public void Send(string destination, string body)
        {
            Send(destination, body, null);
        }

        public void Begin()
        {
            Transmit("BEGIN", null, null);
        }

        public void Commit()
        {
            Transmit("COMMIT", null, null);
        }

        public void Abort()
        {
            Transmit("ABORT", null, null);
        }

        public void Subscribe(string destination, IDictionary headers)
        {
            Transmit("SUBSCRIBE", null, headers, "destination", destination);
        }

        public void Subscribe(string destination)
        {
            Subscribe(destination, null);
        }

        public void Unsubscribe(string destination, IDictionary headers)
        {
            // James says: if you supplied a consumerID in the message then you will still be subbed
            Transmit("UNSUBSCRIBE", null, headers, "destination", destination);
        }

        public void Unsubscribe(string destination)
        {
            Unsubscribe(destination, null);
        }

        private void Transmit(string command, string body, IDictionary headers, params string[] additionalHeaderPairs)
        {
            lock (transmissionLock)
            {
                var message = new StringBuilder();
                message.AppendLine(command);
                for (int i = 0; i < additionalHeaderPairs.Length; i += 2)
                {
                    string key = additionalHeaderPairs[i];
                    string val = additionalHeaderPairs[i + 1];
                    message.AppendFormat("{0}:{1}\r\n", key, val);
                    if (headers != null)
                    {
                        headers.Remove(key); // just in case headers dictionary contains duplicate entry
                    }
                }
                if (headers != null)
                {
                    foreach (object key in headers.Keys)
                    {
                        object val = headers[key];
                        message.AppendFormat("{0}:{1}\r\n", key, val);
                    }
                }
                message.AppendLine();
                message.AppendLine(body);
                message.Append('\u0000');
                websocket.SendFrame(message.ToString());
            }
        }

        public StompMessage WaitForMessage()
        {
            Packet packet = Receive();
            if (packet.command == "MESSAGE")
            {
                return new StompMessage((string)packet.headers["destination"], packet.body, packet.headers);
            }
            else
            {
                return null;
            }
        }

        private Packet Receive()
        {
            var response = websocket.RecieveFrame();
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

            Console.Out.WriteLine(packet);
            return packet;
        }

        private class Packet
        {
            public string command;
            public string body;
            public IDictionary headers = new Hashtable();

            public override string ToString()
            {
                StringBuilder result = new StringBuilder();
                result.Append(command).Append(Environment.NewLine);
                foreach (DictionaryEntry entry in headers)
                {
                    result.Append(entry.Key).Append(':').Append(entry.Value).Append(Environment.NewLine);
                }
                result.Append("----").Append(Environment.NewLine); ;
                result.Append(body);
                result.Append("====").Append(Environment.NewLine);
                return result.ToString();
            }
        }

    }
}