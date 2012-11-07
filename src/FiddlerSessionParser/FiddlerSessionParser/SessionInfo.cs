using System;
using System.Linq;

namespace FiddlerSessionParser
{
    public class SessionInfo
    {
        public string Content;
        public RequestInfo Request;
        public ResponseInfo Response;
        public SessionInfo(string content)
        {
            string[] sessionLines = content.Split(new string[] { Environment.NewLine },StringSplitOptions.None);
            Content = content;

            int responseIndex = FindResponse(sessionLines);
            string[] request = sessionLines.Where((l, i) => i < responseIndex).ToArray();
            Request = new RequestInfo(string.Join(Environment.NewLine, request));
            string[] response = sessionLines.Where((l, i) => i >= responseIndex).ToArray();
            Response = new ResponseInfo(string.Join(Environment.NewLine, response)); 
        }

        private int FindResponse(string[] lines)
        {
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("HTTP/1.1 "))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}