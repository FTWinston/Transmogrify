using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public abstract class DataEndPoint
    {
        public string Name { get; set; }

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

        public TConfig Configuration { get; set; }
    }
}
