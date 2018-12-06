using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class MappingOperation : MappingElement
    {
        public MappingOperation(Operation operation)
        {
            Operation = operation;

            RawInputs = new DataFieldInstance[operation.Inputs.Length];

            RawOutputs = operation.Outputs.Select(o => new MappingOperationOutputFieldInstance(this, o))
                .ToArray();
        }

        [JsonConstructor]
        private MappingOperation(Type operationType)
        {
            Operation = Activator.CreateInstance(operationType) as Operation;
        }

        [JsonIgnore]
        public Operation Operation { get; set; }

        [JsonProperty(PropertyName = "Operation")]
        private Type OperationType => Operation.GetType();

        [JsonIgnore]
        public DataFieldInstance[] Inputs => RawInputs; // These point at another element's fields

        [JsonProperty(PropertyName = "Inputs")]
        private DataFieldInstance[] RawInputs { get; set; }

        [JsonIgnore]
        public DataFieldInstance[] Outputs => RawOutputs; // These point at this element's fields

        [JsonProperty(PropertyName = "Outputs")]
        private MappingOperationOutputFieldInstance[] RawOutputs { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            // Once deserialized, set set the element of every output to this.
            // These weren't serialized, as this is a "parent" of its outputs in the JSON.

            if (RawOutputs == null)
                return;

            foreach (var output in RawOutputs)
                if (output != null)
                    output.Element = this;
        }
    }
}
