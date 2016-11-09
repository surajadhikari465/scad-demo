using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateMerchTaxMappingCommandHandler : ICommandHandler<UpdateMerchTaxMappingCommand>
    {
        private IconContext context;

        public UpdateMerchTaxMappingCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateMerchTaxMappingCommand data)
        {
            HierarchyClassTrait hierarchyClassTrait = this.context.HierarchyClassTrait.SingleOrDefault(hct =>
                hct.traitID == Traits.MerchDefaultTaxAssociatation && hct.hierarchyClassID == data.MerchandiseHierarchyClassId);

            // Don't do anything if there is no value and no HierarchyClassTrait to add/update/delete.
            if (data.TaxHierarchyClassId == 0 || hierarchyClassTrait == null)
            {
                return;
            }

            string originalValue = hierarchyClassTrait.traitValue;

            if (originalValue != data.TaxHierarchyClassId.ToString())
            {
                hierarchyClassTrait.traitValue = data.TaxHierarchyClassId.ToString();
                context.SaveChanges();
            }
        }
    }
}
