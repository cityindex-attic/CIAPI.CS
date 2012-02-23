LIGHTSTREAMER - .NET CLIENT DEVELOPMENT RESOURCES
=================================================

This directory includes some resources that can be used to develop a .NET
application that works as a client to Lightstreamer Server, in order to
get real-time streaming data.

The .NET Client Library is the .NET porting of the Lightstreamer Java Client
Library (the ls_client layer only) and is packaged as a standard .NET DLL.
Only .NET environment version 2.0 or greater is supported.

The .NET Client Library exposes a simple interface to enable application
to communicate with Lightstreamer Server, by opening and closing a connection,
performing subscriptions and unsubscriptions and receiving data.
Clients access Library services by creating an object of type
Lightstreamer.DotNet.Client.LSClient. Each LSClient object manages a private
connection to Lightstreamer Server.

- Version.txt
  Reports the current SDK version and compatibility constraints.

- doc/API-reference/*
  The Client API Interface class documentation files.

- doc/API-reference-index.html
  The home page for the Client API Interface class documentation.

- lib/DotNetClient_N2.dll
- lib/DotNetClient_N2.pdb
  Binaries to be used for building and running client applications in a .NET 2.0
  or greater environment.

- lib/StrongName/DotNetClient_N2.dll
- lib/StrongName/DotNetClient_N2.pdb
  A version of the library DLL for .NET environment 2.0 or greater
  identified with a "strong name".
  Note that the included version number may be slightly different
  than the SDK version reported in Version.txt

- examples/test
  Contains the C# source files of a sample application that uses Lightstreamer
  .NET Client and can be used to test the capability of the Client API to
  connect and receive data from Lightstreamer Server.
  These source files can be used as a starting point for a Client application
  implementation or as a reference use of the Client API.
  TestClient.cs contains the source Main(). Please, refer to the instructions
  included in this source file in order to configure and run the tests.
  The test includes the necessary code and a sample configuration file for 
  Library logging through Log4Net.
  The test requires that the DEMO Adapter Set is installed into
  Lightstreamer Server.
  See DOCS-SDKs/sdk_adapter_java/examples/StockListDemo_DataAdapter/Deployment_LS/README.TXT
  for adapter installation details.

- examples/DotNetClientStockListDemo
  Contains a full example of a .NET client application that employs
  the .NET client library.
  See the included README.TXT file for details.
