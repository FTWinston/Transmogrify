using Transmogrify.Data;

namespace Transmogrify.Engine
{
    internal abstract class DataPoint
    {
        public bool Populated { get; private set; }
        public abstract DataType DataType { get; }
        public object Value { get; private set; }

        public void SetValue(object value)
        {
            Value = value;
            Populated = true;
        }

        public void Clear()
        {
            Populated = false;
            Value = null;
        }
    }
}
