using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAPI.CS2.CodeGeneration
{
    /// <summary>
    /// This data structure is built from JSON Schema
    /// for use in code generation.
    /// </summary>
    public class DataClassDescription
    {
        public DataClassDescription()
        {
            Properties = new DataPropertyDescriptionCollection();
            Attributes=new Dictionary<string, object>();
        }


        public string Name { get; set; }
        public string Base { get; set; }
        public string Summary { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        public DataPropertyDescriptionCollection Properties { get; set; }
    }


    public class DataPropertyDescriptionCollection : Dictionary<string, DataPropertyDescription>
    {
        public bool PascalCased { get; set; }

    }
    public class DataPropertyDescription
    {
        public DataPropertyDescription()
        {
            Attributes =new Dictionary<string, object>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }

}
