var schema = {

    "UserType": {
        "id": "UserType",
        "type": "string",
        "description": "",
        "enum": ["anonymous", "unregistered", "registered", "moderator"],
        "options": [{
            "value": "anonymous",
            "label": "Anonymous",
            "description": ""
        }, {
            "value": "unregistered",
            "label": "Unregistered",
            "description": ""
        }, {
            "value": "registered",
            "label": "Registered",
            "description": ""
        }, {
            "value": "moderator",
            "label": "Moderator",
            "description": ""
        }]
    },

    "User": {
        "id": "User",
        "type": "object",
        "description": "",
        "properties": {
            "user_id": {
                "description": "id of the user",
                "type": "integer",
                "required": true
            },
            "user_type": {
                "description": "type of the user",
                "required": true,
                "ref": "$.UserType"
            },
            "display_name": {
                "description": "displayable name of the user",
                "type": "string",
                "required": true,
                "maxLength": 40
            },
            "reputation": {
                "description": "reputation of the user",
                "type": "integer",
                "required": true
            },
            "on_site": {
                "description": "",
                "ref": "$.Site"
            },
            "email_hash": {
                "description": "email hash, suitable for fetching a gravatar",
                "type": "string",
                "required": true,
                "maxLength": 32
            }
        }
    },

    "UsersByIdAssociatedResponse": {
        "id": "UsersByIdAssociatedResponse",
        "type": "object",
        "description": "",
        "properties": {
            "PriceBars": {
                "type": "array",
                "items": {
                    "$ref": "#.User"
                },
                "description": ""
            }
        }
    },

    "Styling": {
        "id": "Styling",
        "type": "object",
        "description": "",
        "properties": {
            "link_color": {
                "description": "color of links, as a CSS style color value",
                "type": "string",
                "format": "color"
            },
            "tag_foreground_color": {
                "description": "foreground color of tags, as a CSS style color value",
                "type": "string",
                "format": "color"
            },
            "tag_background_color": {
                "description": "background/fill color of tags, as a CSS style color value",
                "type": "string",
                "format": "color"
            }
        }
    },

    "SiteState": {
        "id": "SiteState",
        "type": "string",
        "description": "The state of a site.",
        "enum": ["normal", "closed_beta", "open_beta", "linked_meta"],
        "options": [{
            "value": "normal",
            "label": "Normal",
            "description": ""
        }, {
            "value": "closed_beta",
            "label": "Closed Beta",
            "description": ""
        }, {
            "value": "open_beta",
            "label": "Open Beta",
            "description": ""
        }, {
            "value": "linked_meta",
            "label": "Linked Meta",
            "description": ""
        }]
    },

    "Site": {
        "id": "Site",
        "type": "object",
        "description": "",
        "properties": {
            "name": {
                "description": "name of the site",
                "type": "string",
                "required": true,
                "maxLength": 100
            },
            "logo_url": {
                "description": "absolute path to the logo for the site",
                "type": "string",
                "format": "uri",
                "maxLength": 512
            },
            "api_endpoint": {
                "description": "absolute path to the api endpoint for the site, sans the version string",
                "type": "string",
                "format": "uri",
                "maxLength": 100
            },
            "site_url": {
                "description": "absolute path to the front page of the site",
                "type": "string",
                "format": "uri",
                "maxLength": 100
            },
            "description": {
                "description": "description of the site, suitable for display to a user",
                "type": "string",
                "maxLength": 512
            },
            "icon_url": {
                "description": "absolute path to an icon suitable for representing the site, it is a consumers responsibility to scale",
                "type": "string",
                "format": "uri",
                "maxLength": 100
            },
            "aliases": {
                "type": "array",
                "items": [{
                    "type": "string",
                    "format": "uri"
                }]
            },
            "state": {
                "description": "The state of this site.",
                "required": true,
                "$ref": "#.SiteState"
            },
            "styling": {
                "description": "",
                "ref": "$.Styling"
            }
        }
    },

    "SitesResponse": {
        "id": "SitesResponse",
        "type": "object",
        "description": "",
        "properties": {
            "PriceBars": {
                "type": "array",
                "items": {
                    "$ref": "#.Site"
                },
                "description": ""
            }
        }
    }
}





