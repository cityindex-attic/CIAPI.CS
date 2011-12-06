using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CodeGeneration
{
    internal class Program
    {
        private static string DereferenceRootRelative(string type)
        {
            if (type.StartsWith("#.") || type.StartsWith("#/"))
            {
                type = type.Substring(2);
            }
            return type;
        }

        private static string DetermineNetType(string typename, JObject property)
        {
            string format = "";
            if (property["format"] != null)
            {
                format = property["format"].Value<string>();
            }


            switch (typename)
            {
                case "integer":
                    typename = "int";
                    break;
                case "number":
                    switch (format)
                    {
                        case "":

                            break;
                        case "decimal":
                            typename = "decimal";
                            break;
                        default:
                            throw new Exception("unrecognized format value for number type " + format);
                    }
                    break;
                case "string":
                    switch (format)
                    {
                        case "":

                            break;
                        case "wcf-date":
                            typename = "DateTime";
                            break;
                        default:
                            throw new Exception("unrecognized format value for string type " + format);
                    }
                    break;
                case "array":
                    var items = (JArray) property["items"];
                    if (items.Count == 1)
                    {
                        JToken item = items[0];
                        switch (item.Type)
                        {
                            case JTokenType.Object:
                                // should be a reference
                                JToken reference = item["$ref"];
                                if (reference == null)
                                {
                                    throw new Exception("should be a reference");
                                }
                                typename = DereferenceRootRelative(reference.Value<string>()) + "[]";
                                break;
                            case JTokenType.String:
                                switch ((item).Value<string>())
                                {
                                    case "integer":
                                        typename = "int[]";
                                        break;
                                    default:
                                        throw new Exception("invalid item type");
                                }
                                break;
                            default:
                                throw new Exception("invalid item type");
                        }
                    }
                    else
                    {
                        throw new Exception("invalid number of items");
                    }
                    break;
                case "boolean":
                    typename = "bool";
                    break;
                default:
                    throw new Exception("unrecognized typename " + typename);
            }

            return typename;
        }

        private static string GetPropertyType(JObject propertySchema)
        {
            string propertyType = null;

            switch (propertySchema["type"].Type)
            {
                case JTokenType.Array:
                    // means nullable

                    var types = (JArray) propertySchema["type"];
                    if (types.Count != 2)
                    {
                        throw new Exception("invalid number of type elements");
                    }
                    if (types[0].Value<string>() == "null")
                    {
                        propertyType = types[1].Value<string>();
                    }
                    else if (types[1].Value<string>() == "null")
                    {
                        propertyType = types[0].Value<string>();
                    }
                    else
                    {
                        throw new Exception("only description of nullable allowed as union type");
                    }
                    propertyType = DetermineNetType(propertyType, propertySchema);
                    propertyType = propertyType + "?";
                    break;
                case JTokenType.String:
                    propertyType = propertySchema["type"].Value<string>();
                    propertyType = DetermineNetType(propertyType, propertySchema);
                    break;
                case JTokenType.Object:
                    // is a reference
                    propertyType = propertySchema["type"]["$ref"].Value<string>();
                    propertyType = DereferenceRootRelative(propertyType);

                    break;
                default:
                    throw new Exception("unrecognized json type " + propertySchema["type"].Type);
            }
            return propertyType;
        }

        private static void Main(string[] args)
        {
            var sb = new StringBuilder();
            string schemaText = File.ReadAllText("meta\\schema.json");
            JObject schema = JObject.Parse(schemaText);
            var properties = (JObject)schema["properties"];

            string schemaPatchText = File.ReadAllText("meta\\schema.patch.json");
            JObject schemaPatch = JObject.Parse(schemaPatchText);
            

            var schemaPatchProperties = (JObject)schemaPatch["properties"];
            foreach (JProperty schemaPatchProp in schemaPatchProperties.Properties())
            {
                string propName = schemaPatchProp.Name;
                JObject schemaProp = (JObject) properties[propName];
                foreach (JProperty newProp in schemaPatchProp.Value["properties"])
                {
                    ((JObject)schemaProp["properties"]).Add(newProp);
                }

            }

            
            foreach (JProperty schemaProp in properties.Properties())
            {
                
                string typeName = schemaProp.Name;
                string baseTypeName = null;
                JToken schemaValue = schemaProp.Value;

                if(schemaValue["enum"]!=null)
                {
                    
                }
                else
                {
                    JToken extends = schemaValue["extends"];
                    if (extends != null)
                    {
                        baseTypeName = DereferenceRootRelative(extends.Value<string>());
                    }


                    sb.AppendLine("public partial class " + typeName +
                                  (string.IsNullOrEmpty(baseTypeName) ? "" : (" : " + baseTypeName)));
                    sb.AppendLine("{");
                    JToken dtoProperties = schemaValue["properties"];
                    if (dtoProperties != null)
                    {
                        foreach (JProperty dtoProp in dtoProperties)
                        {
                            string propertyName = dtoProp.Name;
                            var propertySchema = (JObject)dtoProp.Value;


                            string propertyType = GetPropertyType(propertySchema);

                            sb.AppendLine("\t" + "public " + propertyType + " " + propertyName + " {get; set;}");
                        }
                    }

                    sb.AppendLine("}");
                }


            }


            string dtoCode = sb.ToString();
            Console.WriteLine(dtoCode);

            string smdText = File.ReadAllText("meta\\smd.json");
            JObject smd = JObject.Parse(smdText);
        }
    }
}