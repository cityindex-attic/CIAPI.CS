using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JsonClient.CodeGeneration.IO
{
    public class NetworkFile
    {

        public static string ReadAllText(string path)
        {
            Uri uri;
            string json;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                var request = WebRequest.Create(path);
                if (request is HttpWebRequest)
                {
                    ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                json = reader.ReadToEnd();
            }
            else
            {
                json = File.ReadAllText(Path.GetFullPath(path));
            }
            return json;
        }
    }

}
