using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Mapping
    {
        Project Project { get; set; }
        public DataCollection Source { get; set; }
        public DataCollection Destination { get; set; }

        public List<MappingElement> Elements { get; set; }

        internal int NextElementIdentifier { get; set; } // not serializable
    }
}
