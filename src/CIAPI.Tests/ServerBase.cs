using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace CIAPI.Tests
{
    public abstract class ServerBase
    {
        private readonly int _port;
        private readonly int _recieveBufferSize;
        private bool _listen;
        private TcpListener _listener;

        public int Port
        {
            get { return _port; }
        }
        protected ServerBase(int port, int recieveBufferSize)
        {
            _port = port;
            _recieveBufferSize = recieveBufferSize;
        }

        public void Stop()
        {
            _listen = false;
        }

        public void Start()
        {
            _listen = true;
            try
            {
                _listener = new TcpListener(IPAddress.Loopback, _port);
                _listener.Start();
                Console.WriteLine("Server started on " + _port);
                var th = new Thread(Listen);
                th.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error starting server: \r\n" + e);
            }
        }


        private void WriteToSocket(Byte[] data, ref Socket socket)
        {
            try
            {
                if (socket.Connected)
                {
                    int count;
                    if ((count = socket.Send(data, data.Length, 0)) == -1)
                        Console.WriteLine("Socket Error cannot Send Packet");
                    else
                    {
                        Console.WriteLine("No. of bytes send {0}", count);
                    }
                }
                else
                    Console.WriteLine("Connection Dropped....");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred : {0} ", e);
            }
        }


        private void Listen()
        {
            while (_listen)
            {
                Socket socket = _listener.AcceptSocket();


                if (socket.Connected)
                {
                    Console.WriteLine("Socket connected");


                    var buffer = new Byte[_recieveBufferSize];
                    int count = socket.Receive(buffer, buffer.Length, 0);
                    if (count == _recieveBufferSize)
                    {
                        throw new Exception("RecieveBufferSize is too small");
                    }

                    var request = new RequestInfo(Encoding.ASCII.GetString(buffer));

                    ResponseInfo response = HandleRequest(request);

                    byte[] responseBytes = Encoding.ASCII.GetBytes(response.ToString());

                    WriteToSocket(responseBytes, ref socket);

                    socket.Close();
                    Console.WriteLine("Socket Closed");
                }
            }
        }

        public abstract ResponseInfo HandleRequest(RequestInfo request);

        #region Nested type: RequestEventArgs

        public class RequestEventArgs : EventArgs
        {
            public RequestInfo Request;
            public ResponseInfo Response;
        }

        #endregion

        #region Nested type: RequestInfo

        public class RequestInfo
        {
            public string Body = "";
            public NameValueCollection Headers = new NameValueCollection();
            public string Method = "";
            public NameValueCollection Parameters = new NameValueCollection();
            public string Route = "";

            public RequestInfo(string request)
            {
                ParseRequest(request);
            }

            private void ParseRequest(string request)
            {
                string[] lines = request.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                // first line is request descriptor

                string line = lines[0].Trim();
                Method = line.Substring(0, line.IndexOf(' '));
                Route = line.Substring(Method.Length, line.LastIndexOf("HTTP/1.1", StringComparison.Ordinal) - Method.Length).Trim();
                int queryIndex = Route.IndexOf('?');

                if (queryIndex > 0)
                {
                    string queryString = Route.Substring(queryIndex + 1);
                    Route = Route.Substring(0, queryIndex);
                    Parameters = HttpUtility.ParseQueryString(queryString);
                }
                int lineIndex = 1;
                bool isBody = false;


                while (lineIndex < lines.Length)
                {
                    line = lines[lineIndex].Trim('\0');
                    if (string.IsNullOrEmpty(line) && !isBody)
                    {
                        isBody = true;
                    }
                    else
                    {
                        if (isBody)
                        {
                            Body = Body + line + "\r\n";
                        }
                        else
                        {
                            string key = line.Substring(0, line.IndexOf(':'));
                            string value = line.Substring(key.Length + 2);
                            Headers[key] = value;
                        }
                    }

                    lineIndex++;
                }
                Body = Body.Trim();
            }
        }

        #endregion

        #region Nested type: ResponseInfo

        public class ResponseInfo
        {
            public string Body = "";
            public NameValueCollection Headers = new NameValueCollection();
            public string Status = "200 OK";

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("HTTP/1.1 {0}", Status));

                foreach (string key in Headers)
                {
                    sb.AppendLine(string.Format("{0}:{1}", key, Headers[key]));
                }
                if (!string.IsNullOrEmpty(Body))
                {
                    sb.AppendLine();
                    sb.Append(Body);
                }

                return sb.ToString();
            }
        }

        #endregion
    }
}