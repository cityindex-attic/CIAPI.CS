using System;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    [DataContract]
    public class NameValuePair
    {
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "value")]
        public virtual string Value { get; set; }
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
        public override string ToString()
        {
            return string.Format("[{0},{1}] {2}", Name, Value, Comment);
        }
    }
}