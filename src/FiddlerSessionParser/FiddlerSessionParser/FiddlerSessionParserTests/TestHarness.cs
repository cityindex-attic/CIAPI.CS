using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FiddlerSessionParser
{
    [TestFixture]
    public class TestHarness
    {
        [Test]
        public void CanParseSavedSessions()
        {
            string path = @"..\..\1_Full.txt";
            var parser = new Parser();
            List<SessionInfo> sessions = parser.ParseFile(path);
        }
    }
}
