using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class MappingOperationOutputFieldInstance : DataFieldInstance
    {
        public MappingOperationOutputFieldInstance(MappingElement element, DataField field)
            : base(element, field)
        {
        }

        [JsonConstructor]
        private MappingOperationOutputFieldInstance()
            : base()
        {

        }

        [JsonIgnore]
        public override MappingElement Element { get; set; }
    }
}
