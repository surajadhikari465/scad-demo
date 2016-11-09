using System.Data.Entity;

namespace Irma.Framework
{
    public partial class IrmaContext : DbContext
    {
        public IrmaContext(string connectionString)
            : base(connectionString)
        {

        }
    }
}
