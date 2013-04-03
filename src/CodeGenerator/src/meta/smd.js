exports.smd =

{
    "SMDVersion": "2.6",
    "version": "0.111.0.0",
    "description": "CIAPI SMD",
    "services": {
        "rpc": {
            "target": "",
            "services": {
                "LogOn": {
                    "description": "Create a new session. This is how you \"log on\" to the CIAPI.",
                    "target": "session",
                    "uriTemplate": "/",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiLogOnResponseDTO"
                    },
                    "group": "Authentication",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ApiLogOnRequestDTO",
                          "name": "apiLogOnRequest",
                          "description": "The request to create a session *(log on)*."
                      }
                    ]
                },
                "DeleteSession": {
                    "description": "Delete a session. This is how you \"log off\" from the CIAPI.",
                    "target": "session",
                    "uriTemplate": "/deleteSession?UserName={UserName}&session={session}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiLogOffResponseDTO"
                    },
                    "group": "Authentication",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "UserName",
                          "description": "Username is case sensitive. May be set as a service parameter or as a request header.",
                          "demoValue": "CC735158",
                          "minLength": 6,
                          "maxLength": 20
                      },
                      {
                          "type": "string",
                          "name": "Session",
                          "description": "The session token. May be set as a service parameter or as a request header.",
                          "demoValue": "5998CBE8-3594-4232-A57E-09EC3A4E7AA8",
                          "format": "guid",
                          "minLength": 36,
                          "maxLength": 36
                      }
                    ]
                },
                "ChangePassword": {
                    "description": "Change a user's password.",
                    "target": "session",
                    "uriTemplate": "/changePassword",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiChangePasswordResponseDTO"
                    },
                    "group": "Authentication",
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ApiChangePasswordRequestDTO",
                          "name": "apiChangePasswordRequest",
                          "description": "The change password request details."
                      }
                    ]
                },
                "GetPriceBars": {
                    "description": "Get historic price bars for the specified market in OHLC *(open, high, low, close)* format, suitable for plotting in candlestick charts. Returns price bars in ascending order up to the current time. When there are no prices for a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has \"gaps\", i.e. the gap between the date & time of each price bar might not be equal to interval x span. \n\n Sample Urls: \n\n* /market/1234/history?interval=MINUTE&span=15&PriceBars=180 \n* /market/735/history?interval=HOUR&span=1&PriceBars=240 \n* /market/1577/history?interval=DAY&span=1&PriceBars=10",
                    "target": "market",
                    "uriTemplate": "/{MarketId}/barhistory?interval={interval}&span={span}&PriceBars={PriceBars}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetPriceBarResponseDTO"
                    },
                    "group": "Price History",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "MarketId",
                          "description": "The ID of the market.",
                          "demoValue": "71442"
                      },
                      {
                          "type": "string",
                          "name": "interval",
                          "description": "The pricebar interval.",
                          "demoValue": "MINUTE"
                      },
                      {
                          "type": "integer",
                          "name": "span",
                          "description": "The number of each interval per pricebar.",
                          "demoValue": 1
                      },
                      {
                          "type": "string",
                          "name": "PriceBars",
                          "description": "The total number of price bars to return.",
                          "demoValue": "15"
                      }
                    ]
                },
                "GetPriceTicks": {
                    "description": "Get historic price ticks for the specified market. Returns price ticks in ascending order up to the current time. The length of time that elapses between each tick is usually different.",
                    "target": "market",
                    "uriTemplate": "/{MarketId}/tickhistory?PriceTicks={PriceTicks}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetPriceTickResponseDTO"
                    },
                    "group": "Price History",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "MarketId",
                          "description": "The market ID.",
                          "demoValue": "71442"
                      },
                      {
                          "type": "string",
                          "name": "PriceTicks",
                          "description": "The total number of price ticks to return.",
                          "demoValue": "10"
                      }
                    ]
                },
                "ListNewsHeadlinesWithSource": {
                    "description": "Get a list of current news headlines.",
                    "target": "news",
                    "uriTemplate": "/{source}/{category}?MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListNewsHeadlinesResponseDTO"
                    },
                    "group": "News",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "source",
                          "description": "The news feed source provider. Valid options are: **dj**|**mni**|**ci**.",
                          "demoValue": "dj"
                      },
                      {
                          "type": "string",
                          "name": "category",
                          "description": "Filter headlines by category. Valid categories depend on the source used:  for **dj**: *uk*|*aus*, for **ci**: *SEMINARSCHINA*, for **mni**: *ALL*.",
                          "demoValue": "UK",
                          "minLength": 2,
                          "maxLength": 3
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "Specify the maximum number of headlines returned.",
                          "demoValue": 10,
                          "default": 25,
                          "minimum": 1,
                          "maximum": 500
                      }
                    ]
                },
                "ListNewsHeadlines": {
                    "description": "Get a list of current news headlines.",
                    "target": "news",
                    "uriTemplate": "/headlines",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListNewsHeadlinesResponseDTO"
                    },
                    "group": "News",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ListNewsHeadlinesRequestDTO",
                          "name": "request",
                          "description": "Object specifying the various request parameters.",
                      },
                    ]
                },
                "GetNewsDetail": {
                    "description": "Get the detail of the specific news story matching the story ID in the parameter.",
                    "target": "news",
                    "uriTemplate": "/{source}/{storyId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetNewsDetailResponseDTO"
                    },
                    "group": "News",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "source",
                          "description": "The news feed source provider. Valid options are **dj**|**mni**|**ci**.",
                          "demoValue": "dj"
                      },
                      {
                          "type": "string",
                          "name": "storyId",
                          "description": "The news story ID.",
                          "demoValue": "12654",
                          "minLength": 1,
                          "maxLength": 9
                      }
                    ]
                },
                "ListCfdMarkets": {
                    "description": "Returns a list of CFD markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.",
                    "target": "cfd/markets",
                    "uriTemplate": "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={ClientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListCfdMarketsResponseDTO"
                    },
                    "group": "CFD Markets",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "searchByMarketName",
                          "description": "The characters that the CFD market name starts with. *(Optional)*.",
                          "demoValue": "voda",
                          "minLength": 1,
                          "maxLength": 120
                      },
                      {
                          "type": "string",
                          "name": "searchByMarketCode",
                          "description": "The characters that the market code starts with, normally this is the RIC code for the market. *(Optional)*.",
                          "minLength": 1,
                          "maxLength": 50
                      },
                      {
                          "type": "integer",
                          "name": "ClientAccountId",
                          "description": "The logged on user's ClientAccountId. This only shows you the markets that the user can trade. *(Required)*.",
                          "demoValue": 123456,
                          "minimum": 1,
                          "maximum": 2147483647,
                          "required": true
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of markets to return.",
                          "demoValue": 20,
                          "minimum": 1,
                          "maximum": 200,
                          "default": 20
                      },
                      {
                          "type": "boolean",
                          "name": "useMobileShortName",
                          "description": "True if the market name should be in short form. Helpful when displaying data on a small screen.",
                          "default": false
                      }
                    ]
                },
                "ListSpreadMarkets": {
                    "description": "Returns a list of Spread Betting markets filtered by market name and/or market code. Leave the market name and code parameters empty to return all markets available to the User.",
                    "target": "spread/markets",
                    "uriTemplate": "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={ClientAccountId}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListSpreadMarketsResponseDTO"
                    },
                    "group": "Spread Markets",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "searchByMarketName",
                          "description": "The characters that the Spread market name starts with. *(Optional)*.",
                          "demoValue": "voda",
                          "minLength": 1,
                          "maxLength": 120
                      },
                      {
                          "type": "string",
                          "name": "searchByMarketCode",
                          "description": "The characters that the Spread market code starts with, normally this is the RIC code for the market. *(Optional)*.",
                          "demoValue": "VOD.L",
                          "minLength": 1,
                          "maxLength": 50
                      },
                      {
                          "type": "integer",
                          "name": "ClientAccountId",
                          "description": "The logged on user's ClientAccountId. *(This only shows you markets that you can trade on.)*",
                          "demoValue": 123456,
                          "minimum": 1,
                          "maximum": 84272157
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of markets to return.",
                          "demoValue": 20,
                          "minimum": 1,
                          "maximum": 500,
                          "optional": true,
                          "default": 20
                      },
                      {
                          "type": "boolean",
                          "name": "useMobileShortName",
                          "description": "True if the market name should be in short form. Helpful when displaying data on a small screen.",
                          "default": false
                      }
                    ]
                },
                "GetMarketInformation": {
                    "description": "Get Market Information for the single specified market supplied in the parameter.",
                    "target": "market",
                    "uriTemplate": "/{MarketId}/information",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetMarketInformationResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 1000,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "MarketId",
                          "description": "The market ID.",
                          "demoValue": "71442"
                      }
                    ]
                },
                "ListMarketInformationSearch": {
                    "description": "Returns market information for the markets that meet the search criteria. The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets.",
                    "target": "market",
                    "uriTemplate": "/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&IncludeOptions={includeOptions}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListMarketInformationSearchResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "boolean",
                          "name": "searchByMarketCode",
                          "description": "Sets the search to use market code.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "searchByMarketName",
                          "description": "Sets the search to use market Name.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "spreadProductType",
                          "description": "Sets the search to include spread bet markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "cfdProductType",
                          "description": "Sets the search to include CFD markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "binaryProductType",
                          "description": "Sets the search to include binary markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "includeOptions",
                          "description": "When set to true, the search captures and returns options markets. When set to false, options markets are excluded from the search results.",
                          "demoValue": true
                      },
                      {
                          "type": "string",
                          "name": "query",
                          "description": "The text to search for. Matches part of market name / code from the start.",
                          "demoValue": "UK 100"
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of results to return.",
                          "demoValue": 50
                      },
                      {
                          "type": "boolean",
                          "name": "useMobileShortName",
                          "description": "True if the market name should be in short form.  Helpful when displaying data on a small screen.",
                          "default": false
                      }
                    ]
                },
                "ListMarketSearch": {
                    "description": "Returns a list of markets that meet the search criteria. The search can be performed by market code and/or market name, and can include CFDs and Spread Bet markets. Leave the query string empty to return all markets available to the user.",
                    "target": "market",
                    "uriTemplate": "/search?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&IncludeOptions={includeOptions}&Query={query}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListMarketSearchResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "boolean",
                          "name": "searchByMarketCode",
                          "description": "Sets the search to use market code.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "searchByMarketName",
                          "description": "Sets the search to use market Name.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "spreadProductType",
                          "description": "Sets the search to include spread bet markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "cfdProductType",
                          "description": "Sets the search to include CFD markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "binaryProductType",
                          "description": "Sets the search to include binary markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "includeOptions",
                          "description": "When set to true, the search captures and returns options markets. When set to false, options markets are excluded from the search results.",
                          "demoValue": true
                      },
                      {
                          "type": "string",
                          "name": "query",
                          "description": "The text to search for. Matches part of market name / code from the start. *(Optional)*.",
                          "demoValue": "UK 100"
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of results to return.",
                          "demoValue": 50
                      },
                      {
                          "type": "boolean",
                          "name": "useMobileShortName",
                          "description": "True if the market name should be in short form.  Helpful when displaying data on a small screen.",
                          "default": false
                      }
                    ]
                },
                "SearchWithTags": {
                    "description": "Get market information and tags for the markets that meet the search criteria. Leave the query string empty to return all markets and tags available to the user.",
                    "target": "market",
                    "uriTemplate": "/searchwithtags?Query={query}&TagId={tagId}&SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&IncludeOptions={includeOptions}&MaxResults={maxResults}&UseMobileShortName={useMobileShortName}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.MarketInformationSearchWithTagsResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "query",
                          "description": "The text to search for. Matches part of market name / code from the start. *(Optional)*.",
                          "demoValue": "UK 100"
                      },
                      {
                          "type": "integer",
                          "name": "tagId",
                          "description": "The ID for the tag to be searched. *(Optional)*.",
                          "demoValue": 0
                      },
                      {
                          "type": "boolean",
                          "name": "searchByMarketCode",
                          "description": "Sets the search to use market code.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "searchByMarketName",
                          "description": "Sets the search to use market Name.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "spreadProductType",
                          "description": "Sets the search to include spread bet markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "cfdProductType",
                          "description": "Sets the search to include CFD markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "binaryProductType",
                          "description": "Sets the search to include binary markets.",
                          "demoValue": true
                      },
                      {
                          "type": "boolean",
                          "name": "includeOptions",
                          "description": "When set to true, the search captures and returns options markets. When set to false, options markets are excluded from the search results.",
                          "demoValue": true
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of results to return. Default is 20.",
                          "demoValue": 50
                      },
                      {
                          "type": "boolean",
                          "name": "useMobileShortName",
                          "description": "True if the market name should be in short form. Helpful when displaying data on a small screen.",
                          "default": false
                      }
                    ]
                },
                "TagLookup": {
                    "description": "Gets all of the tags that the requesting user is allowed to see. Tags are returned in a primary / secondary hierarchy. There are no parameters in this call.",
                    "target": "market",
                    "uriTemplate": "/taglookup",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.MarketInformationTagLookupResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": []
                },
                "ListMarketInformation": {
                    "description": "Get Market Information for the specified list of markets.",
                    "target": "market",
                    "uriTemplate": "/information",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ListMarketInformationResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 1000,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ListMarketInformationRequestDTO",
                          "name": "listMarketInformationRequestDTO",
                          "description": "Get Market Information for the specified list of markets."
                      }
                    ]
                },
                "SaveMarketInformation": {
                    "description": "Save Market Information for the specified list of markets.",
                    "target": "market",
                    "uriTemplate": "/information/save",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiSaveMarketInformationResponseDTO"
                    },
                    "group": "Market",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.SaveMarketInformationRequestDTO",
                          "name": "listMarketInformationRequestSaveDTO",
                          "description": "Save Market Information for the specified list of markets."
                      }
                    ]
                },
                "Save": {
                    "description": "Save client preferences.",
                    "target": "clientpreference",
                    "uriTemplate": "/save",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.UpdateDeleteClientPreferenceResponseDTO"
                    },
                    "group": "Preference",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.SaveClientPreferenceRequestDTO",
                          "name": "saveClientPreferenceRequestDTO",
                          "description": "The client preferences key/value pairs to save."
                      }
                    ]
                },
                "Get": {
                    "description": "Get client preferences.",
                    "target": "clientpreference",
                    "uriTemplate": "/get",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.GetClientPreferenceResponseDTO"
                    },
                    "group": "Preference",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ClientPreferenceRequestDTO",
                          "name": "clientPreferenceRequestDto",
                          "description": "The client preference key to get."
                      }
                    ]
                },
                "GetKeyList": {
                    "description": "Get list of client preferences keys. There are no parameters in this call.",
                    "target": "clientpreference",
                    "uriTemplate": "/getkeylist",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetKeyListClientPreferenceResponseDTO"
                    },
                    "group": "Preference",
                    "cacheDuration": 0,
                    "parameters": []
                },
                "Delete": {
                    "description": "Delete client preference key.",
                    "target": "clientpreference",
                    "uriTemplate": "/delete",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.UpdateDeleteClientPreferenceResponseDTO"
                    },
                    "group": "Preference",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ClientPreferenceRequestDTO",
                          "name": "clientPreferenceKey",
                          "description": "The client preference key to delete."
                      }
                    ]
                },
                "Order": {
                    "description": "Place an order on a particular market. \nDo not set any order ID fields when requesting a new order, the platform will generate them.",
                    "target": "order",
                    "uriTemplate": "/newstoplimitorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "throttleScope": "trading",
                    "parameters": [
                      {
                          "$ref": "#.NewStopLimitOrderRequestDTO",
                          "name": "order",
                          "description": "The order request."
                      }
                    ]
                },
                "CancelOrder": {
                    "description": "Cancel an order.",
                    "target": "order",
                    "uriTemplate": "/cancel",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "$ref": "#.CancelOrderRequestDTO",
                          "name": "cancelOrder",
                          "description": "The cancel order request."
                      }
                    ]
                },
                "UpdateOrder": {
                    "description": "Update an order *(for adding a stop/limit or attaching an OCO relationship)*.",
                    "target": "order",
                    "uriTemplate": "/updatestoplimitorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "$ref": "#.UpdateStopLimitOrderRequestDTO",
                          "name": "order",
                          "description": "The update order request."
                      }
                    ]
                },
                "ListOpenPositions": {
                    "description": "Queries for a specified trading account's trades / open positions. \n\nThis URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetOpenPosition](http://labs.cityindex.com/docs/Content/HTTP%20Services/GetOpenPosition.htm) when you get updates on the order stream to get the updated data in this format. \n\n**Notes on Parameters** \n\n>**ClientAccountId** - this can be passed in order to retrieve all information on all trading accounts for which it is the parent. \n>**TradingAccountId** - this can be passed to retrieve information specific to a certain trading account *(the child of ClientAccount)*. \n\n If *neither* ClientAccountId nor TradingAccountId is passed, then the information returned by default from the API is ClientAccount.",
                    "target": "order",
                    "uriTemplate": "/openpositions?TradingAccountId={TradingAccountId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListOpenPositionsResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "integer",
                          "name": "TradingAccountId",
                          "description": "The ID of the trading account to get orders for."
                      }
                    ]
                },
                "ListActiveStopLimitOrders": {
                    "description": "Queries for a specified trading account's active stop / limit orders. \n\nThis URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetActiveStopLimitOrder](http://labs.cityindex.com/docs/Content/HTTP%20Services/GetActiveStopLimitOrder.htm) when you get updates on the order stream to get the updated data in this format. \n\n**Notes on Parameters** \n\n>**ClientAccountId** - this can be passed in order to retrieve all information on all trading accounts for which it is the parent. \n>**TradingAccountId** - this can be passed to retrieve information specific to a certain trading account *(the child of ClientAccount)*. \n\n If *neither* ClientAccountId nor TradingAccountId is passed, then the information returned by default from the API is ClientAccount.",
                    "target": "order",
                    "uriTemplate": "/activestoplimitorders?TradingAccountId={TradingAccountId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListActiveStopLimitOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "integer",
                          "name": "TradingAccountId",
                          "description": "The ID of the trading account to get orders for."
                      }
                    ]
                },
                "ListActiveOrders": {
                    "description": "Queries the specified trading account for all open positions and active orders. \n\nThis URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call the HTTP service [GetOpenPosition](http://labs.cityindex.com/docs/Content/HTTP%20Services/GetOpenPosition.htm) when you get updates on the order stream to get the updated data in this format. \n\n**Notes on Parameters** \n\n>**ClientAccountId** - this can be passed in order to retrieve all information on all trading accounts for which it is the parent. \n>**TradingAccountId** - this can be passed to retrieve information specific to a certain trading account *(the child of ClientAccount)*. \n\n If *neither* ClientAccountId nor TradingAccountId is passed, then the information returned by default from the API is ClientAccount.",
                    "target": "order",
                    "uriTemplate": "/activeorders",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListActiveOrdersResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "$ref": "#.ListActiveOrdersRequestDTO",
                          "name": "requestDTO",
                          "description": "Contains the request for a ListActiveOrders query."
                      }
                    ]
                },
                "GetActiveStopLimitOrder": {
                    "description": "Queries for an active stop limit order with a specified order ID. It returns a null value if the order doesn't exist, or is not an active stop limit order.\n \nThis URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service [ListActiveStopLimitOrders](http://labs.cityindex.com/docs/Content/HTTP%20Services/ListActiveStopLimitOrders.htm) for the initial data to display in the grid, and call this URI when you get updates on the order stream to get the updated data in this format. For a more comprehensive order response, see the HTTP service [GetOrder](http://labs.cityindex.com/docs/Content/HTTP%20Services/GetOrder.htm).",
                    "target": "order",
                    "uriTemplate": "/{OrderId}/activestoplimitorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetActiveStopLimitOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "string",
                          "name": "OrderId",
                          "description": "The requested order ID."
                      }
                    ]
                },
                "GetOpenPosition": {
                    "description": "Queries for a trade / open position with a specified order ID. It returns a null value if the order doesn't exist, or is not a trade / open position. \n\nThis URI is intended to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call the HTTP service [ListOpenPositions](http://labs.cityindex.com/docs/Content/HTTP%20Services/ListOpenPositions.htm) for the initial data to display in the grid, and call this URI when you get updates on the order stream to get the updated data in this format. \nFor a more comprehensive order response, see the HTTP service [GetOrder](http://labs.cityindex.com/docs/Content/HTTP%20Services/GetOrder.htm).",
                    "target": "order",
                    "uriTemplate": "/{OrderId}/openposition",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetOpenPositionResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "string",
                          "name": "OrderId",
                          "description": "The requested order ID."
                      }
                    ]
                },
                "ListTradeHistory": {
                    "description": "Queries for a specified trading account's trade history. The result set will contain orders with a status of __(3 - Open, 9 - Closed)__, and includes __orders that were a trade / stop / limit order__. There's currently no corresponding GetTradeHistory *(as with [ListOpenPositions](http://labs.cityindex.com/docs/Content/HTTP%20Services/ListOpenPositions.htm))*. \n\n**Notes on Parameters** \n\n>**ClientAccountId** - this can be passed in order to retrieve all information on all trading accounts for which it is the parent. \n>**TradingAccountId** - this can be passed to retrieve information specific to a certain trading account *(the child of ClientAccount)*. \n\n If *neither* ClientAccountId nor TradingAccountId is passed, then the information returned by default from the API is ClientAccount.",
                    "target": "order",
                    "uriTemplate": "/order/tradehistory?TradingAccountId={TradingAccountId}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListTradeHistoryResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "integer",
                          "name": "TradingAccountId",
                          "description": "The ID of the trading account to get orders for."
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of results to return."
                      }
                    ]
                },
                "ListStopLimitOrderHistory": {
                    "description": "Queries for a specified trading account's stop / limit order history. The result set includes __only orders that were originally stop / limit orders__ that currently have one of the following statuses __(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)__.  There is currently no corresponding GetStopLimitOrderHistory *(as with [ListActiveStopLimitOrders](http://labs.cityindex.com/docs/Content/HTTP%20Services/ListActiveStopLimitOrders.htm))*. \n\n**Notes on Parameters** \n\n>**ClientAccountId** - this can be passed in order to retrieve all information on all trading accounts for which it is the parent. \n>**TradingAccountId** - this can be passed to retrieve information specific to a certain trading account *(the child of ClientAccount)*. \n\n If *neither* ClientAccountId nor TradingAccountId is passed, then the information returned by default from the API is ClientAccount.",
                    "target": "order",
                    "uriTemplate": "/stoplimitorderhistory?TradingAccountId={TradingAccountId}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListStopLimitOrderHistoryResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "integer",
                          "name": "TradingAccountId",
                          "description": "The ID of the trading account to get orders for."
                      },
                      {
                          "type": "integer",
                          "name": "maxResults",
                          "description": "The maximum number of results to return."
                      }
                    ]
                },
                "GetOrder": {
                    "description": "Queries for an order by a specific order ID. The current implementation only returns active orders *(i.e. those with a status of __1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered__)*.",
                    "target": "order",
                    "uriTemplate": "/{OrderId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "type": "string",
                          "name": "OrderId",
                          "description": "The requested order ID."
                      }
                    ]
                },
                "Trade": {
                    "description": "Place a trade on a particular market. \nDo not set any order ID fields when requesting a new trade, the platform will generate them.",
                    "target": "order",
                    "uriTemplate": "/newtradeorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "throttleScope": "trading",
                    "parameters": [
                      {
                          "$ref": "#.NewTradeOrderRequestDTO",
                          "name": "trade",
                          "description": "The trade request."
                      }
                    ]
                },
                "UpdateTrade": {
                    "description": "Update a trade *(for adding a stop/limit etc)*.",
                    "target": "order",
                    "uriTemplate": "/updatetradeorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "cacheDuration": 0,
                    "throttleScope": "trading",
                    "parameters": [
                      {
                          "$ref": "#.UpdateTradeOrderRequestDTO",
                          "name": "update",
                          "description": "The update trade request."
                      }
                    ]
                },
                "GetClientAndTradingAccount": {
                    "description": "Returns the User's ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.",
                    "target": "useraccount",
                    "uriTemplate": "/ClientAndTradingAccount",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.AccountInformationResponseDTO"
                    },
                    "group": "AccountInformation",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": []
                },
                "SaveAccountInformation": {
                    "description": "Saves the users account information.",
                    "target": "useraccount",
                    "uriTemplate": "/Save",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiSaveAccountInformationResponseDTO"
                    },
                    "group": "AccountInformation",
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ApiSaveAccountInformationRequestDTO",
                          "name": "saveAccountInformationRequest",
                          "description": "Saves the users account information."
                      }
                    ]
                },
                "GetSystemLookup": {
                    "description": "Use the message lookup service to get localised text names for the various status codes & IDs returned by the API. For example, a query for **OrderStatusReason** will contain text names for all the possible values of **OrderStatusReason** in the [ApiOrderResponseDTO](http://labs.cityindex.com/docs/Content/Data%20Types/ApiOrderResponseDTO.htm). You should only request the list once per session *(for each entity you're interested in)*.",
                    "target": "message",
                    "uriTemplate": "/lookup?LookupEntityName={LookupEntityName}&CultureId={CultureId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ApiLookupResponseDTO"
                    },
                    "group": "Messages",
                    "cacheDuration": 3600000,
                    "parameters": [
                      {
                          "type": "string",
                          "name": "LookupEntityName",
                          "description": "The entity to lookup. For example: **OrderStatusReason**, **InstructionStatusReason**, **OrderApplicability**, **Currency**, **QuoteStatus**, **QuoteStatusReason** or **Culture**."
                      },
                      {
                          "type": "integer",
                          "name": "CultureId",
                          "description": "The Culture ID used to override the translated text description. *(Optional)*."
                      }
                    ]
                },
                "GetClientApplicationMessageTranslation": {
                    "description": "Use the message translation service to get client specific translated text strings.",
                    "target": "message",
                    "uriTemplate": "/translation?ClientApplicationId={ClientApplicationId}&CultureId={CultureId}&AccountOperatorId={AccountOperatorId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ApiClientApplicationMessageTranslationResponseDTO"
                    },
                    "group": "Messages",
                    "cacheDuration": 3600000,
                    "parameters": [
                      {
                          "type": "integer",
                          "name": "ClientApplicationId",
                          "description": "Client application identifier. *(Optional)*"
                      },
                      {
                          "type": "integer",
                          "name": "CultureId",
                          "description": "Culture ID which corresponds to a culture code. *(Optional)*"
                      },
                      {
                          "type": "integer",
                          "name": "AccountOperatorId",
                          "description": "Account operator identifier. *(Optional)*"
                      }
                    ]
                },
                "GetClientApplicationMessageTranslationWithInterestingItems": {
                    "description": "Use the message translation service to get client specific translated textual strings for specific keys.",
                    "target": "message",
                    "uriTemplate": "/translationWithInterestingItems",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiClientApplicationMessageTranslationResponseDTO"
                    },
                    "group": "Messages",
                    "cacheDuration": 0,
                    "parameters": [
                      {
                          "$ref": "#.ApiClientApplicationMessageTranslationRequestDTO",
                          "name": "apiClientApplicationMessageTranslationRequestDto",
                          "description": "DTO of the required data for translation lookup."
                      }
                    ]
                },
                "GetWatchlists": {
                    "description": "Gets all watchlists for the user account. There are no parameters for this call.",
                    "target": "watchlists",
                    "uriTemplate": "/",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.ListWatchlistResponseDTO"
                    },
                    "group": "Watchlist",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": []
                },
                "SaveWatchlist": {
                    "description": "Save watchlist.",
                    "target": "watchlist",
                    "uriTemplate": "/Save",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiSaveWatchlistResponseDTO"
                    },
                    "group": "Watchlist",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ApiSaveWatchlistRequestDTO",
                          "name": "apiSaveWatchlistRequestDto",
                          "description": "The watchlist to save."
                      }
                    ]
                },
                "DeleteWatchlist": {
                    "description": "Delete a watchlist.",
                    "target": "watchlist",
                    "uriTemplate": "/delete",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiDeleteWatchlistResponseDTO"
                    },
                    "group": "Watchlist",
                    "cacheDuration": 0,
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.ApiDeleteWatchlistRequestDTO",
                          "name": "deleteWatchlistRequestDto",
                          "description": "The watchlist to delete."
                      }
                    ]
                },
                "GetVersionInformation": {
                    "description": "Gets version information for a specific client application and *(optionally)* account operator.",
                    "target": "clientapplication/versioninformation",
                    "uriTemplate": "?AppKey={AppKey}&AccountOperatorId={AccountOperatorId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "#.GetVersionInformationResponseDTO"
                    },
                    "group": "Client Application",
                    "cacheDuration": 360000,
                    "parameters": [
                      {
                          "type": "string",
                          "name": "AppKey",
                          "description": "A string to uniquely identify the application."
                      },
                      {
                          "type": "integer",
                          "name": "AccountOperatorId",
                          "description": "An optional parameter to identify the account operator string to uniquely identify the application."
                      }
                    ]
                },
                "SimulateTrade": {
                    "description": "API call that allows a simulated new trade to be placed.",
                    "target": "order",
                    "uriTemplate": "/simulate/newtradeorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "#.ApiSimulateTradeOrderResponseDTO"
                    },
                    "group": "Authentication",
                    "throttleScope": "data",
                    "parameters": [
                      {
                          "$ref": "#.NewTradeOrderRequestDTO",
                          "name": "Trade",
                          "description": "The simulated trade request."
                      }
                    ]
                }
            }
        },
        "streaming": {
            "target": "",
            "services": {
                "NewsHeadlines": {
                    "description": "Stream of current news headlines. Try NEWS.HEADLINES.UK for a mock stream.",
                    "target": "CITYINDEXSTREAMING",
                    "channel": "NEWS.HEADLINES.{category}",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "NewsDTO"
                    },
                    "group": "Streaming API",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "category",
                          "description": "A news category. See [Usage Notes: News Feeds](http://labs.cityindex.com/docs/Content/News.htm)",
                          "minLength": 1,
                          "maxLength": 100,
                          "demoValue": "UK"
                      }
                    ]
                },
                "Prices": {
                    "description": "Stream of current prices. Try PRICES.PRICE.154297 (GBP/USD (per 0.0001) CFD) which prices Mon - Fri 24hrs.",
                    "target": "CITYINDEXSTREAMING",
                    "channel": "PRICES.PRICE.{marketIds}",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "PriceDTO"
                    },
                    "group": "Streaming API",
                    "parameters": [
                      {
                          "type": "array",
                          "items": [
                            {
                                "type": "integer"
                            }
                          ],
                          "name": "marketIds",
                          "description": "The marketIds",
                          "demoValue": "[\"71442\", \"71443\"]"
                      }
                    ]
                },
                "DefaultPrices": {
                    "description": "Stream of default prices for the specified account operator. This stream does not require authentication, so can be used on a public website.  **NB:** this stream returns prices for a group of markets, so check the MarketId & Name field when displaying.",
                    "target": "CITYINDEXSTREAMINGDEFAULTPRICES",
                    "channel": "AC{AccountOperatorId}",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "#.PriceDTO"
                    },
                    "group": "Streaming API",
                    "parameters": [
                      {
                          "type": "string",
                          "name": "AccountOperatorId",
                          "description": "The account operator ID whose default market prices are required. Generally you want to hard code this depending on the brand you are using.  See [http://faq.labs.cityindex.com/questions/what-are-the-list-of-accountoperatorids](http://faq.labs.cityindex.com/questions/what-are-the-list-of-accountoperatorids). The AccountOperatorId parameter requires an AC prefix, for example AC1234 instead of just 1234. ",
                          "demoValue": "3347"
                      }
                    ]
                },
                "ClientAccountMargin": {
                    "description": "Stream of clients current margin.",
                    "target": "STREAMINGCLIENTACCOUNT",
                    "channel": "CLIENTACCOUNTMARGIN.ALL",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "#.ClientAccountMarginDTO"
                    },
                    "group": "Streaming API"
                },
                "TradeMargin": {
                    "description": "Stream of trade margin.",
                    "target": "STREAMINGCLIENTACCOUNT",
                    "channel": "TRADEMARGIN.All",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "#.TradeMarginDTO"
                    },
                    "group": "Streaming API"
                },
                "Orders": {
                    "description": "Stream of orders.",
                    "target": "STREAMINGCLIENTACCOUNT",
                    "channel": "ORDERS.All",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "#.OrderDTO"
                    },
                    "group": "Streaming API"
                },
                "Quotes": {
                    "description": "Stream of quotes.",
                    "target": "STREAMINGTRADINGACCOUNT",
                    "channel": "QUOTE.ALL",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-4",
                    "returns": {
                        "$ref": "#.QuoteDTO"
                    },
                    "group": "Streaming API"
                }
            }
        }
    }
}