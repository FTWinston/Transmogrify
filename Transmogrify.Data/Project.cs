using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

                foreach (var strAssemblyName in value)
                {
                    if (alreadyLoaded.Contains(strAssemblyName))
                        continue;

                    var assemblyName = new AssemblyName(strAssemblyName);

                    var assemblyPath = $"{assemblyName.Name}.dll";

                    var assembly = Assembly.LoadFrom(assemblyPath);

                    if (assembly.FullName != strAssemblyName)
                    {
                        Console.WriteLine($"Warning, loaded unexpected version of {assemblyName.Name}");
                        Console.WriteLine($"Specified in project: {strAssemblyName}");
                        Console.WriteLine($"In local directory:   {assembly.FullName}");
                    }
                }
            }
        }

        [JsonProperty(Order = 2, ItemTypeNameHandling = TypeNameHandling.All)]
        public List<DataEndPoint> EndPoints { get; } = new List<DataEndPoint>();

        [JsonProperty(Order = 3)]
        public List<Mapping> Mappings { get; } = new List<Mapping>();
    }
}
