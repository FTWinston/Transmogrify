using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Transmogrify.Data.Serialization
{
    class DataTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            // need to work around object references
            var reference = item["$ref"];
            if (reference != null)
            {
                return serializer.ReferenceResolver.ResolveReference(serializer, reference.Value<string>());
            }

            object newValue;

            if (item[nameof(SimpleDataType.ActualType)] != null)
            {
                newValue = item.ToObject<SimpleDataType>();
            }
            else
            {
                newValue = item.ToObject<ComplexDataType>();
            }

            var id = item["$id"];
            if (id != null)
            {
                serializer.ReferenceResolver.AddReference(serializer, id.Value<string>(), newValue);
            }

            return newValue;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
