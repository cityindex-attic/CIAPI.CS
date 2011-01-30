namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Collections;

    internal interface ISetSupport : IList, ICollection, IEnumerable
    {
        bool Add(object obj);
        bool AddAll(ICollection c);
    }
}

