@echo off

rem ---------------------------------------------------------------------------
rem Set the DotNet installation path as the current directory. Change the 
rem directory accordingly to your installation path of DotNet Adapter.

pushd .


rem ---------------------------------------------------------------------------
rem Start the StockList Demo Client, specifiyng to connect to the local
rem Lightstreamer Server instance.

start "DotNetStockListDemo" DotNetStockListDemo localhost 8080


rem ---------------------------------------------------------------------------
rem All done. Goes back to the original current directory and pauses, in case 
rem of any error.

echo Processes started. All done.
popd
pause
