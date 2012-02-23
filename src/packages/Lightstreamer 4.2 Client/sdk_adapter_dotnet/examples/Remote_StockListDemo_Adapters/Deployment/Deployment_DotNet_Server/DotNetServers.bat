@echo off

rem ---------------------------------------------------------------------------
rem Set the DotNet installation path as the current directory. Change the 
rem directory according to your installation path of DotNet Adapter.

pushd .


rem Lightstreamer .Net Adapter Server Usage:
rem 	DotNetServer <class-fqn>
rem 		[/name <name>] [/config "<config-file>"]
rem 		[/host <address>] [/rrport <port>] [/notifport <port>]
rem 		["<param1>=<value1>" ... "<paramN>=<valueN>"]
rem 	Where:
rem 		<class-fqn>   is the fully qualified name of the adapter class
rem 		<name>        is the adapter symbolic name (2)
rem 		<config-file> is the local path to the adapter configuration file (3)
rem 		<address>     is the host name or ip address of LS server (4)
rem 		<port>        is the tcp port number where LS proxy is listening on (5)
rem 		<paramN>      is the Nth adapter parameter name (6)
rem 		<valueN>      is the value of the Nth adapter parameter (6)
rem 	Notes:
rem 		(1) The Server can run either a Metadata or a Data Adapter;
rem 			the adapter class code should be contained in a "dll" file
rem 			which should be placed in the current directory
rem 		(2) The adapter name is optional, if it is not given the adapter will be
rem 			assigned a progressive number name like "#1", "#2" and so on
rem 		(3) The configuration file path will be passed to the adapter in the
rem 			"configFile" Init() argument
rem 		(4) The connection will be from here to LS, not viceversa
rem 		(5) The notification port is necessary for a Data Adapter, while it is
rem 			ignored for a Metadata Adapter
rem 		(6) The parameters name/value pairs will be passed to the adapter
rem 			as an hashtable in the "parameters" Init() argument


rem ---------------------------------------------------------------------------
rem Start the StockListDemo Remote Data Adapter in networked mode, specifiyng 
rem to connect to the local host. Change the "/host" parameter according to 
rem your configuration.

start "StockListDemoAdapter" /MIN DotNetServer_N2 Lightstreamer.Adapters.StockListDemo.Data.StockListDemoAdapter /host localhost /rrport 6661 /notifport 6662


rem ---------------------------------------------------------------------------
rem Start the LiteralBasedProvider Remote Metadata Adapter in networked 
rem mode, specifiyng to connect to the local host. Change the "/host" 
rem parameter according to your configuration.

start "LiteralBasedProvider" /MIN DotNetServer_N2 Lightstreamer.Adapters.Metadata.LiteralBasedProvider /host localhost /rrport 6663 max_bandwidth=40 max_frequency=3 buffer_size=30


rem ---------------------------------------------------------------------------
rem All done. Goes back to the original current directory and pauses, in case 
rem of any error.

echo Processes started. All done.
popd
pause
