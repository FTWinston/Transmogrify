using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class ComplexDataItem
    {
        public ComplexDataItem(ComplexDataType type)
        {
            Type = type;
            Values = new Dictionary<DataField, object>(type.Fields.Count);
        }

        [JsonConstructor]
        private ComplexDataItem()
        {
            Values = new Dictionary<DataField, object>();
        }

        public ComplexDataType Type { get; set; }

        public Dictionary<DataField, object> Values { get; }
    }
}
