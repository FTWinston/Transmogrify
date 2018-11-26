using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Mapping
    {
        public EndPointDataCollection Source { get; set; }

        public EndPointDataCollection Destination { get; set; }

        public List<MappingOperation> Operations { get; set; } = new List<MappingOperation>();

        public List<MappingOutput> Outputs { get; set; } = new List<MappingOutput>();

        [JsonIgnore]
        internal int NextElementIdentifier { get; set; }
    }
}
