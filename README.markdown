#  CIAPI.CS ![Complete](http://labs.cityindex.com/wp-content/uploads/2012/01/lbl-complete.png)![Supported](http://labs.cityindex.com/wp-content/uploads/2012/01/lbl-supported.png)
A complete .NET client library for connecting to the CityIndex Trading API.

Includes builds for:

  * .NET 4
  * .NET 3.5
  * Silverlight
  * Windows Phone 7

and is distributed via NuGet.

### Status
Covers the complete API, and is actively being developed and supported

### Usage

    //Setup a connection to the rpc endpoint
    var rpcClient = new Client(new Uri("https://{REST api url}"));
    //Create a session
    rpcClient.LogIn("{username}", "{password});
    //Retrieve some data
    var priceBars = rpcClient.PriceHistory.GetPriceBars({marketId}, "MINUTE", 10, "10");
    
    //Setup a connection to the streaming endpoint
    var streamingClient = StreamingClientFactory
          .CreateStreamingClient(new Uri("https://{streaming api url}"), "{username}", rpcClient.Session);
    //Subscribe to a stream
    var priceListener = streamingClient.BuildPricesListener({marketId});
    priceListener.MessageReceived += (s, message) => { Console.Write(message.Data.ToString()) };
    
    //When done, disconnect and clean up
    streamingClient.TearDownListener(priceListener);
    streamingClient.Dispose();
    rpcClient.LogOut();
    rpcClient.Dispose();
    
See the [Integration tests for further examples](https://github.com/cityindex/CIAPI.CS/tree/master/src/CIAPI.IntegrationTests)

### Copyright and license
This library is opensource and [licensed under the Apache v2 license](https://github.com/cityindex/CIAPI.CS/LICENSE.txt)

Note that in order to connect to the CityIndex Trading API you must be a CityIndex client and abide by the 
CityIndex terms and conditions related to usage of your account.

### Getting started
Compiled versions of CIAPI.CS are distributed via NuGet.org.  You must be running NuGet v1.6+.

To install the latest stable version, search for CIAPI.CS in the NuGet VS2010 GUI, or run:

    Install-Package CIAPI.CS
   
From the NuGet Console in VS2010.

You can also install a pre-release "bleedingedge" version:

    Install-Package CIAPI.CS

Versioning numbers follow the [SemVer](semver.org) standard.

See the [Integration tests](https://github.com/cityindex/CIAPI.CS/tree/master/src/CIAPI.IntegrationTests) 
for examples on how to use the library.

### Support
Refer to the [CIAPI.docs](https://ciapipreprod.cityindextest9.co.uk/CIAPI.docs) for an overview of the available API methods
and what each of them do.

If you get stuck, search [faq.labs.cityindex.com](http://faq.labs.cityindex.com) for help.  
If you are still stuck, post a new FAQ question; tagging it ciapi.cs.

Use the [GitHub issue tracker](https://github.com/cityindex/CIAPI.CS/issues) to report bugs and make feature requests.

*NB! Bugs reported without attached test cases (preferably as unit tests) will not be investigated*

We welcome patches - please fork the project & submit a pull request.

### Building from source

   * Clone this repo
   * Refer to [DEV_SETUP.txt](https://github.com/cityindex/CIAPI.CS/blob/master/DEV_SETUP.txt) for pre-reqs
   * Open src/{desired platform}.sln in VS2010
   * Run build.cmd to compile, run all tests and package
