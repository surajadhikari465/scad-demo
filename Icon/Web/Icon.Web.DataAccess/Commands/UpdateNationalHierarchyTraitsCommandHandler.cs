using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateNationalHierarchyTraitsCommandHandler : ICommandHandler<UpdateNationalHierarchyTraitsCommand>
    {
        private IconContext context;

        public UpdateNationalHierarchyTraitsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateNationalHierarchyTraitsCommand data)
        {
            var nationalClassToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.NationalHierarchy.hierarchyClassID);
            var currentNationalClassTrait = nationalClassToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == data.TraitCode);

            if (currentNationalClassTrait != null)
            {
                string currentNationalTraitValue = currentNationalClassTrait.traitValue;

                if (currentNationalTraitValue == data.TraitValue)
                {
                    return;
                }
            }

            //if (data.NationalClassCode != null)
            //{
            //    bool duplicateBrandAbbreviationExists = context.HierarchyClassTrait.ContainsDuplicateBrandAbbreviation(data.BrandAbbreviation);

            //    if (duplicateBrandAbbreviationExists)
            //    {
            //        throw new DuplicateValueException(String.Format("The brand abbreviation {0} already exists.", data.BrandAbbreviation));
            //    }
            //}

            if (currentNationalClassTrait == null)
            {
                if (String.IsNullOrEmpty(data.TraitValue))
                {
                    return;
                }
                else
                {
                    nationalClassToUpdate.AddHierarchyClassTrait(context, data.TraitCode, data.TraitValue);
                }
            }
            else
            {
                currentNationalClassTrait.UpdateHierarchyClassTrait(context, data.TraitValue, removeIfNullOrEmpty: true);
            }
        }
    }
}
