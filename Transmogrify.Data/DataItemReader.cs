using System;
using System.Collections;
using System.Collections.Generic;

namespace Transmogrify.Data
{
    public abstract class DataItemReader : IEnumerator<DataStructure>
    {
        public abstract DataStructure Current { get; }

        object IEnumerator.Current => Current;

        public abstract void Dispose();

        public abstract bool MoveNext();

        public abstract void Reset();
    }
}
