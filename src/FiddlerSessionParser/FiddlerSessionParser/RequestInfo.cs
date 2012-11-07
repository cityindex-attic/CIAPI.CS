using System;
using System.Collections.Specialized;
using System.Linq;
namespace FiddlerSessionParser
{
    public class RequestInfo
    {
        public string Content;
        public string Method;
        public string Url;
        public NameValueCollection Headers = new NameValueCollection();
        public string Body;
        public RequestInfo(string content)
        {
            Content = content.Trim();
            var lines = Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            int indexOf = lines[0].IndexOf(' ');
            Method = lines[0].Substring(0, indexOf).Trim();
            Url = lines[0].Substring(indexOf + 1, lines[0].IndexOf("HTTP/1.1", StringComparison.Ordinal) - indexOf - 1).Trim();
            int lineIndex = 1;
            while (lines[lineIndex] != "")
            {
                indexOf = lines[lineIndex].IndexOf(":", StringComparison.Ordinal);
                string key = lines[lineIndex].Substring(0, indexOf).Trim();
                string value = lines[lineIndex].Substring(indexOf + 1).Trim();
                Headers.Add(key, value);
                
                if(++lineIndex==lines.Length)
                {
                    return;
                }
            }
            Body = string.Join(Environment.NewLine, lines.Where((l, i) => i > lineIndex).ToArray());
        }
    }
}