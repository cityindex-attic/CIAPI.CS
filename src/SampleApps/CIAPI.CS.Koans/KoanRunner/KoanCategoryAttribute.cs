using System;

namespace CIAPI.CS.Koans.KoanRunner
{
    public class KoanCategoryAttribute : Attribute
    {
        public KoanCategoryAttribute()
        {
            Ignore = false;
        }

        public bool Ignore { get; set; }
    }
}