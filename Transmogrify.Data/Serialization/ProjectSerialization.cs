using Newtonsoft.Json;
using System;

namespace Transmogrify.Data.Serialization
{
    public static class ProjectSerialization
    {
        public static JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {

            };

            settings.Converters.Add(new DataTypeCreationConverter());

            return settings;
        }
    }
}
