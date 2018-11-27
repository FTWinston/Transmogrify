using Newtonsoft.Json;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public abstract class DataType
    {
        protected DataType(string name)
        {
            Name = name;
        }

        protected DataType()
        {

        }

        public string Name { get; set; }
    }
}
