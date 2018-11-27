using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public abstract class DataCollection : MappingElement
    {
        protected DataCollection(string name, ComplexDataType itemType)
        {
            Name = name;
            ItemType = itemType;

            Fields = itemType.Fields
                .Select(f => new DataFieldInstance(this, f))
                .ToList();
        }

        protected DataCollection()
        {
            Fields = new List<DataFieldInstance>();
        }

        public string Name { get; set; }

        public ComplexDataType ItemType { get; set; }

        public List<DataFieldInstance> Fields { get; }

        public abstract DataItemReader GetReader();

        public abstract DataItemWriter GetWriter();
    }
}
