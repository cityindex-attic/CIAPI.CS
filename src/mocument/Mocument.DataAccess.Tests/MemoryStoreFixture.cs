using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mocument.Model;
using NUnit.Framework;

namespace Mocument.DataAccess.Tests
{
    [TestFixture]
    public class MemoryStoreFixture
    {
        [Test]
        public void BasicCRUD()
        {
            

           
                var store = new MemoryStore();
                var tape = new Tape()
                {
                    Id = "foo"
                };
                store.Insert(tape);
                var tape2 = store.Select(tape.Id);
                Assert.IsNotNull(tape2);

                // make sure tapes are cloned
                tape2.Comment = "bar";
                var tape3 = store.Select(tape2.Id);
                Assert.AreNotEqual(tape2.Comment, tape3.Comment);

                // update
                store.Update(tape2);
                tape3 = store.Select(tape2.Id);
                Assert.AreEqual(tape2.Comment, tape3.Comment);

                // delete
                store.Delete(tape3.Id);
                Assert.AreEqual(0, store.List().Count);

           
        }
    }
}
