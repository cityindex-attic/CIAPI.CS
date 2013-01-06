using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System;
using System.IO;
using Mocument.DataAccess.SQLite;
using Mocument.ReverseProxyServer;

namespace Mocument.ReverseProxyService
{
    class WinService : System.ServiceProcess.ServiceBase
    {
        private static int _port;
        private static bool _secured;
        private static SQLiteStore _store;
        private static int _proxySSLPort;
        private static string _proxyHostName;
        private static Server _server;

        // The main entry point for the process
        static void Main()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;
            ServicesToRun =
              new System.ServiceProcess.ServiceBase[] { new WinService() };
            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceName = "Mocument.ReverseProxyService";
        }
        private string folderPath = @"c:\temp";
        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _proxySSLPort = int.Parse(appSettings["proxySSLPort"]);
            _proxyHostName = appSettings["proxyHostName"];
            _port = int.Parse(appSettings["proxyPort"]);
            _secured = bool.Parse(appSettings["proxySecured"]);
            _store = new SQLiteStore("mocument");
            _server = new Server(_port,_proxySSLPort,_proxyHostName, _secured, _store);
            _server.Start();
        }
        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            _server.Stop();
            _server = null;
            _store.Dispose();
             

        }
    }
}
