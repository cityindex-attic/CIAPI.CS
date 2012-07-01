using System;

namespace Salient.ReliableHttpClient
{
    /// <summary>
    /// throw this exception when something has obviously gone wrong with the
    /// program logic.
    /// </summary>
    [Serializable]
    public class DefectException : Exception
    {
        public DefectException(string message)
            : base(message)
        {

        }
        public DefectException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}