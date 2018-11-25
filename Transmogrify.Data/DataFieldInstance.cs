using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class DataFieldInstance : IComparable<DataFieldInstance>
    {
        public DataFieldInstance(MappingElement element, DataField field)
        {
            Element = element;
            Field = field;
        }

        public MappingElement Element { get; set; } // TODO: just serialize an ID

        public DataField Field { get; set; } // TODO: just serialize an ID

        public int CompareTo(DataFieldInstance other)
        {
            int val = Element.CompareTo(other.Element);

            if (val != 0)
                return val;

            return Field.CompareTo(other.Field);
        }
    }
}
