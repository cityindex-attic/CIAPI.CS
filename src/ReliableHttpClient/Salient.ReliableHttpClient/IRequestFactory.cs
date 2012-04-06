using System;
using System.Net;

namespace Salient.ReliableHttpClient
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

        /// <summary>
        /// The amount of time to wait for a response before throwing an exception
        /// </summary>
        TimeSpan RequestTimeout { get; set; }
    }
}