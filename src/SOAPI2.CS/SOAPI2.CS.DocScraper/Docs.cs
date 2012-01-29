using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SOAPI2.CS.DocScraper
{
    /// <summary>
    /// #TODO: parse https://api.stackexchange.com/docs/wrapper
    /// </summary>
    [Serializable]
    public class Docs
    {
        public static string MungeMethodName(string methodName)
        {
            methodName = methodName.ToLower();
            methodName = methodName.Replace("/", " ").Trim();
            methodName = methodName.Replace("-", " ");
            methodName = methodName.Replace("{ids}", "by ids");
            methodName = methodName.Replace("{id}", "by id");
            methodName = methodName.Replace("{tags}", "by tags");
            methodName = methodName.Replace("{tag}", "by tag");
            methodName = methodName.Replace("{period}", "by period");
            methodName = methodName.Replace("{toid}", "to id");
            methodName = methodName.Replace("{accesstokens}", " ");
            methodName = methodName.Replace("{filters}", " ");
            //methodName = methodName.ToTitleCase();
            methodName = methodName.Replace(" ", "_");
            methodName = methodName.Trim();
            return methodName;
        }
        private string _methodsIndex;
        private string _typesIndex;
        private static JObject CreateJRef(string fieldType)
        {
            var fieldTypeObj = new JObject();

            fieldTypeObj["$ref"] = "#." + fieldType;
            return fieldTypeObj;
        }
        public JObject CreateSchema()
        {
            var schemaObj = new JObject();
            JObject typesObj = new JObject();
            schemaObj["properties"] = typesObj;

            foreach (var type in this.Types)
            {
                var typeObj = new JObject();

                typesObj[type.Name] = typeObj;
                typeObj["id"] = type.Name;
                
                if (type.IsEnum)
                {
                    typeObj["type"] = "string";
                    var enumValues = new JArray(type.Fields.Select(f => f.Name).ToArray());
                    typeObj["enum"] = enumValues;

                }
                else
                {
                    typeObj["type"] = "object";
                    var fieldsObj = new JObject();
                    typeObj["properties"] = fieldsObj;
                    foreach (var field in type.Fields)
                    {
                        var fieldObj = new JObject();
                        fieldsObj[field.Name] = fieldObj;


                        string fieldType = field.Type;

                        JToken fieldTypeObj=null;

                        string format = "";

                        if (field.IsPrimitive && !field.IsEnum)
                        {
                      
                            

                            
                            switch (fieldType)
                            {
                                case "date":
                                    fieldType = "string";
                                    format = "utc-millisec";
                                    break;
                                case "decimal":
                                    fieldType = "number";
                                    format = "integer";
                                    break;
                                case "integer":
                                    fieldType = "number";
                                    format = "decimal";
                                    break;
                                case"boolean":
                                case "string":
                                    //noop
                                    break;
                                    default:
                                    throw new Exception("unrecogized primitive type " + fieldType);
                            }
                            
                            
                            fieldTypeObj = new JValue(fieldType);
                        }
                        else
                        {
                            
                            fieldTypeObj = CreateJRef(fieldType);
                            
                        }


                        if (field.IsArray)
                        {
                            fieldObj["type"] = "array";
                            JArray items = new JArray();
                            fieldObj["items"] = items;
                            items.Add(fieldTypeObj);
                        }
                        else
                        {
                            fieldObj["type"] = fieldTypeObj;
                        }

                        if (!string.IsNullOrEmpty(format))
                        {
                            fieldObj["format"] = format;
                        }


                        fieldObj["description"] = field.Description;
                        fieldObj["included_by_default"] = field.IncludedInDefaultFilter;
                        fieldObj["unsafe"] = field.UnchangedInUnsafeFilters;
                    }

                }

                typeObj["description"] = type.Description;
            }

            return schemaObj;
        }
        public Docs()
        {
            MethodGroups = new List<MethodGroup>();
            Types = new List<TypeInfo>();
        }

        public List<TypeInfo> Types { get; set; }

        public string MethodsIndex
        {
            get { return _methodsIndex; }
            set
            {
                _methodsIndex = value;

                ParseMethods();
            }
        }

        public string TypesIndex
        {
            get { return _typesIndex; }
            set
            {
                _typesIndex = value;

                ParseTypes(this);
            }
        }

        public List<MethodGroup> MethodGroups { get; set; }

        private void ParseTypes(Docs docs)
        {
            string types = TypesIndex;
            // they broke this saturday - grrrr
//            types = types.Substring(0, types.IndexOf("<div id=\"footer\">")).Trim();  
            types = types.Substring(0, types.IndexOf("<div class=\"footer\">")).Trim();
            const string STR_H2TopLevelTypesh2 = "<h2>Top Level Types</h2>";
            types = types.Substring(types.IndexOf(STR_H2TopLevelTypesh2) + STR_H2TopLevelTypesh2.Length).Trim();
            MatchCollection typeMatches = Regex.Matches(types, "<a href=\"(?<url>/docs/types/(?<type>.*?))\">",
                                                        RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            foreach (Match typeMatch in typeMatches)
            {
                string newType = typeMatch.Groups["type"].Value.Trim();
                string newUrl = typeMatch.Groups["url"].Value.Trim();
                if (Types.Where(t => t.Type == newType).Count() == 0)
                {

                    Types.Add(new TypeInfo(docs) { Type = newType, Url = newUrl });
                }
            }
        }

        private void ParseMethods()
        {
            string methods = MethodsIndex;
            bool isNetworkWide = false;
            methods = methods.Substring(0, methods.IndexOf("<div class=\"sidebar\">"));

            //
            methods = methods.Substring(methods.IndexOf("<h2>Per-Site Methods</h2>") + 25);
            int networkIndex = methods.IndexOf("<h2>Network Methods</h2>");
            MatchCollection headers = Regex.Matches(methods, "<h3>(?<header>.*?)</h3>",
                                                    RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            foreach (Match header in headers)
            {
                isNetworkWide = header.Index > networkIndex;
                string headerText = header.Groups["header"].Value;
                var group = new MethodGroup();
                group.GroupName = headerText.Replace(" ", "_").Replace("-", "_").Trim();
                ;
                group.GroupTitle = headerText;
                group.Methods = new List<MethodInfo>();
                group.IsNetworkWide = isNetworkWide;
                MethodGroups.Add(group);

                string methodTemp = methods.Substring(header.Index);
                methodTemp = methodTemp.Substring(0, methodTemp.IndexOf("</ul>"));
                MatchCollection methodListMatch = Regex.Matches(methodTemp, "(?<method><li>.*?</li>)+",
                                                                RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase |
                                                                RegexOptions.Singleline);
                foreach (Match method in methodListMatch)
                {
                    string methodLink = method.Groups["method"].Value.Trim();






                    Match methodMatch = Regex.Match(methodLink,
                                                    "<a href=\"(?<url>.*?)\">(?<template>.*?)</a>.*?&ndash;\\s*(?<description>.*?)\\s*</li>",
                                                    RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase |
                                                    RegexOptions.Singleline);
                    string methodUrl = methodMatch.Groups["url"].Value.Trim();
                    string methodDescription = methodMatch.Groups["description"].Value.Trim();
                    string methodUriTemplate = methodMatch.Groups["template"].Value.Trim();
                    string methodName = MungeMethodName(methodUriTemplate);


                    var requiresAuthentication = Regex.IsMatch(methodLink, "class=\"need-auth\"", RegexOptions.Singleline | RegexOptions.IgnoreCase);





                    group.Methods.Add(new MethodInfo(this)
                                          {
                                              RequiresAuthentication = requiresAuthentication,
                                              Name = methodName,
                                              // Description = methodDescription, need to use HtmlAgilityPack to parse methods so we can get innnertext
                                              Url = methodUrl,
                                              UriTemplate = methodUriTemplate
                                          });
                    string methodUriTemplateReplace = methodUriTemplate.Replace("{id}", "{ids}");
                    if (methodUriTemplateReplace.StartsWith("/users/{ids}"))
                    {
                        //>/me/
                        string meTemplate = "/me" +
                            methodUriTemplateReplace.Substring(methodUriTemplateReplace.IndexOf("/users/{ids}") + 12);
                        string meName = MungeMethodName(meTemplate);
                        group.Methods.Add(new MethodInfo(this)
                                              {
                                                  RequiresAuthentication = true,
                                                  Description = methodDescription,
                                                  Url = methodUrl,
                                                  UriTemplate = meTemplate,
                                                  Name = meName
                                              });
                    }
                }
            }


        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            sb.AppendLine("                METHODS");
            sb.AppendLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");

            foreach (MethodGroup group in MethodGroups)
            {
                sb.AppendLine("\n=====================================================================\n");
                sb.AppendLine((group.IsNetworkWide ? "[global]" : "[site]") + group.GroupTitle);
                sb.AppendLine("\t----------------------------------------------");
                foreach (MethodInfo method in group.Methods)
                {
                    string paramList = string.Join(", ", method.Parameters.Select(p => "[" + p.Type + "] " + p.Name).ToArray());
                    sb.AppendLine("\t" + method.UriTemplate + "(" + paramList + ")\t" + method.ReturnType + (method.ReturnTypeSingular ? "" : "[]") + " \t" + (method.RequiresAuthentication ? "[authenticated]" : ""));
                }
            }

            sb.AppendLine("\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            sb.AppendLine("                TYPES");
            sb.AppendLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
            foreach (var type in Types)
            {
                sb.AppendLine(type.Name + "\t" + type.Url + (type.Inferred ? " [inferred]" : ""));

                foreach (var field in type.Fields)
                {
                    sb.AppendLine("\t\t" + string.Format("IsPrimitive:{0}, IsArray:{1}, IsEnum:{2}", field.IsPrimitive, field.IsArray, field.IsEnum) + " " + field.Name + "\t" + field.Type);
                }
            }
            return sb.ToString();
        }
        private void ValidateReturnType(MethodInfo method)
        {
            var returnType = method.ReturnType;
            if (returnType == null)
            {
                if (method.Name != "errors_by_id")
                {
                    throw new Exception(string.Format("return type null {0}", method.Name));
                }


            }
            else
            {
                var type = this.Types.FirstOrDefault(t => t.Name == returnType);
                if (type == null)
                {
                    throw new Exception(string.Format("return type not found {0} {1}", method.Name, returnType));
                }
            }
        }
        private void ValidateParameterTypes(MethodInfo method)
        {
            // validate parameter types
            foreach (var p in method.Parameters)
            {
                string pType = p.Type;
                switch (pType)
                {
                    case "boolean":
                    case "date":
                    case "guid_list":
                    case "number":
                    case "number_list":
                    case "string":
                    case "string_list":
                    case "object":
                        // is primitive
                        break;
                    default:
                        var type = Types.FirstOrDefault(t => t.Name == pType);
                        if (type == null)
                        {
                            throw new Exception(string.Format("parameter type not found {0} {1}", method.Name, pType));
                        }
                        break;
                }
            }
        }
        public void Validate()
        {

            foreach (var group in this.MethodGroups)
            {
                foreach (var method in group.Methods)
                {
                    ValidateReturnType(method);
                    ValidateParameterTypes(method);
                    
                }

                foreach (var type in this.Types)
                {
                    foreach (var field in type.Fields)
                    {
                        string pType = field.Type;
                        switch (pType)
                        {
                            case "boolean":
                            case "date":
                            case "guid_list":
                            case "number":
                            case "number_list":
                            case "string":
                            case "string_list":
                            case "object":
                                break;
                            case "integer": //#TODO: ask why Integer is being used as field type but not parameter type?
                            case "decimal": //#TODO: ask why Decimal is being used as field type but not parameter type?
                                // is primitive
                                //Debug.WriteLine(string.Format("{1} {0}",type.Name,pType));
                                break;
                            default:
                                var type2 = Types.FirstOrDefault(t => t.Name == pType);
                                        if (type2 == null)
                                        {
                                            throw new Exception(string.Format("field type not found {0}.{1} {2}", type.Name, field.Name, pType));
                                        }
                                break;
                        }

                    }
                }
            }
        }
    }
}