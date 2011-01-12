using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Tests;

using JsonClient.CodeGeneration.IO;
using NUnit.Framework;

namespace JsonClient.CodeGeneration.Tests
{
    [TestFixture]
    public class NetworkFileFixture
    {
        [Test]
        public void NetworkFileCanReadLocal()
        {
            const string expected = "foo";
            string actual = NetworkFile.ReadAllText(@"metadata\foo.txt");
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void NetworkFileCanReadRemote()
        {
            const string expected = "SMDVersion";
            string actual = NetworkFile.ReadAllText(new Uri(new Uri(TestConfig.RpcUrl), "smd").AbsoluteUri);
            StringAssert.Contains(expected,actual);
        }


    }
}
