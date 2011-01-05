namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections;

    [Obsolete("The use of this listener is deprecated in favor of the IHandyTableListener and the subscribe methods based on it")]
    public interface IExtendedTableListener
    {
        void OnRawUpdatesLost(string item, int lostUpdates);
        void OnSnapshotEnd(string item);
        void OnUnsubscr(string item);
        void OnUnsubscrAll();
        void OnUpdate(string item, IDictionary fields);
    }
}

