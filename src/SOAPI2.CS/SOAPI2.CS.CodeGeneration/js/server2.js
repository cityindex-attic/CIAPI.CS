var http = require('http');
var url = require('url');
var javaDto = require('./java.dto.js');
var csharpDto = require('./csharp.dto.js');
var javascriptDto = require('./javascript.dto.js');
var options = {
    host: '50.19.161.92',
    port: 80,
    path: '/TradingAPI/metadata/schema.js'
};
http.createServer(function (req, res) {
    res.writeHead(200, {
        'Content-Type': 'text/plain'
    });

    var data = "";

    http.get(options, function (res2) {

        res2.on('data', function (chunk) {
            data = data + chunk;
        });

        res2.on('end', function () {

            data = data.substring(data.indexOf("ciConfig.schema=") + 16);
            data = data.substring(0, data.length - 1);

            data = "{\"namespace\": \"CIAPI.DTO\",\"properties\": " + data + "}";



            schema = JSON.parse(data);
            var qs = url.parse(req.url);
            var pathname = qs.pathname;
            var output = "";
            if (pathname) {
                var segments = pathname.split("/");
                if (segments.length > 2) {
                    var lang = segments[1];
                    var codeType = segments[2];
                    switch (lang) {
                        case "csharp":
                            switch (codeType) {
                                case "dto":
                                    output = csharpDto.emit(schema);
                                    break;
                                case "routes":
                                    break;
                            }
                            break;
                        case "java":
                            switch (codeType) {
                                case "dto":
                                    output = javaDto.emit(schema);
                                    break;
                                case "routes":
                                    break;
                            }
                            break;
                        case "javascript":
                            switch (codeType) {
                                case "dto":
                                    break;
                                case "routes":
                                    break;
                            }
                            break;
                    }
                }
            }
            res.end(output);
        });
    }).on('error', function (e) {
        res.end("Got error: " + e.message);
    });
}).listen(process.env.PORT);