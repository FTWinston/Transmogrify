using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    public class MappingElement : IComparable<MappingElement>
    {
        protected MappingElement(Mapping mapping)
        {
            Mapping = mapping;
            Identifier = Mapping.NextElementIdentifier++;
        }

        [JsonIgnore]
        public Mapping Mapping { get; }

        [JsonIgnore]
        internal int Identifier { get; }

        public int CompareTo(MappingElement other)
        {
            return Identifier.CompareTo(other.Identifier);
        }
    }
}
