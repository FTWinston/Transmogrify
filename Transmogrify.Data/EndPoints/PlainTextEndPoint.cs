using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Transmogrify.Data.EndPoints
{
    public class PlainTextEndPoint : DataEndPoint<PlainTextEndPoint.PlainTextConfig>
    {
        public PlainTextEndPoint(string name)
            : base(name)
        {

        }

        public override IEnumerable<EndPointDataCollection> PopulateCollections(Mapping mapping)
        {
            // Data type has a single field, representing a line of the file contents

            var dataType = new ComplexDataType("Line", new DataField("Value", SimpleDataType.String));
            var collection = new EndPointDataCollection(mapping, this, "Lines", dataType);

            yield return collection;
        }

        protected internal override DataItemReader GetReader(EndPointDataCollection collection)
        {
            return new PlainTextReader(this, collection);
        }

        protected internal override DataItemWriter GetWriter(EndPointDataCollection collection)
        {
            return new PlainTextWriter(this, collection);
        }

        public class PlainTextConfig
        {
            public string FilePath { get; set; }
        }


        private class PlainTextReader : DataItemReader
        {
            public PlainTextReader(PlainTextEndPoint endPoint, EndPointDataCollection collection)
            {
                this.endPoint = endPoint;
                this.collection = collection;
                lineNumber = -1;
                lines = File.ReadAllLines(endPoint.Configuration.FilePath);
            }

            private readonly PlainTextEndPoint endPoint;
            private readonly EndPointDataCollection collection;
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

            private readonly PlainTextEndPoint endPoint;
            private readonly EndPointDataCollection collection;
            private List<string> lines = new List<string>();

            public override void Write(ComplexDataItem item)
            {
                lines.Add(item.Values.First().Value.ToString());
            }

            public override void Flush()
            {
                File.WriteAllLines(endPoint.Configuration.FilePath, lines);
            }

            public override void Dispose()
            {

            }
        }
    }
}
