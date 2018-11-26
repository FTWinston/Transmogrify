using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class ComplexDataType : DataType
    {
        [JsonConstructor]
        public ComplexDataType(string name, List<DataField> fields)
            : base(name, false)
        {
            Fields = fields;
        }

        public ComplexDataType(string name, params DataField[] fields)
            : base(name, false)
        {
            Fields = new List<DataField>(fields);
        }

        public List<DataField> Fields { get; set; }
    }
}
