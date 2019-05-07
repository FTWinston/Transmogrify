using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Transmogrify.Data
{
    public static class AssemblyLoader
    {
        public static void LoadAssemblies(IEnumerable<string> assemblyNames)
        {
            var alreadyLoaded = new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName));

            foreach (var strAssemblyName in assemblyNames)
            {
                if (alreadyLoaded.Contains(strAssemblyName))
                    continue;

                var assemblyName = new AssemblyName(strAssemblyName);

                var assemblyPath = $"{assemblyName.Name}.dll";

                var assembly = Assembly.LoadFrom(assemblyPath);

                if (assembly.FullName != strAssemblyName)
                {
                    Console.WriteLine($"Warning, loaded unexpected version of {assemblyName.Name}");
                    Console.WriteLine($"Specified: {strAssemblyName}");
                    Console.WriteLine($"Found:     {assembly.FullName}");
                }
            }
        }
    }
}
