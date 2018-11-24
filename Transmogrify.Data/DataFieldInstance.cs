using System;

namespace Transmogrify.Data
{
    public class DataFieldInstance : IComparable<DataFieldInstance>
    {
        public DataFieldInstance(MappingElement element, DataField field)
        {
            Element = element;
            Field = field;
        }

        public MappingElement Element { get; set; }
        public DataField Field { get; set; }

        public int CompareTo(DataFieldInstance other)
        {
            int val = Element.CompareTo(other.Element);

            if (val != 0)
                return val;

            return Field.CompareTo(other.Field);
        }
    }
}
