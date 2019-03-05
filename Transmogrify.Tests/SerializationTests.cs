using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Transmogrify.Data;
using Transmogrify.Data.Serialization;
using Xunit;

namespace Transmogrify.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeProject1()
        {
            var project = TestProjects.CreateBasicProject1();

            var strProject = ProjectSerialization.SerializeProject(project);
        }

        [Fact]
        public void SerializeProject2()
        {
            var project = TestProjects.CreateBasicProject2();

            var strProject = ProjectSerialization.SerializeProject(project);
        }

        [Fact]
        public void SerializeProject1Config()
        {
            var project = TestProjects.CreateBasicProject1();

            var strConfig = ProjectSerialization.SerializeProjectConfig(project);
        }

        [Fact]
        public void SerializeProject2Config()
        {
            var project = TestProjects.CreateBasicProject2();

            var strConfig = ProjectSerialization.SerializeProjectConfig(project);
        }

        [Theory]
        [InlineData("Transmogrify.Tests.Project001.json", "Transmogrify.Tests.Project001.config.json")]
        [InlineData("Transmogrify.Tests.Project002.json", "Transmogrify.Tests.Project002.config.json")]
        public Project DeserializeProject(string projectResourceName, string configResourceName)
        {
            string strProject;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(projectResourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                strProject = reader.ReadToEnd();
            }

            var project = ProjectSerialization.LoadFromString(strProject);

            string strConfig;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(configResourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                strConfig = reader.ReadToEnd();
            }

            ProjectSerialization.LoadConfigFromString(project, strConfig);

            return project;
        }

        [Fact]
        public void SerializeAndDeserializeProject1()
        {
            var project = TestProjects.CreateBasicProject1();

            var strProject = ProjectSerialization.SerializeProject(project);

            var project2 = ProjectSerialization.LoadFromString(strProject);
        }

        [Fact]
        public void SerializeAndDeserializeProject2()
        {
            var project = TestProjects.CreateBasicProject2();

            var strProject = ProjectSerialization.SerializeProject(project);

            var project2 = ProjectSerialization.LoadFromString(strProject);
        }

        [Fact]
        public void SerializeAndDeserializeProject1Config()
        {
            var project = TestProjects.CreateBasicProject1();

            var strConfig = ProjectSerialization.SerializeProjectConfig(project);

            ProjectSerialization.LoadConfigFromString(project, strConfig);
        }

        [Fact]
        public void SerializeAndDeserializeProject2Config()
        {
            var project = TestProjects.CreateBasicProject2();

            var strConfig = ProjectSerialization.SerializeProjectConfig(project);

            ProjectSerialization.LoadConfigFromString(project, strConfig);
        }

        [Theory]
        [InlineData("Transmogrify.Tests.Project001.json")]
        [InlineData("Transmogrify.Tests.Project002.json")]
        public void DeserializeAndSerializeProject(string resourceName)
        {
            string strProject;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                strProject = reader.ReadToEnd();
            }

            var project = ProjectSerialization.LoadFromString(strProject);

            var strProject2 = ProjectSerialization.SerializeProject(project);

            Assert.Equal(strProject, strProject2);
        }
    }
}
