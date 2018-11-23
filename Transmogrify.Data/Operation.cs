using System.Threading.Tasks;

namespace Transmogrify.Data
{
    public abstract class Operation
    {
        public abstract DataType[] InputTypes { get; }
        public abstract DataType[] OutputTypes { get; }

        public abstract Task<object[]> Perform(object[] inputs);
    }
}
