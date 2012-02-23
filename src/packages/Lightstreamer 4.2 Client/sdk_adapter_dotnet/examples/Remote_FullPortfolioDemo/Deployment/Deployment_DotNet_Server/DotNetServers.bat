@echo off

rem ---------------------------------------------------------------------------
rem Set the DotNet installation path as the current directory. Change the 
rem directory according to your installation path of DotNet Adapter.

pushd .


rem ---------------------------------------------------------------------------
rem Start the StockListDemo Remote Data Adapter in networked mode, specifiyng 
rem to connect to the local host. Change the "/host" parameter according to 
rem your configuration.

start "StockListDemoAdapter" /MIN DotNetServer_N2 Lightstreamer.Adapters.StockListDemo.Data.StockListDemoAdapter /host localhost /rrport 6684 /notifport 6685


rem ---------------------------------------------------------------------------
rem Start the PortfolioDemo Remote Data and Metadata Adapter
rem in networked mode, specifying to connect to the local host.
rem The portfolioDemo Remote Metadata Adapter is also suitable for handling
rem requests pertaining the StckListDemo Remote Data Adapter.
rem Change the "/host" parameter according to your configuration.

start "PortfolioDemoRemoteAdapters" /MIN DotNetPortfolioDemoLauncher_N2 /host localhost /data_rrport 6681 /data_notifport 6682 /metadata_rrport 6683 max_bandwidth=40 max_frequency=3 buffer_size=30


rem ---------------------------------------------------------------------------
rem All done. Goes back to the original current directory and pauses, in case 
rem of any error.

echo Processes started. All done.
popd
pause
