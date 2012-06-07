
console.log("Initialising program....");

// get the target output directory, make sure it has a trailing path separator
var outDir = process.argv[2];
var path = require('path');
if(!outDir || outDir=='undefined'){
    throw new Error("Please supply absolute output directory path as first command line argument.")
}else{
    var pathSeparator = (process.platform === 'win32' ? '\\' : '/');
    if(outDir.substr(outDir.length-1)!=pathSeparator){
        outDir += pathSeparator;
    }
    outDir = path.normalize(outDir);
}
console.log("Writing to "+outDir);

// load required resources
var schema = require("./meta/schema.js").schema;
var schemaPatch = require("./meta/schema.patch.js").schemaPatch;
var JSchemaProvider = require("./JSchemaProvider.js").JSchemaProvider;
var ActionScriptVisitor = require("./JSchemaProvider.ActionScriptVisitor.js").ActionScriptVisitor;

// patch in additional members to DTO
console.log("patching DTO....");

if(schemaPatch.hasOwnProperty("properties")){
    var schemaPatchProperties = schemaPatch["properties"];
    for (var patchTypeName in schemaPatchProperties) {
        if (schemaPatchProperties.hasOwnProperty(patchTypeName) && schema.hasOwnProperty("properties")) {
            var schemaProperties = schema["properties"];
            var patchType = schemaPatchProperties[patchTypeName];
            var targetType = schemaProperties[patchTypeName];
            if(patchType.hasOwnProperty("properties")){
                for (var patchPropertyName in patchType["properties"]) {
                    if (patchType["properties"].hasOwnProperty(patchPropertyName)) {
                        if (patchPropertyName == "$DELETE$") {
                            for (var toDelete in patchType["properties"]["$DELETE$"]) {
                                if(patchType["properties"]["$DELETE$"].hasOwnProperty(toDelete)){
                                    var toDeletePropName = patchType["properties"]["$DELETE$"][toDelete];
                                    delete targetType["properties"][toDeletePropName];
                                }
                            }
                        }
                        else {
                            if(targetType){
                                targetType["properties"][patchPropertyName] = patchType["properties"][patchPropertyName];
                            }
                        }
                    }
                }
            }
        }
    }
}

// parse the schema into ActionScript DTO
console.log("visiting code....");

var visitor = new ActionScriptVisitor();
visitor.outputDir = outDir;
var provider = new JSchemaProvider(visitor);
provider.schema = schema;
provider.visit("root", schema, "schema");

console.log("Fiat voluntas tua.");