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

        public Mapping Mapping { get; } // not serializable
        internal int Identifier { get; } // not serializable

        public int CompareTo(MappingElement other)
        {
            throw new NotImplementedException();
        }
    }
}
