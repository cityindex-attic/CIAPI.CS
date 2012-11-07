using System;
using System.Collections.Specialized;
using System.Linq;

namespace FiddlerSessionParser
{
    public class ResponseInfo
    {
        public string Content;

        public int StatusCode;
        public string Status;
        public NameValueCollection Headers = new NameValueCollection();
        public string Body;

        public ResponseInfo(string content)
        {
            Content = content.Trim();
            var lines = Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            int indexOf = lines[0].IndexOf(' ');
            int nextIndexOf = lines[0].IndexOf(' ',indexOf +1);

            StatusCode = int.Parse(lines[0].Substring(indexOf, nextIndexOf - indexOf).Trim());
            Status = lines[0].Substring(nextIndexOf).Trim();

            int lineIndex = 1;
            while (lines[lineIndex] != "")
            {
                indexOf = lines[lineIndex].IndexOf(":", StringComparison.Ordinal);
                string key = lines[lineIndex].Substring(0, indexOf).Trim();
                string value = lines[lineIndex].Substring(indexOf + 1).Trim();
                Headers.Add(key, value);

                if (++lineIndex == lines.Length)
                {
                    return;
                }
            }
            Body = string.Join(Environment.NewLine, lines.Where((l, i) => i > lineIndex).ToArray());
        }
    }
}