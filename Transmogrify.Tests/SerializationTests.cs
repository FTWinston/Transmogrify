using Newtonsoft.Json;
using System;
using System.Linq;
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

            mapping.Outputs.Add(new MappingOutput(mapping, sourceCollection.Fields.First(), destCollection.Fields.First().Field));

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

        [Fact]
        public void DeserializeProject()
        {
            var strProject = @"
{
  ""EndPoints"": [
    {
      ""$id"": ""1"",
      ""$type"": ""Transmogrify.Data.EndPoints.PlainTextEndPoint, Transmogrify.Data"",
      ""Name"": ""Source"",
      ""Configuration"": {
        ""FilePath"": ""source.txt""
      }
    },
    {
      ""$id"": ""2"",
      ""$type"": ""Transmogrify.Data.EndPoints.PlainTextEndPoint, Transmogrify.Data"",
      ""Name"": ""Destination"",
      ""Configuration"": {
        ""FilePath"": ""dest.txt""
      }
    }
  ],
  ""Mappings"": [
    {
      ""Source"": {
        ""$id"": ""3"",
        ""EndPoint"": {
          ""$ref"": ""1""
        },
        ""Name"": ""Lines"",
        ""ItemType"": {
          ""Fields"": [
            {
              ""$id"": ""4"",
              ""Name"": ""Value"",
              ""Type"": {
                ""$id"": ""5"",
                ""ActualType"": ""System.String, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"",
                ""Name"": ""String"",
                ""IsSimple"": true
              }
            }
          ],
          ""Name"": ""Line"",
          ""IsSimple"": false
        },
        ""Fields"": [
          {
            ""$id"": ""6"",
            ""Element"": {
              ""$ref"": ""3""
            },
            ""Field"": {
              ""$ref"": ""4""
            }
          }
        ]
      },
      ""Destination"": {
        ""$id"": ""7"",
        ""EndPoint"": {
          ""$ref"": ""2""
        },
        ""Name"": ""Lines"",
        ""ItemType"": {
          ""Fields"": [
            {
              ""$id"": ""8"",
              ""Name"": ""Value"",
              ""Type"": {
                ""$ref"": ""5""
              }
            }
          ],
          ""Name"": ""Line"",
          ""IsSimple"": false
        },
        ""Fields"": [
          {
            ""$id"": ""9"",
            ""Element"": {
              ""$ref"": ""7""
            },
            ""Field"": {
              ""$ref"": ""8""
            }
          }
        ]
      },
      ""Operations"": [],
      ""Outputs"": [
        {
          ""Source"": {
            ""$ref"": ""6""
          },
          ""Output"": {
            ""$ref"": ""8""
          }
        }
      ]
    }
  ]
}
";
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
    }
}
