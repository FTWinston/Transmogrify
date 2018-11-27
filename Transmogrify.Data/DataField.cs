using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class DataField : IComparable<DataField>
    {
        public DataField(string name, DataType type)
        {
            Name = name;
            Type = type;
        }

        [JsonConstructor]
        private DataField()
        {

        }

        public string Name { get; set; }

        public DataType Type { get; set; }

        public int CompareTo(DataField other)
        {
            // TODO: this needs to be able to work with structured data, that might have fields with the same name.
            // I'd put a NextIdentifier on ComplexDataType if we had a reference to the "parent" type here, but we don't.
            return Name.CompareTo(other.Name);
        }
    }
}
