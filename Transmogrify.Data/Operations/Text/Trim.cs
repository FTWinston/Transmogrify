using System.Threading.Tasks;

namespace Transmogrify.Data.Operations.Text
{
    public class Trim : Operation
    {
        public override DataField[] Inputs { get; } = new[]
        {
            new DataField("Value", SimpleDataType.String)
        };

        public override DataField[] Outputs { get; } = new[]
        {
            new DataField("Value", SimpleDataType.String)
        };

        public override Task<object[]> Perform(object[] inputs)
        {
            var value = (inputs[0] as string).Trim();

            var results = new object[] { value };

            return Task.FromResult(results);
        }
    }
}
