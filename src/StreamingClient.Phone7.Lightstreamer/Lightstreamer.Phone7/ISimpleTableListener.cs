﻿namespace Lightstreamer.DotNet.Client
{
    using System;

    [Obsolete("The use of this listener is deprecated in favor of the IHandyTableListener and the subscribe methods based on it.")]
    public interface ISimpleTableListener
    {
        void OnRawUpdatesLost(int item, int lostUpdates);
        void OnSnapshotEnd(int item);
        void OnUnsubscrAll();
        void OnUpdate(int item, string[] values);
    }
}

