using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class DataStructure
    {
        public DataStructure(DataStructureType type)
        {
            Type = type;
            Values = new Dictionary<DataField, object>(type.Fields.Count);
        }

        [JsonConstructor]
        private DataStructure()
        {
            Values = new Dictionary<DataField, object>();
        }

        public DataStructureType Type { get; set; }

        public Dictionary<DataField, object> Values { get; }
    }
}
