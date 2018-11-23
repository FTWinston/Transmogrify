using System.Linq;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    internal class OperationDataPoint : DataPoint
    {
        public OperationDataPoint(Operation operation, DataPoint[] inputs)
        {
            Operation = operation;
        }

        public Operation Operation { get; }

        public override DataType DataType => Operation.OutputTypes.First(); // TODO: shall we make each operation only output one item?

        public DataPoint[] Inputs { get; set; }
    }
}
