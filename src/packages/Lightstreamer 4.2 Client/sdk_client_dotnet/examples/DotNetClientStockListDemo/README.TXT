LIGHTSTREAMER - .NET CLIENT EXAMPLE
===================================

This directory contains the C# source code and all the resources needed to run
a Demo Client that receives live prices from Lightstremer Server.
The Demo Client requires that the DEMO Adapter Set is installed
into Lightstreamer Server.
See DOCS-SDKs/sdk_adapter_java/examples/StockListDemo_DataAdapter/Deployment_LS/README.TXT
for adapter installation details.

The example is comprised of the following files:

- Source/*
- Properties/*
- Images/*
  The Demo Client source and image files.

- lib/*
  The libraries needed for the Client application recompilation; this includes
  Lightstreamer .NET interface library.
  These binaries are based on the normal (i.e. not the "strong name") version
  of the library.

- bin/*
  The Demo Client generated binaries.
  These binaries are based on the normal (i.e. not the "strong name") version
  of the library.

- Deployment/*
  A runnable installation of the Demo Client. Ensure that the hostname and port
  of your current Lightstreamer Server installation are correctly specified in
  the DotNetClient.bat launch script.
  The DotNetClientDemo_N2 executable can also be run by double-clicking it;
  in its default configuration, the client will try to connect to
  Lightstreamer Server at http://localhost:80.
