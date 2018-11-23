namespace Transmogrify.Data
{
    public class EndPointDataCollection : DataCollection
    {
        public EndPointDataCollection(DataEndPoint endPoint, string name, ComplexDataType type)
            : base(name, type)
        {
            EndPoint = endPoint;
        }

        public DataEndPoint EndPoint { get; }

        public override DataItemReader GetReader() => EndPoint.GetReader(this);

        public override DataItemWriter GetWriter() => EndPoint.GetWriter(this);
    }
}
