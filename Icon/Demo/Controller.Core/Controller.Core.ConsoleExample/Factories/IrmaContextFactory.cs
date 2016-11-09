using Controller.Core.Factories;
using Controller.Core.Wrappers;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.ConsoleExample.Factories
{
    public class IrmaContextFactory : IGenericFactory<IDisposableWrapper<IrmaContext>, string>
    {
        public IDisposableWrapper<IrmaContext> Create(string region)
        {
            return new DisposableWrapper<IrmaContext>(new IrmaContext("IRMA_" + region));
        }
    }
}
