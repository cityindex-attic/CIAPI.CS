using System;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess
{
    [Serializable]
    public class EntryCompareResult
    {
        public Entry Match { get; set; }
    }
}