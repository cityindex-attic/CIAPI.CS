using System;
using System.Collections.Generic;

namespace SOAPI2.DocScraper
{
    [Serializable]
    public class MethodGroup
    {
        public bool IsNetworkWide { get; set; }
        public string GroupTitle { get; set; }
        public string GroupName { get; set; }
        public List<MethodInfo> Methods { get; set; }
    }
}