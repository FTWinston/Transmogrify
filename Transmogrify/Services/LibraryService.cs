using System;
using System.Collections.Generic;
using System.Linq;
using Transmogrify.Data;

namespace Transmogrify.Services
{
    public class LibraryService
    {
        // TODO: this ought to save relative file paths rather than assembly names ... at least in the config
        // TODO: use config
        public string[] Library { get; private set; } = new[] {
            "Transmogrify.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Transmogrify.Operations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        };

        public void LoadLibraryAssemblies()
        {
            AssemblyLoader.LoadAssemblies(Library);
        }

        public void UpdateLibrary(IEnumerable<string> libraries)
        {
            Library = libraries.ToArray();

            LoadLibraryAssemblies();
        }

        public IEnumerable<Type> GetAvailableEndpointTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && typeof(DataEndPoint).IsAssignableFrom(t));
        }
    }
}
