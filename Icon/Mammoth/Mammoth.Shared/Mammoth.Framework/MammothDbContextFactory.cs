using Icon.DbContextFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Framework
{
    public class MammothContextFactory : IDbContextFactory<MammothContext>
    {
        public MammothContext CreateContext()
        {
            return new MammothContext();
        }

        public MammothContext CreateContext(object settings)
        {
            throw new NotImplementedException();
        }

        public MammothContext CreateContext(string connectionStringName)
        {
            throw new NotImplementedException();
        }
    }
}
