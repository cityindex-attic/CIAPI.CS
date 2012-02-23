using System;
using Lightstreamer.DotNet.Client;

public interface ILightstreamerListener
{
    // Lightstreamer Client connection status changes arrive here
    void OnStatusChange(int phase, int status, string message);
    // Lightstreamer Client data updates arrive here
    void OnItemUpdate(int phase, int itemPos, string itemName, IUpdateInfo update);
    // Lightstreamer Client lost updates info arrives here
    void OnLostUpdate(int phase, int itemPos, string itemName, int lostUpdates);
    // Connection Listener is requesting a reconnection
    void OnReconnectRequest(int phase);
}


