using System;

namespace SOAPI2.DocScraper
{
    [Serializable]
    public class FieldInfo
    {
        
        public string EnumValues { get; set; }
        public bool IsArray { get; set; }
        public bool IsEnum { get; set; }
        public bool IsPrimitive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IncludedInDefaultFilter { get; set; }
        public bool UnchangedInUnsafeFilters { get; set; }

        public String Type { get; set; }
    }
}