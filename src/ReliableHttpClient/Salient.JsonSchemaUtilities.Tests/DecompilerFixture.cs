using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Salient.JsonSchemaUtilities.Tests
{
    [TestFixture]
    public class DecompilerFixture
    {
        [Test]
        public void Test()
        {
            Decompiler dc = new Decompiler();
            dc.Decompile();
        }
    }
}
