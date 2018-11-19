using System.Collections.Generic;

namespace Transmogrify
{
    public abstract class DataEndPoint
    {
        protected DataEndPoint(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<CollectionDataType> Collections { get; } = new List<CollectionDataType>();

        public abstract void PopulateCollections();
    }
}
