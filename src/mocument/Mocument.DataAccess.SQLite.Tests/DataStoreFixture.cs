using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mocument.Model;
using NUnit.Framework;

namespace Mocument.DataAccess.SQLite.Tests
{
    [TestFixture]
    public class DataStoreFixture
    {
        [Test]
        public void noid()
        {
            File.Delete("mocument");
            var s = new SQLiteStore("mocument");
            s.ClearDatabase();
            var x = s.List();
            Assert.AreEqual(0, x.Count);

            var tape = new Tape()
                           {
                               Id = "foo.bar",
                               Comment = "comment",
                               Description = "desc",
                               OpenForRecording = false,
                               AllowedIpAddress = "1.2.2.2"
                           };

            s.Insert(tape);
            x = s.List();
            Assert.AreEqual(1, x.Count);

            s.Delete(tape.Id);
            x = s.List();
            Assert.AreEqual(0, x.Count);



        }
    }
}
