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

            var strProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );
        }

        [Fact]
        public void SerializeProject2()
        {
            var project = TestProjects.CreateBasicProject2();

            var strProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );
        }

        [Theory]
        [InlineData("Transmogrify.Tests.Project001.json")]
        [InlineData("Transmogrify.Tests.Project002.json")]
        public Project DeserializeProject(string resourceName)
        {
            string strProject;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                strProject = reader.ReadToEnd();
            }

            var project = JsonConvert.DeserializeObject<Project>(strProject,
                ProjectSerialization.GetSerializerSettings()
            );

            return project;
        }

        [Fact]
        public void SerializeAndDeserializeProject1()
        {
            var project = TestProjects.CreateBasicProject1();

            var strProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );

            var project2 = JsonConvert.DeserializeObject<Project>(strProject,
                ProjectSerialization.GetSerializerSettings()
            );
        }

        [Fact]
        public void SerializeAndDeserializeProject2()
        {
            var project = TestProjects.CreateBasicProject2();

            var strProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );

            var project2 = JsonConvert.DeserializeObject<Project>(strProject,
                ProjectSerialization.GetSerializerSettings()
            );
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

            var project = JsonConvert.DeserializeObject<Project>(strProject,
                ProjectSerialization.GetSerializerSettings()
            );


            var strProject2 = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );

            Assert.Equal(strProject, strProject2);
        }
    }
}
