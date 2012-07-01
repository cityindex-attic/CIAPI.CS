using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salient.ReliableHttpClient.Serialization.Tests.TestTypes
{

    public class RecursiveClass
    {
        public string Id { get; set; }
        public RecursiveClass Nested { get; set; }
    }
}
