using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public abstract class DataCollection : MappingElement
    {
        public DataCollection(Mapping mapping, string name, ComplexDataType type)
            : base(mapping)
        {
            Name = name;
            ItemType = type;

            Fields = type.Fields.Select(f => new DataFieldInstance(this, f))
                .ToArray();
        }

        public string Name { get; }

        public ComplexDataType ItemType { get; }

        public IList<DataFieldInstance> Fields { get; }

        public abstract DataItemReader GetReader();

        public abstract DataItemWriter GetWriter();
    }
}
