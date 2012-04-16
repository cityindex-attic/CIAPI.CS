using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Salient.ReliableHttpClient.Serialization
{
    public interface IJsonSerializer
    {

        string SerializeObject(object value);
        T DeserializeObject<T>(string json);
    }



}
