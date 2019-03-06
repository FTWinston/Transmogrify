using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Mapping
    {
        public string Name { get; set; }
        
        public EndPointDataCollection Source { get; set; }

        public EndPointDataCollection Destination { get; set; }

        public List<Operation> Operations { get; set; } = new List<Operation>();

        public List<MappingOutput> Outputs { get; set; } = new List<MappingOutput>();

        [JsonIgnore]
        internal int NextElementIdentifier { get; set; }
    }
}
