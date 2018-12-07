using System;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public abstract class DataItemWriter : IDisposable
    {
        public abstract void Dispose();

        public abstract void Write(DataStructure item);

        public abstract void Flush();
    }
}
