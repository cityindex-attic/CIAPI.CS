var http = require('http');
var url = require('url');
var fs = require('fs');
var javaDto = require('./java.dto.js');
var csharpDto = require('./csharp.dto.js');
var meta = require('./schema.js');

var schema = meta.schema;
 
http.createServer(function (req, res) {
    res.writeHead(200, {
        'Content-Type': 'text/plain'
    });

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
}).listen(8888);
