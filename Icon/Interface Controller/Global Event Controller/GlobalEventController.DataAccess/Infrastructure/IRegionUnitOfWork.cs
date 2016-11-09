using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public interface IRegionUnitOfWork
    {
        IrmaContext SetContext(string region);
    }
}
