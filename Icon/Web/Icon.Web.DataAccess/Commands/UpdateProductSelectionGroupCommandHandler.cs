using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateProductSelectionGroupCommandHandler : ICommandHandler<UpdateProductSelectionGroupCommand>
    {
        private IconContext context;

        public UpdateProductSelectionGroupCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateProductSelectionGroupCommand data)
        {
            var psg = context.ProductSelectionGroup.SingleOrDefault(p => p.ProductSelectionGroupId == data.ProductSelectionGroupId);

            if (psg == null)
            {
                throw new ArgumentException("No ProductSelectionGroup was found with an ID of " + data.ProductSelectionGroupId);
            }

            if (psg.ProductSelectionGroupName != data.ProductSelectionGroupName)
            {
                data.ProductSelectionGroupNameChanged = true;
            }

            if(psg.ProductSelectionGroupTypeId != data.ProductSelectionGroupTypeId)
            {
                data.ProductSelectionGroupTypeChanged = true;
            }

            psg.ProductSelectionGroupName = data.ProductSelectionGroupName;
            psg.ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId;
            psg.TraitId = data.TraitId;
            psg.TraitValue = data.TraitValue;
            psg.MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId;

            this.context.SaveChanges();
        }
    }
}