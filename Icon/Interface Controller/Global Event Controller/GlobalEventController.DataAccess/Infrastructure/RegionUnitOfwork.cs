using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;
using InterfaceController.Common;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public class RegionUnitOfwork : IRegionUnitOfWork
    {
        private IrmaContext context;
        public IrmaContext SetContext(string region)
        {
            this.context = new IrmaContext(ConnectionBuilder.GetConnection(region));
            return this.context;
        }
    }
}
