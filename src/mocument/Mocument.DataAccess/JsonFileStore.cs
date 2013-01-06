using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mocument.Model;
using Newtonsoft.Json;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess
{
    /// <summary>
    /// for controlled testing only. not threadsafe and is filebased so extreme load is not advised.
    /// ABSOLUTELY NOT SUITABLE FOR MULTIPROCESS ACCESS
    /// </summary>
    public class JsonFileStore : IStore
    {
        private readonly bool _deleteFileOnDispose;
        private readonly string _filepath;
        private readonly object _lockObject;
        private bool _disposed;
        private List<Tape> _list;
 
        public JsonFileStore()
        {
 

            _lockObject = new object();

            _filepath = Path.GetTempFileName();
            _deleteFileOnDispose = true;
            EnsureDatabase();
        }

        public JsonFileStore(string filepath)
        {
            _lockObject = new object();

            _filepath = filepath;
            EnsureDatabase();
        }

        #region IStore Members

        public void ClearDatabase()
        {
            lock (_lockObject)
            {
                _list.Clear();
                WriteJson();
            }
        }

        public void EnsureDatabase()
        {
            lock (_lockObject)
            {
                try
                {
                    ReadJson();
                }
                catch (Exception)
                {
                    _list = new List<Tape>();
                    WriteJson();
                }
            }
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

                WriteJson();
            }
        }

        public void Update(Tape tape)
        {
            lock (_lockObject)
            {
                if (string.IsNullOrEmpty(tape.Id))
                {
                    throw new ArgumentNullException("id");
                }
                Tape existing = _list.FirstOrDefault(t => t.Id == tape.Id);
                if (existing == null)
                {
                    throw new Exception("cannot find key");
                }
                _list.Remove(existing);
                // hack in lieu of complicated cloning
                _list.Add(JsonConvert.DeserializeObject<Tape>(JsonConvert.SerializeObject(tape, Formatting.Indented, GetJsonSerializerSettings()), GetJsonSerializerSettings()));

                WriteJson();
            }
        }

        public void Insert(Tape tape)
        {
            lock (_lockObject)
            {
                if (string.IsNullOrEmpty(tape.Id))
                {
                    throw new ArgumentNullException("id");
                }
                if (Select(tape.Id) != null)
                {
                    throw new Exception("cannot insert duplicate key");
                }
                // hack in lieu of complicated cloning
                _list.Add(JsonConvert.DeserializeObject<Tape>(JsonConvert.SerializeObject(tape, Formatting.Indented, GetJsonSerializerSettings()), GetJsonSerializerSettings()));
                WriteJson();
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
                return JsonConvert.DeserializeObject<List<Tape>>(JsonConvert.SerializeObject(_list, Formatting.Indented, GetJsonSerializerSettings()), GetJsonSerializerSettings());
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
                    comparers = new IEntryComparer[] { new DefaultEntryComparer() };
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
                _list = JsonConvert.DeserializeObject<List<Tape>>(json, GetJsonSerializerSettings());
                if (_list == null)
                {
                    throw new Exception("invalid json");
                }
                WriteJson();
            }
        }

        public string ToJson()
        {
            string json = JsonConvert.SerializeObject(_list, Formatting.Indented,GetJsonSerializerSettings());
            return json;
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
                       {
                           NullValueHandling = NullValueHandling.Ignore
                       };
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            if (_deleteFileOnDispose)
            {
                File.Delete(_filepath);
            }
            else
            {
                WriteJson();
            }
        }

        #endregion

        private void ReadJson()
        {
            lock (_lockObject)
            {
                FromJson(File.ReadAllText(_filepath));
            }
        }

        private void WriteJson()
        {
            lock (_lockObject)
            {
                File.WriteAllText(_filepath, ToJson());
            }
        }
    }
}