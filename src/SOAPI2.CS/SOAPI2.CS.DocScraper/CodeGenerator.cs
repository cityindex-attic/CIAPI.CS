using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SOAPI2.DocScraper
{
    public class CodeGenerator
    {
        private JObject _schema;
        private JObject _smd;
        public CodeGenerator(JObject schema, JObject smd)
        {
            _smd = smd;
            _schema = schema;
        }
        private string GetReferenceType(JToken fieldValueType)
        {
            string fieldType;

            string reference = (string)fieldValueType["$ref"];
            Regex genericTypeRx = new Regex("#.(?<type>.*?)<(?<param>.*?)>", RegexOptions.ExplicitCapture);
            var genericTypeMatch = genericTypeRx.Match(reference);
            if (genericTypeMatch.Success)
            {
                string genTypeName = genericTypeMatch.Groups["type"].Value;
                JObject genType = (JObject)_schema["properties"][genTypeName];
                string genParamTypeName = genericTypeMatch.Groups["param"].Value;
                JObject genParamType = (JObject)_schema["properties"][genParamTypeName];
                if (genParamType != null)
                {
                    genParamTypeName = genParamType["id"].Value<string>().PascalCased() + "Class";
                }


                fieldType = genType["id"].Value<string>().PascalCased() + "Class<" + genParamTypeName + ">";
            }
            else
            {

                JToken refType = _schema["properties"][reference.Substring(2)];
                if (refType != null)
                {
                    if (refType["enum"] == null)
                    {
                        fieldType = reference.Substring(2).PascalCased() + "Class";
                    }
                    else
                    {
                        fieldType = reference.Substring(2).PascalCased();
                    }
                }
                else
                {
                    // could be a generic type param?
                    fieldType = reference.Substring(2).PascalCased();
                }

            }
            return fieldType;
        }
        private string GetComplexFieldType(JObject fieldValue)
        {
            JToken fieldValueType = fieldValue["type"];
            return GetReferenceType(fieldValueType);
        }
        public void GenerateModel(StringBuilder sb)
        {
            var tabs = "";


            sb.AppendLine(string.Format("{0}using {1};", tabs, "System"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "System.Collections.Generic"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "SOAPI2.Converters"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "Newtonsoft.Json"));
            sb.AppendLine(string.Format("{0}namespace {1}", tabs, "SOAPI2.Model"));
            sb.AppendLine(string.Format("{0}{{", tabs));



            tabs = "\t";
            foreach (JProperty kvp in _schema["properties"])
            {
                JObject prop = (JObject)kvp.Value;
                string summary = (string)prop["description"];
                if (!string.IsNullOrEmpty(summary))
                {
                    sb.AppendLine(string.Format("{0}/// <summary>", tabs));
                    summary = summary.Trim();
                    string[] summaryLines = summary.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    foreach (string summaryLine in summaryLines)
                    {
                        sb.AppendLine(string.Format("{0}/// {1}", tabs, summaryLine));
                    }
                    sb.AppendLine(string.Format("{0}/// </summary>", tabs));
                }


                string className = prop["id"].Value<string>().PascalCased();

                JArray enumValues = (JArray)prop["enum"];


                if (enumValues != null)
                {
                    // #NOTE: i considered using constants for these but it would make
                    // building queries not 'type-safe' introducing confusion
                    // the risk is that if a dev stores data and uses the numeric value
                    // instead of the ToString() and then the API changes the values can be
                    // wrong. So guidance needs to be given to NOT depend on the numeric value
                    // and only store these in text format
                    sb.AppendLine(string.Format("{0}public enum {1}", tabs, className));
                    sb.AppendLine(string.Format("{0}{{", tabs));
                    bool isFirst = true;
                    foreach (string enumValue in enumValues)
                    {

                        //

                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            sb.AppendLine(",");
                        }
                        //sb.AppendLine(string.Format("{0}\t[JsonProperty(\"{1}\")]", tabs, enumValue));
                        sb.Append(string.Format("{0}\t@{1}", tabs, enumValue));
                    }
                    sb.AppendLine(string.Format("{0}", tabs));
                    sb.AppendLine(string.Format("{0}}}", tabs));
                }
                else
                {
                    string genericType = (string)prop["generic_type"];

                    // #TODO: some types have fields of same name, which is a no-no. need to come up with a good suffix 
                    sb.AppendLine(string.Format("{0}public class {1}{2}", tabs, className + "Class", (string.IsNullOrEmpty(genericType) ? "" : "<" + genericType.PascalCased() + ">")));
                    sb.AppendLine(string.Format("{0}{{", tabs));

                    foreach (JProperty field in prop["properties"])
                    {
                        string jsonConverter = null;
                        sb.AppendLine(string.Format("{0}\t[JsonProperty(\"{1}\")]", tabs, field.Name));
                        JObject fieldValue = (JObject)field.Value;
                        string fieldType = "";

                        if (fieldValue["type"].Type == JTokenType.Object)
                        {
                            fieldType = GetComplexFieldType(fieldValue);


                        }
                        else
                        {

                            fieldType = (string)fieldValue["type"];
                            switch (fieldType)
                            {
                                case "array":
                                    if (((JArray)fieldValue["items"]).Count != 1)
                                    {
                                        throw new Exception("unexpected items");
                                    }
                                    JToken firstItemsValue = ((JArray)fieldValue["items"])[0];
                                    if (firstItemsValue.Type == JTokenType.Object)
                                    {
                                        JObject itemsValue = (JObject)firstItemsValue;
                                        fieldType = GetReferenceType(itemsValue);


                                    }
                                    else if (firstItemsValue.Type == JTokenType.String)
                                    {
                                        fieldType = firstItemsValue.Value<string>();

                                    }
                                    else
                                    {

                                        throw new NotImplementedException();
                                    }
                                    fieldType = "List<" + fieldType + ">";
                                    break;
                                case "number":

                                    if (!string.IsNullOrEmpty((string)fieldValue["format"]))
                                    {
                                        switch ((string)fieldValue["format"])
                                        {
                                            case "utc-millisec":
                                                fieldType = "DateTimeOffset";

                                                jsonConverter = "UnixDateTimeOffsetConverter";
                                                break;

                                            case "decimal":
                                                fieldType = "decimal";
                                                break;
                                            case "integer":
                                                fieldType = "int";
                                                break;
                                            default:
                                                throw new NotImplementedException("format " + (string)fieldValue["format"]);
                                        }
                                    }
                                    else
                                    {
                                        fieldType = "double";
                                    }
                                    break;
                                case "boolean":
                                    fieldType = "bool";
                                    break;
                                case "string":

                                    if (!string.IsNullOrEmpty((string)fieldValue["format"]))
                                    {
                                        switch ((string)fieldValue["format"])
                                        {



                                            default:
                                                throw new NotImplementedException("format " + (string)fieldValue["format"]);
                                        }
                                    }
                                    break;
                                default:

                                    //Debug.WriteLine(fieldType);

                                    fieldType = "object";

                                    break;
                            }
                        }


                        //[JsonConverter(typeof(FOO))]
                        if (!string.IsNullOrEmpty(jsonConverter))
                        {
                            sb.AppendLine(string.Format("{0}\t[JsonConverter(typeof({1}))]", tabs, jsonConverter));
                        }
                        sb.AppendLine(string.Format("{0}\tpublic {1} {2} {{get; set;}}", tabs, fieldType, field.Name.PascalCased()));

                        sb.AppendLine();
                    }


                    sb.AppendLine(string.Format("{0}", tabs));
                    sb.AppendLine(string.Format("{0}}}", tabs));
                }



            }
            tabs = "";
            sb.AppendLine(string.Format("{0}}}", tabs));

        }

        private Dictionary<string, Dictionary<string, JObject>> GetGroupedMethods()
        {
            var groupedMethods = new Dictionary<string, Dictionary<string, JObject>>();
            foreach (JProperty kvp in _smd["services"])
            {
                var serviceName = kvp.Name;
                string serviceGroupName = (string)kvp.Value["group"];
                Dictionary<string, JObject> serviceGroup = null;

                if (groupedMethods.ContainsKey(serviceGroupName))
                {
                    serviceGroup = groupedMethods[serviceGroupName];
                }
                else
                {
                    serviceGroup = new Dictionary<string, JObject>();
                    groupedMethods.Add(serviceGroupName, serviceGroup);
                }
                serviceGroup.Add(serviceName, (JObject)kvp.Value);
            }
            return groupedMethods;
        }
        public void GenerateRoutes(StringBuilder sb)
        {

            var tabs = "";

            sb.AppendLine(string.Format("{0}using {1};", tabs, "System"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "System.Collections.Generic"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "Newtonsoft.Json"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "CityIndex.JsonClient"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "SOAPI2.Converters"));
            sb.AppendLine(string.Format("{0}using {1};", tabs, "SOAPI2.Model"));
            sb.AppendLine(string.Format("{0}namespace {1}", tabs, "SOAPI2"));
            sb.AppendLine(string.Format("{0}{{", tabs));
            tabs = "\t";

            sb.AppendLine(string.Format("{0}public partial class SoapiClient", tabs));
            sb.AppendLine(string.Format("{0}{{", tabs));
            var groupedMethods = GetGroupedMethods();

            var groupNames = groupedMethods.Keys.ToArray();
            tabs = "\t\t";
            sb.AppendLine("        public SoapiClient(string apiKey, string appId) // #TODO: uri from SMD target");
            sb.AppendLine("            : base(new Uri(\"https://api.stackexchange.com/2.0/\"), new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(), new ErrorResponseDTOJsonExceptionFactory(), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, \"data\"), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, \"trading\"), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, \"default\")))");
            sb.AppendLine("        {");
            sb.AppendLine("");
            sb.AppendLine("   ");
            sb.AppendLine("");
            sb.AppendLine("#if SILVERLIGHT");
            sb.AppendLine("#if WINDOWS_PHONE");
            sb.AppendLine("		  UserAgent = \"SOAPI2.PHONE7.\"+ GetVersionNumber();");
            sb.AppendLine("#else");
            sb.AppendLine("		  UserAgent = \"SOAPI2.SILVERLIGHT.\"+ GetVersionNumber();");
            sb.AppendLine("#endif");
            sb.AppendLine("#else");
            sb.AppendLine("            UserAgent = \"SOAPI2.\" + GetVersionNumber();");
            sb.AppendLine("#endif");
            sb.AppendLine("            _client = this;");
            sb.AppendLine("            _appId = appId;");
            sb.AppendLine("            _apiKey = apiKey;");
            foreach (string groupName in groupNames)
            {
                sb.AppendLine(string.Format("{0}    this.{1} = new _{1}(this);", tabs, groupName));
            }
 
            sb.AppendLine("");
            sb.AppendLine("        }");
            sb.AppendLine("        public SoapiClient(string apiKey, string appId,Uri uri, IRequestController requestController)");
            sb.AppendLine("            : base(uri, requestController)");
            sb.AppendLine("        {");
            sb.AppendLine("");
            sb.AppendLine("   ");
            sb.AppendLine("#if SILVERLIGHT");
            sb.AppendLine("#if WINDOWS_PHONE");
            sb.AppendLine("		  UserAgent = \"SOAPI2.PHONE7.\"+ GetVersionNumber();");
            sb.AppendLine("#else");
            sb.AppendLine("		  UserAgent = \"SOAPI2.SILVERLIGHT.\"+ GetVersionNumber();");
            sb.AppendLine("#endif");
            sb.AppendLine("#else");
            sb.AppendLine("            UserAgent = \"SOAPI2.\" + GetVersionNumber();");
            sb.AppendLine("#endif");
            sb.AppendLine("            _client = this;");
            sb.AppendLine("            _appId = appId;");
            sb.AppendLine("            _apiKey = apiKey;");
            sb.AppendLine("");
            foreach (string groupName in groupNames)
            {
                sb.AppendLine(string.Format("{0}    this.{1} = new _{1}(this);", tabs, groupName));
            }
 
            sb.AppendLine("        }");

 

            foreach (string groupName in groupNames)
            {
                // create sub types for clean architecture

                string subGroupTypeName = "_" + groupName;
                
                sb.AppendLine(string.Format("{0}public class {1}", tabs, subGroupTypeName));
                sb.AppendLine(string.Format("{0}{{", tabs));
                sb.AppendLine(string.Format("{0}\tprivate SoapiClient _client;", tabs));
                sb.AppendLine(string.Format("{0}\tpublic {1}(SoapiClient client)", tabs, subGroupTypeName));
                sb.AppendLine(string.Format("{0}\t{{", tabs));
                sb.AppendLine(string.Format("{0}\t\t_client=client;", tabs));
                sb.AppendLine(string.Format("{0}\t}}", tabs));

                // methods

                var group = groupedMethods[groupName];
                foreach (var kvp in group)
                {
                    var methodName = kvp.Key;
                    var methodObj = kvp.Value;
                    var contentType = (string)methodObj["contentType"];
                    var responseContentType = (string)methodObj["responseContentType"];
                    var transport = (string)methodObj["transport"];
                    var envelope = (string)methodObj["envelope"];
                    var cacheDuration = (string)methodObj["cacheDuration"];
                    var throttleScope = (string)methodObj["throttleScope"];
                    var uriTemplate = (string)methodObj["uriTemplate"];
                    var returns = (JObject) methodObj["returns"];
                    
                    var description = (string)methodObj["description"];

                    string returnType = "void";
                    if (returns != null)
                    {
                        returnType = GetReferenceType(returns);    
                    }
                    
                    sb.AppendLine(string.Format("{0}\tpublic {2} Get{1}()", tabs, methodName.PascalCased(), returnType));
                    sb.AppendLine(string.Format("{0}\t{{", tabs));
                    sb.AppendLine(string.Format("{0}\t\tthrow new NotImplementedException();", tabs));
                    sb.AppendLine(string.Format("{0}\t}}", tabs));
                }
                sb.AppendLine(string.Format("{0}}}", tabs));
                sb.AppendLine(string.Format("{0}public {1} {2}{{get; private set;}}", tabs, subGroupTypeName, groupName));


            }
            tabs = "\t";
            sb.AppendLine(string.Format("{0}}}", tabs));
            tabs = "";
            sb.AppendLine(string.Format("{0}}}", tabs));
        }
    }
}
