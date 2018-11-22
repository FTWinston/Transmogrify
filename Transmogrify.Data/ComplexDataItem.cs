using System;
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

        public ComplexDataItem(ComplexDataType type, IList<DataField> fields)
            : this(type)
        {
            foreach (var field in fields)
                Values.Add(field, null);
        }

        public ComplexDataType Type { get; }

        public Dictionary<DataField, object> Values { get; }
    }
}
