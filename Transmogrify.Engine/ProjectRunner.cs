using System.Collections.Generic;
using System.Threading.Tasks;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class ProjectRunner
    {
        public async Task Run(Project project)
        {
            var migrationQueue = DetermineMappingOrder(project);

            foreach (var migration in migrationQueue)
            {
                await RunMapping(migration);
            }
        }

        public Queue<Mapping> DetermineMappingOrder(Project project)
        {
            // TODO: actually put thought into this, in terms of intermediate collections, etc.
            // ... hmm, this probably should be done at design/saving time, same as deciding a mapping's operation order.
            return new Queue<Mapping>(project.Mappings);
        }

        protected async Task RunMapping(Mapping mapping)
        {
            // TODO: Allow this to run multi-threaded, make one MappingRunner per thread (and re-use them)
            var mappingRunner = new MappingRunner(mapping);

            using (var writer = mapping.Destination.GetWriter())
            {
                using (var reader = mapping.Source.GetReader())
                {
                    while (reader.MoveNext())
                    {
                        var sourceItem = reader.Current;
                        var destItem = await mappingRunner.Run(sourceItem);
                        writer.Write(destItem);
                    }
                }

                writer.Flush();
            }
        }
    }
}
