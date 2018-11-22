using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Transmogrify.Data.EndPoints
{
    public class PlainTextEndPoint : DataEndPoint
    {
        public PlainTextEndPoint(string name)
            : base(name)
        {

        }

        public override void PopulateCollections()
        {
            Collections.Add(new EndPointDataCollection(this, "Lines", DataType));
        }

        protected internal override DataItemReader GetReader(EndPointDataCollection collection)
        {
            return new PlainTextReader(this, collection);
        }

        protected internal override DataItemWriter GetWriter(EndPointDataCollection collection)
        {
            return new PlainTextWriter(this, collection);
        }

        // TODO: if populating from a file, read all field headings ... just call them Column 1 ... X if HasHeaders is false
        // Collections has a single item, representing the file contents...
        public ComplexDataType DataType { get; set; }


        // configuration options

        public string FilePath { get; set; }

        public bool HasHeaders { get; set; }

        public char Delimiter { get; set; }

        public char Quote { get; set; }

        public char Escape { get; set; }

        public char Comment { get; set; }


        private class PlainTextReader : DataItemReader
        {
            public PlainTextReader(PlainTextEndPoint endPoint, EndPointDataCollection collection)
            {
                this.endPoint = endPoint;
                this.collection = collection;
                lineNumber = -1;
                lines = File.ReadAllLines(endPoint.FilePath);
            }

            private PlainTextEndPoint endPoint;
            private EndPointDataCollection collection;
            private int lineNumber;
            private string[] lines;

            public override ComplexDataItem Current
            {
                get
                {
                    var line = lines[lineNumber];

                    var item = new ComplexDataItem(collection.ItemType);

                    item.Values.Add(collection.ItemType.Fields.First(), line);

                    return item;
                }
            }

            public override bool MoveNext()
            {
                return ++lineNumber < lines.Length;
            }

            public override void Reset()
            {
                lineNumber = -1;
            }

            public override void Dispose()
            {

            }
        }

        private class PlainTextWriter : DataItemWriter
        {
            public PlainTextWriter(PlainTextEndPoint endPoint, EndPointDataCollection collection)
            {
                this.endPoint = endPoint;
                this.collection = collection;
            }

            private PlainTextEndPoint endPoint;
            private EndPointDataCollection collection;
            private List<string> lines = new List<string>();

            public override void Write(ComplexDataItem item)
            {
                lines.Add(item.Values.First().Value.ToString());
            }

            public override void Flush()
            {
                File.WriteAllLines(endPoint.FilePath, lines);
            }

            public override void Dispose()
            {

            }
        }
    }
}
