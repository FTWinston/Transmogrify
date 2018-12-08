using System.Collections.Generic;
using System.Reflection;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class OperationRunner
    {
        private Dictionary<MethodInfo, FastInvokeHandler> MethodInvokers { get; }

        public OperationRunner()
        {
            MethodInvokers = new Dictionary<MethodInfo, FastInvokeHandler>();
        }
        
        public object[] Run(Operation operation, object[] inputValues)
        {
            // TODO: if using out parameters, they won't be passed in here; add them in!

            var method = operation.Method;

            if (!MethodInvokers.TryGetValue(method, out FastInvokeHandler invoker))
            {
                // TODO: invoker should include info on return type being passed out (bool) and parameters being passed out (as out or ref params)

                invoker = FastMethodInvoker.GetMethodInvoker(method);
                MethodInvokers.Add(method, invoker);
            }

            var returnValue = invoker(null, inputValues);

            // if return type is void, return value will be null; don't output that. (But do output any regular null return values.)
            var outputValues = method.ReturnType == typeof(void)
                ? new object[] { }
                : new object[] { returnValue };

            // TODO: account for out and ref parameters

            return outputValues;
        }
    }
}
