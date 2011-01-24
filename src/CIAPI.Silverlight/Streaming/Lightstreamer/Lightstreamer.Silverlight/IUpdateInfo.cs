namespace Lightstreamer.DotNet.Client
{
    using System;

    public interface IUpdateInfo
    {
        [Obsolete("Use the ItemName property instead of the GetItemName method.")]
        string GetItemName();
        [Obsolete("Use the ItemPos property instead of the GetItemPos method.")]
        int GetItemPos();
        string GetNewValue(int fieldPos);
        string GetNewValue(string fieldName);
        [Obsolete("Use the NumFields property instead of the GetNumFields method.")]
        int GetNumFields();
        string GetOldValue(int fieldPos);
        string GetOldValue(string fieldName);
        [Obsolete("Use the Snapshot property instead of the IsSnapshot method.")]
        bool IsSnapshot();
        bool IsValueChanged(int fieldPos);
        bool IsValueChanged(string fieldName);

        string ItemName { get; }

        int ItemPos { get; }

        int NumFields { get; }

        bool Snapshot { get; }
    }
}

