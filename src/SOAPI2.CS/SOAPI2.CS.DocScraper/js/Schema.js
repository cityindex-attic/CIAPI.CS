var schema = {
    properties: {

        "order": {
            "id": "order",
            "type": "string",
            "enum": ["desc", "asc"],
            "option": [{
                "value": 1,
                "label": "desc",
                "description": ""
            }, {
                "value": 2,
                "label": "asc",
                "description": ""
            }]
        },
        "period": {
            "id": "period",
            "type": "string",
            "enum": ["all_time", "month"],
            "option": [{
                "value": 1,
                "label": "all_time",
                "description": ""
            }, {
                "value": 2,
                "label": "month",
                "description": ""
            }]
        }
    }

};