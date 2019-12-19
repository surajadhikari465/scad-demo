using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddHierarchyClassCommandHandler : ICommandHandler<AddHierarchyClassCommand>
    {
        private IconContext context;

        public AddHierarchyClassCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddHierarchyClassCommand data)
        {
            HierarchyClass subteamHierachyClass = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == data.SubTeamHierarchyClassId);

            var duplicateHierarchyClasses = context.HierarchyClass.ContainsDuplicateName(data.NewHierarchyClass.hierarchyClassName,
                data.NewHierarchyClass.hierarchyLevel, data.NewHierarchyClass.hierarchyID, data.NewHierarchyClass.hierarchyClassID, subteamHierachyClass, data.NewHierarchyClass.hierarchyParentClassID, false);

            if (duplicateHierarchyClasses)
            {
                throw new DuplicateValueException(String.Format(@"Error: The name ""{0}"" is already in use at this level of the hierarchy.", data.NewHierarchyClass.hierarchyClassName));
            }

            // Check for duplicate POS Department Numbers.
            if (!String.IsNullOrEmpty(data.PosDeptNumber))
            {
                ValidatePosDeptNumber(data.NewHierarchyClass, data.PosDeptNumber);
            }

            context.HierarchyClass.Add(data.NewHierarchyClass);

            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.TaxAbbreviation, data.TaxAbbreviation);
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.MerchFinMapping, data.SubTeamHierarchyClassId.ToString());
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.NonMerchandise, data.NonMerchandiseTrait);
            if (data.NewHierarchyClass.hierarchyID != Hierarchies.National)
                AddHierarchyClassTraitWithNullValue(data.NewHierarchyClass, TraitCodes.SentToEsb);
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.PosDepartmentNumber, data.PosDeptNumber);
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.TeamNumber, data.TeamNumber);
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.TeamName, data.TeamName);
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.NationalClassCode, data.NationalClassCode);
            AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.SubBrickCode, data.SubBrickCode);
            AddProhibitDiscountForSubBrickFromParentHierarchyClass(data);

            context.SaveChanges();
        }

        private void AddProhibitDiscountForSubBrickFromParentHierarchyClass(AddHierarchyClassCommand data)
        {
            if (data.NewHierarchyClass.hierarchyLevel == HierarchyLevels.SubBrick)
            {
                var parentHierarchyClass = context.HierarchyClass.FirstOrDefault(
                    hc => hc.hierarchyClassID == data.NewHierarchyClass.hierarchyParentClassID);

                if (parentHierarchyClass == null)
                {
                    throw new ArgumentException("Could not find the parent brick of new sub brick.");
                }

                var parentProhibitDiscountTraitTrue = parentHierarchyClass.HierarchyClassTrait
                    .Any(hct => hct.traitID == Traits.ProhibitDiscount && hct.traitValue == "1");

                if (parentProhibitDiscountTraitTrue)
                {
                    AddHierarchyClassTrait(data.NewHierarchyClass, TraitCodes.ProhibitDiscount, "1");
                }
            }
        }

        private void AddHierarchyClassTrait(HierarchyClass hierarchyClass, string traitCode, string value)
        {
            if (String.IsNullOrEmpty(traitCode) || String.IsNullOrEmpty(value))
            {
                return;
            }

            HierarchyClassTrait addHierarchyClassTrait = new HierarchyClassTrait();
            addHierarchyClassTrait.traitID = context.Trait.Single(t => t.traitCode == traitCode).traitID;
            addHierarchyClassTrait.traitValue = value;
            addHierarchyClassTrait.hierarchyClassID = hierarchyClass.hierarchyClassID;

            context.HierarchyClassTrait.Add(addHierarchyClassTrait);
        }

        private void AddHierarchyClassTraitWithNullValue(HierarchyClass hierarchyClass, string traitCode)
        {
            if (String.IsNullOrEmpty(traitCode))
            {
                return;
            }

            HierarchyClassTrait addHierarchyClassTrait = new HierarchyClassTrait();
            addHierarchyClassTrait.traitID = context.Trait.Single(t => t.traitCode == traitCode).traitID;
            addHierarchyClassTrait.traitValue = null;
            addHierarchyClassTrait.hierarchyClassID = hierarchyClass.hierarchyClassID;

            context.HierarchyClassTrait.Add(addHierarchyClassTrait);
        }

        /// <summary>
        /// Validates whether or not the three digit numerical POS Department Number is a duplicate
        /// </summary>
        /// <param name="hierarchyClass">HierarchyClass financial class object</param>
        /// <param name="traitValue">Pos Dept Number</param>
        private void ValidatePosDeptNumber(HierarchyClass hierarchyClass, string traitValue)
        {
            // If this isn't a financial hierarchy class, do nothing.
            if (hierarchyClass.hierarchyID != Hierarchies.Financial)
            {
                return;
            }

            // Prevent duplicate POS Department Number.
            if (this.context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.PosDepartmentNumber && hct.traitValue == traitValue))
            {
                throw new DuplicateValueException(String.Format("The POS Department Number {0} is already assigned to a different subteam.", traitValue));
            }
        }
    }
}
