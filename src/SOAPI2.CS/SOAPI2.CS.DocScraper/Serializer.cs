using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SOAPI2.DocScraper
{
    public class Serializer<T> 
    {
        public Serializer()
        {
        }

        public void SerializeObject(string filename, T objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public T DeSerializeObject(string filename)
        {
            T objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (T)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}