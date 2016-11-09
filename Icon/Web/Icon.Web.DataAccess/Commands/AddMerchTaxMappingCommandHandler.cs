using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddMerchTaxMappingCommandHandler : ICommandHandler<AddMerchTaxMappingCommand>
    {
        private IconContext context;

        public AddMerchTaxMappingCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddMerchTaxMappingCommand data)
        {
            var duplicateMapping = context.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.MerchDefaultTaxAssociatation && hct.hierarchyClassID == data.MerchandiseHierarchyClassId);

            if (duplicateMapping != null)
            {
                throw new CommandException(String.Format("A previous mapping exists for Merchandise Hierarchy Class {0}.", data.MerchandiseHierarchyClassId));
            }

            var merchTaxMappingTrait = new HierarchyClassTrait
            {
                traitID = context.Trait.Single(t => t.traitCode == TraitCodes.MerchDefaultTaxAssociatation).traitID,
                traitValue = data.TaxHierarchyClassId.ToString(),
                hierarchyClassID = data.MerchandiseHierarchyClassId
            };

            context.HierarchyClassTrait.Add(merchTaxMappingTrait);
            context.SaveChanges();
        }
    }
}
