using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIAPI.Core
{
    public delegate void ApiAsyncCallback<T>(ApiAsyncResult<T> ar) where T : class,new();
}
