using System.Collections;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public abstract class DataCollection
    {
        public DataCollection(string name, ComplexDataType type)
        {
            Name = name;
            ItemType = type;
        }

        public string Name { get; }
        public ComplexDataType ItemType { get; }

        public abstract IEnumerator<ComplexDataItem> GetReader();

        public abstract DataItemWriter GetWriter();
    }
}
