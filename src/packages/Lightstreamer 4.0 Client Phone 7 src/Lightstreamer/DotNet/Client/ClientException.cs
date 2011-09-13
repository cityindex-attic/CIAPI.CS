namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ClientException : Exception
    {
        internal ClientException(string message) : base(message)
        {
        }

        internal ClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

