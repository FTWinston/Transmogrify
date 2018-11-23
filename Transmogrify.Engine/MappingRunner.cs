using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class MappingRunner
    {
        public Mapping Mapping { get; }

        public MappingRunner(Mapping mapping)
        {
            Mapping = mapping;

            InputDataPoints = new List<InputFieldDataPoint>(
                Mapping.Source.ItemType.Fields.Select(f => new InputFieldDataPoint(f))
            );

            OtherDataPoints = new List<OperationDataPoint>(
                Mapping.Elements.Select(e => new OperationDataPoint(e.Operation, e.Inputs))
            );

            // TODO: populate OtherDataPoints based on mapping elements
        }

        private List<InputFieldDataPoint> InputDataPoints { get; }
        private List<OperationDataPoint> OtherDataPoints { get; }

        public async Task<ComplexDataItem> Run(ComplexDataItem sourceItem)
        {
            foreach (var input in InputDataPoints)
            {
                // TODO: this will only work for root-level fields, not nested / structured ones. Do something about that.
                var value = sourceItem.Values.First(f => f.Key == input.Field).Value;
                input.SetValue(value);
            }

            var destItem = new ComplexDataItem(Mapping.Destination.ItemType);

            foreach (var dataPoint in OtherDataPoints)
            {
                var inputs = dataPoint.Inputs.Select(i => i.Value).ToArray(); // TODO: make this less inefficient
                var outputs = await dataPoint.Operation.Perform(inputs);
                dataPoint.SetValue(outputs);
            }

            // TODO: need something (that isn't a data point) to populate the outputs

            return destItem;
        }
    }
}
