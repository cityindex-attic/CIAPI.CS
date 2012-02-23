@echo off

rem ---------------------------------------------------------------------------
rem Set the DotNet installation path as the current directory. Change the 
rem directory according to your installation path of DotNet Adapter.

pushd .


rem Lightstreamer StockListDemo .NET Adapters Standalone Server Usage:
rem 	DotNetStockListDemoLauncher
rem 		[/name <name>] /host <address>
rem 		/data_rrport <port> /data_notifport <port> /metadata_rrport <port>
rem 		["<param1>=<value1>" ... "<paramN>=<valueN>"]
rem 	Where:
rem 		<name>        is the symbolic name for both the adapters (2)
rem 		<address>     is the host name or ip address of LS server (3)
rem 		<port>        is the tcp port number where LS proxy is listening on (4)
rem 		<paramN>      is the Nth Metadata Adapter parameter name (5)
rem 		<valueN>      is the value of the Nth Metadata Adapter parameter (5)
rem 	Notes:
rem 		(1) The DotNetStockListDemoLauncher is a custom Remote Server which
rem 			runs the StockListDemo Data and Metadata adapters;
rem 			it is not a generic Remote Server; hence, the adapter class names
rem 			cannot be configured
rem 		(2) The adapter name is optional, if it is not given the adapter will be
rem 			assigned a progressive number name like "#1", "#2" and so on
rem 		(3) The connection will be from here to LS, not viceversa
rem 		(4) The notification port is necessary for a Data Adapter, while it is
rem 			not needed for a Metadata Adapter
rem 		(5) The parameters name/value pairs will be passed to the LiteralBasedProvider
rem 			Metadata Adapter as an hashtable in the "parameters" Init() argument
rem 			The StockListDemo Data Adapter requires no parameters


rem ---------------------------------------------------------------------------
rem Start the LiteralBasedProvider Remote Metadata Adapter and the StockList
rem Demo Remote Data Adapter in networked mode, specifying to connect to the
rem local host. Change the "/host" parameter according to your configuration.

start "StockListDemoRemoteAdapters" /MIN DotNetStockListDemoLauncher_N2 /host localhost /data_rrport 6661 /data_notifport 6662 /metadata_rrport 6663 max_bandwidth=40 max_frequency=3 buffer_size=30


rem ---------------------------------------------------------------------------
rem All done. Goes back to the original current directory and pauses, in case 
rem of any error.

echo Processes started. All done.
popd
pause
