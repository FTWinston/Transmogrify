using Newtonsoft.Json;
using System;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class OperationOutputFieldInstance : DataFieldInstance
    {
        public OperationOutputFieldInstance(MappingElement element, DataField field)
            : base(element, field)
        {
        }

        [JsonConstructor]
        private OperationOutputFieldInstance()
            : base()
        {


        }

        [JsonIgnore]
        public override MappingElement Element { get; set; }
    }
}
