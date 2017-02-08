using Icon.DbContextFactory;
using System;

namespace Irma.Framework
{
    public class IrmaDbContextFactory : IDbContextFactory<IrmaContext>
    {
        public IrmaContext CreateContext()
        {
            return new IrmaContext();
        }

        public IrmaContext CreateContext(object settings)
        {
            throw new NotImplementedException();
        }

        public IrmaContext CreateContext(string connectionStringName)
        {
            return new IrmaContext(connectionStringName);
        }
    }
}
