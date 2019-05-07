using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Transmogrify.Data
{
    public class Project
    {
        [JsonProperty(Order = 1, PropertyName = "Dependencies")]
        private string[] DependentAssemblies
        {
            get
            {
                var endPointAssemblies = EndPoints
                    .Select(ep => ep.GetType().Assembly);

                var operationAssemblies = Mappings
                    .SelectMany(m => m.Operations)
                    .Select(mo => mo.Method.DeclaringType.Assembly);

                return endPointAssemblies
                    .Union(operationAssemblies)
                    .Distinct()
                    .Select(a => a.FullName)
                    .ToArray();
            }
            set
            {
                AssemblyLoader.LoadAssemblies(value);
            }
        }

        [JsonProperty(Order = 2, ItemTypeNameHandling = TypeNameHandling.All)]
        public List<DataEndPoint> EndPoints { get; } = new List<DataEndPoint>();

        [JsonProperty(Order = 3)]
        public List<Mapping> Mappings { get; } = new List<Mapping>();
    }
}
