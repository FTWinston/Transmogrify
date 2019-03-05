using System.Collections.Generic;
using System.Drawing;

namespace Transmogrify.Data.EndPoints
{
    public class TSqlEndPoint : DataEndPoint<TSqlEndPoint.TSqlConfig>
    {
        public TSqlEndPoint(string name)
            : base(name)
        {

        }

        public override Color Color => Color.IndianRed;

        public override IEnumerable<EndPointDataCollection> PopulateCollections(Mapping mapping)
        {
            // TODO: read all tables from database
            // Collections has an entry for each table or view
            throw new System.NotImplementedException();
        }

        protected internal override DataItemReader GetReader(EndPointDataCollection collection)
        {
            throw new System.NotImplementedException();
        }

        protected internal override DataItemWriter GetWriter(EndPointDataCollection collection)
        {
            throw new System.NotImplementedException();
        }

        public class TSqlConfig
        {
            public string ConnectionString { get; set; }
        }
    }
}
