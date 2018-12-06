using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Transmogrify.Engine;
using Xunit;

namespace Transmogrify.Tests
{
    public class ExecutionTests
    {

        const string sourceText = @"blah
blah blah
  blahblah
blah 
";

        [Fact]
        public async Task RunProject1()
        {
            try
            {
                File.WriteAllText(TestProjects.SourceTextFile, sourceText);

                var project = TestProjects.CreateBasicProject1();

                var runner = new ProjectRunner();
                await runner.Run(project);

                var destText = File.ReadAllText(TestProjects.DestinationTextFile);

                Assert.Equal(sourceText, destText);
            }
            finally
            {
                if (File.Exists(TestProjects.SourceTextFile))
                    File.Delete(TestProjects.SourceTextFile);

                if (File.Exists(TestProjects.DestinationTextFile))
                    File.Delete(TestProjects.DestinationTextFile);
            }
        }

        [Fact]
        public async Task RunProject2()
        {
            try
            {
                File.WriteAllText(TestProjects.SourceTextFile, sourceText);

                var project = TestProjects.CreateBasicProject2();

                var runner = new ProjectRunner();
                await runner.Run(project);

                var destText = File.ReadAllText(TestProjects.DestinationTextFile);

                var expectedText = string.Join(Environment.NewLine,
                    destText.Split(Environment.NewLine)
                        .Select(l => l.Trim())
                );

                Assert.Equal(expectedText, destText);
            }
            finally
            {
                if (File.Exists(TestProjects.SourceTextFile))
                    File.Delete(TestProjects.SourceTextFile);

                if (File.Exists(TestProjects.DestinationTextFile))
                    File.Delete(TestProjects.DestinationTextFile);
            }
        }
    }
}
