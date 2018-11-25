using Newtonsoft.Json;
using System;
using System.Linq;
using Transmogrify.Data;
using Transmogrify.Data.EndPoints;
using Xunit;

namespace Transmogrify.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeProject()
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

            mapping.Outputs.Add(new MappingOutput(mapping, sourceCollection.Fields.First(), destCollection.Fields.First().Field));

            project.Mappings.Add(mapping);


            var strMapping = JsonConvert.SerializeObject(project, Formatting.Indented,
                new JsonSerializerSettings { /* PreserveReferencesHandling = PreserveReferencesHandling.Objects */ }
            );


        }
    }
}
