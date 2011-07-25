
exports.schema =
{
    "namespace": " CIAPI.DTO",
    "properties": {
        "ApiStopLimitOrderHistoryDTO": {
            "id": "ApiStopLimitOrderHistoryDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order's unique identifier."
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The markets unique identifier."
                },
                "MarketName": {
                    "type": "string",
                    "description": "The market's name."
                },
                "Direction": {
                    "type": "string",
                    "description": "The direction, buy or sell."
                },
                "OriginalQuantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The quantity of the order when it became a trade / was cancelled etc."
                },
                "Price": {
                    "type": [
            "null",
            "number"
          ],
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The price / rate that the order was filled at."
                },
                "TriggerPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The price / rate that the the order was set to trigger at."
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The trading account that the order is on."
                },
                "TypeId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The type of the order stop, limit or trade."
                },
                "OrderApplicabilityId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "When the order applies until. i.e. good till cancelled (GTC) good for day (GFD) or good till time (GTT)."
                },
                "Currency": {
                    "type": "string",
                    "description": "The trade currency."
                },
                "StatusId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "the order status."
                },
                "LastChangedDateTimeUtc": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The last time that the order changed. Note - Does not include things such as the current market price."
                }
            },
            "description": "A stop or limit order from a historical perspective."
        },
        "ApiIfDoneDTO": {
            "id": "ApiIfDoneDTO",
            "type": "object",
            "properties": {
                "Stop": {
                    "type": {
                        "$ref": "ApiStopLimitOrderDTO"
                    },
                    "description": "The price at which the stop order will be filled"
                },
                "Limit": {
                    "type": {
                        "$ref": "ApiStopLimitOrderDTO"
                    },
                    "description": "The price at which the limit order will be filled"
                }
            },
            "description": "An IfDone order represents an order which is placed when the corresponding order is filled, e.g attaching a stop/limit to a trade or order"
        },
        "ApiTradingAccountDTO": {
            "id": "ApiTradingAccountDTO",
            "type": "object",
            "properties": {
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Trading Account Id"
                },
                "TradingAccountCode": {
                    "type": "string",
                    "description": "Trading Account Code"
                },
                "TradingAccountStatus": {
                    "type": "string",
                    "description": "Trading Account Status"
                },
                "TradingAccountType": {
                    "type": "string",
                    "description": "Trading Account Type"
                }
            },
            "description": "Information about a TradingAccount"
        },
        "PriceBarDTO": {
            "id": "PriceBarDTO",
            "type": "object",
            "properties": {
                "BarDate": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The date of the start of the price bar interval"
                },
                "Open": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "For the equities model of charting, this is the price at the start of the price bar interval."
                },
                "High": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The highest price occurring during the interval of the price bar"
                },
                "Low": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The lowest price occurring during the interval of the price bar"
                },
                "Close": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The price at the end of the price bar interval"
                }
            },
            "description": "The details of a specific price bar, useful for plotting candlestick charts"
        },
        "PriceDTO": {
            "id": "PriceDTO",
            "type": "object",
            "properties": {
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The Market that the Price is related to."
                },
                "TickDate": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The date of the Price. Always expressed in UTC."
                },
                "Bid": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The current Bid price (price at which the customer can sell)."
                },
                "Offer": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The current Offer price (price at which the customer can buy, some times referred to as Ask price)."
                },
                "Price": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The current mid price."
                },
                "High": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The highest price reached for the day."
                },
                "Low": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The lowest price reached for the day"
                },
                "Change": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The change since the last price (always positive. See Direction for direction)"
                },
                "Direction": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The direction of movement since the last price. 1 == up, 0 == down"
                },
                "AuditId": {
                    "type": "string",
                    "description": "A unique id for this price. Treat as a unique, but random string"
                }
            },
            "description": "A Price for a specific Market."
        },
        "ApiTradeHistoryDTO": {
            "id": "ApiTradeHistoryDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order id."
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The market id."
                },
                "MarketName": {
                    "type": "string",
                    "description": "The name of the market."
                },
                "Direction": {
                    "type": "string",
                    "description": "The direction of the trade."
                },
                "OriginalQuantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The original quantity of the trade, before part closures."
                },
                "Price": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The open price of the trade."
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The trading account that the order is on."
                },
                "Currency": {
                    "type": "string",
                    "description": "The trade currency."
                },
                "LastChangedDateTimeUtc": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The last time that the order changed. Note - Does not include things such as the current market price."
                }
            },
            "description": "A Trade from a historical perspective."
        },
        "ApiBasicStopLimitOrderDTO": {
            "id": "ApiBasicStopLimitOrderDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order's unique identifier."
                },
                "TriggerPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The order's trigger price."
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The quantity of the product."
                }
            },
            "description": "A stop or limit order with a limited number of data fields."
        },
        "NewsDTO": {
            "id": "NewsDTO",
            "type": "object",
            "properties": {
                "StoryId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The unique identifier for a news story"
                },
                "Headline": {
                    "type": "string",
                    "description": "The News story headline"
                },
                "PublishDate": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The date on which the news story was published. Always in UTC"
                }
            },
            "description": "A news headline"
        },
        "ApiClientAccountWatchlistItemDTO": {
            "id": "ApiClientAccountWatchlistItemDTO",
            "type": "object",
            "properties": {
                "WatchlistId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Parent watchlist id"
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist item market id"
                },
                "DisplayOrder": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist item display order"
                }
            },
            "description": "Api watchlist item"
        },
        "ApiLookupDTO": {
            "id": "ApiLookupDTO",
            "type": "object",
            "properties": {
                "Id": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "lookups id."
                },
                "Description": {
                    "type": "string",
                    "description": "lookup items description."
                },
                "DisplayOrder": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "order the items should be displayed on a user interface."
                },
                "TranslationTextId": {
                    "type": [
            "null",
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "translation text id."
                },
                "TranslationText": {
                    "type": "string",
                    "description": "translated text."
                },
                "IsActive": {
                    "type": "boolean",
                    "description": "is active."
                },
                "IsAllowed": {
                    "type": "boolean",
                    "description": "is allowed."
                }
            },
            "description": "Generic look up data."
        },
        "ApiOpenPositionDTO": {
            "id": "ApiOpenPositionDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order's unique identifier."
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The markets unique identifier."
                },
                "MarketName": {
                    "type": "string",
                    "description": "The market's name."
                },
                "Direction": {
                    "type": "string",
                    "description": "The direction, buy or sell."
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The quantity of the order."
                },
                "Price": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The price / rate that the trade was opened at."
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The trading account that the order is on."
                },
                "Currency": {
                    "type": "string",
                    "description": "The trade currency."
                },
                "Status": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order status."
                },
                "StopOrder": {
                    "type": {
                        "$ref": "ApiBasicStopLimitOrderDTO"
                    },
                    "description": "The stop order attached to this order."
                },
                "LimitOrder": {
                    "type": {
                        "$ref": "ApiBasicStopLimitOrderDTO"
                    },
                    "description": "The limit order attached to this order."
                },
                "LastChangedDateTimeUTC": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The last time that the order changed. Note - Does not include things such as the current market price."
                }
            },
            "description": "A Trade, or order that is currently open."
        },
        "ApiMarketEodDTO": {
            "id": "ApiMarketEodDTO",
            "type": "object",
            "properties": {
                "MarketEodUnit": {
                    "type": "string",
                    "description": "Unit"
                },
                "MarketEodAmount": {
                    "type": [
            "null",
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "End of day amount."
                }
            },
            "description": "market end of day information."
        },
        "ApiMarketInformationDTO": {
            "id": "ApiMarketInformationDTO",
            "type": "object",
            "properties": {
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "market id."
                },
                "Name": {
                    "type": "string",
                    "description": "The market name"
                },
                "MarginFactor": {
                    "type": [
            "null",
            "number"
          ],
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Margin factor, expressed as points or a percentage."
                },
                "MinDistance": {
                    "type": [
            "null",
            "number"
          ],
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The minimum distance from the current price you can place an order."
                },
                "WebMinSize": {
                    "type": [
            "null",
            "number"
          ],
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The minimum quantity that can be traded over the web."
                },
                "MaxSize": {
                    "type": [
            "null",
            "number"
          ],
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The max size of an order."
                },
                "Market24H": {
                    "type": "boolean",
                    "description": "Is the market a 24 hour market."
                },
                "PriceDecimalPlaces": {
                    "type": [
            "null",
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "the number of decimal places in the market's price."
                },
                "DefaultQuoteLength": {
                    "type": [
            "null",
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "default quote length."
                },
                "TradeOnWeb": {
                    "type": "boolean",
                    "description": "Can you trade this market on the web."
                },
                "LimitUp": {
                    "type": "boolean",
                    "description": "New sell orders will be rejected. Orders resulting in a short open position will be red carded."
                },
                "LimitDown": {
                    "type": "boolean",
                    "description": "New buy orders will be rejected. Orders resulting in a long open position will be red carded."
                },
                "LongPositionOnly": {
                    "type": "boolean",
                    "description": "Cannot open a short position. Equivalent to limit up."
                },
                "CloseOnly": {
                    "type": "boolean",
                    "description": "Can only close open positions. Equivalent to both Limit up and Limit down."
                },
                "MarketEod": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiMarketEodDTO"
            }
          ],
                    "description": "list of market end of day dtos."
                }
            },
            "description": "Contains market information."
        },
        "ApiOrderDTO": {
            "id": "ApiOrderDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order identifier"
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "A market's unique identifier"
                },
                "Direction": {
                    "type": "string",
                    "description": "Direction identifier for trade, values supported are buy or sell"
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Size of the order"
                },
                "Price": {
                    "type": [
            "null",
            "number"
          ],
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The price at which the order is to be filled"
                },
                "TradingAccountId": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "TradingAccount associated with the order"
                },
                "CurrencyId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Currency id for order (as represented in the trading system)"
                },
                "StatusId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Status id of order (as represented in the trading system)"
                },
                "TypeId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The type of the order, Trade, stop or limit."
                },
                "IfDone": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiIfDoneDTO"
            }
          ],
                    "description": "List of IfDone Orders which will be filled when the initial order is triggered"
                },
                "OcoOrder": {
                    "type": {
                        "$ref": "ApiStopLimitOrderDTO"
                    },
                    "description": "Corresponding OcoOrder (One Cancels the Other)"
                }
            },
            "description": "Represents an order"
        },
        "ApiActiveStopLimitOrderDTO": {
            "id": "ApiActiveStopLimitOrderDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order's unique identifier."
                },
                "ParentOrderId": {
                    "type": [
            "null",
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order's parent OrderId."
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The markets unique identifier."
                },
                "MarketName": {
                    "type": "string",
                    "description": "The market's name."
                },
                "Direction": {
                    "type": "string",
                    "description": "The direction, buy or sell."
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The quantity of the product."
                },
                "TriggerPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The marked to market price at which the order will trigger at."
                },
                "TradingAccountId": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The trading account that the order is on."
                },
                "Type": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The type of order, i.e. stop or limit."
                },
                "Applicability": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "When the order applies until. i.e. good till cancelled (GTC) good for day (GFD) or good till time (GTT)."
                },
                "ExpiryDateTimeUTC": {
                    "type": [
            "null",
            "string"
          ],
                    "format": "wcf-date",
                    "description": "The associated expiry DateTime."
                },
                "Currency": {
                    "type": "string",
                    "description": "The trade currency."
                },
                "Status": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order status."
                },
                "StopOrder": {
                    "type": {
                        "$ref": "ApiBasicStopLimitOrderDTO"
                    },
                    "description": "The if done stop order."
                },
                "LimitOrder": {
                    "type": {
                        "$ref": "ApiBasicStopLimitOrderDTO"
                    },
                    "description": "The if done limit order"
                },
                "OcoOrder": {
                    "type": {
                        "$ref": "ApiBasicStopLimitOrderDTO"
                    },
                    "description": "The order on the other side of a one Cancels the other relationship."
                },
                "LastChangedDateTimeUTC": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The last time that the order changed. Note - Does not include things such as the current market price."
                }
            },
            "description": "A stop or limit order that is currently active."
        },
        "ApiMarketDTO": {
            "id": "ApiMarketDTO",
            "type": "object",
            "properties": {
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "A market's unique identifier"
                },
                "Name": {
                    "type": "string",
                    "minLength": 1,
                    "maxLength": 120,
                    "description": "The market name"
                }
            },
            "description": "basic information about a Market"
        },
        "QuoteDTO": {
            "id": "QuoteDTO",
            "type": "object",
            "properties": {
                "QuoteId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The uniqueId of the Quote"
                },
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The Order the Quote is related to"
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The Market the Quote is related to"
                },
                "BidPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The Price of the original Order request for a Buy"
                },
                "BidAdjust": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The amount the bid price will be adjusted to become an order when the customer is buying (BidPrice + BidAdjust = BuyPrice)"
                },
                "OfferPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The Price of the original Order request for a Sell"
                },
                "OfferAdjust": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The amount the offer price will be adjusted to become an order when the customer is selling (OfferPrice + OfferAdjust = OfferPrice)"
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The Quantity is the number of units for the trade i.e CFD Quantity = Number of CFD's to Buy or Sell , FX Quantity = amount in base currency."
                },
                "CurrencyId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The system internal Id for the ISO Currency the equivalent ISO Code can be found using the API Call TODO Fill when the API call is available"
                },
                "StatusId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The Status id of the Quote. The list of different Status values can be found using the API call TODO Fill when call avaliable"
                },
                "TypeId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The quote type id."
                },
                "RequestDateTime": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The timestamp the quote was requested. Always expressed in UTC"
                }
            },
            "description": "A quote for a specific order request"
        },
        "ApiStopLimitOrderDTO": {
            "id": "ApiStopLimitOrderDTO",
            "type": "object",
            "extends": "#/ApiOrderDTO",
            "properties": {
                "ExpiryDateTimeUTC": {
                    "type": [
            "null",
            "string"
          ],
                    "format": "wcf-date",
                    "description": "The associated expiry DateTime for a pair of GoodTillDate IfDone orders"
                },
                "Applicability": {
                    "type": "string",
                    "description": "Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD)"
                }
            },
            "description": "Represents a stop/limit order"
        },
        "OrderDTO": {
            "id": "OrderDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Order id"
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Market id."
                },
                "ClientAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Client account id."
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Trading account id."
                },
                "CurrencyId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Trade currency id."
                },
                "CurrencyISO": {
                    "type": "string",
                    "description": "Trade currency ISO code."
                },
                "Direction": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "direction of the order."
                },
                "AutoRollover": {
                    "type": "boolean",
                    "description": "Does the order automatically roll over."
                },
                "ExecutionPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "the price the order was executed at."
                },
                "LastChangedTime": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The date time that the order was last changed. Always expressed in UTC."
                },
                "OpenPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "the open price of the order."
                },
                "OriginalLastChangedDateTime": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The date of the Order. Always expressed in UTC"
                },
                "OriginalQuantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The orders original quantity, before any part / full closures."
                },
                "PositionMethodId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The position method of the order."
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The current quantity of the order."
                },
                "Type": {
                    "type": "string",
                    "description": "the type of the order (1 = Trade / 2 = Stop / 3 = Limit)"
                },
                "Status": {
                    "type": "string",
                    "description": "The order status id."
                },
                "ReasonId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "the order status reason is."
                }
            },
            "description": "An order for a specific Trading Account"
        },
        "PriceTickDTO": {
            "id": "PriceTickDTO",
            "type": "object",
            "properties": {
                "TickDate": {
                    "type": "string",
                    "format": "wcf-date",
                    "description": "The datetime at which a price tick occurred. Accurate to the millisecond"
                },
                "Price": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The mid price"
                }
            },
            "description": "The mid price at a particular point in time."
        },
        "ApiClientAccountWatchlistDTO": {
            "id": "ApiClientAccountWatchlistDTO",
            "type": "object",
            "properties": {
                "WatchlistId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist item id"
                },
                "WatchlistDescription": {
                    "type": "string",
                    "description": "Watchlist description"
                },
                "DisplayOrder": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist display order"
                },
                "Items": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiClientAccountWatchlistItemDTO"
            }
          ],
                    "description": "Watchlist items"
                }
            },
            "description": "Client account watchlist"
        },
        "ClientAccountMarginDTO": {
            "id": "ClientAccountMarginDTO",
            "type": "object",
            "properties": {
                "Cash": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "cash balance expressed in the clients base currency."
                },
                "Margin": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "The client account's total margin requirement expressed in base currency."
                },
                "MarginIndicator": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Margin indicator expressed as a percentage."
                },
                "NetEquity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Net equity expressed in the clients base currency."
                },
                "OpenTradeEquity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "open trade equity (open / unrealised PNL) expressed in the client's base currency."
                },
                "TradeableFunds": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "tradable funds expressed in the client's base currency."
                },
                "PendingFunds": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "N/A"
                },
                "TradingResource": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "trading resource expressed in the client's base currency."
                },
                "TotalMarginRequirement": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "total margin requirement expressed in the client's base currency."
                },
                "CurrencyId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The clients base currency id."
                },
                "CurrencyISO": {
                    "type": "string",
                    "description": "The clients base currency iso code."
                }
            },
            "description": "The current margin for a specific client account"
        },
        "NewsDetailDTO": {
            "id": "NewsDetailDTO",
            "type": "object",
            "extends": "#/NewsDTO",
            "properties": {
                "Story": {
                    "type": "string",
                    "minLength": 0,
                    "maxLength": 2147483647,
                    "description": "The detail of the story. This can contain HTML characters."
                }
            },
            "description": "Contains details of a specific news story"
        },
        "ApiTradeOrderDTO": {
            "id": "ApiTradeOrderDTO",
            "type": "object",
            "extends": "#/ApiOrderDTO",
            "properties": {},
            "description": "Represents a trade order"
        },
        "ApiOrderResponseDTO": {
            "id": "ApiOrderResponseDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "order id."
                },
                "StatusReason": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "order status reason id."
                },
                "Status": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "order status id."
                },
                "Price": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "price."
                },
                "CommissionCharge": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "commission charge."
                },
                "IfDone": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiIfDoneResponseDTO"
            }
          ],
                    "description": "list of if done orders."
                },
                "GuaranteedPremium": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "premium for guaranteed orders."
                },
                "OCO": {
                    "type": {
                        "$ref": "ApiOrderResponseDTO"
                    },
                    "description": "an order in an OCO relationship with this order."
                }
            },
            "description": "order response"
        },
        "ApiStopLimitResponseDTO": {
            "id": "ApiStopLimitResponseDTO",
            "type": "object",
            "extends": "#/ApiOrderResponseDTO",
            "properties": {},
            "description": "The response from the stop limit order request"
        },
        "ListNewsHeadlinesResponseDTO": {
            "id": "ListNewsHeadlinesResponseDTO",
            "type": "object",
            "properties": {
                "Headlines": {
                    "type": "array",
                    "items": [
            {
                "$ref": "NewsDTO"
            }
          ],
                    "description": "A list of News headlines"
                }
            },
            "description": "The response from a GET request for News headlines"
        },
        "ApiLogOnRequestDTO": {
            "id": "ApiLogOnRequestDTO",
            "type": "object",
            "properties": {
                "UserName": {
                    "type": "string",
                    "minLength": 6,
                    "maxLength": 20,
                    "description": "Username is case sensitive"
                },
                "Password": {
                    "type": "string",
                    "minLength": 6,
                    "maxLength": 20,
                    "description": "Password is case sensitive"
                }
            },
            "description": "request to create a session (log on)."
        },
        "GetOrderResponseDTO": {
            "id": "GetOrderResponseDTO",
            "type": "object",
            "properties": {
                "TradeOrder": {
                    "type": {
                        "$ref": "ApiTradeOrderDTO"
                    },
                    "description": "The details of the order if it's a trade / open position."
                },
                "StopLimitOrder": {
                    "type": {
                        "$ref": "ApiStopLimitOrderDTO"
                    },
                    "description": "The details of the order if it's a stop limit order."
                }
            },
            "description": "Response containing the order. Only one of the two fields will be populated; this depends upon the type of order (Trade or Stop / Limit)."
        },
        "GetActiveStopLimitOrderResponseDTO": {
            "id": "GetActiveStopLimitOrderResponseDTO",
            "type": "object",
            "properties": {
                "ActiveStopLimitOrder": {
                    "type": {
                        "$ref": "ApiActiveStopLimitOrderDTO"
                    },
                    "description": "The active stop limit order. If it is null then the active stop limit order does not exist."
                }
            },
            "description": "Response containing the active stop limit order."
        },
        "UpdateWatchlistDisplayOrderRequestDTO": {
            "id": "UpdateWatchlistDisplayOrderRequestDTO",
            "type": "object",
            "properties": {
                "NewDisplayOrderIdSequence": {
                    "type": "array",
                    "items": [
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Represents the new client watchlist displayOrderId list sequence"
                }
            },
            "description": "Get market information for a list of markets."
        },
        "CancelOrderRequestDTO": {
            "id": "CancelOrderRequestDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The order identifier."
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "TradingAccount associated with the cancel order request."
                }
            },
            "description": "A cancel order request."
        },
        "InsertWatchlistItemRequestDTO": {
            "id": "InsertWatchlistItemRequestDTO",
            "type": "object",
            "properties": {
                "WatchlistDisplayOrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The watchlist display order id to add the item"
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The market item to add"
                }
            },
            "description": "Update watchlist with item"
        },
        "GetPriceTickResponseDTO": {
            "id": "GetPriceTickResponseDTO",
            "type": "object",
            "properties": {
                "PriceTicks": {
                    "type": "array",
                    "items": [
            {
                "$ref": "PriceTickDTO"
            }
          ],
                    "description": "An array of price ticks, sorted in ascending order by PriceTick.TickDate"
                }
            },
            "description": "The response from a request for Price Ticks"
        },
        "ApiLogOffRequestDTO": {
            "id": "ApiLogOffRequestDTO",
            "type": "object",
            "properties": {
                "UserName": {
                    "type": "string",
                    "description": "user name of the session to delete (log off)."
                },
                "Session": {
                    "type": "string",
                    "description": "session identifier of the session to delete (log off)."
                }
            },
            "description": "request to delete a session (log off)"
        },
        "ListMarketInformationRequestDTO": {
            "id": "ListMarketInformationRequestDTO",
            "type": "object",
            "properties": {
                "MarketIds": {
                    "type": "array",
                    "items": [
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The list of market ids"
                }
            },
            "description": "Get market information for a list of markets."
        },
        "ApiTradeOrderResponseDTO": {
            "id": "ApiTradeOrderResponseDTO",
            "type": "object",
            "properties": {
                "Status": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The status of the order (Pending, Accepted, Open, etc)"
                },
                "StatusReason": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The id corresponding to a more descriptive reason for the order status"
                },
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The unique identifier associated to the order returned from the underlying trading system"
                },
                "Orders": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiOrderResponseDTO"
            }
          ],
                    "description": "List of orders with their associated response"
                },
                "Quote": {
                    "type": {
                        "$ref": "ApiQuoteResponseDTO"
                    },
                    "description": "Quote response"
                }
            },
            "description": "The response from the trade request"
        },
        "ApiQuoteResponseDTO": {
            "id": "ApiQuoteResponseDTO",
            "type": "object",
            "properties": {
                "QuoteId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "quote id."
                },
                "Status": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "quote status."
                },
                "StatusReason": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "quote status reason."
                }
            },
            "description": "quote response."
        },
        "ApiLogOnResponseDTO": {
            "id": "ApiLogOnResponseDTO",
            "type": "object",
            "properties": {
                "Session": {
                    "type": "string",
                    "minLength": 36,
                    "maxLength": 100,
                    "description": "Your session token (treat as a random string) <BR /> Session tokens are valid for a set period from the time of their creation. <BR /> The period is subject to change, and may vary depending on who you logon as."
                }
            },
            "description": "Response to a CreateSessionRequest"
        },
        "ListMarketInformationResponseDTO": {
            "id": "ListMarketInformationResponseDTO",
            "type": "object",
            "properties": {
                "MarketInformation": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiMarketInformationDTO"
            }
          ],
                    "description": "The requested list of market information."
                }
            },
            "description": "Response from am market information request."
        },
        "AccountInformationResponseDTO": {
            "id": "AccountInformationResponseDTO",
            "type": "object",
            "properties": {
                "LogonUserName": {
                    "type": "string",
                    "description": "logon user name."
                },
                "ClientAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "client account id."
                },
                "ClientAccountCurrency": {
                    "type": "string",
                    "description": "Base currency of the client account."
                },
                "TradingAccounts": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiTradingAccountDTO"
            }
          ],
                    "description": "a list of trading accounts."
                }
            },
            "description": "response from an account information query."
        },
        "NewStopLimitOrderRequestDTO": {
            "id": "NewStopLimitOrderRequestDTO",
            "type": "object",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Order identifier of the order to update"
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "A market's unique identifier"
                },
                "Currency": {
                    "type": "string",
                    "description": "Currency to place order in"
                },
                "AutoRollover": {
                    "type": "boolean",
                    "description": "Flag to indicate whether the trade will automatically roll into the next market when the current market expires"
                },
                "Direction": {
                    "type": "string",
                    "description": "Direction identifier for order/trade, values supported are buy or sell"
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Size of the order/trade"
                },
                "BidPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower of the two"
                },
                "OfferPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Market prices are quote as a pair (buy/sell or bid/offer), the OfferPrice is the higher of the market price pair"
                },
                "AuditId": {
                    "type": "string",
                    "description": "Unique identifier for a price tick"
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "TradingAccount associated with the trade/order request"
                },
                "IfDone": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiIfDoneDTO"
            }
          ],
                    "description": "List of IfDone Orders which will be filled when the initial trade/order is triggered"
                },
                "OcoOrder": {
                    "type": {
                        "$ref": "NewStopLimitOrderRequestDTO"
                    },
                    "description": "Corresponding OcoOrder (One Cancels the Other)"
                },
                "Applicability": {
                    "type": "string",
                    "description": "Identifier which relates to the expiry of the order/trade, i.e. GoodTillDate (GTD), GoodTillCancelled (GTC) or GoodForDay (GFD)"
                },
                "ExpiryDateTimeUTC": {
                    "type": [
            "null",
            "string"
          ],
                    "format": "wcf-date",
                    "description": "The associated expiry DateTime for a pair of GoodTillDate IfDone orders"
                },
                "Guaranteed": {
                    "type": "boolean",
                    "description": "Flag to determine whether an order is guaranteed to trigger and fill at the associated trigger price"
                },
                "TriggerPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Price at which the order is intended to be triggered"
                }
            },
            "description": "A request for a stop/limit order"
        },
        "UpdateStopLimitOrderRequestDTO": {
            "id": "UpdateStopLimitOrderRequestDTO",
            "type": "object",
            "extends": "#/NewStopLimitOrderRequestDTO",
            "properties": {},
            "description": "A request for updating a stop/limit order"
        },
        "GetPriceBarResponseDTO": {
            "id": "GetPriceBarResponseDTO",
            "type": "object",
            "properties": {
                "PriceBars": {
                    "type": "array",
                    "items": [
            {
                "$ref": "PriceBarDTO"
            }
          ],
                    "description": "An array of finalized price bars, sorted in ascending order based on PriceBar.BarDate"
                },
                "PartialPriceBar": {
                    "type": {
                        "$ref": "PriceBarDTO"
                    },
                    "description": "The (non-finalized) price bar data for the current period (i.e, the period that hasn't yet completed)"
                }
            },
            "description": "The response from a GET price bar history request. Contains both an array of finalized price bars, and a partial (not finalized) bar for the current period"
        },
        "ListStopLimitOrderHistoryResponseDTO": {
            "id": "ListStopLimitOrderHistoryResponseDTO",
            "type": "object",
            "properties": {
                "StopLimitOrderHistory": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiStopLimitOrderHistoryDTO"
            }
          ],
                    "description": "A list of historical stop / limit orders."
                }
            },
            "description": "Contains the result of a ListStopLimitOrderHistory query"
        },
        "GetMarketInformationResponseDTO": {
            "id": "GetMarketInformationResponseDTO",
            "type": "object",
            "properties": {
                "MarketInformation": {
                    "type": {
                        "$ref": "ApiMarketInformationDTO"
                    },
                    "description": "The requested market information."
                }
            },
            "description": "Response from am market information request."
        },
        "GetMessagePopupResponseDTO": {
            "id": "GetMessagePopupResponseDTO",
            "type": "object",
            "properties": {
                "AskForClientApproval": {
                    "type": "boolean",
                    "description": "Should the client application ask for client approval."
                },
                "Message": {
                    "type": "string",
                    "description": "The message to display to the client."
                }
            },
            "description": "Message popup response denoting whether the client application should display a popup notification at startup."
        },
        "ApiErrorResponseDTO": {
            "id": "ApiErrorResponseDTO",
            "type": "object",
            "properties": {
                "ErrorMessage": {
                    "type": "string",
                    "description": "This is a description of the ErrorMessage property"
                },
                "ErrorCode": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The error code"
                }
            },
            "description": "This is a description of ErrorResponseDTO"
        },
        "ListMarketInformationSearchResponseDTO": {
            "id": "ListMarketInformationSearchResponseDTO",
            "type": "object",
            "properties": {
                "MarketInformation": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiMarketInformationDTO"
            }
          ],
                    "description": "The requested list of market information."
                }
            },
            "description": "Response from a market information search request."
        },
        "GetOpenPositionResponseDTO": {
            "id": "GetOpenPositionResponseDTO",
            "type": "object",
            "properties": {
                "OpenPosition": {
                    "type": {
                        "$ref": "ApiOpenPositionDTO"
                    },
                    "description": "The open position. If it is null then the open position does not exist."
                }
            },
            "description": "Response containing the open position."
        },
        "ListSpreadMarketsResponseDTO": {
            "id": "ListSpreadMarketsResponseDTO",
            "type": "object",
            "properties": {
                "Markets": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiMarketDTO"
            }
          ],
                    "description": "A list of Spread Betting markets"
                }
            },
            "description": "Contains the result of a ListSpreadMarkets query"
        },
        "ListOpenPositionsResponseDTO": {
            "id": "ListOpenPositionsResponseDTO",
            "type": "object",
            "properties": {
                "OpenPositions": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiOpenPositionDTO"
            }
          ],
                    "description": "A list of trades / open positions."
                }
            },
            "description": "Contains the result of a ListOpenPositions query"
        },
        "ErrorCode": {
            "id": "ErrorCode",
            "type": "integer",
            "enum": [
        0,
        403,
        500,
        4000,
        4001,
        4002,
        4003,
        4004,
        4010,
        4011,
        5001
      ],
            "options": [
        {
            "value": 0,
            "label": "NoError",
            "description": ""
        },
        {
            "value": 403,
            "label": "Forbidden",
            "description": ""
        },
        {
            "value": 500,
            "label": "InternalServerError",
            "description": ""
        },
        {
            "value": 4000,
            "label": "InvalidParameterType",
            "description": ""
        },
        {
            "value": 4001,
            "label": "ParameterMissing",
            "description": ""
        },
        {
            "value": 4002,
            "label": "InvalidParameterValue",
            "description": ""
        },
        {
            "value": 4003,
            "label": "InvalidJsonRequest",
            "description": ""
        },
        {
            "value": 4004,
            "label": "InvalidJsonRequestCaseFormat",
            "description": ""
        },
        {
            "value": 4010,
            "label": "InvalidCredentials",
            "description": ""
        },
        {
            "value": 4011,
            "label": "InvalidSession",
            "description": ""
        },
        {
            "value": 5001,
            "label": "NoDataAvailable",
            "description": ""
        }
      ],
            "description": "This is a description of the ErrorCode enum",
            "demoValue": "403"
        },
        "ListOrdersResponseDTO": {
            "id": "ListOrdersResponseDTO",
            "type": "object",
            "properties": {},
            "description": "This Dto is not currently used"
        },
        "ListCfdMarketsResponseDTO": {
            "id": "ListCfdMarketsResponseDTO",
            "type": "object",
            "properties": {
                "Markets": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiMarketDTO"
            }
          ],
                    "description": "A list of CFD markets"
                }
            },
            "description": "Contains the result of a ListCfdMarkets query"
        },
        "ApiLogOffResponseDTO": {
            "id": "ApiLogOffResponseDTO",
            "type": "object",
            "properties": {
                "LoggedOut": {
                    "type": "boolean",
                    "description": "LogOut status"
                }
            },
            "description": "Response from a session delete request."
        },
        "ListWatchlistResponseDTO": {
            "id": "ListWatchlistResponseDTO",
            "type": "object",
            "properties": {
                "ClientAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Client account id"
                }
            },
            "description": "Gets the client watchlist"
        },
        "ListTradeHistoryResponseDTO": {
            "id": "ListTradeHistoryResponseDTO",
            "type": "object",
            "properties": {
                "TradeHistory": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiTradeHistoryDTO"
            }
          ],
                    "description": "A list of historical trades."
                }
            },
            "description": "Contains the result of a ListTradeHistory query"
        },
        "ListActiveStopLimitOrderResponseDTO": {
            "id": "ListActiveStopLimitOrderResponseDTO",
            "type": "object",
            "properties": {
                "ActiveStopLimitOrders": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiActiveStopLimitOrderDTO"
            }
          ],
                    "description": "The requested list of active stop / limit orders."
                }
            },
            "description": "Contains the result of a ListActiveStopLimitOrder query"
        },
        "NewTradeOrderRequestDTO": {
            "id": "NewTradeOrderRequestDTO",
            "type": "object",
            "properties": {
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "A market's unique identifier"
                },
                "Currency": {
                    "type": "string",
                    "description": "Currency to place order in"
                },
                "AutoRollover": {
                    "type": "boolean",
                    "description": "Flag to indicate whether the trade will automatically roll into the next market when the current market expires"
                },
                "Direction": {
                    "type": "string",
                    "description": "Direction identifier for order/trade, values supported are buy or sell"
                },
                "Quantity": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Size of the order/trade"
                },
                "QuoteId": {
                    "type": [
            "null",
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Quote Id"
                },
                "BidPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Market prices are quoted as a pair (buy/sell or bid/offer), the BidPrice is the lower of the two"
                },
                "OfferPrice": {
                    "type": "number",
                    "format": "decimal",
                    "minValue": -7.9228162514264338E+28,
                    "maxValue": 7.9228162514264338E+28,
                    "description": "Market prices are quote as a pair (buy/sell or bid/offer), the OfferPrice is the higher of the market price pair"
                },
                "AuditId": {
                    "type": "string",
                    "description": "Unique identifier for a price tick"
                },
                "TradingAccountId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "TradingAccount associated with the trade/order request"
                },
                "IfDone": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiIfDoneDTO"
            }
          ],
                    "description": "List of IfDone Orders which will be filled when the initial trade/order is triggered"
                },
                "Close": {
                    "type": "array",
                    "items": [
            "integer"
          ],
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "List of existing order id's that require part or full closure"
                }
            },
            "description": "A request for a trade order"
        },
        "UpdateTradeOrderRequestDTO": {
            "id": "UpdateTradeOrderRequestDTO",
            "type": "object",
            "extends": "#/NewTradeOrderRequestDTO",
            "properties": {
                "OrderId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Order identifier of the order to update"
                }
            },
            "description": "A request for updating a trade order"
        },
        "GetNewsDetailResponseDTO": {
            "id": "GetNewsDetailResponseDTO",
            "type": "object",
            "properties": {
                "NewsDetail": {
                    "type": {
                        "$ref": "NewsDetailDTO"
                    },
                    "description": "The details of the news item"
                }
            },
            "description": "JSON returned from News Detail GET request"
        },
        "ApiLookupResponseDTO": {
            "id": "ApiLookupResponseDTO",
            "type": "object",
            "properties": {
                "CultureId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The culture id requested"
                },
                "LookupEntityName": {
                    "type": "string",
                    "description": "The lookup name requested"
                },
                "ApiLookupDTOList": {
                    "type": "array",
                    "items": [
            {
                "$ref": "ApiLookupDTO"
            }
          ],
                    "description": "List of lookup entities from the database"
                }
            },
            "description": "Gets the lookup entities from trading database given the lookup name and culture id"
        },
        "ApiIfDoneResponseDTO": {
            "id": "ApiIfDoneResponseDTO",
            "type": "object",
            "properties": {
                "Stop": {
                    "type": {
                        "$ref": "ApiOrderResponseDTO"
                    },
                    "description": "Stop"
                },
                "Limit": {
                    "type": {
                        "$ref": "ApiOrderResponseDTO"
                    },
                    "description": "Limit"
                }
            },
            "description": "if done"
        },
        "SystemStatusDTO": {
            "id": "SystemStatusDTO",
            "type": "object",
            "properties": {
                "StatusMessage": {
                    "type": "string",
                    "description": "a status message"
                }
            },
            "description": "system status"
        },
        "SystemStatusRequestDTO": {
            "id": "SystemStatusRequestDTO",
            "type": "object",
            "properties": {
                "TestDepth": {
                    "type": "string",
                    "description": "depth to test."
                }
            },
            "description": "system status request."
        }
    }
}