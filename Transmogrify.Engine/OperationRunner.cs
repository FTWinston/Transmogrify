using System.Collections.Generic;
using System.Reflection;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class OperationRunner
    {
        private Dictionary<MethodInfo, MethodData> MethodCallData { get; }

        public OperationRunner()
        {
            MethodCallData = new Dictionary<MethodInfo, MethodData>();
        }
        
        public object[] Run(Operation operation, object[] inputValues)
        {
            // TODO: construct full parameter array. Use methodData.InputParameters.
            // If using out parameters, they won't be passed in here; add them in!
            
            var method = operation.Method;

            if (!MethodCallData.TryGetValue(method, out MethodData methodData))
            {
                methodData = MethodDataService.GetData(method);
                MethodCallData.Add(method, methodData);
            }

            var returnValue = methodData.Delegate(null, inputValues);

            // if return type is void, return value will be null; don't output that. (But do output any regular null return values.)
            var outputValues = methodData.OutputReturnType
                ? new object[] { }
                : new object[] { returnValue };

            // TODO: use methodData.OutputParameters

            return outputValues;
        }
    }
}
