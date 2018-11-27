using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class ComplexDataType : DataType
    {
        public ComplexDataType(string name, params DataField[] fields)
            : base(name)
        {
            Fields = new List<DataField>(fields);
        }

        [JsonConstructor]
        private ComplexDataType()
        {
            Fields = new List<DataField>();
        }

        public List<DataField> Fields { get; set; }
    }
}
