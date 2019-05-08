using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public abstract class DataEndPoint
    {
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonIgnore]
        public abstract string TypeName { get; }

        [JsonIgnore]
        public abstract Color Color { get; }

        public abstract IEnumerable<EndPointDataCollection> PopulateCollections(Mapping mapping);

        protected internal abstract DataItemReader GetReader(EndPointDataCollection collection);

        protected internal abstract DataItemWriter GetWriter(EndPointDataCollection collection);
    }

    public abstract class DataEndPoint<TConfig> : DataEndPoint
        where TConfig : new()
    {
        protected DataEndPoint(string name)
        {
            Name = name;
            Configuration = new TConfig();
        }

        [JsonProperty(Order = 2)]
        public TConfig Configuration { get; set; }
    }
}
