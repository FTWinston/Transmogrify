using System;
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
            var methodInfo = operation.Method;

            if (!MethodCallData.TryGetValue(methodInfo, out MethodData methodData))
            {
                var callDelegate = MethodInvokerService.GetInvoker(methodInfo);

                methodData = new MethodData(methodInfo, callDelegate);

                MethodCallData.Add(methodInfo, methodData);
            }

            object[] parameters = GetParameters(methodData, inputValues);

            var returnValue = methodData.Delegate(null, parameters);

            return GetOutputs(methodData, returnValue, parameters);
        }

        private object[] GetParameters(MethodData methodData, object[] inputValues)
        {
            if (methodData.UseInputsDirectly)
                return inputValues;

            // populate, leaving blanks for any out parameters
            object[] parameters = new object[methodData.InputParameters.Length];

            int iRead = 0;
            for (int iParam = 0; iParam < parameters.Length; iParam++)
            {
                if (methodData.InputParameters[iParam])
                {
                    parameters[iParam] = inputValues[iRead++];
                }
            }

            return parameters;
        }

        private object[] GetOutputs(MethodData methodData, object returnValue, object[] parameters)
        {
            if (methodData.ReturnParametersDirectly)
                return parameters;

            // if return type is void, return value will be null; don't output that. (But do output any regular null return values.)
            var outputValues = new object[methodData.NumOutputs];

            int iWrite;
            if (methodData.OutputReturnType)
            {
                outputValues[0] = returnValue;
                iWrite = 1;
            }
            else
                iWrite = 0;

            // populate outputValues from parameters, for indexes where methodData.OutputParameters is true
            for (int iParam = 0; iParam < parameters.Length; iParam++)
            {
                if (methodData.OutputParameters[iParam])
                {
                    outputValues[iWrite++] = parameters[iParam];
                }
            }

            return outputValues;
        }
    }
}
