using System;

namespace Transmogrify.Data
{
    public class MappingOutput : MappingElement
    {
        public MappingOutput(Mapping mapping, DataFieldInstance source, DataField output)
            : base(mapping)
        {
            Source = source;
            Output = output;
        }

        public DataFieldInstance Source { get; set; } // TODO: just serialize an ID

        public DataField Output { get; set; } // TODO: just serialize an ID
    }
}
