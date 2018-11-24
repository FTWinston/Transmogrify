using System.Collections.Generic;

namespace Transmogrify.Data
{
    public abstract class DataEndPoint
    {
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

        public string Name { get; set; }
        public TConfig Configuration { get; set; }

    }
}
