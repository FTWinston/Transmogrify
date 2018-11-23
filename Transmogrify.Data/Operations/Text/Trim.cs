using System.Threading.Tasks;

namespace Transmogrify.Data.Operations.Text
{
    public class Trim : Operation
    {
        public override DataType[] InputTypes => new[]
        {
            SimpleDataType.String
        };

        public override DataType[] OutputTypes => new[]
        {
            SimpleDataType.String
        };

        public override Task<object[]> Perform(object[] inputs)
        {
            var value = (inputs[0] as string).Trim();

            var results = new object[] { value };

            return Task.FromResult(results);
        }
    }
}
