using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Transmogrify.Data.EndPoints
{
    public class CsvEndPoint : DataEndPoint<CsvEndPoint.CsvConfig>
    {
        public CsvEndPoint(string name)
            : base(name)
        {

        }

        public override Color Color => Color.GreenYellow;

        public override IEnumerable<EndPointDataCollection> PopulateCollections(Mapping mapping)
        {
            yield return new EndPointDataCollection(this, "Rows", DataType);
        }

        protected internal override DataItemReader GetReader(EndPointDataCollection collection)
        {
            return new CsvReader(this, collection);
        }

        protected internal override DataItemWriter GetWriter(EndPointDataCollection collection)
        {
            return new CsvWriter(this, collection);
        }

        // TODO: if populating from a file, read all field headings ... just call them Column 1 ... X if HasHeaders is false
        // Collections has a single item, representing the file contents...
        public DataStructureType DataType { get; set; }

        public CsvHelper.Configuration.Configuration ConvertConfiguration()
        {
            // See https://joshclose.github.io/CsvHelper/configuration

            var configuration = new CsvHelper.Configuration.Configuration
            {
                SanitizeForInjection = true,
                HasHeaderRecord = Configuration.HasHeaders,
                Delimiter = Configuration.Delimiter,
                Quote = Configuration.Quote,
                Comment = Configuration.Comment,
            };

            return configuration;
        }

        public class CsvConfig
        {
            public string FilePath { get; set; }

            public bool HasHeaders { get; set; } = true;

            public string Delimiter { get; set; } = ",";

            public char Quote { get; set; } = '"';

            public char Comment { get; set; } = '#';

            public bool AlwaysQuote { get; set; } = false;
        }

        private class CsvReader : DataItemReader
        {
            public CsvReader(CsvEndPoint endPoint, EndPointDataCollection collection)
            {
                this.endPoint = endPoint;
                this.collection = collection;

                streamReader = new StreamReader(endPoint.Configuration.FilePath);
                csvReader = new CsvHelper.CsvReader(streamReader, endPoint.ConvertConfiguration());
                recordEnumerator = csvReader.GetRecords<dynamic>().GetEnumerator();
            }

            private readonly CsvEndPoint endPoint;
            private readonly EndPointDataCollection collection;

            private StreamReader streamReader;
            private CsvHelper.CsvReader csvReader;
            private IEnumerator<dynamic> recordEnumerator;

            public override DataStructure Current
            {
                get
                {
                    // TODO: see https://joshclose.github.io/CsvHelper/reading#getting-fields to get specific fields

                    var csvRecord = recordEnumerator.Current;
                    Type recordType = csvRecord.GetType();

                    var item = new DataStructure(collection.ItemType);

                    var fieldEnumerator = collection.ItemType.Fields.GetEnumerator();

                    // TODO: ideally, iterate over fields without using reflection
                    foreach (var prop in recordType.GetProperties())
                    {
                        if (!fieldEnumerator.MoveNext())
                            break;

                        var field = fieldEnumerator.Current;

                        object value = prop.GetValue(csvReader);

                        string propName = prop.Name;
                        item.Values.Add(field, value);
                    }

                    return item;
                }
            }

            public override bool MoveNext()
            {
                return recordEnumerator.MoveNext();
            }

            public override void Reset()
            {
                recordEnumerator.Reset();
            }

            public override void Dispose()
            {
                csvReader.Dispose();
                streamReader.Dispose();
            }
        }


        private class CsvWriter : DataItemWriter
        {
            public CsvWriter(CsvEndPoint endPoint, EndPointDataCollection collection)
            {
                this.endPoint = endPoint;
                this.collection = collection;

                streamWriter = new StreamWriter(endPoint.Configuration.FilePath, false);
                csvWriter = new CsvHelper.CsvWriter(streamWriter, endPoint.ConvertConfiguration());
            }

            private readonly CsvEndPoint endPoint;
            private readonly EndPointDataCollection collection;

            private StreamWriter streamWriter;
            private CsvHelper.CsvWriter csvWriter;

            public override void Write(DataStructure item)
            {
                foreach (var field in item.Values)
                {
                    csvWriter.WriteField(field.Value);
                }

                csvWriter.NextRecord();
            }

            public override void Flush()
            {
                
            }

            public override void Dispose()
            {
                csvWriter.Dispose();
                streamWriter.Dispose();
            }
        }
    }
}
