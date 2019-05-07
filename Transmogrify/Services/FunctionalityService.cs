using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Transmogrify.Data;

namespace Transmogrify.Services
{
    public static class FunctionalityService
    {
        public static IEnumerable<Type> GetAvailableEndpointTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && t.IsAssignableFrom(typeof(DataEndPoint)));
        }
    }
}
