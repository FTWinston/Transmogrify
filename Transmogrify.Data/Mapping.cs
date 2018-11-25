using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Mapping
    {
        [JsonIgnore]
        Project Project { get; set; }

        public DataCollection Source { get; set; } // TODO: should just serialize an ID

        public DataCollection Destination { get; set; } // TODO: should just serialize an ID

        public List<MappingOperation> Operations { get; set; } = new List<MappingOperation>();

        public List<MappingOutput> Outputs { get; set; } = new List<MappingOutput>();

        [JsonIgnore]
        internal int NextElementIdentifier { get; set; }
    }
}
