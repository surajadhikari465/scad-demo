using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateBrandHierarchyClassTraitsCommandHandler : ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>
    {
        private IconContext context;

        public UpdateBrandHierarchyClassTraitsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateBrandHierarchyClassTraitsCommand data)
        {
            var brandToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.Brand.hierarchyClassID);
            var currentBrandAbbreviationTrait = brandToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation);

            if (currentBrandAbbreviationTrait != null)
            {
                string currentBrandAbbreviation = currentBrandAbbreviationTrait.traitValue;

                if (currentBrandAbbreviation == data.BrandAbbreviation)
                {
                    return;
                }
            }

            if (data.BrandAbbreviation != null)
            {
                bool duplicateBrandAbbreviationExists = context.HierarchyClassTrait.ContainsDuplicateBrandAbbreviation(data.BrandAbbreviation);

                if (duplicateBrandAbbreviationExists)
                {
                    throw new DuplicateValueException(String.Format("The brand abbreviation {0} already exists.", data.BrandAbbreviation));
                }
            }

            if (currentBrandAbbreviationTrait == null)
            {
                if (String.IsNullOrEmpty(data.BrandAbbreviation))
                {
                    return;
                }
                else
                {
                    brandToUpdate.AddHierarchyClassTrait(context, TraitCodes.BrandAbbreviation, data.BrandAbbreviation);
                }
            }
            else
            {
                currentBrandAbbreviationTrait.UpdateHierarchyClassTrait(context, data.BrandAbbreviation, removeIfNullOrEmpty: true);
            }
        }
    }
}
