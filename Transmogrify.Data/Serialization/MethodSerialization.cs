using System;
using System.Linq;
using System.Reflection;

namespace Transmogrify.Data.Serialization
{
    static class MethodSerialization
    {
        public static string GetUniqueIdentifier(MethodInfo mi)
        {
            string signatureString = string.Join(",", mi.GetParameters().Select(p => p.ParameterType.Name).ToArray());

            if (mi.IsGenericMethod)
            {
                string typeParams = string.Join(",", mi.GetGenericArguments().Select(g => g.AssemblyQualifiedName).ToArray());
                return $"{mi.Name}<{typeParams}>({signatureString})";
            }

            return $"{mi.Name}({signatureString})";
        }

        public static MethodInfo GetMethod(Type type, string methodIdentifier)
        {
            return type.GetMethods()
                .FirstOrDefault(mi => GetUniqueIdentifier(mi) == methodIdentifier);
        }
    }
}
