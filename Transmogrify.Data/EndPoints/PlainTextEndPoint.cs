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

        public override IEnumerable<MappingCollection> PopulateCollections(Mapping mapping)
        {
            // Data type has a single field, representing a line of the file contents

            // TODO: revisit MappingCollection vs DataCollection ... shouldn't DataCollections (OR SOMETHING data related) exist outside the mapping?

            // TODO: the data type needs to know about the collection, and the collection needs to know about the data type. Unpick this.

            var field = new DataField(collection, "Value", SimpleDataType.String);
            var collection = new EndPointDataCollection(this, "Lines", dataType);
            var dataType = new ComplexDataType("Line", field);

            yield return new MappingCollection(mapping, collection);
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
                File.WriteAllLines(endPoint.Configuration.FilePath, lines);
            }

            public override void Dispose()
            {

            }
        }
    }
}
