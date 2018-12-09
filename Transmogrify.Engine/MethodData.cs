using System.Linq;
using System.Reflection;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    class MethodData
    {   
        public MethodInvoker Delegate { get; set; }

        public bool OutputReturnType { get; set; }

        public bool UseInputsDirectly { get; set; }

        public bool ReturnParametersDirectly { get; set; }

        public bool[] InputParameters { get; set; }

        public bool[] OutputParameters { get; set; }

        public int NumOutputs { get; set; }

        public MethodData(MethodInfo methodInfo, MethodInvoker callDelegate)
        {
            var parameters = methodInfo.GetParameters();
            var inputs = DetectInputParameters(parameters);
            var outputs = DetectOutputParameters(parameters);
            bool outputReturnType = methodInfo.ReturnType != typeof(void);

            Delegate = callDelegate;
            OutputReturnType = outputReturnType;
            InputParameters = inputs;
            OutputParameters = outputs;
            NumOutputs = outputs.Count(o => o) + (outputReturnType ? 1 : 0);
            UseInputsDirectly = inputs.All(i => i); // && outputs.All(o => !o); // uncomment to stop ref parameters modifying input array
            ReturnParametersDirectly = !outputReturnType && outputs.All(o => o);
        }

        private static bool[] DetectInputParameters(ParameterInfo[] parameters)
        {
            return parameters
                .Select(p => p.UseAsInput())
                .ToArray();
        }

        private static bool[] DetectOutputParameters(ParameterInfo[] parameters)
        {
            return parameters
                .Select(p => p.UseAsOutput())
                .ToArray();
        }
    }
}
