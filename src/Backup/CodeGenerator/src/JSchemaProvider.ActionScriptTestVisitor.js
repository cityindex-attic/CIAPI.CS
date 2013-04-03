/// <reference path="JSchemaProvider.js" />
(function () {
    var outputString = "";
    exports.ActionScriptTestVisitor = function () {
        this._lines = [];
        this.outputDir = "";
    };
    exports.ActionScriptTestVisitor.prototype = {
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
            if (!current.value["enum"]){
                switch (this.provider.stack.length) {
                    case 3: // create Tests for types that are not enums.
                        outputString = "";
                        this.writeLine("package uk.co.cityindex.dto");
                        this.writeLine("{");
                        this.writeLine("import org.flexunit.asserts.assertEquals;");
                        this.writeLine("import org.flexunit.asserts.assertNotNull;");
                        this.writeLine("import org.flexunit.asserts.assertNull;");
                        this.writeLine("import org.flexunit.asserts.assertTrue;");
                        this.writeLine("");
                        if (current.value.description) {
                            this.writeLine("    /**");
                            this.writeLine("     * Test for " + current.key+".");
                            this.writeLine("     * ");
                            this.writeLine("     * DO NOT EDIT -- test is generated from API metadata. Changes will be overwritten.");
                            this.writeLine("     * Generated on "+new Date().toGMTString());
                            this.writeLine("     * ");
                            this.writeLine("     */");
                        }
                        this.writeLine("    public class Test" + current.key);
                        this.writeLine("    {");
                        this.writeLine("        ");
                        this.writeLine("        private var subject:"+current.key+";");
                        this.writeLine("        ");
                        this.writeLine("        [Before]");
                        this.writeLine("        public function setUp():void");
                        this.writeLine("        {");
                        this.writeLine("            assertNull(\"Subject not null before test\", subject);");
                        this.writeLine("            subject = new "+current.key+"();");
                        this.writeLine("            assertNotNull(\"Subject null after instantiation\", subject);");
                        this.writeLine("        }");
                        this.writeLine("        ");
                        this.writeLine("        [After]");
                        this.writeLine("        public function tearDown():void");
                        this.writeLine("        {");
                        this.writeLine("            assertNotNull(\"Subject null after test\", subject);");
                        this.writeLine("            subject = null;");
                        this.writeLine("            assertNull(\"Subject not null after nullification\", subject);");
                        this.writeLine("        }");
                        this.writeLine("        ");
                        break;

                    case 5: // create basic getter setter tests for each type property.
                        var propertyType = this.resolveType(current.value);
                        this.writeLine("        [Test]");
                        if (current.value.description) {
                            this.writeLine("        /**");
                            this.writeLine("         * Test we can get and set " + current.value.description);
                            this.writeLine("         */");
                        }
                        this.writeLine("        public function canGetSet" + current.key + "():void");
                        this.writeLine("        {");
                        switch (propertyType){
                            case "Number":
                                this.writeLine("            assertTrue(\"Subject does not have NaN "+nameLowerCaseLead(current.key)+"\", isNaN(subject."+nameLowerCaseLead(current.key)+"));");
                                this.writeLine("            var test"+current.key+":Number = 1;");
                                this.writeLine("            subject."+nameLowerCaseLead(current.key)+" = test"+current.key+";");
                                this.writeLine("            assertEquals(\"Subject does not have expected "+nameLowerCaseLead(current.key)+"\", 1, subject."+nameLowerCaseLead(current.key)+");");
                                break;
                            case "String":
                                this.writeLine("            assertNull(\"Subject does not have null "+nameLowerCaseLead(current.key)+"\", subject."+nameLowerCaseLead(current.key)+");");
                                this.writeLine("            var test"+current.key+":String = \"test\";");
                                this.writeLine("            subject."+nameLowerCaseLead(current.key)+" = test"+current.key+";");
                                this.writeLine("            assertEquals(\"Subject does not have expected "+nameLowerCaseLead(current.key)+"\", \"test\", subject."+nameLowerCaseLead(current.key)+");");
                                break;
                            default:
                                this.writeLine("            //TODO generation not yet implemented for "+propertyType);
                                break;
                        }
                        this.writeLine("        }");
                        this.writeLine("        ");
                        break;
                }
            }
        },
        visit_property_end:function () {
            switch (this.provider.stack.length) {
                case 3:
                    var current = this.provider.peek();
                    if (!current.value["enum"]) {
                        // parameterised constructor for pass through initialisation post JSON deserialisation
                        this.writeLine("");
                        this.writeLine("        [Test]");
                        this.writeLine("        /**");
                        this.writeLine("         * Test parameterised constructor instantiation.");
                        this.writeLine("         */");
                        this.writeLine("        public function test" + current.key + "():void");
                        this.writeLine("        {");
                        this.writeLine("            var data:Object = new Object();");
                        var dto = current.value;
                        if(dto.hasOwnProperty("properties")){
                            var props = dto["properties"];
                            for(var key in props){
                                if(props.hasOwnProperty(key)){
                                    var prop = props[key];
                                    if(prop && prop.type){
                                        var resolvedType = this.resolveType(prop);
                                        if(resolvedType.toString().indexOf('Vector')>-1){
                                            //TODO need to create a Vector of childtype...
                                            continue;
                                        }
                                        switch (resolvedType){
                                            case 'Number':
                                                this.writeLine("                data."+key+" = 1;");
                                                break;
                                            case 'String':
                                                this.writeLine("                data."+key+" = \"test\";");
                                                break;
                                            case 'Boolean':
                                                this.writeLine("                data."+key+" = true;");
                                                break;
                                            case 'Date':
                                                this.writeLine("                data."+key+" = new Date();");
                                                break;
                                            default:
                                                console.log('constructor instantiation test not handling resolved property type: '+resolvedType);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        this.writeLine("        var instance:"+current.key+" = new "+current.key+"(data);");
                        this.writeLine("        assertNotNull(\"failed to instantiate with data argument\",instance);");

                        if(dto.hasOwnProperty("properties")){
                            var props2 = dto["properties"];
                            for(var key2 in props2){
                                if(props2.hasOwnProperty(key2)){
                                    var prop2 = props2[key2];
                                    if(prop2 && prop2.type){
                                        var resolvedType2 = this.resolveType(prop2);
                                        if(resolvedType2.toString().indexOf('Vector')>-1){
                                            //TODO need to test for instantiation of Vector of childtype...
                                            continue;
                                        }
                                        switch (resolvedType2){
                                            case 'Number':
                                                this.writeLine("                assertEquals(\"Property not set as per constructor argument\",1,instance."+nameLowerCaseLead(key2)+");");
                                                break;
                                            case 'String':
                                                this.writeLine("                assertEquals(\"Property not set as per constructor argument\",\"test\",instance."+nameLowerCaseLead(key2)+");");
                                                break;
                                            case 'Boolean':
                                                this.writeLine("                assertTrue(\"Property not set as per constructor argument\",instance."+nameLowerCaseLead(key2)+");");
                                                break;
                                            case 'Date':
                                                this.writeLine("                assertNotNull(\"Property not set as per constructor argument\",instance."+nameLowerCaseLead(key2)+");");
                                                break;
                                            default:
                                                // any misses will have been logged above
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        this.writeLine("        }");
                        this.writeLine("");
                        this.writeLine("    }");
                        this.writeLine("");
                        this.writeLine("}");
                        this.writeLine("");
                        var myOutput = outputString;
                        var fs = require('fs');
                        fs.open(this.outputDir + 'Test'+current.key + '.as', 'w', 666, function (e, id) {
                            fs.write(id, myOutput, 0, myOutput.length, 0, function (id) {
                                fs.close(id);
                            });
                        });
                    }
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