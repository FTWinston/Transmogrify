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
        public OperationRunner OperationRunner { get; }
        private Dictionary<DataFieldInstance, object> DataValues { get; }

        public MappingRunner(Mapping mapping)
        {
            Mapping = mapping;
            OperationRunner = new OperationRunner();
            DataValues = new Dictionary<DataFieldInstance, object>();
        }

        public async Task<DataStructure> Run(DataStructure sourceItem)
        {
            DataValues.Clear();
            var destItem = new DataStructure(Mapping.Destination.ItemType);

            ReadSourceValues(sourceItem);

            foreach (var operationElement in Mapping.Operations)
            {
                await ProcessOperation(operationElement);
            }

            WriteDestinationValues(destItem);

            return destItem;
        }

        private void ReadSourceValues(DataStructure sourceItem)
        {
            foreach (var sourceField in Mapping.Source.Fields)
            {
                // TODO: this will only work for root-level fields, not nested / structured ones. Do something about that.
                DataValues[sourceField] = sourceItem.Values[sourceField.Field];
            }
        }

        private async Task ProcessOperation(Operation operation)
        {
            // TODO: make this more efficient!
            // Perhaps DataValues could use integer keys, and we store an array of inputDataIndices and outputDataIndices for each operation?
            var inputValues = operation.Inputs
                .Select(i =>
                {
                    if (!DataValues.TryGetValue(i, out object value))
                        throw new InvalidOperationException("Failed to find value in field " + i);
                    return value;
                })
                .ToArray();

            object[] outputValues = OperationRunner.Run(operation, inputValues);

            // TODO: can this be made more efficient?
            for (int i = 0; i < operation.Outputs.Length; i++)
            {
                var output = operation.Outputs[i];
                DataValues[output] = outputValues[i];
            }

            // TODO: step-through task for debugging could go here
            await Task.CompletedTask;
        }

        private void WriteDestinationValues(DataStructure destItem)
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
