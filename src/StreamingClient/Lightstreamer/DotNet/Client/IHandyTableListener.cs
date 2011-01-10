namespace Lightstreamer.DotNet.Client
{
    using System;

    public interface IHandyTableListener
    {
        void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates);
        void OnSnapshotEnd(int itemPos, string itemName);
        void OnUnsubscr(int itemPos, string itemName);
        void OnUnsubscrAll();
        void OnUpdate(int itemPos, string itemName, UpdateInfo update);
    }
}

