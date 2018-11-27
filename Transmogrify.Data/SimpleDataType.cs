using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class SimpleDataType : DataType
    {
        public SimpleDataType(string name, Type actualType)
            : base(name)
        {
            ActualType = actualType;
        }

        [JsonConstructor]
        private SimpleDataType()
        {

        }

        public Type ActualType { get; set; }

        public static SimpleDataType String = new SimpleDataType("String", typeof(string));
        public static SimpleDataType Bool = new SimpleDataType("Bool", typeof(bool));
        public static SimpleDataType Byte = new SimpleDataType("Byte", typeof(byte));
        public static SimpleDataType Short = new SimpleDataType("Short", typeof(short));
        public static SimpleDataType Int = new SimpleDataType("Int", typeof(int));
        public static SimpleDataType Long = new SimpleDataType("Long", typeof(long));
        public static SimpleDataType Float = new SimpleDataType("Float", typeof(float));
        public static SimpleDataType Double = new SimpleDataType("Double", typeof(double));
        public static SimpleDataType Decimal = new SimpleDataType("Decimal", typeof(decimal));
        public static SimpleDataType DateTime = new SimpleDataType("DateTime", typeof(DateTime));
    }
}
