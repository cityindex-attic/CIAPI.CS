
console.log("generating code....");

var schema = require("./meta/schema.js").schema;
var smd = require("./meta/smd.js").smd;
var schemaPatch = require("./meta/schema.patch.js").schemaPatch;
var routesPatch = require("./meta/routes.patch.js").routesPatch;

var JSchemaProvider = require("./JSchemaProvider.js").JSchemaProvider;
var CSharpVisitor = require("./JSchemaProvider.CSharpVisitor.js").CSharpVisitor;
var CSharpRouteGenerator = require("./CSharpRouteGenerator.js").CSharpRouteGenerator;

//var LSChannelGenerator = require("./CSharpLightStreamerChannelGenerator.js").LSChannelGenerator;

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
                    targetType.properties[patchPropertyName] = patchType.properties[patchPropertyName];
                }

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
var rpcGenerator = new CSharpRouteGenerator(smd.services.rpc, schema, "CIAPI.Rpc", "Client", ["System", "System.Collections.Generic", "Salient.ReliableHttpClient", "Salient.ReliableHttpClient.Serialization", "CIAPI.Serialization", "CIAPI.DTO"], routesPatch);
var rpcRoutes = rpcGenerator.generate();
//var channelGenerator = new LSChannelGenerator();
//var channels = channelGenerator.generateChannels(smd.services.streaming);

// errors in this script will break build
// throw new Error("intentional");

var fs = require('fs'), str = 'string to append to file';
fs.open('../../CIAPI/Generated/DTO.cs', 'w', 666, function (e, id) {
    fs.write(id, output, 'w', 'utf8', function () {
        fs.close(id, function () {
            console.log("  generated DTO");
        });
    });
});

fs.open('../../CIAPI/Generated/Routes.cs', 'w', 666, function (e, id) {
    fs.write(id, rpcRoutes, 'w', 'utf8', function () {
        fs.close(id, function () {
            console.log("  generated ROUTES");
        });
    });
});

//fs.open('../../CIAPI/Generated/LightstreamerClient.cs', 'w', 666, function (e, id) {
//    fs.write(id, channels, 'w', 'utf8', function () {
//        fs.close(id, function () {
//            console.log("  generated CHANNELS");
//        });
//    });
//});

