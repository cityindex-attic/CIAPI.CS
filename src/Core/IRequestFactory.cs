using System.Net;

namespace CIAPI.Core
{
    public interface IRequestFactory
    {
        WebRequest Create(string uri);
    }
}