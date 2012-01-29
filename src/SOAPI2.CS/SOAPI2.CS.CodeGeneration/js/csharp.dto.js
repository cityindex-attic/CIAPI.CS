var common = require('./common.js');

function applyFormatCS(propertyType, property) {
    switch (propertyType) {
        case "string":
            // new formats "wcf-date"
            if (property.format == "wcf-date") {
                propertyType = "DateTime";
            }
            else {
                propertyType = "string";
            }
            break;
        case "number":
            // new formats "decimal[-precision?]"
            if (property.format == "decimal") {
                propertyType = "decimal";
            }
            else {
                propertyType = "float";
            }
            break;
        case "integer":
            propertyType = "int";
            break;
        case "boolean":
            propertyType = "bool";
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
    code += "{\n";
    for (var dtoName in schema.properties) {
        var dtoType = schema.properties[dtoName];
        code += "   /// <summary>\n";
        code += "   /// " + dtoType.description + "\n";
        code += "   /// </summary>\n";
        if (dtoType["enum"]) {
            // is an enum
            code += "   public enum " + dtoName + "\n";
            code += "   {" + "\n";
            common.each(dtoType.options, function (obj) {
                if (obj.description) {
                    code += "        /// <summary>\n";
                    code += "        /// " + obj.description + "\n";
                    code += "        /// </summary>\n";
                };
                code += "        " + obj.label + " = " + obj.value + ",\n";
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
            for (var propertyName in dtoType.properties) {
                var property = dtoType.properties[propertyName];
                propertyTypeName = common.resolveType(property, applyFormatCS);
                code += "       /// <summary>\n";
                code += "       /// " + property.description + "\n";
                code += "       /// </summary>\n";
                code += "       public " + propertyTypeName + " " + propertyName + " { get; set; }\n\n";
            }
        }
        code += "   }" + "\n\n";
    }
    code += "}" + "\n";
    return code;
}

exports.emit = emit;