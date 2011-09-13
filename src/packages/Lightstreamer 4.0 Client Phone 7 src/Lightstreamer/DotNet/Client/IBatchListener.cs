namespace Lightstreamer.DotNet.Client
{
    using System;

    internal interface IBatchListener
    {
        void OnMessageBatched();
    }
}

