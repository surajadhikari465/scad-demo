using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class RemoveHierarchyClassFromIrmaItemsCommandHandler : ICommandHandler<RemoveHierarchyClassFromIrmaItemsCommand>
    {
        private IconContext context;

        public RemoveHierarchyClassFromIrmaItemsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RemoveHierarchyClassFromIrmaItemsCommand data)
        {
            if(data.HierarchyId == Hierarchies.Tax)
            {
                var irmaItems = context.IRMAItem.Where(i => i.taxClassID == data.HierarchyClassId);
                foreach (var irmaItem in irmaItems)
                {
                    irmaItem.taxClassID = null;
                }
            }
            else if(data.HierarchyId == Hierarchies.Merchandise)
            {
                var irmaItems = context.IRMAItem.Where(i => i.merchandiseClassID == data.HierarchyClassId);
                foreach (var irmaItem in irmaItems)
                {
                    irmaItem.merchandiseClassID = null;
                }
            }
            context.SaveChanges();
        }
    }
}
