using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
 
using Mocument.Model;
using Newtonsoft.Json;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess.SQLite
{
    public class SQLiteStore : IStore
    {
        private readonly ConnectionStringSettings _connectionStringSettings;

        public SQLiteStore(string connectionStringName)
        {
            _connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            EnsureDatabase();
        }

        private DbConnection CreateConnection()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName);
            DbConnection connection = factory.CreateConnection();

            if (connection == null)
            {
                throw new DataException("Could not create database connection");
            }


            connection.ConnectionString = _connectionStringSettings.ConnectionString;

            return connection;

        }

        public void ClearDatabase()
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "DELETE FROM Tapes";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EnsureDatabase()
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "select Id from Tapes where id=''";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        //
                        command.CommandText =
                            "CREATE TABLE \"Tapes\" (Id TEXT PRIMARY KEY NOT NULL UNIQUE,Description TEXT,Comment TEXT,OpenForRecording BOOL, AllowedIpAddress TEXT,JSON TEXT, Mode TEXT, Position INT)";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void SetParameter(DbCommand command, string paramName, object paramValue)
        {
            DbParameter param = command.CreateParameter();
            param.DbType = DbType.String;
            param.Direction = ParameterDirection.Input;
            param.ParameterName = paramName;
            param.Value = paramValue;
            command.Parameters.Add(param);
        }


        public void Delete(string id)
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "DELETE FROM Tapes where Id=@Id";
                    SetParameter(command, "Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Tape tape)
        {
            // there is a bug in the web admin that i am having trouble finding
            // so some hacks are needed here

            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    if (tape.log == null)
                    {
                        // updating only meta from webui - 
                        var t = Select(tape.Id);
                        t.AllowedIpAddress = tape.AllowedIpAddress;
                        t.Comment = tape.Comment;
                        t.Description = tape.Description;
                        t.OpenForRecording = tape.OpenForRecording;
                        tape = t;
                    }

                    command.CommandText =
                "UPDATE TAPES SET  Description=@Description,Comment=@Comment,OpenForRecording=@OpenForRecording,AllowedIpAddress=@AllowedIpAddress,JSON=@JSON,Mode=@Mode,Position=@Position  WHERE Id=@Id";

                    SetParameter(command, "Description", tape.Description);
                    SetParameter(command, "Comment", tape.Comment);
                    SetParameter(command, "OpenForRecording", tape.OpenForRecording);
                    SetParameter(command, "AllowedIpAddress", tape.AllowedIpAddress);
                    SetParameter(command, "JSON", JsonConvert.SerializeObject(tape, Formatting.Indented));
                    SetParameter(command, "Mode", tape.Mode);
                    SetParameter(command, "Position", tape.Position);


                    SetParameter(command, "Id", tape.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Insert(Tape tape)
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        "insert into tapes (Id,Description,Comment,OpenForRecording,AllowedIpAddress,JSON,Mode,Position) values (@Id,@Description,@Comment,@OpenForRecording,@AllowedIpAddress,@JSON,@Mode,@Position)";

                    SetParameter(command, "Id", tape.Id);
                    SetParameter(command, "Description", tape.Description);
                    SetParameter(command, "Comment", tape.Comment);
                    SetParameter(command, "OpenForRecording", tape.OpenForRecording);
                    SetParameter(command, "AllowedIpAddress", tape.AllowedIpAddress);
                    SetParameter(command, "JSON", JsonConvert.SerializeObject(tape, Formatting.Indented));
                    SetParameter(command, "Mode", tape.Mode);
                    SetParameter(command, "Position", tape.Position);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Tape Select(string id)
        {
            Tape result = null;
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select JSON from Tapes where Id=@Id";
                    SetParameter(command, "Id", id);
                    connection.Open();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string content = reader.GetString(0);
                            result = JsonConvert.DeserializeObject<Tape>(content);
                        }
                    }
                }
            }
            return result;
        }

        public List<Tape> List()
        {
            var result = new List<Tape>();
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select JSON from Tapes";
                    connection.Open();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string content = reader.GetString(0);
                            var tape = JsonConvert.DeserializeObject<Tape>(content);
                            result.Add(tape);
                        }
                    }
                }
            }

            return result;
        }

        public List<Tape> List(Func<Tape, bool> selector)
        {
            return List().Where(selector).ToList();
        }

        public Entry MatchEntry(string tapeId, Entry entryToMatch, IEntryComparer[] comparers = null)
        {
            Tape tape = Select(tapeId);

            // provide a default comparer
            if (comparers == null || comparers.Length == 0)
            {
                comparers = new IEntryComparer[] { new DefaultEntryComparer() };
            }

            var potentialMatches = tape.log.entries;
            return (
                       from entryComparer in comparers
                       select entryComparer.FindMatch(potentialMatches, entryToMatch)
                           into result
                           where result.Match != null
                           select result.Match)
                .FirstOrDefault();
        }

        public void FromJson(string json)
        {
            var list = JsonConvert.DeserializeObject<List<Tape>>(json);
            ClearDatabase();
            foreach (var tape in list)
            {
                Insert(tape);
            }
        }

        public string ToJson()
        {
            var list = List();
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            return json;
        }

        private bool _disposed;
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
                  }
    }
}