using System;

namespace Transmogrify.Data
{
    public class DataField : IComparable<DataField>
    {
        public DataField(MappingElement element, string name, DataType type)
        {
            Element = element;
            Name = name;
            Type = type;
        }

        MappingElement Element { get; set; }
        public string Name { get; set; }
        public DataType Type { get; set; }

        public int CompareTo(DataField other)
        {
            var value = Element.CompareTo(other.Element);

            if (value != 0)
                return value;

            return Name.CompareTo(other.Name);
        }
    }
}
