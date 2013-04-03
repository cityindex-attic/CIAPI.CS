using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using CIAPI.Rpc;
using Mocument.DataAccess;
using Mocument.ReverseProxyServer;
using NUnit.Framework;
using Newtonsoft.Json;
using Salient.ReflectiveLoggingAdapter;

namespace CIAPI.RecordedTests.Infrastructure
{
    /// <summary>
    /// We want to be able to switch between record and playback with a config switch
    /// </summary>
    [TestFixture]
    public abstract class CIAPIRecordingFixtureBase
    {
        static CIAPIRecordingFixtureBase()
        {
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
   => new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            
        }

        private MocumentMode _mocumentMode;
        private string _userName;
        private string _password;
        private string _mocumentKey;
        private string _apiUrl;
        private string _apiKey;
        private string _streamingUrl;
        private Server _server;
        private IStore _trafficLog;
        private string _mocumentDataPath;

        protected internal IStore TrafficLog
        {
            get { return _trafficLog; }
        }

        protected internal string ApiKey
        {
            get { return _apiKey; }
        }
        protected internal string UserName
        {
            get { return _userName; }
        }

        protected internal string Password
        {
            get { return _password; }
        }

        [TestFixtureTearDown]
        public virtual void FixtureTeardown()
        {
            if (null != _server)
            {
                _server.Stop();
            }
        }

        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {


            GetMode(GetType().GetCustomAttributes(typeof(MocumentModeOverrideAttribute), true));

            _mocumentDataPath = ConfigurationManager.AppSettings["mocumentDataPath"];
            _mocumentDataPath = Path.GetFullPath(_mocumentDataPath);
            _mocumentKey = ConfigurationManager.AppSettings["mocumentKey"];
            _userName = ConfigurationManager.AppSettings["apiUserName"];
            _password = ConfigurationManager.AppSettings["apiPassword"];
            _apiUrl = ConfigurationManager.AppSettings["apiRpcUrl"];
            _streamingUrl = ConfigurationManager.AppSettings["apiStreamingUrl"];
            _apiKey = ConfigurationManager.AppSettings["apiKey"];
            _server = CreateServer();

        }

        private void GetMode(object[] att)
        {
            _mocumentMode = att.Length > 0
                                ? ((MocumentModeOverrideAttribute) att[0]).Mode
                                : ((MocumentMode)
                                   Enum.Parse(typeof (MocumentMode), ConfigurationManager.AppSettings["mocumentMode"], true));

            switch (_mocumentMode)
            {
                case MocumentMode.Record:
                case MocumentMode.Play:
                    break;
                default:
                    throw new ConfigurationErrorsException("invalid Mocument.Mode. expect record or play");
            }
        }

   
        protected  Uri BuildUri(string tapeName,string u=null)
        {
            string tapeId = tapeName;
            var apiUri = u ?? _apiUrl;

            string api = apiUri.Substring(new Uri(apiUri).Scheme.Length + 3);
            string scheme;
            int port;
            if (new Uri(apiUri).Scheme.ToLower() == "https")
            {
                scheme = "https://";
                port = _server.SSLPort;

            }
            else
            {
                scheme = "http://";
                port = _server.Port;
 
            }
      
            string url = scheme + "localhost.:" + port + "/" + _mocumentMode.ToString().ToLower() + "/" + _mocumentKey + "/" + tapeId +
                         "/" + api;
            return new Uri(url);
        }

        private IStore CreateStore()
        {
            if (!Directory.Exists(_mocumentDataPath))
            {
                Directory.CreateDirectory(_mocumentDataPath);
            }
            string typename = GetType().Name;


            string path = Path.Combine(_mocumentDataPath, typename + ".har");
            switch (_mocumentMode)
            {
                case MocumentMode.Record:
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    break;
                case MocumentMode.Play:
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("could not open data file for fixture", path);
                    }
                    break;
                default:
                    throw new ConfigurationErrorsException("invalid Mocument.Mode. expect record or play");
            }
            _trafficLog = new JsonFileStore(path);
            return _trafficLog;
        }


        private Server CreateServer()
        {
            int port = Server.GetAvailablePort(32000, 33000, IPAddress.Loopback, false);
            int sslPort = Server.GetAvailablePort(32000, 33000, IPAddress.Loopback, false, port);
            IStore store = CreateStore();
            var server = new Server(port, sslPort, "localhost.", false, store);
            server.Start();
            return server;
        }

        protected internal string GetErrorInfo(Exception exception)
        {
            var json = JsonConvert.SerializeObject(TrafficLog.List().First(), Formatting.Indented);

            var errorInfo = string.Format("\r\n---\r\n{0}\r\n---\r\n{1}", exception, json);
            return errorInfo;
        }


        protected Client BuildRpcClient(string tapeId)
        {
            // WARNING: do not nest or otherwise refactor this method
            // buildUri is looking back 2 stack frames to get the method that called this


            var rpcClient = new Client(BuildUri(tapeId), new Uri(_streamingUrl), _apiKey);
            rpcClient.LogIn(_userName, _password);
            return rpcClient;
        }
        protected Client BuildUnauthenticatedRpcClient(string tapeId)
        {
            // WARNING: do not nest or otherwise refactor this method
            // buildUri is looking back 2 stack frames to get the method that called this

            var rpcClient = new Client(BuildUri(tapeId), new Uri(_streamingUrl), _apiKey);
            return rpcClient;
        }
    }
}