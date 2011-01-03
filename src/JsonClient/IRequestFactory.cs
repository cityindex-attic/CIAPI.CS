using System.Net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Abstract interface for WebRequest factory
    /// </summary>
    public interface IRequestFactory
    {
        ///<summary>
        /// Returns a <see cref="WebRequest"/> for <paramref name="uri"/>
        ///</summary>
        ///<param name="uri"></param>
        ///<returns></returns>
        WebRequest Create(string uri);
    }
}