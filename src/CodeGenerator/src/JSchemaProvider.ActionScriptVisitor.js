/// <reference path="JSchemaProvider.js" />
(function () {
    var outputString = "";
    exports.ActionScriptVisitor = function () {
        this._lines = [];
    };
    exports.ActionScriptVisitor.prototype = {
        provider:{},
        normalizeKey:function (key) {
            if (key && (key.indexOf("#.") > -1 || key.indexOf("#/") > -1)) {
                key = key.substring(2);
            }
            return key;
        },
        resolveType:function (property) {
            var propertyType;
            var nullable = false;
            var propIsArray = false;
            if (property.type == "array") {
                propIsArray = true;
                if (!isDefined(property.items)) {
                    throw new Error("items not specified for array type");
                }
                // only support homogenous arrays, so .items should have 1 element
                if (property.items.length != 1) {
                    throw new Error("only homogenous arrays supported");
                }
                var itemType = this.normalizeKey(property.items[0].$ref);
                if (!itemType) {
                    switch (property.items[0]) {
                        case 'string':
                            itemType = 'String';
                            break;
                        case 'integer':
                            itemType = 'Number';
                            break;
                        default:
                            break;
                    }
                }
                propertyType = "Vector.<" + itemType + "> = new Vector.<" + itemType + ">()";
            }
            else if (isArray(property.type)) {
                // a nullable type will look like this
                // "type": ["null","integer"]
                var errMessage;
                // check for null
                if (property.type.length == 1 || property.type.length == 2) {
                    each(property.type, function (value) {
                        if (value == "null") {
                            nullable = true;
                        } else {
                            propertyType = value;
                        }
                    });
                }
                else {
                    errMessage = "only nullable union types implemented ('null' plus one simple type)";
                }
                if (errMessage) {
                    throw new Error(errMessage);
                }
            } else {
                propertyType = property.type;
            }
            if (propertyType.$ref) {
                propertyType = this.normalizeKey(propertyType.$ref);
            } else {
                propertyType = this.applyFormat(propertyType, property);
            }
            return propertyType;
        },
        applyFormat:function (propertyType, property) {
            switch (propertyType) {
                case "string":
                    // new formats "wcf-date"
                    if (property.format == "wcf-date") {
                        propertyType = "Date";
                    }
                    else {
                        propertyType = "String";
                    }
                    break;
                case "number":
                    propertyType = "Number";
                    break;
                case "integer":
                    propertyType = "Number";
                    break;
                case "boolean":
                    propertyType = "Boolean";
                    break;
                case "object":
                    propertyType = "Object";
                    break;
                case "null":
                case "any":
                default:
                    break;
            }
            return propertyType;
        },
        writeLine:function (value) {
            this._lines.push(value);
            outputString += value + "\n";
        },
        toString:function () {
            return this._lines.join("\n");
        },
        visit_property:function () {
            var current = this.provider.peek();
            switch (this.provider.stack.length) {
                case 3: // type definition
                    outputString = "";
                    this.writeLine("package uk.co.cityindex.dto");
                    this.writeLine("{");
                    this.writeLine("");
                    var typeName = "class";
                    if (current.value.description) {
                        this.writeLine("    /**");
                        this.writeLine("     * " + current.value.description);
                        this.writeLine("     * ");
                        this.writeLine("     * DO NOT EDIT -- code is generated from API metadata. Changes will be overwritten.");
                        this.writeLine("     * ");
                        this.writeLine("     */");
                    }
                    this.writeLine("    public " + typeName + " " + current.key + (current.value["extends"] ? (" extends " + this.normalizeKey(current.value["extends"])) : ""));
                    this.writeLine("    {");
                    break;

                case 5: // type property
                    var propertyType = this.resolveType(current.value);
                    if (current.value.description) {
                        this.writeLine("        /**");
                        this.writeLine("         * " + current.value.description);
                        this.writeLine("         */");
                    }
                    this.writeLine("        public var " + nameLowerCaseLead(current.key) + ":" + propertyType + ";");
                    break;
            }
        },
        visit_property_end:function () {
            switch (this.provider.stack.length) {
                case 3:
                    var current = this.provider.peek();
                    if (current.value["enum"]) {
                        // no constructor for an enum, but we do need constants to represent the enum values
                        for (i = 0; i < current.value.options.length; i++){
                            var option = current.value.options[i];
                            if (option.description) {
                                this.writeLine("        /**");
                                this.writeLine("         * " + option.description);
                                this.writeLine("         */");
                            }
                            this.writeLine("        public static const " + option.label.toUpperCase() + ":Number = "+option.value+";");
                        }
                    } else {
                        // parameterised constructor for pass through initialisation post JSON deserialisation
                        this.writeLine("");
                        this.writeLine("        /**");
                        this.writeLine("         * Parameterised constructor allows for instantiation via Trading API response.");
                        this.writeLine("         * @param data deserialised JSON from TradingAPI response");
                        this.writeLine("         */");
                        this.writeLine("        public function " + current.key + "(data:Object = null)");
                        this.writeLine("        {");
                        this.writeLine("            if(data)");
                        this.writeLine("            {");
                        this.writeLine("                //TODO");
                        this.writeLine("            }");
                        this.writeLine("        }");
                        this.writeLine("");
                    }
                    this.writeLine("    }");
                    this.writeLine("");
                    this.writeLine("}");
                    this.writeLine("");
                    var myOutput = outputString;
                    var fs = require('fs');
                    //TODO need a way to pass the output directory
                    fs.open('C:\Users\\adrian.parker\\Desktop\\DTO\\uk\\co\\cityindex\\dto\\' + current.key + '.as', 'w', 666, function (e, id) {
                        fs.write(id, myOutput, 0, myOutput.length, 0, function (id) {
                            fs.close(id);
                        });
                    });
                    break;
            }
        }

    };
    // private lib functions
    function nameLowerCaseLead(str) {
        var ret = str;
        if (str && str.length > 0) {
            ret = str.substr(0, 1).toLowerCase();
            if (str.length > 1) {
                ret += str.substr(1, str.length);
            }
        }
        return ret;
    }

    function isArray(obj) {
        return ((typeof (obj) == "object") && isDefined(obj.length));
    }

    function isDefined(obj) {
        return obj && (typeof (obj) != 'undefined');
    }

    function each(obj, action) {
        var self = this;
        if (obj.prototype && obj.prototype == Array) {
            for (var i = 0; i < obj.length; i++) {
                action.call(self, obj[i], i, obj);
            }
        }
        else if (typeof (obj) == "object") {
            for (var name in obj) {
                if (obj.hasOwnProperty(name)) {
                    action.call(self, obj[name], name, obj);
                }
            }
        }
        else {
            throw new Error("cannot iterate supplied obj");
        }
    }
})();