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

        // TODO: serializer is adding to default values here. Using array stops it, but that's not what we want.
        // Can use a static method to create these with a different constructor, or something to that effect.
        // A further problem is that the destination's fields aren't having their type written out - but the source fields are!
        public List<DataFieldInstance> Fields { get; }

        public abstract DataItemReader GetReader();

        public abstract DataItemWriter GetWriter();
    }
}
