using System;
using System.Collections.Generic;

namespace Transmogrify
{
    public class ComplexDataType : DataType
    {
        public ComplexDataType(string name, IList<DataField> fields)
            : base(name, false)
        {
            Fields = fields;
        }

        public IList<DataField> Fields { get; set; }
    }
}
