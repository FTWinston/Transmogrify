﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Transmogrify.Data.Serialization
{
    class DataTypeCreationConverter : JsonConverter
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

            bool isSimple = item[nameof(DataType.IsSimple)].Value<bool>();

            if (isSimple)
            {
                return item.ToObject<SimpleDataType>();
            }
            else
            {
                return item.ToObject<ComplexDataType>();
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}