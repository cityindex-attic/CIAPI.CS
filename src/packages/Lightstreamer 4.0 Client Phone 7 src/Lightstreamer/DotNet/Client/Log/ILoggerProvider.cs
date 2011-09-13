namespace Lightstreamer.DotNet.Client.Log
{
    using System;

    public interface ILoggerProvider
    {
        ILogger GetLogger(string category);
    }
}

