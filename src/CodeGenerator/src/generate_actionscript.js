console.log("Initialising program....");

if(!process.argv || process.argv.length<4){
    throw new Error("Please invoke with 2 command line arguments - absolute path to dto dir, absolute path to test dir");
}

// get the target src output directory, make sure it has a trailing path separator
var outDir = cleansePath(process.argv[2]);
console.log("Writing DTO to "+outDir);
// get the target test output directory, make sure it has a trailing path separator
var testOutDir = cleansePath(process.argv[3]);
console.log("Writing Tests to "+testOutDir);

// load required resources
var schema = require("./meta/schema.js").schema;
var schemaPatch = require("./meta/schema.patch.js").schemaPatch;
var JSchemaProvider = require("./JSchemaProvider.js").JSchemaProvider;
var ActionScriptVisitor = require("./JSchemaProvider.ActionScriptVisitor.js").ActionScriptVisitor;
var ActionScriptTestVisitor = require("./JSchemaProvider.ActionScriptTestVisitor.js").ActionScriptTestVisitor;

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
console.log("visiting DTO....");

var visitor = new ActionScriptVisitor();
visitor.outputDir = outDir;
var provider = new JSchemaProvider(visitor);
provider.schema = schema;
provider.visit("root", schema, "schema");

// parse the schema into ActionScript DTO Tests
console.log("visiting Tests....");

var testVisitor = new ActionScriptTestVisitor();
testVisitor.outputDir = testOutDir;
var testProvider = new JSchemaProvider(testVisitor);
testProvider.schema = schema;
testProvider.visit("root", schema, "schema");

console.log("Fiat voluntas tua.");




function cleansePath(dir) {
    var path = require('path');
    if (!dir || dir == 'undefined') {
        throw new Error("Please supply absolute output directory path command line arguments.")
    } else {
        var pathSeparator = (process.platform === 'win32' ? '\\' : '/');
        if (dir.substr(dir.length - 1) != pathSeparator) {
            dir += pathSeparator;
        }
        dir = path.normalize(dir);
    }
    return dir;
}