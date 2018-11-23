namespace Transmogrify.Data
{
    public class MappingCollection : MappingElement
    {
        public MappingCollection(Mapping mapping, DataCollection dataCollection)
            : base(mapping)
        {
            DataCollection = dataCollection;
        }

        public DataCollection DataCollection { get; set; }
    }
}
