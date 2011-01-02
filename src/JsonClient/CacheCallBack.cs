namespace CityIndex.JsonClient
{
    /// <summary>
    /// A simple composition for collection storage
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    public class CacheCallBack<TDTO> where TDTO : class, new()
    {
        public ApiAsyncCallback<TDTO> Callback { get; set; }

        public object State { get; set; }
    }
}