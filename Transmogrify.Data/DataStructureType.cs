using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class DataStructureType
    {
        public DataStructureType(string name, params DataField[] fields)
        {
            Name = name;
            Fields = new List<DataField>(fields);
        }

        [JsonConstructor]
        private DataStructureType()
        {
            Fields = new List<DataField>();
        }

        public string Name { get; set; }

        public List<DataField> Fields { get; set; }
    }
}
