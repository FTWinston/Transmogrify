using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                    .Select(ep => ep.GetType().Assembly)
                    .Distinct();

                var operationAssemblies = Mappings
                    .SelectMany(m => m.Operations)
                    .Select(mo => mo.Method.DeclaringType.Assembly)
                    .Distinct();

                return endPointAssemblies
                    .Union(operationAssemblies)
                    .Select(a => a.FullName)
                    .ToArray();
            }
            set
            {
                var alreadyLoaded = new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName));

                foreach (var assemblyName in value)
                {
                    if (alreadyLoaded.Contains(assemblyName))
                        continue;

                    Assembly.Load(assemblyName);
                }
            }
        }

        [JsonProperty(Order = 2, ItemTypeNameHandling = TypeNameHandling.All)]
        public List<DataEndPoint> EndPoints { get; } = new List<DataEndPoint>();

        [JsonProperty(Order = 3)]
        public List<Mapping> Mappings { get; } = new List<Mapping>();
    }
}
