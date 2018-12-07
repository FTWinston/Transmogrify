using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class ComplexDataType
    {
        public ComplexDataType(string name, params DataField[] fields)
        {
            Name = name;
            Fields = new List<DataField>(fields);
        }

        [JsonConstructor]
        private ComplexDataType()
        {
            Fields = new List<DataField>();
        }

        public string Name { get; set; }

        public List<DataField> Fields { get; set; }
    }
}
