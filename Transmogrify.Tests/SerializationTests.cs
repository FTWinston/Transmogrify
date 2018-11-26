using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Transmogrify.Data;
using Transmogrify.Data.EndPoints;
using Transmogrify.Data.Serialization;
using Xunit;

namespace Transmogrify.Tests
{
    public class SerializationTests
    {
        private Project CreateBasicProject()
        {
            Project project = new Project();

            var source = new PlainTextEndPoint("Source");
            source.Configuration.FilePath = "source.txt";
            project.EndPoints.Add(source);

            var dest = new PlainTextEndPoint("Destination");
            dest.Configuration.FilePath = "dest.txt";
            project.EndPoints.Add(dest);

            Mapping mapping = new Mapping();
            var sourceCollection = source.PopulateCollections(mapping).First();
            var destCollection = dest.PopulateCollections(mapping).First();
            mapping.Source = sourceCollection;
            mapping.Destination = destCollection;

            mapping.Outputs.Add(new MappingOutput(sourceCollection.Fields.First(), destCollection.Fields.First().Field));

            project.Mappings.Add(mapping);

            return project;
        }

        [Fact]
        public void SerializeProject()
        {
            var project = CreateBasicProject();

            var strProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );
        }

        [Theory]
        [InlineData("Transmogrify.Tests.Project001.json")]
        public void DeserializeProject(string resourceName)
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
        }

        [Fact]
        public void SerializeAndDeserializeProject()
        {
            var project = CreateBasicProject();

            var strProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                ProjectSerialization.GetSerializerSettings()
            );

            var project2 = JsonConvert.DeserializeObject<Project>(strProject,
                ProjectSerialization.GetSerializerSettings()
            );
        }

        [Theory]
        [InlineData("Transmogrify.Tests.Project001.json")]
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
