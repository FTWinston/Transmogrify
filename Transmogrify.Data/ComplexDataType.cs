using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class ComplexDataType : DataType
    {
        public ComplexDataType(string name, IList<DataField> fields)
            : base(name, false)
        {
            Fields = fields;
        }

        public ComplexDataType(string name, params DataField[] fields)
            : base(name, false)
        {
            Fields = fields;
        }

        public IList<DataField> Fields { get; set; }
    }
}
