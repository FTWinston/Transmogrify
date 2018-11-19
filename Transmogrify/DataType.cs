namespace Transmogrify
{
    public abstract class DataType
    {
        protected DataType(string name, bool isSimple)
        {
            Name = name;
            IsSimple = isSimple;
        }

        public string Name { get; }
        public bool IsSimple { get; }
    }
}
