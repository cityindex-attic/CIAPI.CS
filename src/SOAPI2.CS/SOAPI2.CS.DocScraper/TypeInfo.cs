using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace SOAPI2.DocScraper
{
    [Serializable]
    public class TypeInfo
    {
        public string GenericType { get; set; }
        public bool IsEnum { get; set; }
        
        public List<FieldInfo> Fields { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        private string _source;
        public bool Inferred{ get; set; }
        public string Name { get; set; }
        public Docs Docs { get; set; }
        public TypeInfo(Docs docs)
        {
            Docs = docs;
            Fields = new List<FieldInfo>();
        }

        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
         
                // get description
                this.Name = this.Type.Replace(" ", "_").Replace("-", "_");
                HtmlDocument sourceDoc = new HtmlDocument();
                sourceDoc.LoadHtml(_source);

                // <div id="discussion">
                var summaryNode = sourceDoc.DocumentNode.SelectSingleNode("//div[@id='discussion']");
                if (summaryNode == null)
                {
                    throw new Exception("could not find summary node for " + Type);
                }
                Description = summaryNode.InnerText.Trim();
                HtmlNode fieldsList;
                

                
                if(this.Name=="response_wrapper")
                {
                    // #EDGE CASE

                    fieldsList = sourceDoc.DocumentNode.SelectSingleNode("//div[@id='discussion']/ul");
                }
                else
                {
                    fieldsList = sourceDoc.DocumentNode.SelectSingleNode("//div[@id='fields']/ul");    
                }
                
                if (fieldsList == null)
                {
                    throw new Exception("could not find fields list for " + Type);
                }

                var fields = fieldsList.SelectNodes("li");
                if (fields == null || fields.Count == 0)
                {
                    throw new Exception("error reading fields list for " + Type);
                }
                foreach (HtmlNode field in fields)
                {
                    var typeList = field.SelectNodes("ul");
                    if (typeList == null || typeList.Count != 1)
                    {
                        throw new Exception("error reading fields list for " + Type + ".[TODO field name]");
                    }

                    HtmlNode typeNode = typeList[0];
                    field.RemoveChild(typeNode);
                    HtmlNode filterNode = field.SelectSingleNode("span");

                    // #TODO: set default filter flag
                    field.RemoveChild(filterNode);

                    var fieldInfo = new FieldInfo();
                    string fieldName = field.InnerText.Trim();
                    fieldInfo.Name = fieldName;
                    Fields.Add(fieldInfo);
                    var typeDescriptors = typeNode.SelectNodes("li");
                    if (typeDescriptors == null)
                    {
                        throw new Exception("error reading type descriptors list for " + Type + ".[TODO field name]");
                    }
                    if (typeDescriptors.Count != 1)
                    {

                        if (typeDescriptors.Count == 2)
                        {
                            if (!typeDescriptors[1].InnerText.Contains("unchanged in unsafe filters"))
                            {
                                throw new Exception("expected unsafe filters");
                            }
                            fieldInfo.UnchangedInUnsafeFilters = true;
                        }

                        

                    }
                    
                    if (!typeDescriptors[0].InnerHtml.Contains("href=\"/docs/types/"))
                    {
                        fieldInfo.IsPrimitive = true;

                    }
                    string fieldType = typeDescriptors[0].InnerText;
                    
                    if (fieldType.Contains("an array of"))
                    {
                        fieldInfo.IsArray = true;
                        fieldType = fieldType.Replace("an array of", "");
                    }
                    if (fieldType.Contains("one of"))
                    {
                        fieldInfo.IsEnum = true;
                        fieldType = fieldType.Replace(", or ", ", ");
                        fieldType = fieldType.Replace("one of", "");

         
                        
                    }
                    //

                    



         

                    if (fieldInfo.IsEnum)
                    {
                        fieldInfo.EnumValues = fieldType;
                        fieldType = Type + " " + fieldName;
                        
                    }


                    
                    //// clean up repeated words ?


                    fieldType = fieldType.Replace("-", " ");
                    fieldType = fieldType.Replace("_", " ");
                    fieldType = fieldType.Trim();

                    Regex doubleWordPattern = new Regex("\\b(?<word>\\w+)\\s+(\\k<word>)\\b");
                    var doubleWordPatternMatch = doubleWordPattern.Match(fieldType);
                    if (doubleWordPatternMatch.Success)
                    {
                        string word = doubleWordPatternMatch.Groups["word"].Value;
                        fieldType = doubleWordPattern.Replace(fieldType, word);
                    }
                    fieldType = fieldType.Replace(" ", "_").Replace("-", "_");
                    fieldInfo.Type = fieldType;
                    if(fieldInfo.IsEnum)
                    {
                        var type = new TypeInfo(Docs);
                        type.Type = fieldType;
                        type.Name = fieldType;
                        type.IsEnum = true;
                        type.Inferred = true;
                        foreach (string item in fieldInfo.EnumValues.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                        {
                            var f = new FieldInfo();
                            f.Type = "string";
                            f.Name = item.Trim();
                            type.Fields.Add(f);
                            
                        }
                        this.Docs.Types.Add(type);
                    }

                    // #EDGE CASE
                    if(fieldType=="the_type_found_in_type")
                    {
                        GenericType = fieldType;
                        fieldInfo.IsPrimitive = false;
                    }
                    
                }

            }
        }
    }
}