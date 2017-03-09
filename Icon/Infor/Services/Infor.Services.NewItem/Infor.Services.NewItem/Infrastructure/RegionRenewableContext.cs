using Icon.Common.Context;
using Irma.Framework;
using System;

namespace Infor.Services.NewItem.Infrastructure
{
    public class RegionalRenewableContext : IRenewableContext<IrmaContext>
    {
        public IrmaContext Context { get; set; }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Refresh(object region)
        {
            if (Context != null)
            {
                Context.Dispose();
            }

            Context = new IrmaContext("ItemCatalog_" + region);
        }
    }
}
