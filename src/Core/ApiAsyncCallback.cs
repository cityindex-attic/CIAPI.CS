using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIAPI.Core
{
    public delegate void ApiAsyncCallback<TDTO>(ApiAsyncResult<TDTO> ar) where TDTO : class,new();
}
