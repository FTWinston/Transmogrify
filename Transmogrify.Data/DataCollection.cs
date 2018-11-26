using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public abstract class DataCollection : MappingElement
    {
        public DataCollection(string name, ComplexDataType itemType)
            : base()
        {
            Name = name;
            ItemType = itemType;

            Fields = itemType.Fields
                .Select(f => new DataFieldInstance(this, f))
                .ToList();
        }

        public string Name { get; }

        public ComplexDataType ItemType { get; }

        public List<DataFieldInstance> Fields { get; }

        public abstract DataItemReader GetReader();

        public abstract DataItemWriter GetWriter();
    }
}
