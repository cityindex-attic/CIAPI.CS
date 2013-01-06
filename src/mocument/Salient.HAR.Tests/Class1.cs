using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salient.HAR.Model;

namespace Salient.HAR.Tests
{
    [TestFixture]
    public class SerializationFixture
    {
        [Test]
        public void DeserializationTest01()
        {

            DoRoundTrip("test_files/browser-blocking-time.har");
        }

        [Test]
        public void DeserializationTest02()
        {

            DoRoundTrip("test_files/chilli.har");
        }

        [Test,Ignore("the file seems to be non-compliant in that it has null timings.receive")]
        public void DeserializationTest03()
        {

            DoRoundTrip("test_files/chrome.har");
        }

        [Test]
        public void DeserializationTest04()
        {

            DoRoundTrip("test_files/custom-tags.har");
        }

        [Test]
        public void DeserializationTest05()
        {

            DoRoundTrip("test_files/deepfry.har");
        }

        [Test]
        public void DeserializationTest06()
        {

            DoRoundTrip("test_files/en.wikipedia.org.har");
        }

        [Test]
        public void DeserializationTest07()
        {

            DoRoundTrip("test_files/google.com.har");
        }

        [Test]
        public void DeserializationTest08()
        {

            DoRoundTrip("test_files/inline-scripts-block.har");
        }

        [Test]
        public void DeserializationTest09()
        {

            DoRoundTrip("test_files/languages.har");
        }

        [Test]
        public void DeserializationTest10()
        {

            DoRoundTrip("test_files/missing-timing.har");
        }

        [Test]
        public void DeserializationTest11()
        {

            DoRoundTrip("test_files/missing-timing2.har");
        }

        [Test]
        public void DeserializationTest12()
        {

            DoRoundTrip("test_files/softwareishard.com.har");
        }

        [Test]
        public void DeserializationTest13()
        {

            DoRoundTrip("test_files/solar_system.har");
        }

        [Test]
        public void DeserializationTest14()
        {

            DoRoundTrip("test_files/test.har");
        }

        [Test]
        public void DeserializationTest15()
        {

            DoRoundTrip("test_files/www.frogthinker.org.har");
        }
        [Test]
        public void DeserializationTest16()
        {

            DoRoundTrip("test_files/feedback.ebay.com.ha");
        }

        private static bool DoRoundTrip(string fileName)
        {

            string jsonIn = File.ReadAllText(fileName);
            var harObject = JsonConvert.DeserializeObject<HTTPArchiveType>(jsonIn);
            var jsonOut = JsonConvert.SerializeObject(harObject, Formatting.Indented);

            // now use jsoncovert to parse and output both so we get apples to apples

            string theirs = JObject.Parse(jsonIn).ToString(Formatting.Indented);
            string ours = JObject.Parse(jsonOut).ToString(Formatting.Indented);


            return Compare(theirs, ours);
        }


        private static bool Compare(string theirs, string ours)
        {
            var d = new Differ();
            var result = d.CreateLineDiffs(theirs, ours, false, false);
            if (result.DiffBlocks.Count > 0)
            {
                DumpDiff(theirs, ours);
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void DumpDiff(string theirs, string ours)
        {
            var d = new Differ();

            var inlineBuilder = new InlineDiffBuilder(d);
            DiffPaneModel result = inlineBuilder.BuildDiffModel(theirs, ours);
            foreach (var line in result.Lines)
            {
                if (line.Type == ChangeType.Inserted)
                    Console.Write("+ ");
                else if (line.Type == ChangeType.Deleted)
                    Console.Write("- ");
                else
                    Console.Write("  ");

                Console.WriteLine(line.Text);
            }
        }
    }
}
