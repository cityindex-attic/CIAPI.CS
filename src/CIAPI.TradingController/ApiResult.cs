using System;

namespace CIAPI.TradingController
{
    /// <summary>
    /// The result of an async call. If the 'exception' property is not null
    /// you may infer that it is the exception that was thrown by the returning function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
}