namespace CityIndex.JsonClient
{
    /// <summary>
    /// A simple composition for collection storage
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    public class CacheCallBack<TDTO> 
    {
        ///<summary>
        ///</summary>
        public ApiAsyncCallback<TDTO> Callback { get; set; }

        ///<summary>
        ///</summary>
        public object State { get; set; }
    }
}