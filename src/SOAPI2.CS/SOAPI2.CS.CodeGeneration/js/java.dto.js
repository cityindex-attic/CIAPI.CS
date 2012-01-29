var common = require('./common.js');

function applyFormatJava(propertyType, property) {
    switch (propertyType) {
        case "string":
            // new formats "wcf-date"
            if (property.format == "wcf-date") {
                propertyType = "DateTime";
            }
            else {
                propertyType = "String";
            }
            break;
        case "number":
            // new formats "decimal[-precision?]"
            if (property.format == "decimal") {
                propertyType = "BigDecimal";
            }
            else {
                propertyType = "double";
            }
            break;
        case "integer":
            propertyType = "int";
            break;
        case "boolean":
            propertyType = "boolean";
            break;
        case "object":
            break;
        case "null":
            break;
        case "any":
            break;
        default:
            break;
    }
    return propertyType;
};


function emit(schema) {
    var code = "";
    code += "namespace " + schema.namespace + "\n";
    for (var dtoName in schema.properties) {
        var dtoType = schema.properties[dtoName];
        code += "   /**>\n";
        code += "    * " + dtoType.description + "\n";
        code += "    */\n";
        if (dtoType["enum"]) {
            // is an enum
            code += "   interface " + dtoName + "\n";
            code += "   {" + "\n";
            common.each(dtoType.options, function (obj) {
                if (obj.description) {
                    code += "        /**\n";
                    code += "         * " + obj.description + "\n";
                    code += "         */\n";
                };
                code += "        public static final int " + obj.label.toUpperCase() + " = " + obj.value + ";\n";
            });
            code += "   }" + "\n\n";
        }
        else {
            var baseType = "";
            if (dtoType["extends"]) {
                baseType = " : " + dtoType["extends"].substring(2);
            }
            code += "   public class " + dtoName + baseType + "\n";
            code += "   {" + "\n";
            code += "       /**\n";
            code += "        * default constructor\n";
            code += "        */\n";
            code += "       public " + dtoName + "() {}\n\n";
            for (var propertyName in dtoType.properties) {
                var property = dtoType.properties[propertyName];
                var propertyTypeName = common.resolveType(property, applyFormatJava);
                code += "       /**\n";
                code += "        * " + property.description + "\n";
                code += "        */\n";
                code += "       public " + propertyTypeName + " get" + propertyName + " { return this." + propertyName + "; }\n\n";
                code += "       public void set" + propertyName + "(" + propertyTypeName + " " + propertyName + ") { this." + propertyName + "=" + propertyName + "; }\n\n";
                code += "       private " + propertyTypeName + " " + propertyName + " { get; set; }\n\n";
            }
        }
        code += "   }" + "\n\n";
    }
    code += "}" + "\n";
    return code;
}
exports.emit = emit;