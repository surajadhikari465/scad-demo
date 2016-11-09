using Icon.RenewableContext;
using System;

namespace Icon.Framework.RenewableContext
{
    public class GlobalIconContext : IRenewableContext<IconContext>
    {
        public IconContext Context { get; set; }

        public GlobalIconContext(IconContext context)
        {
            this.Context = context;
        }

        public void Refresh()
        {
            if (Context != null)
            {
                Context.Dispose();
            }

            Context = new IconContext();
        }

        public void Refresh(object parameters)
        {
            throw new NotImplementedException();
        }
    }
}
