using Icon.DbContextFactory;
using System;

namespace Icon.Framework
{
    public class IconDbContextFactory : IDbContextFactory<IconContext>
    {
        public IconContext CreateContext()
        {
            return new IconContext();
        }

        public IconContext CreateContext(object settings)
        {
            throw new NotImplementedException();
        }

        public IconContext CreateContext(string connectionStringName)
        {
            throw new NotImplementedException();
        }
    }
}
