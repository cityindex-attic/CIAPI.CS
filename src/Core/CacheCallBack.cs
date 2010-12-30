namespace CIAPI.Core
{
    public class CacheCallBack<TDTO> where TDTO : class, new()
    {
        public ApiAsyncCallback<TDTO> Callback { get; set; }

        public object State { get; set; }
    }
}