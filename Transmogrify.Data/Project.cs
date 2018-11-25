using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Project
    {
        public List<DataEndPoint> EndPoints { get; set; } = new List<DataEndPoint>();

        public List<Mapping> Mappings { get; set; } = new List<Mapping>();
    }
}
