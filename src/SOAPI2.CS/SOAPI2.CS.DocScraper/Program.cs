using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace SOAPI2.DocScraper
{
    internal class Program
    {
        
        private static void Main(string[] args)
        {
            Docs docs = PullFromFile();
            //Docs docs = PullFromWeb();

            docs.Validate();
            
            var schema = docs.CreateSchema();
            File.WriteAllText("..\\..\\js\\schema.json",schema.ToString());
            
            var smd = docs.CreateSMD(schema);
            File.WriteAllText("..\\..\\js\\smd.json", smd.ToString());

            var sb = new StringBuilder();
            var generator = new CodeGenerator(schema, smd);
            generator.GenerateRoutes(sb);
            File.WriteAllText("..\\..\\..\\SOAPI2.CS\\Routes.cs",sb.ToString());
            sb = new StringBuilder();
            generator.GenerateModel(sb);
            File.WriteAllText("..\\..\\..\\SOAPI2.CS\\Model.cs", sb.ToString());
        }

        private static void PullMethodDocsFromFile(Docs docs)
        {
            foreach (MethodGroup group in docs.MethodGroups)
            {
                foreach (MethodInfo method in group.Methods)
                {
                    string methodName = method.Name;

                    string filename = "..\\..\\html\\methods\\" + methodName + ".htm";
                    string source = File.ReadAllText(filename);
                    method.Source = source;
                }
            }
        }

        private static void PullMethodDocsFromWeb(Docs docs)
        {
            

            foreach (MethodGroup group in docs.MethodGroups)
            {
                foreach (MethodInfo method in group.Methods)
                {
                    string methodName = method.Name;
                    if (string.IsNullOrEmpty(methodName))
                    {
                        throw new Exception("methodName null");
                    }

                    var client = new WebClient();
                    string filename = "..\\..\\html\\methods\\" + methodName + ".htm";
                    string source = client.DownloadString("https://api.stackexchange.com" + method.Url);
                    method.Source = source;
                    File.WriteAllText(filename, source);
                    new AutoResetEvent(false).WaitOne(1000);
                }
            }
        }
        private static void PullTypeDocsFromWeb(Docs docs)
        {
            
            // have to add this manually - is not linked from doc index
            docs.Types.Add(new TypeInfo(docs) { Url = "/docs/types/related-site", Type = "related-site", Name = "related_site" });

            
            docs.Types.Add(new TypeInfo(docs) { Url = "/docs/wrapper", Type = "response-wrapper", Name = "response_wrapper" });
            

            var types = docs.Types.ToArray();
            foreach (var type in types)
            {
                if(type.Inferred)
                {
                    // we made it up - not going to be online
                    continue;
                }
                string typeName = type.Type;
                if (string.IsNullOrEmpty(typeName))
                {
                    throw new Exception("typename null");
                }
                var client = new WebClient();
                string filename = "..\\..\\html\\types\\" + typeName + ".htm";
                string source = client.DownloadString("https://api.stackexchange.com" + type.Url);
                type.Source = source;
                File.WriteAllText(filename, source);
                new AutoResetEvent(false).WaitOne(1000);
            }

        }

 

        private static void PullTypesFromFile(Docs docs)
        {
            // have to add this manually - is not linked from doc index
            docs.Types.Add(new TypeInfo(docs) { Url = "/docs/types/related-site", Type = "related-site", Name = "related_site" });
            
            docs.Types.Add(new TypeInfo(docs) { Url = "/docs/wrapper", Type = "response-wrapper", Name = "response_wrapper" });
            

            var types = docs.Types.ToArray();
            foreach (var type in types)
            {
                if (string.IsNullOrEmpty(type.Type))
                {
                    throw new Exception("typename null");
                }
                if (type.Inferred)
                {
                    continue;

                }
                string filename = "..\\..\\html\\types\\" + type.Type + ".htm";
                string source = File.ReadAllText(filename);
                type.Source = source;
            }
            
        }

        private static Docs Deserialize()
        {
            var s = new Serializer<Docs>();
            Docs docs = s.DeSerializeObject("..\\..\\serialized\\docs.bin");
            return docs;
        }

        private static void Serialize(Docs docs)
        {
            var s = new Serializer<Docs>();
            s.SerializeObject("..\\..\\serialized\\docs.bin", docs);
        }

        private static Docs PullFromFile()
        {
            string methodsIndex = File.ReadAllText("..\\..\\html\\methods.htm");
            string typesIndex = File.ReadAllText("..\\..\\html\\types.htm");

            var docs = new Docs
                           {
                               MethodsIndex = methodsIndex,
                               TypesIndex = typesIndex
                           };
            PullMethodDocsFromFile(docs);
            PullTypesFromFile(docs);
            return docs;
        }

        private static Docs PullFromWeb()
        {
            var client = new WebClient();
            string methodsIndex = client.DownloadString("https://api.stackexchange.com/docs");
            File.WriteAllText("..\\..\\html\\methods.htm", methodsIndex);
            string typesIndex = client.DownloadString("https://api.stackexchange.com/docs?tab=type");
            File.WriteAllText("..\\..\\html\\types.htm", typesIndex);
            var docs = new Docs
                           {
                               MethodsIndex = methodsIndex,
                               TypesIndex = typesIndex
                           };
            PullMethodDocsFromWeb(docs);
            PullTypeDocsFromWeb(docs);
            return docs;
        }
    }
}