using System;
using System.Collections;
using System.Threading;
using System.IO;
using System.Net.Sockets;

using log4net;

using Lightstreamer.DotNet.Server;

[assembly:log4net.Config.XmlConfigurator()]

namespace Lightstreamer.Adapters.StockListDemo {

	public class ServerMain {
		private static ILog _log= LogManager.GetLogger("Lightstreamer.Adapters.StockListDemo.ServerMain");

		public const string PREFIX1= "-";
		public const string PREFIX2= "/";

		public const char SEP= '=';
		
		public const string ARG_HELP_LONG= "help";
		public const string ARG_HELP_SHORT= "?";

		public const string ARG_HOST= "host";
		public const string ARG_METADATA_RR_PORT= "metadata_rrport";
		public const string ARG_DATA_RR_PORT= "data_rrport";
		public const string ARG_DATA_NOTIF_PORT= "data_notifport";
		public const string ARG_NAME= "name";
		
		public static void Main(string [] args) {
			if (args.Length == 0) Help();
			
			_log.Info("Lightstreamer StockListDemo .NET Adapter Standalone Server starting...");
		
			IDictionary parameters= new Hashtable();
			string host= null;
			int rrPortMD= -1;
			int rrPortD= -1;
			int notifPortD= -1;
			string name= null;
			
			for (int i= 0; i < args.Length; i++) {
				string arg= args[i];
				
				if (arg.StartsWith(PREFIX1) || arg.StartsWith(PREFIX2)) {
					arg= arg.Substring(1).ToLower();

					if (arg.Equals(ARG_HELP_SHORT) || arg.Equals(ARG_HELP_LONG)) {
						Help();
					} 
					else if (arg.Equals(ARG_HOST)) {
						i++;
						host= args[i];

						_log.Debug("Found argument: '" + ARG_HOST + "' with value: '" + host + "'");
					}
					else if (arg.Equals(ARG_METADATA_RR_PORT)) {
						i++;
						rrPortMD= Int32.Parse(args[i]);

						_log.Debug("Found argument: '" + ARG_METADATA_RR_PORT + "' with value: '" + rrPortMD + "'");
					}
					else if (arg.Equals(ARG_DATA_RR_PORT)) {
						i++;
						rrPortD= Int32.Parse(args[i]);

						_log.Debug("Found argument: '" + ARG_DATA_RR_PORT + "' with value: '" + rrPortD + "'");
					}
					else if (arg.Equals(ARG_DATA_NOTIF_PORT)) {
						i++;
						notifPortD= Int32.Parse(args[i]);

						_log.Debug("Found argument: '" + ARG_DATA_NOTIF_PORT + "' with value: '" + notifPortD + "'");
					}
					else if (arg.Equals(ARG_NAME)) {
						i++;
						name= args[i];

						_log.Debug("Found argument: '" + ARG_NAME + "' with value: '" + name + "'");
					}
				
				} 
				else {
					int sep= arg.IndexOf(SEP);
					if (sep < 1) {
						_log.Warn("Skipping unrecognizable argument: '" + arg + "'");
						
					} 
					else {
						string par= arg.Substring(0, sep).Trim();
						string val= arg.Substring(sep +1).Trim();
						parameters[par]= val;
						
						_log.Debug("Found parameter: '" + par + "' with value: '" + val + "'");						
					}
				}
			}
			
            try {
                {
		            MetadataProviderServer server = new MetadataProviderServer();
		            server.Adapter = new Lightstreamer.Adapters.Metadata.LiteralBasedProvider();
		            server.AdapterParams = parameters;
		            // server.AdapterConfig not needed by LiteralBasedProvider
		            if (name != null) server.Name = name;
    		        _log.Debug("Remote Metadata Adapter initialized");

                    ServerStarter starter = new ServerStarter(host, rrPortMD, -1);
                    starter.Launch(server);
                }

                {
				    DataProviderServer server = new DataProviderServer();
                    server.Adapter = new Lightstreamer.Adapters.StockListDemo.Data.StockListDemoAdapter();
				    // server.AdapterParams not needed by StockListDemoAdapter
				    // server.AdapterConfig not needed by StockListDemoAdapter
				    if (name != null) server.Name = name;
    				_log.Debug("Remote Data Adapter initialized");

                    ServerStarter starter = new ServerStarter(host, rrPortD, notifPortD);
                    starter.Launch(server);
                }
            } 
			catch (Exception e) {
				_log.Fatal("Exception caught while starting the server: " + e.Message + ", aborting...", e);
			}

			_log.Info("Lightstreamer StockListDemo .NET Adapter Standalone Server running");
        }

		private static void Help() {
			_log.Fatal("Lightstreamer StockListDemo .NET Adapter Standalone Server Help");
			_log.Fatal("Usage: DotNetStockListDemoLauncher");
			_log.Fatal("                     [/name <name>] /host <address>");
			_log.Fatal("                     /metadata_rrport <port> /data_rrport <port> /data_notifport <port>");
			_log.Fatal("                     [\"<param1>=<value1>\" ... \"<paramN>=<valueN>\"]");
			_log.Fatal("Where: <name>        is the symbolic name for both the adapters (1)");
			_log.Fatal("       <address>     is the host name or ip address of LS server (2)");
			_log.Fatal("       <port>        is the tcp port number where LS proxy is listening on (3)");
			_log.Fatal("       <paramN>      is the Nth metadata adapter parameter name (4)");
			_log.Fatal("       <valueN>      is the value of the Nth metadata adapter parameter (4)");
			_log.Fatal("Notes: (1) The adapter name is optional, if it is not given the adapter will be");
			_log.Fatal("           assigned a progressive number name like \"#1\", \"#2\" and so on");
			_log.Fatal("       (2) The communication will be from here to LS, not viceversa");
			_log.Fatal("       (3) The notification port is necessary for a Data Adapter, while it is");
			_log.Fatal("           not needed for a Metadata Adapter");
			_log.Fatal("       (4) The parameters name/value pairs will be passed to the LiteralBasedProvider");
			_log.Fatal("           Metadata Adapter as a Hashtable in the \"parameters\" Init() argument");
			_log.Fatal("           The StockListDemo Data Adapter requires no parameters");
			_log.Fatal("Aborting...");

			System.Environment.Exit(9);
		}
	}

	public class ServerStarter : IExceptionHandler {
		private static ILog _log= LogManager.GetLogger("Lightstreamer.Adapters.StockListDemo.ServerStarter");

        private Server _server;
        private bool _closed;

		private string _host;
		private int _rrPort;
		private int _notifPort;

		public ServerStarter(string host, int rrPort, int notifPort) {
			_host= host;
			_rrPort= rrPort;
			_notifPort= notifPort;
		}

		public void Launch(Server server) {
            _server = server;
            _closed = false;
            _server.ExceptionHandler = this;
			Thread t= new Thread(new ThreadStart(Run));
			t.Start();
        }

        public void Run() {
		    TcpClient _rrSocket = null;
		    TcpClient _notifSocket = null;

            do {
				_log.Info("Connecting...");

				try {
					_log.Info("Opening connection on port " + _rrPort + "...");
					_rrSocket = new TcpClient(_host, _rrPort);
					if (_notifPort >= 0)
					{
						_log.Info("Opening connection on port " + _notifPort + "...");
						_notifSocket = new TcpClient(_host, _notifPort);
					}

					_log.Info("Connected");

					break;
				} 
				catch (SocketException) {
					_log.Warn("Connection failed, retrying in 10 seconds...");

					Thread.Sleep(10000);
				}
			
			} while (true);

			_server.RequestStream= _rrSocket.GetStream();
			_server.ReplyStream= _rrSocket.GetStream();
			if (_notifSocket != null) _server.NotifyStream= _notifSocket.GetStream();

            _server.Start();
        }

		public bool handleIOException(Exception exception) {
			_log.Error("Connection to Lightstreamer Server closed");
            _closed = true;
            _server.Close();
			System.Environment.Exit(0);
            return false;
		}

		public bool handleException(Exception exception) {
            if (! _closed) {
			    _log.Error("Caught exception: " + exception.Message, exception);
                _server.Close();
			    System.Environment.Exit(1);
            }
            return false;
		}

        // Notes about exception handling.
        // 
        // In case of exception, the whole Remote Server process instance
        // can no longer be used;
        // closing it also ensures that the Proxy Adapter closes
        // (thus causing Lightstreamer Server to close)
        // or recovers by accepting connections from a new Remote
        // Server process instance.
        // 
        // Keeping the process instance alive and replacing the Server
        // class instances would be possible.
        // This would issue new connections with Lightstreamer Server.
        // However, new instances of the Remote Adapters would also be needed
        // and a cleanup of the current instances should be performed,
        // by invoking them directly through a custom method.
    }

}
