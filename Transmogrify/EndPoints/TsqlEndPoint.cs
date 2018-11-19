namespace Transmogrify.EndPoints
{
    public class TsqlEndPoint : DataEndPoint
    {
        public TsqlEndPoint(string name)
            : base(name)
        {

        }

        public override void PopulateCollections()
        {
            // TODO: read all tables from database
            // Collections has an entry for each table or view
            throw new System.NotImplementedException();
        }

        // configuration options

        public string ConnectionString { get; set; }
    }
}
