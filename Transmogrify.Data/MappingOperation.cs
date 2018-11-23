using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class MappingOperation : MappingElement
    {
        public MappingOperation(Mapping mapping, Operation operation)
            : base(mapping)
        {
            Operation = operation;
        }

        public Operation Operation { get; set; }

        public DataField[] Inputs { get; set; }
        public DataField[] Outputs { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
