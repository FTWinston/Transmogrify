namespace Transmogrify.Data
{
    public class Migration
    {
        Project Project { get; set; }
        public DataCollection Source { get; set; }
        public DataCollection Destination { get; set; }

        // TODO: operations, field mappings, etc.
    }
}
