console.log("generating DTO....");

var schema = require("./meta/schema.js").schema;
var schemaPatch = require("./meta/schema.patch.js").schemaPatch;


var JSchemaProvider = require("./JSchemaProvider.js").JSchemaProvider;
var CSharpVisitor = require("./JSchemaProvider.CSharpVisitor.js").CSharpVisitor;

// patch in additional members to DTO
for (patchTypeName in schemaPatch.properties) {
    if (schemaPatch.properties.hasOwnProperty(patchTypeName)) {
        var patchType = schemaPatch.properties[patchTypeName];
        var targetType = schema.properties[patchTypeName];
        for (patchPropertyName in patchType.properties) {
            if (patchType.properties.hasOwnProperty(patchPropertyName)) {
                targetType.properties[patchPropertyName] = patchType.properties[patchPropertyName];
            }
        }
    }
}



var visitor = new CSharpVisitor();
var provider = new JSchemaProvider(visitor);
// FIXME: schema should be a ctor param 
provider.schema = schema;
// FIXME: instigator should not take parameters
provider.visit("root", schema, "schema");
var output = visitor.toString();


var fs = require('fs'), str = 'string to append to file';
fs.open('../../CIAPI/Generated/DTO.cs', 'w', 666, function (e, id) {
    fs.write(id, output, 'w', 'utf8', function () {
        fs.close(id, function () {
            console.log("  generated DTO");
        });
    });
});

