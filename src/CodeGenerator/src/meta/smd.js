
exports.smd =
{
    "SMDVersion": "2.6",
    "version": "1",
    "description": "CIAPI SMD",
    "services": {
        "streaming": {
            "target": "http://pushpreprod.cityindextest9.co.uk/",
            "services": {
                "NewsHeadlines": {
                    "description": "Stream of current news headlines.  Try NEWS.MOCKHEADLINES.UK for a mock stream",
                    "target": "CITYINDEXSTREAMING",
                    "channel": "NEWS.HEADLINES.{category}",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-3.6",
                    "returns": {
                        "$ref": "NewsDTO"
                    },
                    "group": "Streaming API",
                    "parameters": [
                {
                    "type": "string",
                    "name": "category",
                    "description": "A news category",
                    "minLength": 1,
                    "maxLength": 100,
                    "demoValue": "UK"
                }
            ]
                },
                "Prices": {
                    "description": "Stream of current prices. Try PRICES.PRICE.154297 (GBP/USD (per 0.0001) CFD) which prices Mon - Fri 24hrs",
                    "target": "CITYINDEXSTREAMING",
                    "channel": "PRICES.PRICE.{marketIds}",
                    "transport": "HTTP",
                    "protocol": "lightstreamer-3.6",
                    "returns": {
                        "$ref": "PriceDTO"
                    },
                    "group": "Streaming API",
                    "parameters": [
                        {
                            "type": "array",
                            "items": [{ "type": "string"}],
                            "name": "marketIds",
                            "description": "The marketIds",
                            "demoValue": "[\"71442\", \"71443\"]"
                        }]
                }

            }
        },
        "rpc": {
            "target": "http://ciapipreprod.cityindextest9.co.uk/TradingApi/",
            "services": {
                "LogOn": {
                    "description": "<p>Create a new session. This is how you \"log on\" to the CIAPI. Post a <a onclick=\"dojo.hash('#type.ApiLogOnRequestDTO'); return false;\" class=\"json-link\" href=\"#\">ApiLogOnRequestDTO</a> to the uri specified below</p>",
                    "target": "session",
                    "uriTemplate": "/",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiLogOnResponseDTO"
                    },
                    "group": "Authentication",
                    "throttleScope": "data",
                    "parameters": [
            {
                "$ref": "#/ApiLogOnRequestDTO",
                "name": "apiLogOnRequest",
                "description": ""
            }
          ]
                },
                "DeleteSession": {
                    "description": "<p>Delete a session. This is how you \"log off\" from the CIAPI.</p>",
                    "target": "session",
                    "uriTemplate": "/deleteSession?userName={userName}&session={session}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiLogOffResponseDTO"
                    },
                    "group": "Authentication",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "userName",
                "description": "Username is case sensitive. May be set as a service parameter or as a request header.",
                "demoValue": "CC735158",
                "minLength": 6,
                "maxLength": 20
            },
            {
                "type": "string",
                "name": "session",
                "description": "The session token. May be set as a service parameter or as a request header.",
                "demoValue": "5998CBE8-3594-4232-A57E-09EC3A4E7AA8",
                "format": "guid",
                "minLength": 36,
                "maxLength": 36
            }
          ]
                },
                "GetPriceBars": {
                    "description": "Get historic price bars in OHLC (open, high, low, close) format, suitable for plotting candlestick chartsReturns price bars in ascending order up to the current time.When there are no prices per a particular time period, no price bar is returned. Thus, it can appear that the array of price bars has \"gaps\", i.e. the gap between the datetime of each price bar might not be equal to interval x spanSample Urls: /market/1234/history?interval=MINUTE&span=15&pricebars=180/market/735/history?interval=HOUR&span=1&pricebars=240/market/1577/history?interval=DAY&span=1&pricebars=10",
                    "target": "market",
                    "uriTemplate": "/{marketId}/barhistory?interval={interval}&span={span}&pricebars={priceBars}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetPriceBarResponseDTO"
                    },
                    "group": "Price History",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "marketId",
                "description": "The marketId",
                "demoValue": "71442"
            },
            {
                "type": "string",
                "name": "interval",
                "description": "The pricebar interval",
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
                "name": "priceBars",
                "description": "The total number of pricebars to return",
                "demoValue": "15"
            }
          ]
                },
                "GetPriceTicks": {
                    "description": "Get historic price ticks. Returns price ticks in ascending order up to the current time. The length of time between each tick will be different.",
                    "target": "market",
                    "uriTemplate": "/{marketId}/tickhistory?priceticks={priceTicks}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetPriceTickResponseDTO"
                    },
                    "group": "Price History",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "marketId",
                "description": "The marketId",
                "demoValue": "71442"
            },
            {
                "type": "string",
                "name": "priceTicks",
                "description": "The total number of price ticks to return",
                "demoValue": "10"
            }
          ]
                },
                "GetMarketInformation": {
                    "description": "<p>Get Market Information for the specified market.</p>",
                    "target": "market",
                    "uriTemplate": "/{marketId}/information",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetMarketInformationResponseDTO"
                    },
                    "group": "Market",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "marketId",
                "description": "The marketId",
                "demoValue": "71442"
            }
          ]
                },
                "ListMarketInformationSearch": {
                    "description": "<p>Queries for market information.</p>",
                    "target": "market",
                    "uriTemplate": "/informationsearch?SearchByMarketCode={searchByMarketCode}&SearchByMarketName={searchByMarketName}&SpreadProductType={spreadProductType}&CfdProductType={cfdProductType}&BinaryProductType={binaryProductType}&Query={query}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListMarketInformationSearchResponseDTO"
                    },
                    "group": "Market",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "boolean",
                "name": "searchByMarketCode",
                "description": "Should the search be done by market code",
                "demoValue": true
            },
            {
                "type": "boolean",
                "name": "searchByMarketName",
                "description": "Should the search be done by market Name",
                "demoValue": true
            },
            {
                "type": "boolean",
                "name": "spreadProductType",
                "description": "Should the search include spread bet markets",
                "demoValue": true
            },
            {
                "type": "boolean",
                "name": "cfdProductType",
                "description": "Should the search include CFD markets",
                "demoValue": true
            },
            {
                "type": "boolean",
                "name": "binaryProductType",
                "description": "Should the search include binary markets",
                "demoValue": true
            },
            {
                "type": "string",
                "name": "query",
                "description": "The text to search for.  Matches part of market name / code from the start.",
                "demoValue": "UK 100"
            },
            {
                "type": "integer",
                "name": "maxResults",
                "description": "The maximum number of results to return",
                "demoValue": 50
            }
          ]
                },
                "ListNewsHeadlines": {
                    "description": "Get a list of current news headlines",
                    "target": "news",
                    "uriTemplate": "?Category={category}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListNewsHeadlinesResponseDTO"
                    },
                    "group": "News",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "category",
                "description": "Filter headlines by category",
                "demoValue": "UK",
                "minLength": 2,
                "maxLength": 3
            },
            {
                "type": "integer",
                "name": "maxResults",
                "description": "Restrict the number of headlines returned",
                "demoValue": 10,
                "default": 25,
                "minimum": 1,
                "maximum": 500
            }
          ]
                },
                "GetNewsDetail": {
                    "description": "Get the detail of a specific news story",
                    "target": "news",
                    "uriTemplate": "/{storyId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetNewsDetailResponseDTO"
                    },
                    "group": "News",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "storyId",
                "description": "The news story Id",
                "demoValue": "12654",
                "minLength": 1,
                "maxLength": 9
            }
          ]
                },
                "ListCfdMarkets": {
                    "description": "Returns a list of CFD markets filtered by market name and/or market code",
                    "target": "cfd/markets",
                    "uriTemplate": "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListCfdMarketsResponseDTO"
                    },
                    "group": "CFD Markets",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "searchByMarketName",
                "description": "The characters that the CFD market name should start with",
                "demoValue": "voda",
                "minLength": 1,
                "maxLength": 120
            },
            {
                "type": "string",
                "name": "searchByMarketCode",
                "description": "The characters that the market code should start with (normally this is the RIC code for the market)",
                "minLength": 1,
                "maxLength": 50
            },
            {
                "type": "integer",
                "name": "clientAccountId",
                "description": "The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)",
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
                "maximum": 500,
                "default": 20
            }
          ]
                },
                "ListSpreadMarkets": {
                    "description": "Returns a list of Spread Betting markets filtered by market name and/or market code",
                    "target": "spread/markets",
                    "uriTemplate": "?MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListSpreadMarketsResponseDTO"
                    },
                    "group": "Spread Markets",
                    "cacheDuration": 10000,
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "searchByMarketName",
                "description": "The characters that the Spread market name should start with",
                "demoValue": "voda",
                "minLength": 1,
                "maxLength": 120
            },
            {
                "type": "string",
                "name": "searchByMarketCode",
                "description": "The characters that the Spread market code should start with (normally this is the RIC code for the market)",
                "demoValue": "VOD.L",
                "minLength": 1,
                "maxLength": 50
            },
            {
                "type": "integer",
                "name": "clientAccountId",
                "description": "The logged on user's ClientAccountId.  (This only shows you markets that you can trade on)",
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
            }
          ]
                },
                "Order": {
                    "description": "<p>Place an order on a particular market. Post a <a onclick=\"dojo.hash('#type.NewStopLimitOrderRequestDTO'); return false;\" class=\"json-link\" href=\"#\">NewStopLimitOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new order, the platform will generate them.</p>",
                    "target": "order",
                    "uriTemplate": "/newstoplimitorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "throttleScope": "trading",
                    "parameters": [
            {
                "$ref": "#/NewStopLimitOrderRequestDTO",
                "name": "order",
                "description": "The order request"
            }
          ]
                },
                "CancelOrder": {
                    "description": "<p>Cancel an order. Post a <a onclick=\"dojo.hash('#type.CancelOrderRequestDTO'); return false;\" class=\"json-link\" href=\"#\">CancelOrderRequestDTO</a> to the uri specified below</p>",
                    "target": "order",
                    "uriTemplate": "/cancel",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "$ref": "#/CancelOrderRequestDTO",
                "name": "cancelOrder",
                "description": "The cancel order request"
            }
          ]
                },
                "UpdateOrder": {
                    "description": "<p>Update an order (for adding a stop/limit or attaching an OCO relationship). Post an <a onclick=\"dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;\" class=\"json-link\" href=\"#\">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p>",
                    "target": "order",
                    "uriTemplate": "/updatestoplimitorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "$ref": "#/UpdateStopLimitOrderRequestDTO",
                "name": "order",
                "description": "<p>Update an order (for adding a stop/limit or attaching an OCO relationship).  Post an <a onclick=\"dojo.hash('#type.UpdateStopLimitOrderRequestDTO'); return false;\" class=\"json-link\" href=\"#\">UpdateStopLimitOrderRequestDTO</a> to the uri specified below</p>"
            }
          ]
                },
                "ListOpenPositions": {
                    "description": "<p>Queries for a specified trading account's trades / open positions.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick=\"dojo.hash('#service.GetOpenPosition'); return false;\" class=\"json-link\" href=\"#\">GetOpenPosition</a> when you get updates on the order stream to get the updated data in this format.</p>",
                    "target": "order",
                    "uriTemplate": "/openpositions?TradingAccountId={tradingAccountId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListOpenPositionsResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "integer",
                "name": "tradingAccountId",
                "description": "The trading account to get orders for."
            }
          ]
                },
                "ListActiveStopLimitOrders": {
                    "description": "<p>Queries for a specified trading account's active stop / limit orders.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call this for the initial data to display in the grid, and call <a onclick=\"dojo.hash('#service.GetActiveStopLimitOrder'); return false;\" class=\"json-link\" href=\"#\">GetActiveStopLimitOrder</a> when you get updates on the order stream to get the updated data in this format.</p>",
                    "target": "order",
                    "uriTemplate": "/activestoplimitorders?TradingAccountId={tradingAccountId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListActiveStopLimitOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "integer",
                "name": "tradingAccountId",
                "description": "The trading account to get orders for."
            }
          ]
                },
                "GetActiveStopLimitOrder": {
                    "description": "<p>Queries for a active stop limit order with a specified order id. It will return a null value if the order doesn't exist, or is not an active stop limit order.<p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick=\"dojo.hash('#service.ListActiveStopLimitOrders'); return false;\" class=\"json-link\" href=\"#\">ListActiveStopLimitOrders</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick=\"dojo.hash('#service.GetOrder'); return false;\" class=\"json-link\" href=\"#\">GetOrder</a><p>",
                    "target": "order",
                    "uriTemplate": "/{orderId}/activestoplimitorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetActiveStopLimitOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "string",
                "name": "orderId",
                "description": "The requested order id."
            }
          ]
                },
                "GetOpenPosition": {
                    "description": "<p>Queries for a trade / open position with a specified order id. It will return a null value if the order doesn't exist, or is not a trade / open position.</p> <p>This uri is intended to be used to support a grid in a UI. One usage pattern is to subscribe to streaming orders, call <a onclick=\"dojo.hash('#service.ListOpenPositions'); return false;\" class=\"json-link\" href=\"#\">ListOpenPositions</a> for the initial data to display in the grid, and call this uri when you get updates on the order stream to get the updated data in this format.</p> <p>For a more comprehensive order response, see <a onclick=\"dojo.hash('#service.GetOrder'); return false;\" class=\"json-link\" href=\"#\">GetOrder</a><p>",
                    "target": "order",
                    "uriTemplate": "/{orderId}/openposition",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetOpenPositionResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "string",
                "name": "orderId",
                "description": "The requested order id."
            }
          ]
                },
                "ListTradeHistory": {
                    "description": "<p>Queries for a specified trading account's trade history. The result set will contain orders with a status of <b>(3 - Open, 9 - Closed)</b>, and includes <b>orders that were a trade / stop / limit order</b>.</p> <p>There's currently no corresponding GetTradeHistory (as with ListOpenPositions).</p>",
                    "target": "order",
                    "uriTemplate": "/tradehistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListTradeHistoryResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "integer",
                "name": "tradingAccountId",
                "description": "The trading account to get orders for."
            },
            {
                "type": "integer",
                "name": "maxResults",
                "description": "The maximum results to return."
            }
          ]
                },
                "ListStopLimitOrderHistory": {
                    "description": "<p>Queries for a specified trading account's stop / limit order history. The result set will include <b>only orders that were originally stop / limit orders</b> that currently have one of the following statuses <b>(3 - Open, 4 - Cancelled, 5 - Rejected, 9 - Closed, 10 - Red Card)</b> </p> <p>There's currently no corresponding GetStopLimitOrderHistory (as with ListActiveStopLimitOrders).</p>",
                    "target": "order",
                    "uriTemplate": "/stoplimitorderhistory?TradingAccountId={tradingAccountId}&MaxResults={maxResults}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ListStopLimitOrderHistoryResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "integer",
                "name": "tradingAccountId",
                "description": "The trading account to get orders for."
            },
            {
                "type": "integer",
                "name": "maxResults",
                "description": "the maximum results to return."
            }
          ]
                },
                "GetOrder": {
                    "description": "<p>Queries for an order by a specific order id.</p> <p>The current implementation only returns active orders (i.e. those with a status of <b>1 - Pending, 2 - Accepted, 3 - Open, 6 - Suspended, 8 - Yellow Card, 11 - Triggered)</b>.</p>",
                    "target": "order",
                    "uriTemplate": "/{orderId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "parameters": [
            {
                "type": "string",
                "name": "orderId",
                "description": "The requested order id."
            }
          ]
                },
                "Trade": {
                    "description": "<p>Place a trade on a particular market. Post a <a onclick=\"dojo.hash('#type.NewTradeOrderRequestDTO'); return false;\" class=\"json-link\" href=\"#\">NewTradeOrderRequestDTO</a> to the uri specified below.</p> <p>Do not set any order id fields when requesting a new trade, the platform will generate them.</p>",
                    "target": "order",
                    "uriTemplate": "/newtradeorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "throttleScope": "trading",
                    "parameters": [
            {
                "$ref": "#/NewTradeOrderRequestDTO",
                "name": "trade",
                "description": "The trade request"
            }
          ]
                },
                "UpdateTrade": {
                    "description": "Update a trade (for adding a stop/limit etc). Post an <a onclick=\"dojo.hash('#type.UpdateTradeOrderRequestDTO'); return false;\" class=\"json-link\" href=\"#\">UpdateTradeOrderRequestDTO</a> to the uri specified below</p>",
                    "target": "order",
                    "uriTemplate": "/updatetradeorder",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "POST",
                    "envelope": "JSON",
                    "returns": {
                        "$ref": "ApiTradeOrderResponseDTO"
                    },
                    "group": "Trades and Orders",
                    "throttleScope": "trading",
                    "parameters": [
            {
                "$ref": "#/UpdateTradeOrderRequestDTO",
                "name": "update",
                "description": "The trade request"
            }
          ]
                },
                "GetClientAndTradingAccount": {
                    "description": "Returns the Users ClientAccountId and a list of their TradingAccounts. There are no parameters for this call.",
                    "target": "useraccount",
                    "uriTemplate": "/UserAccount/ClientAndTradingAccount",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "AccountInformationResponseDTO"
                    },
                    "group": "AccountInformation",
                    "throttleScope": "data",
                    "parameters": []
                },
                "GetMessage": {
                    "description": "[MISSING]",
                    "target": "message",
                    "uriTemplate": "/Message/{id}?language={language}&category={category}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "String"
                    },
                    "group": "Messaging",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "id",
                "description": "[MISSING]"
            },
            {
                "type": "string",
                "name": "language",
                "description": "[MISSING]"
            },
            {
                "type": "string",
                "name": "category",
                "description": "[MISSING]"
            }
          ]
                },
                "GetMessagePopup": {
                    "description": "[MISSING]",
                    "target": "message",
                    "uriTemplate": "/message/popup?language={language}&ClientAccountId={clientAccountId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "GetMessagePopupResponseDTO"
                    },
                    "group": "Messaging",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "language",
                "description": "[MISSING]"
            },
            {
                "type": "integer",
                "name": "clientAccountId",
                "description": "[MISSING]"
            }
          ]
                },
                "AcceptOrRejectMessagePopupResponse": {
                    "description": "[MISSING]",
                    "target": "message",
                    "uriTemplate": "/message/popupchoice?ClientAccountId={clientAccountId}&Accepted={accepted}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "Void"
                    },
                    "group": "Messaging",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "integer",
                "name": "clientAccountId",
                "description": "[MISSING]"
            },
            {
                "type": "boolean",
                "name": "accepted",
                "description": "[MISSING]"
            }
          ]
                },
                "GetSystemLookup": {
                    "description": "[MISSING]",
                    "target": "message",
                    "uriTemplate": "/message/lookup?lookupEntityName={lookupEntityName}&cultureId={cultureId}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ApiLookupResponseDTO"
                    },
                    "group": "Messaging",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "string",
                "name": "lookupEntityName",
                "description": "[MISSING]"
            },
            {
                "type": "integer",
                "name": "cultureId",
                "description": "[MISSING]"
            }
          ]
                },
                "GenerateException": {
                    "description": "Simulates an error condition.",
                    "target": "errors",
                    "uriTemplate": "?errorCode={errorCode}",
                    "contentType": "application/json",
                    "responseContentType": "application/json",
                    "transport": "GET",
                    "envelope": "URL",
                    "returns": {
                        "$ref": "ApiErrorResponseDTO"
                    },
                    "group": "Exception Handling",
                    "throttleScope": "data",
                    "parameters": [
            {
                "type": "integer",
                "name": "errorCode",
                "description": "Simulates an error condition.",
                "demoValue": 4000
            }
          ]
                }

            }
        }
    }
}
