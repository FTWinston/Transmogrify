using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Project
    {
        [JsonProperty(Order = 1, ItemTypeNameHandling = TypeNameHandling.All)]
        public List<DataEndPoint> EndPoints { get; set; } = new List<DataEndPoint>();

        [JsonProperty(Order = 2)]
        public List<Mapping> Mappings { get; set; } = new List<Mapping>();
    }
}
