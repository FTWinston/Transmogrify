using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Transmogrify.Data;
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

        private async Task RunProject(Project project, string expectedText)
        {
            try
            {
                File.WriteAllText(TestProjects.SourceTextFile, sourceText);

                var runner = new ProjectRunner();
                await runner.Run(project);

                var destText = File.ReadAllText(TestProjects.DestinationTextFile);

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

        [Fact]
        public async Task RunProject1()
        {
            var project = TestProjects.CreateBasicProject1();

            await RunProject(project, sourceText);
        }

        [Fact]
        public async Task RunProject2()
        {
            var project = TestProjects.CreateBasicProject2();

            var expectedText = string.Join(Environment.NewLine,
                sourceText.Split(Environment.NewLine)
                    .Select(l => l.Trim())
            );

            await RunProject(project, expectedText);
        }

        [Fact]
        public async Task DeserializeAndRunProject1()
        {
            string resourceName = "Transmogrify.Tests.Project001.json";
            var project = new SerializationTests().DeserializeProject(resourceName);

            await RunProject(project, sourceText);
        }

        [Fact]
        public async Task DeserializeAndRunProject2()
        {
            string resourceName = "Transmogrify.Tests.Project002.json";
            var project = new SerializationTests().DeserializeProject(resourceName);

            var expectedText = string.Join(Environment.NewLine,
                sourceText.Split(Environment.NewLine)
                    .Select(l => l.Trim())
            );

            await RunProject(project, expectedText);
        }
    }
}
