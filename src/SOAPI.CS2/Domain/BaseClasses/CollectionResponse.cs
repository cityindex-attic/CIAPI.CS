using System.Collections.Generic;

namespace SOAPI.CS2.Domain
{
    public abstract class CollectionResponse<TDTO> where TDTO : class,new()
    {
        public abstract List<TDTO> Items { get; set; }
    }
}