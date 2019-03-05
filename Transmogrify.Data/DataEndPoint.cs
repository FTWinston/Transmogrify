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
        public abstract Color Color { get; }

        public abstract IEnumerable<EndPointDataCollection> PopulateCollections(Mapping mapping);

        protected internal abstract DataItemReader GetReader(EndPointDataCollection collection);

        protected internal abstract DataItemWriter GetWriter(EndPointDataCollection collection);

        internal abstract bool TrySetConfiguration(object configuration);

        internal abstract object GetConfiguration();
    }

    public abstract class DataEndPoint<TDesign, TConfig> : DataEndPoint
        where TDesign : new()
        where TConfig : new()
    {
        protected DataEndPoint(string name)
        {
            Name = name;
            Design = new TDesign();
            Configuration = new TConfig();
        }

        [JsonProperty(Order = 2)]
        public TDesign Design { get; set; }

        [JsonIgnore]
        public TConfig Configuration { get; set; }

        internal override bool TrySetConfiguration(object configuration)
        {
            if (configuration is TConfig tconfig)
            {
                Configuration = tconfig;
                return true;
            }

            return false;
        }

        internal override object GetConfiguration()
        {
            return Configuration;
        }
    }
}
