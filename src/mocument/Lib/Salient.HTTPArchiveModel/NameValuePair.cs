using System;

namespace Salient.HTTPArchiveModel
{
    [Serializable]
    public class NameValuePair
    {
        public string name { get; set; }
        public string value { get; set; }
        public string comment { get; set; }
        public override string ToString()
        {
            return string.Format("[{0},{1}] {2}", name, value, comment);
        }
    }
}