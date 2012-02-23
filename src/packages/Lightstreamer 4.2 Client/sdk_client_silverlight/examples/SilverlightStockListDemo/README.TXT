PROVIDED SILVERLIGHT CLIENT EXAMPLES
====================================

This directory contains an example of a Silverlight application that employs
the Silverlight client library.
The example shows a simple Silverlight application fed through a Lightstreamer
connection.

The example is comprised of the following folders:

- Source
  Contains the sources to build the Silverlight application from Visual Studio.

- Properties
  Contains the property files associated to the application sources.
  
- Images
  Contains the image files used by the Silverlight application.

- lib
  Contains Lightstreamer library, to be used for the build process.

- bin
  Contains the application DLL, as compiled from the provided source files.

- deploy
  Contains a deployment image of the demo, which includes the ".xap" file
  needed to contain all the resources that make up the demo (i.e. the demo DLL,
  the Lightstreamer client library DLL and, possibly, other SDK resources)
  and a container html page.
  This deployment image is ready to be deployed under Lightstreamer's internal
  web server, by copying all the contents into some subfolder of the "pages"
  directory.

The example is already pre-deployed into Lightstreamer Server, under the
"pages\demos" folder.

The example requires that Lightstreamer Server is running with the
DEMO Adapter Set installed. The default Server configuration
already includes this Adapter Set.



HOW TO DEPLOY THE EXAMPLE ON YOUR WEB SERVER
============================================

By the current configuration, the demo tries to access Lightstreamer Server
by using the protocol, hostname and port from which the "index.html" page
was requested; in other words, the demo assumes that the static resources
are deployed inside Lightstreamer Server.

In order to deploy the demo static resources on an external web server,
some changes are needed on the deployment image before or after copying it
into the web server folders.
The configuration of the url to be used to connect to Lightstreamer Server
should be added. The configuration section can be easily found in
deploy/index.html, as the if-block containing the
    param name="initparams"
pattern and can be modified manually, without the need for a recompilation.

Then, in order to allow the page to get resources from a different server,
the web server address has to be included in the "clientaccesspolicy" resource
deployed under Lightstreamer Server. See the <silverlight_accesspolicy_enabled>
element in the Server configuration file for details.
