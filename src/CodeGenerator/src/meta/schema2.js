exports.schema =
{
    "namespace": "CIAPI.DTO",
    "version": "0.1036.0.0",
    "properties": {
        "ApiClientAccountWatchlistDTO": {
            "id": "ApiClientAccountWatchlistDTO",
            "type": "object",
            "properties": {
                "WatchlistId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "The ID of the Watchlist."
                },
                "WatchlistDescription": {
                    "type": "string",
                    "description": "Watchlist description."
                },
                "DisplayOrder": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist display order."
                },
                "Items": {
                    "type": "array",
                    "items": [
                        {
                            "$ref": "#.ApiClientAccountWatchlistItemDTO"
                        }
                    ],
                    "description": "Watchlist items."
                }
            },
            "description": "Client account watchlist."
        },
        "ApiClientAccountWatchlistItemDTO": {
            "id": "ApiClientAccountWatchlistItemDTO",
            "type": "object",
            "properties": {
                "WatchlistId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "ID of the parent watchlist."
                },
                "MarketId": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist item market ID."
                },
                "DisplayOrder": {
                    "type": "integer",
                    "minValue": -2147483648,
                    "maxValue": 2147483647,
                    "description": "Watchlist item display order."
                }
            },
            "description": "API watchlist item."
        }
    }
};