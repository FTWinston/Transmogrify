namespace Transmogrify
{
    public class CollectionDataType : DataType
    {
        public CollectionDataType(string name, DataType type)
            : base(name, false)
        {
            ElementType = type;
        }

        public DataType ElementType { get; }
    }
}
