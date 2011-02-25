using System;

namespace CIAPI.CS.Koans.KoanRunner
{
    public class KoanAttribute : Attribute
    {
        public KoanAttribute()
        {
            Ignore = false;
        }
        
        public bool Ignore { get; set; }
        public double Order { get; set; }
    }
}