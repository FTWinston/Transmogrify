namespace Transmogrify.EndPoints
{
    public class CsvEndPoint : DataEndPoint
    {
        public CsvEndPoint(string name)
            : base(name)
        {

        }

        public override void PopulateCollections()
        {
            // TODO: read all field headings from source file ... just call them Column 1 ... X if HasHeaders is false
            // Collections has a single item, representing the file contents...

            throw new System.NotImplementedException();
        }

        // configuration options

        public string FilePath { get; set; }

        public bool HasHeaders { get; set; }

        public char Delimiter { get; set; }

        public char Quote { get; set; }

        public char Escape { get; set; }

        public char Comment { get; set; }
    }
}
