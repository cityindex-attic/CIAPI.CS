var schema = require("../src/meta/schema.js").schema;
var smd = require("../src/meta/smd.js").smd;
var schemaPatch = require("../src/meta/schema.patch.js").schema;
var routesPatch = require("../src/meta/routes.patch.js").routesPatch;

var JSchemaProvider = require("../src/JSchemaProvider.js").JSchemaProvider;
var CSharpVisitor = require("../src/JSchemaProvider.CSharpVisitor.js").CSharpVisitor;
var CSharpRouteGenerator = require("../src/CSharpRouteGenerator.js").CSharpRouteGenerator;

var LSChannelGenerator = require("../src/CSharpLightStreamerChannelGenerator.js").LSChannelGenerator;


var visitor = new CSharpVisitor();
var provider = new JSchemaProvider(visitor);
// FIXME: schema should be a ctor param 
provider.schema = schema;
// FIXME: instigator should not take parameters
provider.visit("root", schema, "schema");
var output = visitor.toString();
var rpcGenerator = new CSharpRouteGenerator(smd.services.rpc, schema, "CIAPI.Rpc", "Client", ["System", "System.Collections.Generic", "CityIndex.JsonClient", "CIAPI.DTO"], routesPatch);
var rpcRoutes = rpcGenerator.generate();
var channelGenerator = new LSChannelGenerator();
var channels = channelGenerator.generateChannels(smd.services.streaming);

// errors in this script will break build
// throw new Error("intentional");

var fs = require('fs'), str = 'string to append to file';
fs.open('../../CIAPI/Generated/DTO.cs', 'w', 666, function (e, id) {
    fs.write(id, output, 'w', 'utf8', function () {
        fs.close(id, function () {
        });
    });
});

fs.open('../../CIAPI/Generated/Routes.cs', 'w', 666, function (e, id) {
    fs.write(id, rpcRoutes, 'w', 'utf8', function () {
        fs.close(id, function () {
        });
    });
});

fs.open('../../CIAPI/Generated/LightstreamerClient.cs', 'w', 666, function (e, id) {
    fs.write(id, channels, 'w', 'utf8', function () {
        fs.close(id, function () {
        });
    });
});

