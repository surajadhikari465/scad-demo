using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddProductSelectionGroupCommandHandler : ICommandHandler<AddProductSelectionGroupCommand>
    {
        private IconContext context;

        public AddProductSelectionGroupCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddProductSelectionGroupCommand data)
        {
            ProductSelectionGroup psg = new ProductSelectionGroup
            {
                ProductSelectionGroupName = data.ProductSelectionGroupName,
                ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId,
                TraitId = data.TraitId,
                TraitValue = data.TraitValue,
                MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                AttributeId = data.AttributeId,
                AttributeValue = data.AttributeValue
            };

            context.ProductSelectionGroup.Add(psg);
            context.SaveChanges();

            data.ProductSelectionGroupId = psg.ProductSelectionGroupId;
        }
    }
}
