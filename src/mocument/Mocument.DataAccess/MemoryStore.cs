using System;
using System.Collections.Generic;
using System.Linq;
using Mocument.Model;
using Newtonsoft.Json;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess
{
    public class MemoryStore : IStore
    {
        private readonly object _lockObject;
        private List<Tape> _list;

        public MemoryStore()
        {
            _lockObject = new object();

            _list = new List<Tape>();
        }

        #region IStore Members

        public void Dispose()
        {
            //noop
        }

        public void ClearDatabase()
        {
            lock (_lockObject)
            {
                _list.Clear();
            }
        }

        public void EnsureDatabase()
        {
            //nooop
        }

        public void Delete(string id)
        {
            lock (_lockObject)
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException("id");
                }
                Tape existing = _list.FirstOrDefault(t => t.Id == id);
                if (existing == null)
                {
                    throw new Exception("cannot find key");
                }
                _list.Remove(existing);
            }
        }

        public void Update(Tape tape)
        {
            lock (_lockObject)
            {
                if (string.IsNullOrEmpty(tape.Id))
                {
// ReSharper disable NotResolvedInText
                    throw new ArgumentNullException("id");
// ReSharper restore NotResolvedInText
                }
                Tape existing = _list.FirstOrDefault(t => t.Id == tape.Id);
                if (existing == null)
                {
                    throw new Exception("cannot find key");
                }
                _list.Remove(existing);
                // hack in lieu of complicated cloning
                _list.Add(JsonConvert.DeserializeObject<Tape>(JsonConvert.SerializeObject(tape, Formatting.Indented)));
            }
        }

        public void Insert(Tape tape)
        {
            lock (_lockObject)
            {
                if (string.IsNullOrEmpty(tape.Id))
                {
// ReSharper disable NotResolvedInText
                    throw new ArgumentNullException("id");
// ReSharper restore NotResolvedInText
                }
                if (Select(tape.Id) != null)
                {
                    throw new Exception("cannot insert duplicate key");
                }
                // hack in lieu of complicated cloning
                _list.Add(JsonConvert.DeserializeObject<Tape>(JsonConvert.SerializeObject(tape, Formatting.Indented)));
            }
        }

        public Tape Select(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            lock (_lockObject)
            {
                return List().FirstOrDefault(t => t.Id == id);
            }
        }

        public List<Tape> List()
        {
            lock (_lockObject)
            {
                // we want to return cloned tapes, not references to those in list.
                // so short of writing clone logic, just roundtrip the list through json serialization
                return JsonConvert.DeserializeObject<List<Tape>>(JsonConvert.SerializeObject(_list, Formatting.Indented));
            }
        }

        public List<Tape> List(Func<Tape, bool> selector)
        {
            lock (_lockObject)
            {
                return List().Where(selector).ToList();
            }
        }

        public Entry MatchEntry(string tapeId, Entry entryToMatch, IEntryComparer[] comparers = null)
        {
            lock (_lockObject)
            {
                Tape tape = Select(tapeId);

                // provide a default comparer
                if (comparers == null || comparers.Length == 0)
                {
                    comparers = new IEntryComparer[] {new DefaultEntryComparer()};
                }

                List<Entry> potentialMatches = tape.log.entries;
                return (
                           from entryComparer in comparers
                           select entryComparer.FindMatch(potentialMatches, entryToMatch)
                           into result
                           where result.Match != null
                           select result.Match)
                    .FirstOrDefault();
            }
        }

        public void FromJson(string json)
        {
            lock (_lockObject)
            {
                _list = JsonConvert.DeserializeObject<List<Tape>>(json);
                if (_list == null)
                {
                    throw new Exception("invalid json");
                }
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_list, Formatting.Indented);
        }

        #endregion
    }
}