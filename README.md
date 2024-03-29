#  CIAPI.CS

A .NET client library for connecting to the CityIndex Trading API.

## Status

![Lifecycle: retired](https://img.shields.io/badge/lifecycle-retired-blue.svg) ![Support: unsupported](https://img.shields.io/badge/support-unsupported-yellow.svg)

This project has been retired and is no longer being supported by City Index Ltd.

* if you should choose to fork it outside of City Index, please let us know so we can link to your project

## Overview

A .NET client library for connecting to the CityIndex Trading API.

Includes builds for:

  * .NET 4
  * .NET 3.5
  * Silverlight
  * Windows Phone 7

and is distributed via NuGet.

### Terms & Conditions

Note that in order to connect to the CityIndex Trading API you must be a CityIndex client and abide by the 
CityIndex terms and conditions related to usage of your account.

### Usage

```cs
//Setup a connection to the rpc endpoint
var rpcClient = new Client(new Uri("https://{REST api url}"));
//Create a session
rpcClient.LogIn("{username}", "{password});
//Retrieve some data
var priceBars = rpcClient.PriceHistory.GetPriceBars({marketId}, "MINUTE", 10, "10");
    
//Setup a connection to the streaming endpoint

            

var streamingClient = rpcClient.CreateStreamingClient();

//Subscribe to a stream

var priceListener = streamingClient.BuildPricesListener({marketId});

priceListener.MessageReceived += (s, message) => { Console.Write(message.Data.ToString()) };

//When done, disconnect and clean up

streamingClient.TearDownListener(priceListener);

streamingClient.Dispose();

rpcClient.LogOut();

rpcClient.Dispose();
```

See the [Integration tests for further examples](https://github.com/cityindex/CIAPI.CS/tree/master/src/CIAPI.IntegrationTests)

### Getting started

Compiled versions of CIAPI.CS are distributed via NuGet.org.  You must be running NuGet v1.6+.

To install the latest stable version, search for CIAPI.CS in the NuGet VS2010 GUI, or run:

    Install-Package CIAPI
   
From the NuGet Console in VS2010.

You can also install a pre-release "bleedingedge" version:

    Install-Package CIAPI -pre

Versioning numbers follow the [SemVer](semver.org) standard.

See the [Integration tests](https://github.com/cityindex/CIAPI.CS/tree/master/src/CIAPI.IntegrationTests) 
for examples on how to use the library.

### Support

*NB! This project has been retired and is no longer being supported by City Index Ltd.*

Refer to the [CIAPI.docs](https://ciapipreprod.cityindextest9.co.uk/CIAPI.docs) for an overview of the available API methods and what each of them do.

If you get stuck, search [faq.labs.cityindex.com](http://faq.labs.cityindex.com) for help. ~~If you are still stuck, post a new FAQ question; tagging it ciapi.cs.~~

~~Use the [GitHub issue tracker](https://github.com/cityindex/CIAPI.CS/issues) to report bugs and make feature requests.~~

~~*NB! Bugs reported without attached test cases (preferably as unit tests) will not be investigated*~~

~~We welcome patches - please fork the project & submit a pull request.~~

### Building from source

   * Clone this repo
   * Refer to [DEV_SETUP.txt](https://github.com/cityindex/CIAPI.CS/blob/master/DEV_SETUP.txt) for pre-reqs
   * Open src/{desired platform}.sln in VS2010
   * Run build.cmd to compile, run all tests and package

## License

Copyright 2010-2013 City Index Ltd.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

  [http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
