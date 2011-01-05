namespace Lightstreamer.DotNet.Client
{
    using System;

    public interface IConnectionListener
    {
        void OnActivityWarning(bool warningOn);
        void OnClose();
        void OnConnectionEstablished();
        void OnDataError(PushServerException e);
        void OnFailure(PushConnException e);
        void OnFailure(PushServerException e);
        void OnNewBytes(long bytes);
        void OnSessionStarted();
    }
}

