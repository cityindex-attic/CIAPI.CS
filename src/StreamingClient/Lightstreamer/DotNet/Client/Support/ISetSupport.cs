namespace Lightstreamer.DotNet.Client.Support
{
    using System;
    using System.Collections;

    public interface ISetSupport : IList, ICollection, IEnumerable
    {
        bool Add(object obj);
        bool AddAll(ICollection c);
    }
}

