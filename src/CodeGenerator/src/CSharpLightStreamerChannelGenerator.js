


(function () {

    var output = "";
    function appendLine(value) {
        output = output.concat(value, "\n");
    };

    function append(value) {
        output = output.concat(value);
    };

    function generateChannels(smd) {

        appendLine("using System.Text.RegularExpressions;");
        appendLine("using CIAPI.DTO;");
        appendLine("using StreamingClient;");
        appendLine("using System.Linq;");

        appendLine("");
        appendLine("namespace CIAPI.Streaming.Lightstreamer");
        appendLine("{");
        appendLine("    public partial class LightstreamerClient");
        appendLine("    {");
        appendLine("        #region IStreamingClient Members");
        appendLine("");


        var adapters = {};

        each(smd.services, function (service, key) {
            var returnType = service.returns.$ref;
            var channel = service.channel;
            adapters[service.target] = service.target;
            var parameterName = "";
            var parameterArray = false;
            if (service.parameters) {
                if (service.parameters.length > 1) {
                    throw new Error("multiple parameters not supported");
                };
                parameterName = service.parameters[0].name;
                parameterArray = service.parameters[0].type == "array";
                if (parameterArray) {
                    if (service.parameters[0].items[0].type != "string") {
                        throw new Error("only string or string array parameters supported");
                    }
                } else {
                    if (service.parameters[0].type != "string") {
                        throw new Error("only string or string array parameters supported");
                    }
                }
            }



            appendLine("        public IStreamingListener<" + returnType + "> Build" + key + "Listener(" + (parameterName ? "string " : "") + (parameterArray ? "[] " : "") + parameterName + ")");
            appendLine("        {");
            if (parameterName) {

                if (parameterArray) {
                    appendLine("          var topic = string.Join(\" \", " + parameterName + ".Select(t => Regex.Replace(\"" + service.channel + "\", \"{" + parameterName + "}\", t)).ToArray());");
                } else {
                    appendLine("            var topic = Regex.Replace(\"" + service.channel + "\", \"{" + parameterName + "}\", " + parameterName + ");");
                }
            } else {
                appendLine("            string topic = \"" + service.channel + "\";");
            };

            appendLine("            return BuildListener<" + returnType + ">(\"" + service.target + "\",topic);");

            appendLine("        }");
            appendLine("");


        });

        appendLine("        protected override string[] GetAdapterList()");
        appendLine("        {");
        var adapterList = "";
        var firstAdapter = true;
        each(adapters, function (adapter) {
            if (firstAdapter) {
                firstAdapter = false;
            } else {
                adapterList = adapterList.concat(",");
            }
            adapterList = adapterList.concat("\"", adapter, "\"");
        });
        //
        appendLine("            return new [] { " + adapterList + " };");
        appendLine("        }");

        appendLine("");
        appendLine("        #endregion");
        appendLine("    }");
        appendLine("}");

        return output;

    }

    function isArray(obj) {
        return (obj && obj.prototype == Array);
    };

    function isDefined(obj) {
        return obj && (typeof (obj) != 'undefined');
    };

    function each(obj, action) {
        var self = this;
        if (obj.prototype && obj.prototype == Array) {
            for (var i = 0; i < obj.length; i++) {
                action.call(self, obj[i], i, obj);
            }
        }
        else if (typeof (obj) == "object") {
            for (var name in obj) {
                if (obj.hasOwnProperty(name)) {
                    action.call(self, obj[name], name, obj);
                };
            };
        }
        else {
            throw new Error("cannot iterate supplied obj");
        };
    };

    exports.LSChannelGenerator = function () {
    };

    exports.LSChannelGenerator.prototype.generateChannels = generateChannels;


})();