using Newtonsoft.Json;

namespace Transmogrify.Data
{
    public class MappingOutput : MappingElement
    {
        public MappingOutput(DataFieldInstance source, DataField output)
        {
            Source = source;
            Output = output;
        }

        [JsonConstructor]
        private MappingOutput()
        {

        }

        public DataFieldInstance Source { get; set; }

        public DataField Output { get; set; }
    }
}
