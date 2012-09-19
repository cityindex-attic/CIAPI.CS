
(function () {





    var provider = exports.JSchemaProvider = function (visitor) {
        this.visitor = visitor;
        this.visitor.provider = this;
        this.attributeNames = [
        "type",
        "extends",
        "properties",
        "patternProperties",
        "additionalProperties",
        "items",
        "additionalItems",
        "required",
        "dependencies",
        "minimum",
        "maximum",
        "exclusiveMinimum",
        "exclusiveMaximum",
        "minItems",
        "maxItems",
        "uniqueItems",
        "pattern",
        "minLength",
        "maxLength",
        "enum",
        "default",
        "title",
        "description",
        "format",
        "divisibleBy",
        "disallow",
        "id",
        "$ref",
        "$schema"];
    };


    provider.prototype = {
        stack: [],
        visitor: {},

        push: function (key, value, type) {
            var current = { "key": key, "value": value, "type": type };
            this.stack.push(current);
        },
        pop: function () {
            this.stack.pop();
        },
        peek: function () {
            return this.stack[this.stack.length - 1];
        },
        visit: function (key, value, type) {


            var depth = this.stack.length;
            this.push(key, value, type);


            if (this.visitor["visit_" + type]) {
                this.visitor["visit_" + type]();
            }

            this["visit_" + type]();

            if (this.visitor["visit_" + type + "_end"]) {
                this.visitor["visit_" + type + "_end"]();
            }

            this.pop();
            if (depth != this.stack.length) {
                throw new Error("stack out of sync");
            }
        },

        "visit_schema": function () {

            var current = this.peek();
            // iterate all possible schema properties

            var self = this;
            each(this.attributeNames, function (value, key) {
                // todo: explosion of ambiguity here with 'value' - clarify
                if (isDefined(current.value[value])) {
                    self.visit(value, current.value[value], value);
                }
            });
        },
        "visit_properties": function () {
            // an obj array of property schema
            var current = this.peek();
            var self = this;
            each(current.value, function (value, key) {
                self.visit(key, value, "property");
            });
        },

        "visit_property": function () {
            // a property is a schema - this looks like duplicate code
            // and ultimately may be 

            var current = this.peek();
            // iterate all possible schema properties

            var self = this;
            each(this.attributeNames, function (value, key) {
                // todo: explosion of ambiguity here with 'value' - clarify
                if (isDefined(current.value[value])) {
                    self.visit(value, current.value[value], value);
                }
            });
        },

        "visit_type": function () {
        },
        "visit_disallow": function () {
            // similar to .type
        },
        "visit_extends": function () {
            // string or array of strings
        },


        "visit_patternProperties": function () { },
        "visit_additionalProperties": function () { },
        "visit_items": function () { },
        "visit_additionalItems": function () { },
        "visit_dependencies": function () { },
        "visit_enum": function () { },
        "visit_default": function () { },


        // simple props
        "visit_$ref": function () { },
        "visit_$schema": function () { },
        "visit_divisibleBy": function () { },
        "visit_pattern": function () { },
        "visit_minItems": function () { },
        "visit_maxItems": function () { },
        "visit_uniqueItems": function () { },
        "visit_minimum": function () { },
        "visit_maximum": function () { },
        "visit_exclusiveMinimum": function () { },
        "visit_exclusiveMaximum": function () { },
        "visit_required": function () { },
        "visit_description": function () { },
        "visit_id": function () { },
        "visit_format": function () { },
        "visit_title": function () { },
        "visit_minLength": function () { },
        "visit_maxLength": function () { }
    };








    // private lib funcs

    function isArray(obj) {
        return (obj && obj.prototype == Array);
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





