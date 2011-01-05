using System;
using System.Collections.Generic;

using JsonClient.CodeGeneration.IO;
using NUnit.Framework;

namespace JsonClient.CodeGeneration.Tests
{
    [TestFixture]
    public class MetaDeserializationFixture
    {
        [Test]
        public void JavaScriptSerializerReturnsDictionaries()
        {
            var meta = NetworkFile.ReadAllText(@"metadata\stackauth-schema.txt");

        }


    
    }
}