LIGHTSTREAMER - .NET ADAPTER DEVELOPMENT RESOURCES
==================================================

This directory includes the resources needed to write Data Adapters and
Metadata Adapters for Lightstreamer Server in a .NET environment.
The adapters will run in a separate process, communicating with the Server
through the Adapter Remoting Infrastructure.

- Version.txt
  Reports the current SDK version and compatibility constraints.
   
- doc/DotNet Adapters.pdf
  An introduction to Lightstreamer .NET Adapters development.
  
- doc/API-reference/*
  The Adapter API Interface class documentation files.

- doc/API-reference-index.html
  The home page for the Adapter API Interface class documentation.

- doc/Tutorial.htm
  A "Hello World" tutorial where both a C# and Visual Basic
  Data Adapter are shown.

- bin/*
- lib/*
  Binaries to be used for building and running Remote Adapters in a .NET 2.0
  or greater environment.

- bin/StrongName/*
- lib/StrongName/*
  A version of the library DLL and the Server Wrapper for a .NET 2.0
  or greater environment identified with a "strong name".
  Note that the included version numbers may be slightly different
  than the SDK version reported in Version.txt

- examples/Remote_StockListDemo_Adapters
  Contains the source code and all the resources needed to install a .NET
  version of the StockListDemo Data and Metadata Adapters.
  The standard type of configuration is shown, where the process which runs
  the Remote Adapters is manually launched beside Lightstreamer Server.
  On the other hand, two different examples of manual launch of the remote
  process are shown, one based on the provided Remote Server and one based
  on a custom server program, also shown.
  Please refer to the "LS DotNet Adapters.pdf" document for deployment details.
  The Adapter should be used in combination with the StockList Demo Web
  front-end, already pre-installed under Lightstreamer Server.

- examples/Remote_Portfolio_Adapters
  Contains the source code and all the resources needed to install a .NET
  version of the PortfolioDemo Data and Metadata Adapters.
  The standard type of configuration is shown, where the process which runs
  the Remote Adapters is manually launched beside Lightstreamer Server.
  The launcher process is based on a custom server program, also shown.
  Please refer to the "LS DotNet Adapters.pdf" document for deployment details.
  The Adapter should be used in combination with the Basic Portfolio Demo
  or Drop-Down Demo Web front-ends, already pre-installed under Lightstreamer
  Server.

- examples/Remote_FullPortfolioDemo
  Contains all the resources needed to install a .NET version of the
  StockListDemo and PortfolioDemo Data and Metadata Adapters.
  Only a deployment example is shown, based on the binaries provided
  in the other two examples.
  The Adapter can be used in combination with the full version of the Portfolio
  Demo Web front-end, already pre-installed under Lightstreamer Server.

Full details of the directory contents can be found in
"LS DotNet Adapters.pdf".
