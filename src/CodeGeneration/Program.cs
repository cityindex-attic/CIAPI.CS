using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CodeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            string schemaText = File.ReadAllText("meta\\schema.json");
            JObject schema = JObject.Parse(schemaText);
            string smdText = File.ReadAllText("meta\\smd.json");
            JObject smd = JObject.Parse(smdText);

        }
    }
}
