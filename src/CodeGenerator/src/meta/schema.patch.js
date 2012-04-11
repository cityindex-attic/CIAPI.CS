
exports.schemaPatch = {
    "properties": {
//        "ApiTradingDayTimesDTO": {
//            "properties": {
//                "$DELETE$": ["StartTimeUtc", "EndTimeUtc"
//                ]

//            }
//        },
//        "ApiMarketSpreadDTO": {
//            "properties": {
//                "$DELETE$": ["SpreadTimeUtc"]

//            }
//        },
//        "ApiMarketInformationDTO": {
//            "properties": {
//                "$DELETE$": ["TradingStartTimeUtc", "TradingEndTimeUtc", ]

//            }
//        },
        "ApiOrderResponseDTO": {
            "properties": {
                "StatusReason_Resolved": {
                    "type": "string",
                    "description": "Plain text StatusReason"
                },
                "Status_Resolved": {
                    "type": "string",
                    "description": "Plain text StatusReason"
                }
            }
        },
        "ApiTradeOrderResponseDTO": {
            "properties": {
                "StatusReason_Resolved": {
                    "type": "string",
                    "description": "Plain text StatusReason"
                },
                "Status_Resolved": {
                    "type": "string",
                    "description": "Plain text StatusReason"
                }
            }
        },
        "ApiActiveStopLimitOrderDTO": {
            "properties": {
                "Applicability_Resolved": {
                    "type": "string",
                    "description": "Plain text StatusReason"
                }
            }
        },

        "ApiOpenPositionDTO": {
            "properties": {
                "Status_Resolved": {
                    "type": "string",
                    "description": "Plain text StatusReason"
                }
            }
        }
    }
};