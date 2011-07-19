using System.Collections.Generic;

namespace SOAPI.CS2.Domain
{
    public abstract class CollectionResponse<TDTO> 
    {
        public abstract List<TDTO> Items { get; set; }
    }
}