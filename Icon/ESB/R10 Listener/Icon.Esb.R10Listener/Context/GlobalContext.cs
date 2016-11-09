using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Context
{
    public class GlobalContext : IRenewableContext<IconContext>
    {
        public IconContext Context { get; private set; }

        public GlobalContext(IconContext context)
        {
            this.Context = context;
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }

        public void Refresh()
        {
            if(this.Context != null)
            {
                this.Context.Dispose();
            }
            this.Context = new IconContext();
        }
    }
}
