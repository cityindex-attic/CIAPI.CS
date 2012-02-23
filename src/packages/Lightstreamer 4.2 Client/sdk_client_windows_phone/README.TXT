LIGHTSTREAMER - WINDOWS PHONE CLIENT DEVELOPMENT RESOURCES
==========================================================

This directory includes the resources needed to make any Windows Phone 
application a direct Client of Lightstreamer Server, in order to get real-time
streaming data.

The Windows Phone Client Library exposes a simple interface to enable applications
to communicate with Lightstreamer Server, by opening and closing a connection,
performing subscriptions and unsubscriptions and receiving data.
Applications access Library services by creating an object of type
Lightstreamer.DotNet.Client.LSClient. Each LSClient object manages a private
connection to Lightstreamer Server.

The Windows Phone Client Library is a porting of the Lightstreamer Silverlight
Client Library and is packaged as a standard .NET DLL.


- Version.txt
  Reports the current SDK version and compatibility constraints.

- doc/API-reference/*
  The Client API Interface class documentation files.

- doc/API-reference-index.html
  The home page for the Client API Interface class documentation.

- lib/WindowsPhoneClient.dll
- lib/WindowsPhoneClient.pdb
  The library to be included inside a Windows Phone application
  to enable the communication with Lightstreamer Server.
  
- lib/StrongName/WindowsPhoneClient.dll
- lib/StrongName/WindowsPhoneClient.pdb
  A version of the library DLL identified with a "strong name".
  Note that the included version number may be slightly different
  than the SDK version reported in Version.txt

- examples/WindowsPhone_StockListDemo
  Contains a simple Windows Phone application fed through a Lightstreamer
  connection. Both the source code and a precompiled version, ready
  for deployment, are provided.
