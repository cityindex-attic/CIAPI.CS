namespace Lightstreamer.DotNet.Client
{
    using System;

    internal interface ITableManager
    {
        void DoUpdate(Lightstreamer.DotNet.Client.ServerUpdateEvent values);
        void NotifyUnsub();

        string DataAdapter { get; }

        int DistinctSnapshotLength { get; }

        int End { get; }

        string Group { get; }

        int MaxBufferSize { get; }

        double MaxFrequency { get; }

        string Mode { get; }

        string Schema { get; }

        string Selector { get; }

        bool Snapshot { get; }

        int Start { get; }

        bool Unfiltered { get; }
    }
}

