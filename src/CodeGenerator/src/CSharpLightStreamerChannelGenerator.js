


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
        appendLine("using CIAPI.StreamingClient;");
        appendLine("using System.Linq;");
        appendLine("using System;");

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
            var parameterType = "";
            var parameterValidator = "";
            if (service.parameters) {
                if (service.parameters.length > 1) {
                    throw new Error("multiple parameters not supported");
                };
                parameterName = service.parameters[0].name;
                parameterArray = service.parameters[0].type == "array";
                parameterValidator = service.parameters[0].pattern;
                if (parameterArray) {
                    parameterType = service.parameters[0].items[0].type;
                } else {
                    parameterType = service.parameters[0].type;
                }

                switch (parameterType) {

                    case "string":
                        break;
                    case "integer":
                        parameterType = "int";
                        break;
                    default:
                        throw new Error("unsupported parameter type:" + parameterType);
                        break;
                }


            }



            appendLine("        public IStreamingListener<" + returnType + "> Build" + key + "Listener(" + (parameterName ? parameterType + " " : "") + (parameterArray ? "[] " : "") + parameterName + ")");
            appendLine("        {");
            if (parameterName) {
                if (parameterValidator) {
                    appendLine("            const string validator = \"" + parameterValidator + "\";");
                    appendLine("            if (!Regex.IsMatch(" + parameterName + ", validator))");
                    appendLine("            {");
                    appendLine("                throw new Exception(\"Invalid " + parameterName + ":\" + " + parameterName + " + \"\\r\\nMust match expression \" + validator);");
                    appendLine("            }");
                }

                if (parameterArray) {
                    appendLine("          var topic = string.Join(\" \", " + parameterName + ".Select(t => Regex.Replace(\"" + service.channel + "\", \"{" + parameterName + "}\", t.ToString())).ToArray());");
                } else {
                    appendLine("            var topic = Regex.Replace(\"" + service.channel + "\", \"{" + parameterName + "}\", " + parameterName + ".ToString());");
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