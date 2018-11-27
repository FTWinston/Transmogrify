using Newtonsoft.Json;
using System;
using System.Linq;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class MappingOperation : MappingElement
    {
        public MappingOperation(Operation operation)
        {
            Operation = operation;

            Inputs = new DataFieldInstance[operation.Inputs.Length];

            Outputs = operation.Outputs.Select(o => new DataFieldInstance(this, o))
                .ToArray();
        }

        [JsonConstructor]
        private MappingOperation(Type operationType)
            : this(Activator.CreateInstance(operationType) as Operation)
        {

        }

        [JsonIgnore]
        public Operation Operation { get; set; }

        [JsonProperty(PropertyName = "Operation")]
        private Type OperationType
        {
            get
            {
                return Operation.GetType();
            }
        }

        public DataFieldInstance[] Inputs { get; } // These point at another element's fields

        public DataFieldInstance[] Outputs { get; } // These point at this element's fields

        public int X { get; set; }
        public int Y { get; set; }
    }
}
