using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SOAPI2.DocScraper
{
    [Serializable]
    public class MethodInfo
    {
        public List<Parameter> Parameters { get; set; }
        
        public string ScriptMethodUri { get; set; }
        public string ScriptMethodName { get; set; }
        public string ScriptFilter { get; set; }
        

        public bool RequiresAuthentication { get; set; }
        public List<string> RequiredScopes { get; set; }
        public string UriTemplate { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        private string _source;
        public Docs Docs { get; set; }
        public MethodInfo(Docs docs)
        {
            Docs = docs;
            Parameters = new List<Parameter>();
            RequiredScopes = new List<string>();
        }

        private void SetReturnType()
        {
            Match returnMatch = Regex.Match(Source, " <p>This method returns(?<return>.*?)\\.</p>",
                                                        RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            if (!returnMatch.Success)
            {
                throw new Exception("could not find method return type " + UriTemplate);
            }
            bool isSingular = false;

            string returnType = returnMatch.Groups["return"].Value.Trim();
            if (returnType.StartsWith("a list of <"))
            {
            }
            else if (returnType.StartsWith("a single <"))
            {
                isSingular = true;
            }
            else if (returnType.StartsWith("an <"))
            {
                isSingular = true;
            }
            else
            {
                throw new Exception("do not understand return type " + UriTemplate);
            }

            Match returnTypeMatch = Regex.Match(returnType, "<a href=\"/docs/types/(?<type>.*?)\">",
                                                RegexOptions.ExplicitCapture | RegexOptions.Singleline |
                                                RegexOptions.IgnoreCase);
            if (!returnTypeMatch.Success)
            {
                throw new Exception("could not parse return type url " + UriTemplate);
            }
            string returnTypeName = returnTypeMatch.Groups["type"].Value.Replace(" ", "_").Replace("-", "_").Trim();
            ReturnTypeSingular = isSingular;
            ReturnType = returnTypeName;
        }

        private void ParseScript()
        {
            Match scriptMatch = Regex.Match(Source, "<script type=\"text/javascript\">\\s*(?<script>var\\s+parameters\\s+=.*?)</script>", RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            if (!scriptMatch.Success)
            {
                throw new Exception("could not find script in method source " + UriTemplate);
            }
            string script = scriptMatch.Groups["script"].Value;
            Script = script;

            var methodNameMatch = Regex.Match(script, "var method = \"(?<uri>/2.0/(?<name>.*?))\";", RegexOptions.ExplicitCapture | RegexOptions.Singleline |
                                                          RegexOptions.IgnoreCase);

            if (!methodNameMatch.Success)
            {
                throw new Exception("could not find script methodname  " + UriTemplate);
            }

            ScriptMethodUri = methodNameMatch.Groups["uri"].Value;

            ScriptMethodName = methodNameMatch.Groups["name"].Value;


            var filterNameMatch = Regex.Match(script, "var filterName = \"(?<filter>.*?)\";", RegexOptions.ExplicitCapture | RegexOptions.Singleline |
                                              RegexOptions.IgnoreCase);

            if (!filterNameMatch.Success)
            {
                throw new Exception("could not find script filterName " + UriTemplate);
            }
            ScriptFilter = filterNameMatch.Groups["filter"].Value;


            var dependantTypesMatch = Regex.Match(script, "var dependentTypes = (?<types>\\[.*?\\]);", RegexOptions.ExplicitCapture | RegexOptions.Singleline |
                                              RegexOptions.IgnoreCase);

            if (!dependantTypesMatch.Success)
            {
                throw new Exception("could not find script dependant types " + UriTemplate);
            }

            
            // JArray scriptDependantTypes = JArray.Parse(dependantTypesMatch.Groups["types"].Value);


            var paramMatch = Regex.Match(script, "var parameters = (?<params>{.*?});",
                                         RegexOptions.ExplicitCapture | RegexOptions.Singleline |
                                         RegexOptions.IgnoreCase);




            if (!paramMatch.Success)
            {
                throw new Exception("could not find script params " + UriTemplate);
            }
            string paramJson = paramMatch.Groups["params"].Value;
            var paramsObj = JObject.Parse(paramJson);


            // determine parameter name and type.


            foreach (var prop in paramsObj)
            {
                bool addParam = true;
                var paramName = prop.Key;



                var parameter = new Parameter
                                    {
                                        Name = paramName,
                                        IsPrimitive = true
                                    };

                TypeInfo pType = null;
                switch (prop.Value.Type)
                {

                    case JTokenType.Array:
                        // indicates a simple 'enum' type.
                        // these should be same for every method that calls them

                        switch (paramName)
                        {
                            case "order":
                                parameter.IsPrimitive = false;
                                parameter.Type = "order";
                                // verify that the shape has not change
                                if (prop.Value.Children().Count() != 2)
                                {
                                    throw new Exception("badly formed 'order' enum");
                                }

                                if (prop.Value.Children().Where(c => c.Value<string>() == "asc").Count() != 1)
                                {
                                    throw new Exception("badly formed 'order' enum");
                                }
                                if (prop.Value.Children().Where(c => c.Value<string>() == "desc").Count() != 1)
                                {
                                    throw new Exception("badly formed 'order' enum");
                                }
                                
                                pType = Docs.Types.FirstOrDefault(t => t.Type == "order");
                                if (pType == null)
                                {
                                    pType = new TypeInfo(Docs);
                                    pType.Type = "order";
                                    pType.IsEnum = true;
                                    pType.Inferred = true;
                                    pType.Name = pType.Type.Replace(" ", "_").Replace("-", "_").Trim();
                                    foreach (JToken item in prop.Value.Children())
                                    {
                                        var field = new FieldInfo();
                                        field.Type = "string";
                                        field.Name = item.Value<string>();
                                        pType.Fields.Add(field);
                                    }
                                    this.Docs.Types.Add(pType);
                                }

                                break;
                            case "period":
                                parameter.IsPrimitive = false;
                                parameter.Type = "period";
                                // verify that the shape has not change
                                if (prop.Value.Children().Count() != 2)
                                {
                                    throw new Exception("badly formed 'period' enum");
                                }

                                if (prop.Value.Children().Where(c => c.Value<string>() == "all_time").Count() != 1)
                                {
                                    throw new Exception("badly formed 'period' enum");
                                }
                                if (prop.Value.Children().Where(c => c.Value<string>() == "month").Count() != 1)
                                {
                                    throw new Exception("badly formed 'period' enum");
                                }
                                pType = Docs.Types.FirstOrDefault(t => t.Type == "period");
                                if (pType == null)
                                {
                                    pType = new TypeInfo(Docs);
                                    pType.Type = "period";
                                    pType.IsEnum = true;
                                    pType.Inferred = true;
                                    string typeType = pType.Type;
                                    pType.Name = typeType.Replace(" ", "_").Replace("-", "_").Trim();
                                    foreach (JToken item in prop.Value.Children())
                                    {
                                        var field = new FieldInfo();
                                        field.Type = "string";
                                        field.Name = item.Value<string>();
                                        pType.Fields.Add(field);
                                    }
                                    this.Docs.Types.Add(pType);
                                }
                                break;
                            case "unsafe":
                                parameter.Type = "boolean";
                                // verify that the shape has not change
                                if (prop.Value.Children().Count() != 2)
                                {
                                    throw new Exception("badly formed 'unsafe' enum");
                                }

                                if (prop.Value.Children().Where(c => c.Value<string>() == "false").Count() != 1)
                                {
                                    throw new Exception("badly formed 'unsafe' enum");
                                }
                                if (prop.Value.Children().Where(c => c.Value<string>() == "true").Count() != 1)
                                {
                                    throw new Exception("badly formed 'unsafe' enum");
                                }
                                break;
                            default:
                                throw new Exception("unexpected array: " + paramName);
                        }

                        break;
                    case JTokenType.Object:
                        switch (paramName)
                        {

                            case "sort":
                                parameter.IsPrimitive = false;
                                parameter.Type = "sort_" + this.Name;

                                // this is an enum type with added meta to help inform use of min/max.

                                // this enum can and will be different for every method so we can either try to identify
                                // like instances and provide a name for it or simply name it after the method, e.g. AnswersSort
                                // i like this but some of the longer method names will result in unwieldy type names but oh well.
                                // e.g. GetUsersByIdAssociatedSort. Maybe reverse the composition to SortGetUsersByIdAssociated. 
                                // this will make autocomplete and intellisense work better as well as grouping the sort enums.


                                // this will also help us determine what type of values are
                                // appropriate for min/max);
                                pType = this.Docs.Types.FirstOrDefault(t => t.Type == parameter.Type);
                                if (pType == null)
                                {
                                    pType = new TypeInfo(Docs);
                                    pType.Type = parameter.Type;
                                    pType.Name = parameter.Type;
                                    pType.IsEnum = true;
                                    pType.Inferred = true;

                                    foreach (JProperty item in prop.Value.Children())
                                    {
                                        var field = new FieldInfo();
                                        field.Type = "string";
                                        field.Name = item.Name;
                                        field.Description = "min/max are " + ((JValue)item.Value).Value;
                                        pType.Fields.Add(field);
                                    }
                                    this.Docs.Types.Add(pType);
                                }
                                break;
                            default:
                                throw new Exception("unexpected object: " + paramName);
                        }
                        break;
                    case JTokenType.String:
                        // simple

                        string propType = prop.Value.Value<string>();
                        switch (propType)
                        {
                            case "access_token":
                                RequiresAuthentication = true;
                                addParam = false;
                                break;
                            case "read_inbox":
                                // this is part of access token scope
                                RequiredScopes.Add(parameter.Type);
                                addParam = false;
                                break;
                            case "depends":
                                parameter.Type = "string";
                                parameter.Format = "sort-dependant";
                                break;
                            case "date":

                                parameter.Type = "number";
                                parameter.Format = "utc-millisec";

                                break;
                            case "guid list":
                                parameter.Type = "string";
                                parameter.Format = "guid-list";
                                break;
                            case "number":
                                parameter.Type = "number";
                                break;
                            case "number list":
                                parameter.Type = "string";
                                parameter.Format = "number-list";
                                break;
                            case "string":
                                parameter.Type = "string";
                                break;
                            case "string list":
                                parameter.Type = "string";
                                parameter.Format = "string-list";
                                break;
                            default:
                                throw new Exception("unexpected property type");
                        }

                        break;
                    default:
                        throw new Exception("unexpected property type");
                }
                if (addParam)
                {
                    Parameters.Add(parameter);
                }


            }

        }
        private void SetDescription()
        {
            var hdoc = new HtmlDocument();
            hdoc.LoadHtml(_source);
            var discussionNode = hdoc.DocumentNode.SelectSingleNode("//div[@class='type-discussion']");
            if (discussionNode == null)
            {
                throw new Exception("could not find summary in method source " + UriTemplate);
            }


            //<span class="need-auth" title="this method requires an access_token">authentication required</span>
            Description = discussionNode.InnerText;

        }
        private void ParseSource()
        {

            //    
            SetDescription();

            ParseScript();


            if (Name == "errors_by_id")
            {
                // TODO: edge case ErrorsById
                return;
            }
            else
            {
                SetReturnType();
            }






        }
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;

                ParseSource();

            }
        }

        public string ReturnType { get; set; }
        public bool ReturnTypeSingular { get; set; }
        public string Script { get; set; }
    }
}