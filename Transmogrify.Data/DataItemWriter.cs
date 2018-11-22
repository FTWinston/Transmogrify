using System;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public abstract class DataItemWriter : IDisposable
    {
        public abstract void Dispose();

        public abstract void Write(ComplexDataItem item);

        public abstract void Flush();
    }
}
