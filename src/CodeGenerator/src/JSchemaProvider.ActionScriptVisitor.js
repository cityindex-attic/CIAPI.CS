/// <reference path="JSchemaProvider.js" />
(function () {
    var outputString = "";
    exports.ActionScriptVisitor = function () {
        this._lines = [];
        this.outputDir = "";
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
            if (property.type == "array") {
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
                    if (property && property.hasOwnProperty("format") && property.format == "wcf-date") {
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
                    this.writeLine("import uk.co.cityindex.utils.DateUtils;");
                    this.writeLine("");
                    var typeName = "class";
                    if (!current.value["enum"]) {
                        this.writeLine("    [RemoteClass]");
                    }
                    if (current.value.description) {
                        this.writeLine("    /**");
                        this.writeLine("     * " + current.value.description);
                        this.writeLine("     * ");
                        this.writeLine("     * DO NOT EDIT -- code is generated from API metadata. Changes will be overwritten.");
                        this.writeLine("     * Generated on "+new Date().toGMTString());
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
                    var arrayCount = 97; // ascii char code for 'a'
                    if (current.value["enum"]) {
                        // no constructor for an enum, but we do need constants to represent the enum values
                        if(current.value.hasOwnProperty("options")){
                            for (var i = 0; i < current.value.options.length; i++){
                                var option = current.value.options[i];
                                if (option.description) {
                                    this.writeLine("        /**");
                                    this.writeLine("         * " + option.description);
                                    this.writeLine("         */");
                                }
                                this.writeLine("        public static const " + option.label.toUpperCase() + ":Number = "+option.value+";");
                            }
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
                        if(current.value["extends"]){
                            this.writeLine("            super(data);");
                        }
                        this.writeLine("            if(data)");
                        this.writeLine("            {");

                        var dto = current.value;
                        if(dto.hasOwnProperty("properties")){
                            var props = dto["properties"];
                            for(var key in props){
                                if(props.hasOwnProperty(key)){
                                    var prop = props[key];
                                    if(prop && prop.type && prop.type=='array'){
                                        var iterVar = String.fromCharCode(arrayCount);
                                        this.writeLine("                if(data."+key+")");
                                        this.writeLine("                {");
                                        this.writeLine("                    for(var "+iterVar+":int = 0; "+iterVar+" < data."+key+".length; "+iterVar+"++)");
                                        this.writeLine("                    {");
                                        if(prop.items && prop.items.length>0 && prop.items[0].hasOwnProperty("$ref")){
                                            this.writeLine("                        "+nameLowerCaseLead(key)+".push(new "+this.normalizeKey(prop.items[0].$ref)+"(data."+key+"["+iterVar+"]));");
                                        } else {
                                            this.writeLine("                        "+nameLowerCaseLead(key)+".push(new "+this.applyFormat(prop.items[0])+"(data."+key+"["+iterVar+"]));");
                                        }
                                        this.writeLine("                    }");
                                        this.writeLine("                }");
                                        arrayCount++;
                                        if(arrayCount>122){
                                            // we've used a...z, smells like error, or we have an API dev gone rogue
                                            throw new Error("More than 26 child arrays!  "+current.key);
                                        }
                                    } else {
                                        if(prop.format && prop.format=='wcf-date'){
                                            this.writeLine("                "+nameLowerCaseLead(key)+" = DateUtils.dateFromJSONDateString(data."+key+");");
                                        } else if(prop.type && prop.type.hasOwnProperty("$ref")) {
                                            this.writeLine("                "+nameLowerCaseLead(key)+" = new "+this.resolveType(prop)+"(data."+key+");");
                                        } else {
                                            this.writeLine("                "+nameLowerCaseLead(key)+" = data."+key+";");
                                        }
                                    }
                                }
                            }
                        }

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
                    fs.open(this.outputDir + current.key + '.as', 'w', 666, function (e, id) {
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