using Newtonsoft.Json;

namespace Transmogrify.Data
{
    public class EndPointDataCollection : DataCollection
    {
        public EndPointDataCollection(DataEndPoint endPoint, string name, DataStructureType itemType)
            : base(name, itemType)
        {
            EndPoint = endPoint;
        }

        [JsonConstructor]
        private EndPointDataCollection()
        {
            
        }

        public DataEndPoint EndPoint { get; set; }

        public override DataItemReader GetReader() => EndPoint.GetReader(this);

        public override DataItemWriter GetWriter() => EndPoint.GetWriter(this);
    }
}
