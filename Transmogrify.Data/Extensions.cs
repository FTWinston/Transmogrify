using System.Reflection;

namespace Transmogrify.Data
{
    public static class Extensions
    {
        public static bool UseAsInput(this ParameterInfo param)
        {
            return !param.IsOut;
        }

        public static bool UseAsOutput(this ParameterInfo param)
        {
            return param.IsOut || param.ParameterType.IsByRef;
        }
    }
}
