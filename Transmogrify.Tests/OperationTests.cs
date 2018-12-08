using System.Reflection;
using Transmogrify.Data;
using Transmogrify.Engine;
using Transmogrify.Operations;
using Xunit;

namespace Transmogrify.Tests
{
    public class OperationTests
    {
        [Fact]
        public void TestSingleInputOutput()
        {
            var operation = new Operation(
                GetType().GetMethod(nameof(Trim), BindingFlags.Static | BindingFlags.NonPublic)
            );

            var inputs = new object[] { "  hello" };

            var outputs = new OperationRunner().Run(operation, inputs);

            Assert.Equal("  hello", inputs[0].ToString());
            Assert.Single(outputs);
            Assert.NotNull(outputs[0]);
            Assert.Equal(inputs[0].ToString().Trim(), outputs[0].ToString());
        }

        [Fact]
        public void TestNoInputNoOutput()
        {
            var operation = new Operation(
                GetType().GetMethod(nameof(DoSomething), BindingFlags.Static | BindingFlags.NonPublic)
            );

            var inputs = new object[] { };

            var outputs = new OperationRunner().Run(operation, inputs);

            Assert.Empty(outputs);
        }

        [Fact]
        public void TestSingleInputNoOutput()
        {
            var operation = new Operation(
                GetType().GetMethod(nameof(DoSomethingWith), BindingFlags.Static | BindingFlags.NonPublic)
            );

            var inputs = new object[] { "  hello" };

            var outputs = new OperationRunner().Run(operation, inputs);

            Assert.Empty(outputs);
        }

        [Fact]
        public void TestRefNoOutput()
        {
            var operation = new Operation(
                GetType().GetMethod(nameof(TrimRef), BindingFlags.Static | BindingFlags.NonPublic)
            );

            var inputs = new object[] { "  hello" };

            var outputs = new OperationRunner().Run(operation, inputs);

            Assert.Equal("  hello", inputs[0].ToString());
            Assert.Single(outputs);
            Assert.NotNull(outputs[0]);
            Assert.Equal(inputs[0].ToString().Trim(), outputs[0].ToString());
        }


        static void DoSomethingWith(string input)
        {
            input.Trim();
        }

        static void DoSomething()
        {

        }

        static string Trim(string input)
        {
            return input.Trim();
        }

        static void TrimRef(ref string input)
        {
            input = input.Trim();
        }
    }
}
