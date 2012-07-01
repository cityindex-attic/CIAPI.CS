using System;
using System.Collections;
using System.Threading;

[assembly:log4net.Config.XmlConfigurator()]
// this requires the .exe.config file provided

namespace Lightstreamer.DotNet.Client.Test {
    
    /// <summary>
    /// Demonstrates basic Server access support, through LSClient facade.
    /// 
    /// Opens a connection to Lightstreamer Server and subscribes to a table,
    /// defined with either a SimpleTableInfo or an ExtendedTableInfo.
    /// It requires that Lightstreamer Server is running with the
    /// DEMO Adapter Set installed.
    /// 
    /// The test can be invoked in five different ways:
    /// TestClient HOST PORT
    /// demonstrates use of SimpleTableInfo for the subscription to a table
    /// TestClient HOST PORT extended
    /// demonstrates use of ExtendedTableInfo for the subscription to a table
    /// TestClient HOST PORT multiple
    /// demonstrates use of ExtendedTableInfo for the subscription to a set
    /// of items that do not belong to a table
    /// TestClient HOST PORT command
    /// demonstrates use of SimpleTableInfo for the subscription to a table
    /// in COMMAND mode, to be handled in COMMAND logic
    /// TestClient HOST PORT command extended
    /// demonstrates use of ExtendedTableInfo for the subscription to a table
    /// in COMMAND mode, to be handled in COMMAND logic
    /// HOST stands for the host name of Lightstreamer Server, while PORT
    /// stands for the port number configured for the Server (the value in the
    /// &lt;port&gt; subelement of the &lt;http_server&gt; block in the configuration file).
    /// 
    /// In order for the COMMAND based versions to produce updates, a PortfolioDemo
    /// should be opened on the same Server and order entry operations should
    /// be performed manually.
    /// </summary>
    public class TestClient {

        /// <summary>
        /// Opens a connection, performs a table subscription and unsubscription
        /// and closes the connection after some time.
        /// </summary>
        /// <param name="args">Should specify the host name of the Server, the port number
        /// and optionally the "extended", "multiple" and "command" flags.
        /// </param>
        /// <throws>Exception Thrown in case of any error.</throws>
        public static void Main(string[] args) {

            LSClient.SetLoggerProvider(new Log4NetLoggerProviderWrapper());

            string pushServerHost = args[0];
            int pushServerPort = Int32.Parse(args[1]);

            ArrayList opts = new ArrayList();
            for (int i = 2; i < args.Length; i++) {
                opts.Add(args[i]);
            }
            bool extended = opts.Contains("extended");
            bool multiple = opts.Contains("multiple");
            bool command = opts.Contains("command");

            Thread.Sleep(2000);

            ConnectionInfo connInfo= new ConnectionInfo();
            connInfo.PushServerUrl= "http://" + pushServerHost + ":" + pushServerPort;
            connInfo.Adapter= "DEMO";

            LSClient myClient = new LSClient();
            myClient.OpenConnection(connInfo, new TestConnectionListener());
        
            Thread.Sleep(5000);

            ArrayList refs = new ArrayList();
            SubscribedTableKey[] tableRefs;

            if (!command) {
                if (extended) {
                    ExtendedTableInfo tableInfo= new ExtendedTableInfo(
                        new string[] { "item1", "item2", "item3" },
                        "MERGE",
                        new string[] { "last_price", "time", "pct_change" },
                        true
                        );

                    tableInfo.DataAdapter = "QUOTE_ADAPTER";
                
                    SubscribedTableKey tableRef = myClient.SubscribeTable(
                        tableInfo,
                        new TestTableListenerForExtended(),
                        false
                        );
                
                    tableRefs = new SubscribedTableKey[] { tableRef };

                } else if (multiple) {
                    ExtendedTableInfo tableInfo= new ExtendedTableInfo(
                        new string[] { "item1", "item2", "item3" },
                        "MERGE",
                        new string[] { "last_price", "time", "pct_change" },
                        true
                        );

                    tableInfo.DataAdapter = "QUOTE_ADAPTER";

                    tableRefs = myClient.SubscribeItems(
                        tableInfo,
                        new TestTableListenerForMultiple()
                        );

                } else {
                    // Group and Schema names have to be manageable by
                    // the LiteralBasedProvider used for the StockListDemo
                    string groupName = "item1 item2 item3";
                    string schemaName = "last_price time pct_change";

                    SimpleTableInfo tableInfo= new SimpleTableInfo(
                        groupName, 
                        "MERGE", 
                        schemaName, 
                        true
                        );

                    tableInfo.DataAdapter = "QUOTE_ADAPTER";

                    SubscribedTableKey tableRef = myClient.SubscribeTable(
                        tableInfo,
                        new TestTableListenerForSimple(),
                        false
                        );

                    tableRefs = new SubscribedTableKey[] { tableRef };
                }
            } else {
                if (extended) {
                    ExtendedTableInfo tableInfo = new ExtendedTableInfo(
                        new String[] { "portfolio1" },
                        "COMMAND",
                        new String[] { "key", "command", "qty" },
                        true
                        );

                    tableInfo.DataAdapter = "PORTFOLIO_ADAPTER";

                    SubscribedTableKey tableRef = myClient.SubscribeTable(
                        tableInfo,
                        new TestPortfolioListenerForExtended(),
                        true
                        );

                    tableRefs = new SubscribedTableKey[] { tableRef };

                } else {
                    // Group and Schema names have to be manageable by
                    // the LiteralBasedProvider used for the StockListDemo
                    String groupName = "portfolio1";
                    String schemaName = "key command qty";

                    SimpleTableInfo tableInfo = new SimpleTableInfo(
                        groupName,
                        "COMMAND",
                        schemaName,
                        true
                        );

                    tableInfo.DataAdapter = "PORTFOLIO_ADAPTER";

                    SubscribedTableKey tableRef = myClient.SubscribeTable(
                        tableInfo,
                        new TestPortfolioListenerForSimple(),
                        true
                        );

                    tableRefs = new SubscribedTableKey[] { tableRef };
                }
            }

            for (int j = 0; j < tableRefs.Length; j++) {
                refs.Add(tableRefs[j]);
            }

            Thread.Sleep(20000);

            SubscribedTableKey[] allTableRefs = new SubscribedTableKey[refs.Count];
            for (int i= 0; i < refs.Count; i++) allTableRefs[i]= (SubscribedTableKey) refs[i];
            myClient.UnsubscribeTables(allTableRefs);

            Thread.Sleep(5000);
        
            myClient.CloseConnection();
        
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }

}
