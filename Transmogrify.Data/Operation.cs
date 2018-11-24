using System.Threading.Tasks;

namespace Transmogrify.Data
{
    public abstract class Operation
    {
        public abstract DataField[] Inputs { get; }
        public abstract DataField[] Outputs { get; }

        public abstract Task<object[]> Perform(object[] inputs);
    }
}
