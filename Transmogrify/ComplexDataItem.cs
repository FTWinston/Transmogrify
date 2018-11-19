using System;
using System.Collections.Generic;

namespace Transmogrify
{
    public class ComplexDataItem
    {
        public ComplexDataItem(ComplexDataType type, IList<DataField> fields)
        {
            Type = type;
            Values = new Dictionary<DataField, object>(fields.Count);

            foreach (var field in fields)
                Values.Add(field, null);
        }

        public ComplexDataType Type { get; }

        public Dictionary<DataField, object> Values { get; }
    }
}
