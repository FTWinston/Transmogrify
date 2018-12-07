using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Transmogrify.Engine;
using Xunit;

namespace Transmogrify.Tests
{
    public class PerformanceTests
    {
        const int iterations = 50000000;

        [Fact]
        public async Task TestObjectArraysAsync()
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };
                var outputs = await UseObjectArraysAsync(inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        [Fact]
        public void TestObjectArrays()
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };
                var outputs = UseObjectArrays(inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        [Fact]
        public void TestPureReflection()
        {
            var timer = new Stopwatch();

            var mi = GetType().GetMethod(nameof(StronglyTyped), BindingFlags.Static | BindingFlags.NonPublic);

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };

                var outputs = mi.Invoke(null, inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        [Fact]
        public void TestCompiled()
        {
            var timer = new Stopwatch();

            var mi = GetType().GetMethod(nameof(StronglyTyped), BindingFlags.Static | BindingFlags.NonPublic);

            Expression<Func<object[], object[]>> lambda = input => new object[] { StronglyTyped(input[0].ToString()) };

            var method = lambda.Compile();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };

                var outputs = method(inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        /*
        [Fact]
        public async Task TestCompiledAsync()
        {
            var timer = new Stopwatch();

            var mi = GetType().GetMethod(nameof(StronglyTypedAsync), BindingFlags.Static | BindingFlags.NonPublic);

            // "Async lambda expressions cannot be converted to expression trees"
            Expression<Func<object[], Task<object[]>>> lambda = async input => new object[] { await StronglyTypedAsync(input[0].ToString()) };

            var method = lambda.Compile();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };

                var outputs = await method(inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }
        */

        [Fact]
        public void TestDynamicMethod()
        {
            var timer = new Stopwatch();
            var mi = GetType().GetMethod(nameof(StronglyTyped), BindingFlags.Static | BindingFlags.NonPublic);

            FastInvokeHandler methodInvoker = FastMethodInvoker.GetMethodInvoker(mi);

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };

                var outputs = methodInvoker(null, inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }
        /*
        [Fact]
        public async Task TestDynamicMethodAsync()
        {
            var timer = new Stopwatch();
            var mi = GetType().GetMethod(nameof(StronglyTypedAsync), BindingFlags.Static | BindingFlags.NonPublic);

            FastInvokeHandler methodInvoker = FastMethodInvoker.GetMethodInvoker(mi);

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };

                var outputs = await methodInvoker(null, inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }
        */

        [Fact]
        public void TestDirect()
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var output = StronglyTyped("  hello");
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        [Fact]
        public async Task TestDirectAsync()
        {
            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var output = await StronglyTypedAsync("  hello");
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        [Fact]
        public void TestCompiledObjectArrays()
        {
            var timer = new Stopwatch();

            var mi = GetType().GetMethod(nameof(StronglyTyped), BindingFlags.Static | BindingFlags.NonPublic);


            Expression<Func<object[], object[]>> lambda = inputs => UseObjectArrays(inputs);

            var method = lambda.Compile();

            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                var inputs = new object[] { "  hello" };

                var outputs = method(inputs);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed.ToString());
        }

        private Task<object[]> UseObjectArraysAsync(object[] inputs)
        {
            var value = (inputs[0] as string).Trim();

            var results = new object[] { value };

            return Task.FromResult(results);
        }

        private object[] UseObjectArrays(object[] inputs)
        {
            var value = (inputs[0] as string).Trim();

            var results = new object[] { value };

            return results;
        }

        private static string StronglyTyped(string input)
        {
            return input.Trim();
        }

        private static Task<string> StronglyTypedAsync(string input)
        {
            return Task.FromResult(input.Trim());
        }
    }
}
