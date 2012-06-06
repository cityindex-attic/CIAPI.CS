
console.log("generating code....");

//var schema = require("./meta/schema.js").schema;
var schema = require("./meta/schema2.js").schema;
var schemaPatch = require("./meta/schema.patch.js").schemaPatch;

var JSchemaProvider = require("./JSchemaProvider.js").JSchemaProvider;
var ActionScriptVisitor = require("./JSchemaProvider.ActionScriptVisitor.js").ActionScriptVisitor;

// patch in additional members to DTO
for (patchTypeName in schemaPatch.properties) {
    if (schemaPatch.properties.hasOwnProperty(patchTypeName)) {
        var patchType = schemaPatch.properties[patchTypeName];
        var targetType = schema.properties[patchTypeName];
        for (patchPropertyName in patchType.properties) {
            if (patchType.properties.hasOwnProperty(patchPropertyName)) {
                if (patchPropertyName == "$DELETE$") {
                    for (toDelete in patchType.properties["$DELETE$"]) {
                        var toDeletePropName = patchType.properties["$DELETE$"][toDelete];
                        delete targetType.properties[toDeletePropName];
                    }
                }
                else {
                    if(targetType){
                        targetType.properties[patchPropertyName] = patchType.properties[patchPropertyName];
                    }
                }
            }
        }
    }
}

var visitor = new ActionScriptVisitor();
var provider = new JSchemaProvider(visitor);
provider.schema = schema;
provider.visit("root", schema, "schema");
