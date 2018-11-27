using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    public abstract class MappingElement : IComparable<MappingElement>
    {
        private static int nextElementIdentifier;

        protected MappingElement()
        {
            ID = ++nextElementIdentifier;
        }

        [JsonIgnore]
        internal int ID { get; }

        public int CompareTo(MappingElement other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
