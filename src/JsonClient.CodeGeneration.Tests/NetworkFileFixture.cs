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

        [Test, Category("DependsOnExternalResource"),Ignore("function not used yet, no need to support")]
        public void NetworkFileCanReadRemote()
        {
            const string expected = "SMDVersion";
            string actual = NetworkFile.ReadAllText(new Uri(new Uri(TestConfig.RpcUrl), "smd").AbsoluteUri);
            StringAssert.Contains(expected,actual);
        }


    }
}
