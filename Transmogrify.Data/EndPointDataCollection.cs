namespace Transmogrify.Data
{
    public class EndPointDataCollection : DataCollection
    {
        public EndPointDataCollection(Mapping mapping, DataEndPoint endPoint, string name, ComplexDataType type)
            : base(mapping, name, type)
        {
            EndPoint = endPoint;
        }

        public DataEndPoint EndPoint { get; } // TODO: just serialize an ID

        public override DataItemReader GetReader() => EndPoint.GetReader(this);

        public override DataItemWriter GetWriter() => EndPoint.GetWriter(this);
    }
}
