namespace Lightstreamer.DotNet.Client
{
    using System;

    public interface IUpdateInfo
    {
        string GetNewValue(int fieldPos);
        string GetNewValue(string fieldName);
        string GetOldValue(int fieldPos);
        string GetOldValue(string fieldName);
        bool IsValueChanged(int fieldPos);
        bool IsValueChanged(string fieldName);

        string ItemName { get; }

        int ItemPos { get; }

        int NumFields { get; }

        bool Snapshot { get; }
    }
}

