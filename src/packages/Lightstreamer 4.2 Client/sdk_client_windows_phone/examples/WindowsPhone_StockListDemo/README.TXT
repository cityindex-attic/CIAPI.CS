Lightstreamer Windows Phone Demo
================================

This folder includes a demo project showing integration
between Lightstreamer Windows Phone Client and the Windows Phone
platform. In particular, this readme file details the steps
required in order to execute the demo together with the Windows
Phone emulator contained in Visual Studio (Express) for Windows Phone.

- Getting started
  To directly import the project as is you can use Microsoft Visual Studio 
  2010 Express for Windows Phone. You can download it from the Microsoft 
  website http://www.microsoft.com/express/Phone/

  NOTE: You may also use the sources of the demo with another IDE
  or without any IDE but such approach is not covered in this readme.
  
  You may run the demo against your local server or using our online server
  at http://push.lightstreamer.com:80.
  The server to which the demo will connect to is configured in the 
  App.xaml.cs file.
  
  You're now ready to import the project into Visual Studio, click on 
  New Project->Windows Phone Application and import all the files located in
  the demo folder.
  
- Compile and Run
  From Visual Studio, click on the WP7StockListDemo project in the Solution
  Explorer menu and press the "Run (debug)" button.
  The Windows Phone Emulator will be started and the application loaded.
  
- Dig the code
  - App.xaml.cs is the main application file, it contains application events
    handling code (application launching, closed, activated, etc), Lightstreamer
    Client streaming controls (start, stop) and part of the auto-reconnection
	  logic required when connection quality is weak.
  - MainPage.xaml.cs is the place where all the application widgets are handled.
    The application in fact, consists in a simple table containing real-time
	  (simulated) stock quotes, whose cells are kept up-to-date by implementing
	  the ILightstreamerListener interface.
	  Several lines in this file are dedicated to the animation code used for
	  highlighting cells in case of real-time updates, you can ignore them
	  altogether if you feel more comfortable.
  - LightstreamerClient.cs is a simple wrapper for LSClient public class
    (see Lightstreamer Silverlight API) and uses two listeners:
	  StocklistConnectionListener.cs and StocklistHandyTableListener.cs,
	  respectively implementing a Connection Status listener and a Real-Time
	  Data Updates listener.
  
  Check out the sources for further explanations.
  
  NOTE: not all the functionalities of the Lightstreamer Windows Phone Demo
  are exposed by the classes listed above. You can easily expand those
  functionalities using the Silverlight Client API as a reference.
  
If in trouble check out the Lightstreamer forum:
http://www.lightstreamer.com/vb/forumdisplay.php?f=34
