using System.Configuration;

namespace CassiniDev.Configuration
{
    [ConfigurationCollection(typeof(MimeTypeElement))]
    public class MimeTypeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MimeTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MimeTypeElement)element).Extension;
        }
    }
}