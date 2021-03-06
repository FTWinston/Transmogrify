using System;
using System.Linq;
using System.Reflection;
using Transmogrify.Data;
using Transmogrify.Data.EndPoints;

namespace Transmogrify.Tests
{
    public class TestProjects
    {
        public const string SourceTextFile = "source.txt";
        public const string DestinationTextFile = "dest.txt";

        public static Project CreateBasicProject1()
        {
            Project project = new Project();

            var source = new PlainTextEndPoint("Source");
            source.Configuration.FilePath = SourceTextFile;
            project.EndPoints.Add(source);

            var dest = new PlainTextEndPoint("Destination");
            dest.Configuration.FilePath = DestinationTextFile;
            project.EndPoints.Add(dest);

            Mapping mapping = new Mapping();
            mapping.Name = "Empty mapping";
            var sourceCollection = source.PopulateCollections(mapping).First();
            var destCollection = dest.PopulateCollections(mapping).First();
            mapping.Source = sourceCollection;
            mapping.Destination = destCollection;

            mapping.Outputs.Add(new MappingOutput(sourceCollection.Fields.First(), destCollection.Fields.First().Field));

            project.Mappings.Add(mapping);

            return project;
        }

        public static Project CreateBasicProject2()
        {
            Project project = new Project();

            var source = new PlainTextEndPoint("Source");
            source.Configuration.FilePath = SourceTextFile;
            project.EndPoints.Add(source);

            var dest = new PlainTextEndPoint("Destination");
            dest.Configuration.FilePath = DestinationTextFile;
            project.EndPoints.Add(dest);

            Mapping mapping = new Mapping();
            mapping.Name = "Basic mapping";
            var sourceCollection = source.PopulateCollections(mapping).First();
            var destCollection = dest.PopulateCollections(mapping).First();
            mapping.Source = sourceCollection;
            mapping.Destination = destCollection;

            var operation = new Operation(GetMethodInfo((Func<string, string>)Transmogrify.Operations.Text.Trim));
            mapping.Operations.Add(operation);
            operation.Inputs[0] = sourceCollection.Fields.First();

            mapping.Outputs.Add(new MappingOutput(operation.Outputs.First(), destCollection.Fields.First().Field));

            project.Mappings.Add(mapping);

            return project;
        }

        static MethodInfo GetMethodInfo(Delegate d)
        {
            return d.Method;
        }
    }
}
