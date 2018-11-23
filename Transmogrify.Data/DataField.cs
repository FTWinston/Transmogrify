using System;

namespace Transmogrify.Data
{
    public class DataField : IComparable<DataField>
    {
        public DataField(string name, DataType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public DataType Type { get; set; }

        public int CompareTo(DataField other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
