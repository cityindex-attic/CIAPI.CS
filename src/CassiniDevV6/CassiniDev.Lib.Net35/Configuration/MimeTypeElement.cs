using System.Configuration;

namespace CassiniDev.Configuration
{
    public class MimeTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("extension", IsRequired = true, IsKey = true)]
        public string Extension
        {
            get
            {
                return (string)this["extension"];
            }
            set
            {
                this["extension"] = value;
            }
        }
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }


    }
}