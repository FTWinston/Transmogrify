using Newtonsoft.Json;
using System.Linq;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class MappingOperation : MappingElement
    {
        public MappingOperation(Mapping mapping, Operation operation)
            : base(mapping)
        {
            Operation = operation;

            Inputs = new DataFieldInstance[operation.Inputs.Length];

            Outputs = operation.Outputs.Select(o => new DataFieldInstance(this, o))
                .ToArray();
        }

        public Operation Operation { get; set; } // TODO: just serialize the type name of this operation

        public DataFieldInstance[] Inputs { get; } // These point at another element's fields

        public DataFieldInstance[] Outputs { get; } // These point at this element's fields

        public int X { get; set; }
        public int Y { get; set; }
    }
}
