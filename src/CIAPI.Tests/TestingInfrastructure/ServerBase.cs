using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace CIAPI.Tests.TestingInfrastructure
{
    public abstract class ServerBase
    {
        private readonly int _port;
        private readonly int _recieveBufferSize;
        private bool _listen;
        private TcpListener _listener;
        public bool Debug;
        public int Port
        {
            get { return _port; }
        }

        private void LogMessage(string message)
        {
            if (!Debug)
            {
                return;
            }
            Trace.WriteLine("TESTSERVER\r\n" + message + "\r\n");
        }
        protected ServerBase(int port, int recieveBufferSize, bool debug)
        {
            Debug = debug;
            _port = port;
            _recieveBufferSize = recieveBufferSize;
        }

        public void Stop()
        {
            _listen = false;
            _listener.Stop();
        }

        public void Start()
        {
            _listen = true;
            try
            {
                _listener = new TcpListener(IPAddress.Loopback, _port);
                _listener.Start();
                LogMessage("Server started on " + _port);
                var th = new Thread(Listen);
                th.Start();
            }
            catch (Exception e)
            {
                LogMessage("Error starting server: \r\n" + e);
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
                        LogMessage("Socket Error cannot Send Packet");
                    else
                    {
                        LogMessage("No. of bytes sent " + count);
                    }
                }
                else
                    LogMessage("Connection Dropped....");
            }
            catch (Exception e)
            {
                LogMessage("Error Occurred :  " + e.ToString());
            }
        }


        private void Listen()
        {
            while (_listen)
            {
                Socket socket;
                try
                {
                    socket = _listener.AcceptSocket();

                }
                catch (SocketException ex)
                {
                    if (ex.Message.Contains("A blocking operation was interrupted by a call to WSACancelBlockingCall"))
                    {
                        return;
                    }
                    throw;
                }


                if (socket.Connected)
                {
                    try
                    {
                        LogMessage("Socket connected");


                        var buffer = new Byte[_recieveBufferSize];
                        StringBuilder sb = new StringBuilder();
                        int count = -1;
                        string reqText;

                        count = socket.Receive(buffer, buffer.Length, 0);
                        reqText = Encoding.ASCII.GetString(buffer, 0, count);
                        sb.Append(reqText);
                        var pattern = new Regex("Content-Length: (?<length>\\d+)", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                        int contentLength;
                        int bodyLength;
                        if (pattern.IsMatch(reqText))
                        {
                            contentLength = int.Parse(pattern.Match(reqText).Groups["length"].Value);
                            if (contentLength > 0)
                            {
                                int pos = reqText.IndexOf("\r\n\r\n");

                                bodyLength = reqText.Length - (pos + 4);
                                while (bodyLength < contentLength)
                                {

                                    count = socket.Receive(buffer, buffer.Length, 0);
                                    reqText = Encoding.ASCII.GetString(buffer, 0, count);
                                    bodyLength = bodyLength + reqText.Length;
                                    sb.Append(reqText);
                                }

                            }
                        }

                        reqText = sb.ToString();
                        LogMessage("SERVER RECEVIED:\n" + reqText);



                        var request = new RequestInfo(reqText);


                        ResponseInfo response = HandleRequest(request);

                        byte[] responseBytes = Encoding.ASCII.GetBytes(response.ToString());

                        WriteToSocket(responseBytes, ref socket);
                        LogMessage("SERVER SENT:\n" + response.ToString());
                    }
                    catch (Exception ex)
                    {

                        ResponseInfo response = new ResponseInfo()
                                                    {
                                                        Body = ex.ToString(),
                                                        Status = "503 Internal Server Error"
                                                    };
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response.ToString());
                        WriteToSocket(responseBytes, ref socket);
                        LogMessage("SERVER SENT:\n" + response.ToString());

                    }
                    socket.Close();
                    LogMessage("Socket Closed");
                }
            }

        }

        public static int GetAvailablePort()
        {
            return GetAvailablePort(9000, 30000, IPAddress.Loopback, false);
        }
        /// <summary>
        /// Returns first available port on the specified IP address. 
        /// The port scan excludes ports that are open on ANY loopback adapter. 
        /// 
        /// If the address upon which a port is requested is an 'ANY' address all 
        /// ports that are open on ANY IP are excluded.
        /// </summary>
        /// <param name="rangeStart"></param>
        /// <param name="rangeEnd"></param>
        /// <param name="ip">The IP address upon which to search for available port.</param>
        /// <param name="includeIdlePorts">If true includes ports in TIME_WAIT state in results. 
        /// TIME_WAIT state is typically cool down period for recently released ports.</param>
        /// <returns></returns>
        public static int GetAvailablePort(int rangeStart, int rangeEnd, IPAddress ip, bool includeIdlePorts)
        {
            IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();

            // if the ip we want a port on is an 'any' or loopback port we need to exclude all ports that are active on any IP
            Func<IPAddress, bool> isIpAnyOrLoopBack = i => IPAddress.Any.Equals(i) ||
                                                           IPAddress.IPv6Any.Equals(i) ||
                                                           IPAddress.Loopback.Equals(i) ||
                                                           IPAddress.IPv6Loopback.
                                                               Equals(i);
            // get all active ports on specified IP. 
            var excludedPorts = new List<ushort>();

            // if a port is open on an 'any' or 'loopback' interface then include it in the excludedPorts
            excludedPorts.AddRange(from n in ipProps.GetActiveTcpConnections()
                                   where
                                       n.LocalEndPoint.Port >= rangeStart &&
                                       n.LocalEndPoint.Port <= rangeEnd && (
                                                                               isIpAnyOrLoopBack(ip) ||
                                                                               n.LocalEndPoint.Address.Equals(ip) ||
                                                                               isIpAnyOrLoopBack(n.LocalEndPoint.Address)) &&
                                       (!includeIdlePorts || n.State != TcpState.TimeWait)
                                   select (ushort)n.LocalEndPoint.Port);

            excludedPorts.AddRange(from n in ipProps.GetActiveTcpListeners()
                                   where n.Port >= rangeStart && n.Port <= rangeEnd && (
                                                                                           isIpAnyOrLoopBack(ip) ||
                                                                                           n.Address.Equals(ip) ||
                                                                                           isIpAnyOrLoopBack(n.Address))
                                   select (ushort)n.Port);

            excludedPorts.AddRange(from n in ipProps.GetActiveUdpListeners()
                                   where n.Port >= rangeStart && n.Port <= rangeEnd && (
                                                                                           isIpAnyOrLoopBack(ip) ||
                                                                                           n.Address.Equals(ip) ||
                                                                                           isIpAnyOrLoopBack(n.Address))
                                   select (ushort)n.Port);

            excludedPorts.Sort();

            for (int port = rangeStart; port <= rangeEnd; port++)
            {
                if (!excludedPorts.Contains((ushort)port))
                {
                    return port;
                }
            }

            return 0;
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
                string[] lines = request.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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