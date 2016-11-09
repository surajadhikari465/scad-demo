using Icon.RenewableContext;
using System;

namespace Irma.Framework.RenewableContext
{
    public class GlobalIrmaContext : IRenewableContext<IrmaContext>
    {
        public IrmaContext Context { get; set; }
        private string connectionString;
        
        public GlobalIrmaContext(IrmaContext context, string connectionString)
        {
            this.Context = context;
            this.connectionString = connectionString;
        }

        public void Refresh()
        {
            if (Context != null)
            {
                Context.Dispose();
            }

            Context = new IrmaContext(connectionString);
        }

        public void Refresh(object parameters)
        {
            throw new NotImplementedException();
        }
    }
}
