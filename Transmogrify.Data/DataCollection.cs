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

        public abstract DataItemReader GetReader();

        public abstract DataItemWriter GetWriter();
    }
}
