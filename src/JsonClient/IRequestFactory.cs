using System.Net;

namespace CityIndex.JsonClient
{
    public interface IRequestFactory
    {
        WebRequest Create(string uri);
    }
}