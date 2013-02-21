using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Fiddler;
using Mocument.DataAccess;
using Mocument.Model;
using Mocument.Transcoders;
using Newtonsoft.Json;
using Salient.HTTPArchiveModel;

namespace Mocument.ReverseProxyServer
{
    public class Server
    {
        private static readonly StateManager StateManager = new StateManager();
        private static readonly ConcurrentDictionary<Session, SessionInfo> RecordCache =
            new ConcurrentDictionary<Session, SessionInfo>();

        public int Port
        {
            get { return _port; }
        }
        public int SSLPort
        {
            get { return _sslPort; }
        }
        public string HostName
        {
            get { return _hostName; }
        }
        Proxy _oSecureEndpoint;
        private readonly int _port;
        private readonly bool _secured;
        private readonly IStore _store;
        private readonly int _sslPort;
        private readonly string _hostName;


        public Server(int port, int sslPort, string hostName, bool secured, IStore dataStore)
        {
            _hostName = hostName;
            _sslPort = sslPort;
            _port = port;
            _secured = secured;
         
            _store = dataStore;
        }
        public void ImportTape(string json)
        {
            Tape t = JsonConvert.DeserializeObject<Tape>(json);
            _store.Insert(t);
        }
        public string ExportTape(string id)
        {
            var t = _store.Select(id);
            var json = JsonConvert.SerializeObject(t, Formatting.Indented);
            return json;
        }

        public void Start()
        {

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Replace("file:\\", "");
            if (!path.EndsWith(@"\")) path += @"\";
            path += "FiddlerRoot.cer";

 
            FiddlerApplication.oDefaultClientCertificate = new X509Certificate(path);
            FiddlerApplication.BeforeRequest += ProcessBeginRequest;

            FiddlerApplication.AfterSessionComplete += ProcessEndResponse;
            CONFIG.IgnoreServerCertErrors = true;
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.ForgetStreamedData", false);
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);



            _oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(_sslPort, true, _hostName);
            if (null == _oSecureEndpoint)
            {

                throw new Exception("could not start up secure endpoint");
            }
            FiddlerApplication.Startup(_port, false, true, true);
        }

        public void Stop()
        {
            FiddlerApplication.BeforeRequest -= ProcessBeginRequest;

            FiddlerApplication.AfterSessionComplete -= ProcessEndResponse;

            if (null != _oSecureEndpoint)
            {
                _oSecureEndpoint.Dispose();
                _oSecureEndpoint = null;
            }

            Console.WriteLine("shutting down fiddler application");
            FiddlerApplication.Shutdown();
            Console.WriteLine("fiddler application shut down");
            Console.WriteLine("sleeping 500ms");
            Thread.Sleep(500);
            Console.WriteLine("server stopped");
        }


        private void ProcessEndResponse(Session oS)
        {

            {
                if (RecordCache.ContainsKey(oS))
                {
                    if (oS.state != SessionStates.Done)
                    {
                        // dirty: #TODO: report and discard
                        return;
                    }

                    SessionInfo info;
                    RecordCache.TryRemove(oS, out info);
                    string tapeId = info.UserId + "." + info.TapeId;
                    Tape tape = _store.Select(tapeId);
                    if (tape == null)
                    {
                        tape = new Tape
                                   {
                                       Id = tapeId
                                   };
                        _store.Insert(tape);
                    }
                    Entry entry = HttpArchiveTranscoder.Export(oS);
                    Entry matched = _store.MatchEntry(tapeId, entry);
                    if (matched == null)
                    {
                        tape.log.entries.Add(entry);
                        _store.Update(tape);
                    }
                }
            }
        }

        private void ProcessBeginRequest(Session oS)
        {
            var info = new SessionInfo(oS);

            oS.host = info.Host;
            oS.PathAndQuery = info.PathAndQuery;

            switch (info.Type)
            {
                case SessionType.Record:
                    RecordSession(oS, info);
                    break;

                case SessionType.Playback:
                    PlaybackSession(oS, info);
                    break;
                case SessionType.InvalidMimeType:
                    oS.utilCreateResponseAndBypassServer();
                    oS.responseCode = 500;
                    oS.utilSetResponseBody("Invalid MIME type");

                    break;
                case SessionType.Export:
                    oS.utilCreateResponseAndBypassServer();
                    oS.responseCode = 200;
                    // #TODO: set content-type etc
                    Tape tape = _store.Select(info.UserId + "." + info.TapeId);
                    if (tape == null)
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.responseCode = 404;
                        oS.utilSetResponseBody("Tape not found");
                        return;
                    }
                    oS.oResponse.headers["Content-Type"] = "text/json";
                    oS.utilSetResponseBody(JsonConvert.SerializeObject(tape, Formatting.Indented));


                    break;

            }
        }

        private void PlaybackSession(Session oS, SessionInfo info)
        {
            try
            {
                string tapeId = info.UserId + "." + info.TapeId;
                Tape tape = _store.Select(tapeId);
                if (tape == null)
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.responseCode = 404;
                    oS.utilSetResponseBody("Tape not found");
                    return;
                }


                // time to find matching session
                Entry entry = HttpArchiveTranscoder.Export(oS, true);

                Entry matchedEntry = _store.MatchEntry(tapeId, entry);


                if (matchedEntry == null)
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.responseCode = 404;
                    oS.utilSetResponseBody("Matching entry not found");
                }
                else
                {
                    Session matchedSession = HttpArchiveTranscoder.Import(matchedEntry);
                    oS.utilCreateResponseAndBypassServer();
                    // #TODO: figger me out
                    // odd, fiddler is compressing respose when it is not compressed from server
                    //oS.responseBodyBytes = matchedSession.responseBodyBytes;

                    oS.utilSetResponseBody(matchedEntry.response.content.text);
                    oS.oResponse.headers = (HTTPResponseHeaders)matchedSession.oResponse.headers.Clone();

                    // #TODO: figger me out
                    // we are sending text back. so whatever became of these 2 bytes?
                    // a newline trimmed somewhere?
                    int length = matchedEntry.response.content.text.Length;
                    int length2 = oS.responseBodyBytes.Length;
                    oS.oResponse.headers["Content-Length"] = length2.ToString();
                    // #TODO: figger me out
                    oS.oResponse.headers.Remove("Content-Encoding");
                }
            }
            catch
            {
                oS.utilCreateResponseAndBypassServer();
                oS.responseCode = 500;
                oS.utilSetResponseBody("Exception occurred");

                throw;
            }
        }

        private void RecordSession(Session oS, SessionInfo info)
        {
            try
            {
                if (_secured)
                {
                    Tape tape = _store.Select(info.UserId + "." + info.TapeId);
                    if (tape == null)
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.responseCode = 404;
                        oS.utilSetResponseBody("Tape not found");
                        return;
                    }
                    if (!tape.OpenForRecording)
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.responseCode = 412;
                        oS.utilSetResponseBody("Tape is not open for recording");
                        return;
                    }
                    string ip = GetClientIp(oS);
                    if (ip != tape.AllowedIpAddress)
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.responseCode = 403;
                        oS.utilSetResponseBody("IP " + GetClientIp(oS) + " not allowed to record.");
                        return;
                    }
                }
                oS.bBufferResponse = true;
                RecordCache.TryAdd(oS, info);
            }
            catch
            {
                oS.utilCreateResponseAndBypassServer();
                oS.responseCode = 500;
                oS.utilSetResponseBody("Exception occurred");
            }
        }

        private static string GetClientIp(Session oS)
        {
            string ip = oS.oRequest.pipeClient.Address.ToString();
            if (ip.StartsWith("::ffff:"))
            {
                ip = ip.Substring(7);
            }
            return ip;
        }

        /// <summary>
        /// From http://cassinidev.codeplex.com/SourceControl/changeset/view/94877#2280469
        /// 
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
        public static int GetAvailablePort(int rangeStart, int rangeEnd, IPAddress ip, bool includeIdlePorts, params int[] excludePorts)
        {

            IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();

            // if the ip we want a port on is an 'any' or loopback port we need to exclude all ports that are active on any IP
            Func<IPAddress, bool> isIpAnyOrLoopBack = i => IPAddress.Any.Equals(i) ||
                                                           IPAddress.IPv6Any.Equals(i) ||
                                                           IPAddress.Loopback.Equals(i) ||
                                                           IPAddress.IPv6Loopback.
                                                               Equals(i);
            // get all active ports on specified IP. 
            var excludedPorts = new List<ushort>( );
            if (null != excludePorts)
            {
                excludedPorts.AddRange(excludePorts.Select(Convert.ToUInt16));
            }

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
    }
}