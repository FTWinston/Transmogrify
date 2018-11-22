using System.Collections.Generic;

namespace Transmogrify.Data
{
    public class Project
    {
        public List<DataEndPoint> EndPoints { get; } = new List<DataEndPoint>();

        public List<Migration> Migrations { get; } = new List<Migration>();
    }
}
