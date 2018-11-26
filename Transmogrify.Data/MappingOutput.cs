using System;

namespace Transmogrify.Data
{
    public class MappingOutput : MappingElement
    {
        public MappingOutput(DataFieldInstance source, DataField output)
        {
            Source = source;
            Output = output;
        }

        public DataFieldInstance Source { get; set; }

        public DataField Output { get; set; }
    }
}
