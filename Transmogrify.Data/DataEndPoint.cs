using System.Collections.Generic;

namespace Transmogrify.Data
{
    public abstract class DataEndPoint
    {
        protected DataEndPoint(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<DataCollection> Collections { get; } = new List<DataCollection>();

        public abstract void PopulateCollections();

        protected internal abstract DataItemReader GetReader(EndPointDataCollection collection);
        protected internal abstract DataItemWriter GetWriter(EndPointDataCollection collection);
    }
}
