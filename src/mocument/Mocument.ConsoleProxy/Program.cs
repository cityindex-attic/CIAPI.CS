using System;
using System.Collections.Specialized;
using System.Configuration;
using Mocument.DataAccess.SQLite;
using Mocument.ReverseProxyServer;

namespace Mocument.ConsoleProxy
{
    internal class Program
    {
        private static int _port;
        private static bool _secured;
        private static SQLiteStore _store;
        private static int _proxySSLPort;
        private static string _proxyHostName;
        private static Server _server;

        private static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("exit");
            _server.Stop();
            _server = null;
            _store.Dispose();
        }

        // ReSharper disable UnusedParameter.Local
        private static void Main(string[] args)
        // ReSharper restore UnusedParameter.Local
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _port = int.Parse(appSettings["proxyPort"]);
            _secured = bool.Parse(appSettings["proxySecured"]);
            _proxySSLPort = int.Parse(appSettings["proxySSLPort"]);
            _proxyHostName = appSettings["proxyHostName"];
            _store = new SQLiteStore("mocument");


            AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;

            _server = new Server(_port, _proxySSLPort, _proxyHostName, _secured, _store);
            Console.CancelKeyPress += ConsoleCancelKeyPress;
            _server.Start();
            Console.WriteLine("Hit CTRL+C to end session.");


            bool bDone = false;
            do
            {
                Console.WriteLine(
                    "\nEnter a command [G=Collect Garbage; Q=Quit]:");
                Console.Write(">");
                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine();
                switch (cki.KeyChar)
                {
                    case 'g':
                        Console.WriteLine("Working Set:\t" + Environment.WorkingSet.ToString("n0"));
                        Console.WriteLine("Begin GC...");
                        GC.Collect();
                        Console.WriteLine("GC Done.\nWorking Set:\t" + Environment.WorkingSet.ToString("n0"));
                        break;

                    case 'q':
                        bDone = true;
                        _server.Stop();
                        break;
                }
            } while (!bDone);
        }


        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _server.Stop();
        }
    }
}