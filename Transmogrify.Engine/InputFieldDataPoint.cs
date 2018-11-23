using Transmogrify.Data;

namespace Transmogrify.Engine
{
    internal class InputFieldDataPoint : DataPoint
    {
        public InputFieldDataPoint(DataField field)
        {
            Field = field;
        }

        public override DataType DataType => Field.Type;

        public DataField Field { get; }
    }
}
