namespace CityIndex.JsonClient
{
    /// <summary>
    /// Delegate used to complete async calls.
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    /// <param name="asyncResult"></param>
    public delegate void ApiAsyncCallback<TDTO>(ApiAsyncResult<TDTO> asyncResult) ;
}