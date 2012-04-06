namespace Salient.ReliableHttpClient
{
    public enum RequestMethod
    {
        /// <summary>
        /// safe, idempotent: can cache, can retry
        /// </summary>
        GET,

        /// <summary>
        /// safe, idempotent: can cache, can retry
        /// </summary>
        HEAD,

        /// <summary>
        /// not safe, idempotent: do not cache, can retry
        /// </summary>
        PUT,

        /// <summary>
        /// not safe, idempotent: do not cache, can retry
        /// </summary>
        DELETE,

        /// <summary>
        /// not safe, not idempotent: do not cache, do not retry
        /// </summary>        
        POST
    }
}