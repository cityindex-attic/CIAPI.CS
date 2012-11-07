using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiddlerSessionParser
{
    public class Parser
    {
        private const string SEPARATOR = "------------------------------------------------------------------";
        public List<SessionInfo> ParseString(string content)
        {
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            List<SessionInfo> sessions = new List<SessionInfo>();
            int startIndex;

            if (lines[0].Contains("HTTP/1.1"))
            {
                startIndex = -1;
            }
            else
            {
                startIndex = FindNextSeparator(0, lines);
            }

            int nextIndex = FindNextSeparator(startIndex + 1, lines);
            while (nextIndex != -1)
            {
                string[] sessionLines = lines.Where((l, i) => i > startIndex && i < nextIndex).ToArray();
                var sessionInfo = new SessionInfo(string.Join(Environment.NewLine, sessionLines));
                sessions.Add(sessionInfo);
                startIndex = nextIndex + 1;
                nextIndex = FindNextSeparator(startIndex + 1, lines);
            }


            return sessions;
        }
        public List<SessionInfo> ParseFile(string path)
        {


            string lines = File.ReadAllText(path);
            return ParseString(lines);
        }




        private int FindNextSeparator(int start, string[] lines)
        {

            for (var i = start; i < lines.Length; i++)
            {
                if (lines[i] == SEPARATOR)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
