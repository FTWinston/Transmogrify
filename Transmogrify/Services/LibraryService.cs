using System;
using System.Collections.Generic;
using System.Linq;
using Transmogrify.Data;

namespace Transmogrify.Services
{
    public class LibraryService
    {
        public string[] Library { get; } = new[] {
            "Transmogrify.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Transmogrify.Operations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        };

        public void LoadLibraryAssemblies()
        {
            AssemblyLoader.LoadAssemblies(Library);
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
