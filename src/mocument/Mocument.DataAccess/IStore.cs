using System;
using System.Collections.Generic;
using Mocument.Model;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess
{
    public interface IStore : IDisposable
    {
        void ClearDatabase();
        void EnsureDatabase();
        void Delete(string id);
        void Update(Tape tape);
        void Insert(Tape tape);
        Tape Select(string id);
        List<Tape> List();
        List<Tape> List(Func<Tape, bool> selector);
        Entry MatchEntry(string tapeId, Entry entryToMatch, IEntryComparer[] comparers = null);
        void FromJson(string json);
        string ToJson();
    }
}