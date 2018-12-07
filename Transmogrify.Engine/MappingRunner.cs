using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class MappingRunner
    {
        public Mapping Mapping { get; }
        private Dictionary<DataFieldInstance, object> DataValues { get; }
        private Dictionary<MethodInfo, FastInvokeHandler> MethodInvokers { get; }

        public MappingRunner(Mapping mapping)
        {
            Mapping = mapping;
            DataValues = new Dictionary<DataFieldInstance, object>();
            MethodInvokers = new Dictionary<MethodInfo, FastInvokeHandler>();
        }

        public async Task<ComplexDataItem> Run(ComplexDataItem sourceItem)
        {
            DataValues.Clear();
            var destItem = new ComplexDataItem(Mapping.Destination.ItemType);

            ReadSourceValues(sourceItem);

            foreach (var operationElement in Mapping.Operations)
            {
                await ProcessOperation(operationElement);
            }

            WriteDestinationValues(destItem);

            return destItem;
        }

        private void ReadSourceValues(ComplexDataItem sourceItem)
        {
            foreach (var sourceField in Mapping.Source.Fields)
            {
                // TODO: this will only work for root-level fields, not nested / structured ones. Do something about that.
                DataValues[sourceField] = sourceItem.Values[sourceField.Field];
            }
        }

        private async Task ProcessOperation(MappingOperation operation)
        {
            var method = operation.Method;

            if (!MethodInvokers.TryGetValue(method, out FastInvokeHandler invoker))
            {
                invoker = FastMethodInvoker.GetMethodInvoker(method);
                MethodInvokers.Add(method, invoker);
            }

            // TODO: make this more efficient!
            var inputValues = operation.Inputs
                .Select(i =>
                {
                    if (!DataValues.TryGetValue(i, out object value))
                        throw new InvalidOperationException("Failed to find value in field " + i);
                    return value;
                })
                .ToArray();

            // TODO: step-through task for debugging could go here
            await Task.CompletedTask;

            var returnValue = invoker(null, inputValues);
            var outputValues = new object[] { returnValue };

            for (int i = 0; i < operation.Outputs.Length; i++)
            {
                var output = operation.Outputs[i];
                object outputValue = outputValues[i];
                DataValues[output] = outputValue;
            }
        }

        private void WriteDestinationValues(ComplexDataItem destItem)
        {
            foreach (var outputElement in Mapping.Outputs)
            {
                if (!DataValues.TryGetValue(outputElement.Source, out object value))
                {
                    throw new InvalidOperationException("Failed to find value in field " + outputElement.Source);
                }

                destItem.Values[outputElement.Output] = value;
            }
        }
    }
}
