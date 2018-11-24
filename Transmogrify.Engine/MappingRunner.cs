using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class MappingRunner
    {
        public Mapping Mapping { get; }
        private Dictionary<DataFieldInstance, object> DataValues { get; }

        public MappingRunner(Mapping mapping)
        {
            Mapping = mapping;
            DataValues = new Dictionary<DataFieldInstance, object>();
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

        private async Task ProcessOperation(MappingOperation operationElement)
        {
            var operation = operationElement.Operation;

            // TODO: make this more efficient!
            var inputValues = operationElement.Inputs
                .Select(i =>
                {
                    if (!DataValues.TryGetValue(i, out object value))
                        throw new InvalidOperationException("Failed to find value in field " + i);
                    return value;
                })
                .ToArray();

            var outputValues = await operation.Perform(inputValues);

            for (int i = 0; i < operation.Outputs.Length; i++)
            {
                var output = operationElement.Outputs[i];
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
