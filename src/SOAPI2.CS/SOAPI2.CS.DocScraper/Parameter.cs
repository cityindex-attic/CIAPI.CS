using System;

namespace SOAPI2.DocScraper
{
    [Serializable]
    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public bool IsPrimitive { get; set; }
    }
}