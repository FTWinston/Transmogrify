using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Mapping
    {
        Project Project { get; set; }
        public DataCollection Source { get; set; }
        public DataCollection Destination { get; set; }

        public List<MappingOperation> Operations { get; set; }
        public List<MappingOutput> Outputs { get; set; }

        internal int NextElementIdentifier { get; set; } // not serializable
    }
}
