using System.Collections.Generic;
using Icon.Framework;
using Irma.Framework;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public interface IContextManager
    {
        IconContext IconContext { get; set; }
        Dictionary<string, IrmaContext> IrmaContexts { get; set; }

        void RefreshContexts();
    }
}