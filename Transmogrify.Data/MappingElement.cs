using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    public class MappingElement : IComparable<MappingElement>
    {
        protected MappingElement(Mapping mapping)
        {
            Mapping = mapping;
            ID = ++Mapping.NextElementIdentifier;
        }

        [JsonIgnore]
        public Mapping Mapping { get; }

        [JsonIgnore]
        internal int ID { get; }

        public int CompareTo(MappingElement other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
