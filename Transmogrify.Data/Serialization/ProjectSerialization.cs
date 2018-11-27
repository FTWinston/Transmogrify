using Newtonsoft.Json;

namespace Transmogrify.Data.Serialization
{
    public static class ProjectSerialization
    {
        public static JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {

            };

            settings.Converters.Add(new DataTypeConverter());

            return settings;
        }
    }
}
