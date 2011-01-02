using System.Net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Abstract interface for WebRequest factory
    /// </summary>
    public interface IRequestFactory
    {
        WebRequest Create(string uri);
    }
}