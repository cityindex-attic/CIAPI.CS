using System.Collections.Generic;

namespace JsonClient.CodeGeneration
{
    public class SchemaItem
    {

        public SchemaItemType ItemType { get; set; }


        public string Id { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public bool Required { get; set; }
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }
        public bool ExclusiveMinimum { get; set; }
        public bool ExclusiveMaximum { get; set; }
        public int MinItems { get; set; }
        public int MaxItems { get; set; }
        public bool UniqueItems { get; set; }
        public string Pattern { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public decimal DivisibleBy { get; set; }
        // disallow
        
        
        
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public List<SchemaItem> Items { get; set; }

        public List<SchemaItem> Enum { get; set; }

        public SchemaItem Default { get; set; }


        public SchemaItem Extends { get; set; }


        public Dictionary<string, SchemaItem> Properties { get; set; }

    }
}