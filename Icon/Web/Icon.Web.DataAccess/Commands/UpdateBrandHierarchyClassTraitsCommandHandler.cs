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
            string traitValue;
            var brandToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.Brand.hierarchyClassID);
            if(brandToUpdate == null) return;

            var allTraits = brandToUpdate.HierarchyClassTrait.ToArray();
            var existingTrait = allTraits.SingleOrDefault(x => x.Trait.traitCode == TraitCodes.BrandAbbreviation);

            if (existingTrait != null && !string.IsNullOrWhiteSpace(data.BrandAbbreviation) && (string.IsNullOrWhiteSpace(existingTrait.traitValue) ? String.Empty : existingTrait.traitValue).Trim() != data.BrandAbbreviation.Trim())
            {
                if (context.HierarchyClassTrait.Where(x => x.hierarchyClassID != brandToUpdate.hierarchyClassID).ContainsDuplicateBrandAbbreviation(data.BrandAbbreviation))
                {
                    throw new DuplicateValueException(String.Format("The brand abbreviation {0} already exists.", data.BrandAbbreviation));
                }
            }
            
            foreach(var code in new int[]{ Traits.BrandAbbreviation,  Traits.Designation, Traits.ParentCompany, Traits.ZipCode, Traits.Locality })
            {
                existingTrait = allTraits.SingleOrDefault(x => x.traitID == code);

                switch(code)
                {
                    case Traits.BrandAbbreviation:
                        traitValue = data.BrandAbbreviation;
                        break;
                    case Traits.Designation:
                        traitValue = data.Designation;
                        break;
                    case Traits.ParentCompany:
                        traitValue = data.ParentCompany;
                        break;
                    case Traits.ZipCode:
                        traitValue = data.ZipCode;
                        break;
                    case Traits.Locality:
                        traitValue = data.Locality;
                        break;
                    default:
                        traitValue = null;
                        break;
                }

                if(existingTrait == null)
                {
                    brandToUpdate.AddHierarchyClassTrait(context, code, traitValue);
                }
                else
                {
                    existingTrait.UpdateHierarchyClassTrait(context, traitValue, removeIfNullOrEmpty: true);
                }
            }
        }
    }
}