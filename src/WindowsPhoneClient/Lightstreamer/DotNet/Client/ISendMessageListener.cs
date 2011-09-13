namespace Lightstreamer.DotNet.Client
{
    using System;

    public interface ISendMessageListener
    {
        void OnAbort(MessageInfo originalMessage, int prog, Exception problem);
        void OnError(int code, string error, MessageInfo originalMessage, int prog);
        void OnProcessed(MessageInfo originalMessage, int prog);
    }
}

