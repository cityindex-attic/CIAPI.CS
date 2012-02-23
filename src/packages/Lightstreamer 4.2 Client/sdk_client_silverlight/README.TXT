LIGHTSTREAMER - SILVERLIGHT CLIENT DEVELOPMENT RESOURCES
========================================================

This directory includes the resources needed to make any Silverlight-based
application a direct Client of Lightstreamer Server, in order to get real-time
streaming data.

The Silverlight Client Library exposes a simple interface to enable applications
to communicate with Lightstreamer Server, by opening and closing a connection,
performing subscriptions and unsubscriptions and receiving data.
Applications access Library services by creating an object of type
Lightstreamer.DotNet.Client.LSClient. Each LSClient object manages a private
connection to Lightstreamer Server.

The Silverlight Client Library is a porting of the Lightstreamer .NET Client
Library and is packaged as a standard .NET DLL.

The Silverlight Client Library is compatible with Silverlight environment
version 3 or newer.


- Version.txt
  Reports the current SDK version and compatibility constraints.

- doc/API-reference/*
  The Client API Interface class documentation files.

- doc/API-reference-index.html
  The home page for the Client API Interface class documentation.

- lib/SilverlightClient.dll
- lib/SilverlightClient.pdb
  The library to be included inside a Silverlight application to enable the
  communication with Lightstreamer Server.

- lib/StrongName/SilverlightClient.dll
- lib/StrongName/SilverlightClient.pdb
  A version of the library DLL identified with a "strong name".
  Note that the included version number may be slightly different
  than the SDK version reported in Version.txt

- examples/SilverlightStockListDemo
  Contains a simple Silverlight application fed through a Lightstreamer
  connection. Both the source code and a precompiled version, ready
  for deployment, are provided.
  See the included README.TXT for deployment instructions.

