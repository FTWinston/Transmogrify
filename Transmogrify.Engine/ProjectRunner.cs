using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transmogrify.Data;

namespace Transmogrify.Engine
{
    public class ProjectRunner
    {
        public async Task Run(Project project)
        {
            var migrationQueue = DetermineMigrationOrder(project);

            foreach (var migration in migrationQueue)
            {
                await RunMigration(migration);
            }
        }

        public Queue<Migration> DetermineMigrationOrder(Project project)
        {
            // TODO: actually put thought into this, in terms of intermediate collections, etc.
            return new Queue<Migration>(project.Migrations);
        }

        protected async Task RunMigration(Migration migration)
        {
            using (var writer = migration.Destination.GetWriter())
            {
                using (var reader = migration.Source.GetReader())
                {
                    while (reader.MoveNext())
                    {
                        var sourceItem = reader.Current;
                        var destItem = await TransformDataItem(migration, sourceItem);
                        writer.Write(destItem);
                    }
                }

                writer.Flush();
            }
        }

        protected async Task<ComplexDataItem> TransformDataItem(Migration migration, ComplexDataItem sourceItem)
        {
            var destItem = new ComplexDataItem(migration.Destination.ItemType);

            // TODO: apply migration's operations to populate the destination item's fields, I guess?

            return destItem;
        }
    }
}
