using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateHierarchyClassTraitCommandHandler : ICommandHandler<UpdateHierarchyClassTraitCommand>
    {
        private IconContext context;
        private bool nonMerchandiseTraitChange = false;
        private bool subTeamChange = false;
        private bool prohibitDiscountChange = false;
        private List<HierarchyClassTrait> hierarchyClassTraits;

        public UpdateHierarchyClassTraitCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateHierarchyClassTraitCommand data)
        {
            HierarchyClass hierarchyClass = this.context.HierarchyClass.Find(data.UpdatedHierarchyClass.hierarchyClassID);

            ValidateTaxCode(hierarchyClass, data.TaxAbbreviation);
            ValidatePosDeptNumber(hierarchyClass, data.PosDeptNumber);

            this.hierarchyClassTraits = this.context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == data.UpdatedHierarchyClass.hierarchyClassID).ToList();
            UpdateHierarchyClassTrait(hierarchyClass, Traits.TaxAbbreviation, data.TaxAbbreviation);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.GlAccount, data.GlAccount);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.NonMerchandise, data.NonMerchandiseTrait);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.ProhibitDiscount, data.ProhibitDiscount);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.PosDepartmentNumber, data.PosDeptNumber);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.TeamNumber, data.TeamNumber);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.TeamName, data.TeamName);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.TaxRomance, data.TaxRomance);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.NationalClassCode, data.NationalClassCode);
            UpdateHierarchyClassTrait(hierarchyClass, Traits.SubBrickCode, data.SubBrickCode);
            UpdateSubTeamTrait(hierarchyClass, data.SubTeamHierarchyClassId);
            
            this.context.SaveChanges();

            UpdateAssociatedItemTypes(data.UpdatedHierarchyClass, data.NonMerchandiseTrait, data.UserName);
            AddProductMessages(data.UpdatedHierarchyClass);
            AddItemSubTeamEvents(data.UpdatedHierarchyClass);

            data.SubteamChanged = subTeamChange;
        }

        /// <summary>
        /// Validates whether or not the seven digit numerical tax code is a duplicate
        /// </summary>
        /// <param name="hierarchyClass">HierarchyClass tax class object</param>
        /// <param name="traitValue">Tax Abbreviation</param>
        private void ValidateTaxCode(HierarchyClass hierarchyClass, string traitValue)
        {
            // If this isn't a tax class, do nothing.
            if (hierarchyClass.hierarchyID != Hierarchies.Tax)
            {
                return;
            }

            // Prevent duplicate tax abbreviations.
            string taxCode = traitValue.Split()[0];

            var existingTaxCodes = this.context.HierarchyClassTrait
                .Where(hct => hct.traitID == Traits.TaxAbbreviation)
                .ToList()
                .Select(t => new
                    {
                        hierarchyClassId = t.hierarchyClassID,
                        traitValue = t.traitValue.Split()[0]
                    });

            if (existingTaxCodes.Any(tcl => tcl.traitValue == taxCode && tcl.hierarchyClassId != hierarchyClass.hierarchyClassID))
            {
                throw new HierarchyClassTraitUpdateException(String.Format("The tax code {0} is already assigned to a different tax class.", taxCode));
            }
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
            if (this.context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.PosDepartmentNumber && hct.traitValue == traitValue && hct.hierarchyClassID != hierarchyClass.hierarchyClassID))
            {
                throw new HierarchyClassTraitUpdateException(String.Format("The POS Department Number {0} is already assigned to a different subteam.", traitValue));
            }
        }

        private void UpdateHierarchyClassTrait(HierarchyClass hierarchyClass, int traitId, string traitValue)
        {
            HierarchyClassTrait hierarchyClassTrait = this.hierarchyClassTraits.SingleOrDefault(hct => hct.traitID == traitId);

            // Don't do anything if there is no value and no HierarchyClassTrait to add/update/delete.
            if (String.IsNullOrEmpty(traitValue) && hierarchyClassTrait == null)
            {
                return;
            }

            // If this is a sub-brick, don't worry about the prohibit discount trait.
            if (hierarchyClass.hierarchyLevel == HierarchyLevels.SubBrick && traitId == Traits.ProhibitDiscount)
            {
                return;
            }

            // Delete HierarchyClassTrait if trait value is blank.
            if (String.IsNullOrEmpty(traitValue) && hierarchyClassTrait != null)
            {
                this.context.HierarchyClassTrait.Remove(hierarchyClassTrait);

                if (hierarchyClass.hierarchyLevel == HierarchyLevels.Gs1Brick && traitId == Traits.ProhibitDiscount)
                {
                    var subBrickNodesId = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == hierarchyClass.hierarchyClassID).Select(hc => hc.hierarchyClassID).ToList();

                    this.context.HierarchyClassTrait.RemoveRange(
                        this.context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.ProhibitDiscount && subBrickNodesId.Contains(hct.hierarchyClassID)));

                    prohibitDiscountChange = (traitId == Traits.ProhibitDiscount);
                }

                nonMerchandiseTraitChange = (traitId == Traits.NonMerchandise);
            }

            // Add new HierarchyClassTrait since there isn't one to update.
            else if (hierarchyClassTrait == null)
            {
                HierarchyClassTrait addHierarchyClassTrait = new HierarchyClassTrait
                {
                    traitID = traitId,
                    traitValue = traitValue,
                    hierarchyClassID = hierarchyClass.hierarchyClassID
                };

                this.context.HierarchyClassTrait.Add(addHierarchyClassTrait);

                if (hierarchyClass.hierarchyLevel == HierarchyLevels.Gs1Brick && traitId == Traits.ProhibitDiscount)
                {
                    var subBrickNodes = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == hierarchyClass.hierarchyClassID).ToList();
                    var subBrickHierarchyClassTraits = new List<HierarchyClassTrait>();

                    foreach (var subBrick in subBrickNodes)
                    {
                        subBrickHierarchyClassTraits.Add(new HierarchyClassTrait
                        {
                            traitID = traitId,
                            traitValue = traitValue,
                            hierarchyClassID = subBrick.hierarchyClassID
                        });
                    }

                    this.context.HierarchyClassTrait.AddRange(subBrickHierarchyClassTraits);

                    prohibitDiscountChange = (traitId == Traits.ProhibitDiscount);
                }

                nonMerchandiseTraitChange = (traitId == Traits.NonMerchandise);
            }

            // Update existing traitValue.
            else
            {
                string originalValue = hierarchyClassTrait.traitValue;
                if (originalValue != traitValue)
                {
                    hierarchyClassTrait.traitValue = traitValue;
                    nonMerchandiseTraitChange = (traitId == Traits.NonMerchandise);
                }
            }
        }

        private void UpdateSubTeamTrait(HierarchyClass hierarchyClass, int subTeamId)
        {
            if (subTeamId == 0)
            {
                return;
            }
            else
            {
                // Get subteam name from Financial hierarchy based on subTeamId
                HierarchyClassTrait hierarchyClassTrait = context.HierarchyClassTrait
                    .Single(hct => hct.traitID == Traits.MerchFinMapping && hct.hierarchyClassID == hierarchyClass.hierarchyClassID);

                HierarchyClass subTeam = context.HierarchyClass.Single(hc => hc.hierarchyClassID == subTeamId);

                // Only update if the sub-team is changing
                string originalValue = hierarchyClassTrait.traitValue;
                if (originalValue != subTeam.hierarchyClassName)
                {
                    hierarchyClassTrait.traitValue = subTeam.hierarchyClassName;
                    subTeamChange = true;

                    if (subTeam.HierarchyClassTrait.Any(hct => hct.traitID == Traits.NonAlignedSubteam))
                    {
                        AddOrRemoveHierarchyClassTrait(hierarchyClass, Traits.NonAlignedSubteam, true, "1");
                    }
                    else
                    {
                        AddOrRemoveHierarchyClassTrait(hierarchyClass, Traits.NonAlignedSubteam, false, "1");
                    }
                }
            }
        }

        /// <summary>
        /// Updates the ItemType of each item associated to the subBrick that is being edited.
        /// Nothing happens if the HierarchyClass being edited is not a SubBrick or if the Non-merchandise Trait did not change
        /// </summary>
        /// <param name="hierarchyClass">HierarchyClass being updated</param>
        /// <param name="nonMerchandiseTraitValue">The Non-Merchandise trait value</param>
        private void UpdateAssociatedItemTypes(HierarchyClass hierarchyClass, string nonMerchandiseTraitValue, string userName)
        {
            // Don't do anything if the HierarchyClass is not a sub-brick or if the non-merchandise trait did not change.
            if (!nonMerchandiseTraitChange || !(hierarchyClass.hierarchyID == Hierarchies.Merchandise && hierarchyClass.hierarchyLevel == HierarchyLevels.SubBrick))
            {
                return;
            }

            string itemTypeCode = String.Empty;
            switch (nonMerchandiseTraitValue)
            {
                case NonMerchandiseTraits.BottleDeposit:
                case NonMerchandiseTraits.Crv:
                    itemTypeCode = ItemTypeCodes.Deposit;
                    break;
                case NonMerchandiseTraits.BottleReturn:
                case NonMerchandiseTraits.CrvCredit:
                    itemTypeCode = ItemTypeCodes.Return;
                    break;
                case NonMerchandiseTraits.Coupon:
                    itemTypeCode = ItemTypeCodes.Coupon;
                    break;
                case NonMerchandiseTraits.LegacyPosOnly:
                case NonMerchandiseTraits.NonRetail:
                    itemTypeCode = ItemTypeCodes.NonRetail;
                    break;
                case NonMerchandiseTraits.BlackhawkFee:
                    itemTypeCode = ItemTypeCodes.Fee;
                    break;
                default:
                    itemTypeCode = ItemTypeCodes.RetailSale;
                    break;
            }

            this.context.UpdateItemTypeByHierarchyClass(hierarchyClass.hierarchyClassID, itemTypeCode, userName);
        }

        /// <summary>
        /// Adds a Product Message for each item associated with the sub-brick.
        /// Messages will only be created for validated items.
        /// </summary>
        /// <param name="hierarchyClass">HierarchyClass object</param>
        private void AddProductMessages(HierarchyClass hierarchyClass)
        {
            // Don't do anything if the HierarchyClass is not a brick or sub-brick.
            if (hierarchyClass.hierarchyID != Hierarchies.Merchandise && (hierarchyClass.hierarchyLevel != HierarchyLevels.Gs1Brick || hierarchyClass.hierarchyLevel != HierarchyLevels.SubBrick))
            {
                return;
            }

            // Don't do anything if neither the non-merchandise trait nor the sub-team changed.
            if (!(nonMerchandiseTraitChange || subTeamChange || prohibitDiscountChange))
            {
                return;
            }

            if (hierarchyClass.hierarchyLevel == HierarchyLevels.Gs1Brick)
            {
                var subBrickNodes = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == hierarchyClass.hierarchyClassID).ToList();

                foreach (var subBrick in subBrickNodes)
                {
                    this.context.GenerateItemUpdateMessagesByHierarchyClass(subBrick.hierarchyClassID);
                }
            }
            else if (hierarchyClass.hierarchyLevel == HierarchyLevels.SubBrick)
            {
                this.context.GenerateItemUpdateMessagesByHierarchyClass(hierarchyClass.hierarchyClassID);
            }
        }

        private void AddItemSubTeamEvents(HierarchyClass hierarchyClass)
        {
            // Don't do anything if the HierarchyClass is not a sub-brick.
            if (hierarchyClass.hierarchyID != Hierarchies.Merchandise && hierarchyClass.hierarchyLevel != HierarchyLevels.SubBrick)
            {
                return;
            }

            // Don't do anything if the sub-team is not changed.
            if (!subTeamChange)
            {
                return;
            }

            SqlParameter hierarchyID = new SqlParameter("hierarchyID", SqlDbType.Int);
            hierarchyID.Value = hierarchyClass.hierarchyClassID;

            string sql = "EXEC app.GenerateItemUpdateEventsForHierarchy @hierarchyID";

            try
            {
                context.Database.ExecuteSqlCommand(sql, hierarchyID);
            }
            catch (Exception ex)
            {
                throw new CommandException("The UpdateHierarchyClassTraitCommandHandler threw an exception while trying to generate item sub team events.", ex);
            }
        }

        private void AddOrRemoveHierarchyClassTrait(HierarchyClass hierarchyClass, int traitID, bool addTrait, string traitValue)
        {
            var hierarchyClassTrait = hierarchyClass.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == traitID);

            if (addTrait)
            {
                if (hierarchyClassTrait == null)
                {
                    hierarchyClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                    {
                        hierarchyClassID = hierarchyClass.hierarchyClassID,
                        traitID = traitID,
                        traitValue = traitValue
                    });

                    if (hierarchyClass.hierarchyID == Hierarchies.Financial && traitID == Traits.NonAlignedSubteam)
                    {
                        var merchandiseHierarchyClasses = context.HierarchyClass
                            .Where(hc => hc.hierarchyID == Hierarchies.Merchandise &&
                                    hc.hierarchyLevel == HierarchyLevels.SubBrick &&
                                    hc.HierarchyClassTrait.Any(hct => hct.traitValue == hierarchyClass.hierarchyClassName && hct.traitID == Traits.MerchFinMapping));

                        foreach (var merchandiseHierarchyClass in merchandiseHierarchyClasses)
                        {
                            if (!merchandiseHierarchyClass.HierarchyClassTrait.Any(hct => hct.traitID == Traits.NonAlignedSubteam))
                            {
                                merchandiseHierarchyClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                                    {
                                        hierarchyClassID = merchandiseHierarchyClass.hierarchyClassID,
                                        traitID = traitID,
                                        traitValue = traitValue
                                    });
                            }
                        }
                    }
                }
            }
            else
            {
                if (hierarchyClassTrait != null)
                {
                    context.HierarchyClassTrait.Remove(hierarchyClassTrait);

                    if (hierarchyClass.hierarchyID == Hierarchies.Financial && traitID == Traits.NonAlignedSubteam)
                    {
                        var disableEventGenerationTraits = context.HierarchyClass
                            .Where(hc => hc.hierarchyID == Hierarchies.Merchandise &&
                                    hc.hierarchyLevel == HierarchyLevels.SubBrick &&
                                    hc.HierarchyClassTrait.Any(hct => hct.traitValue == hierarchyClass.hierarchyClassName && hct.traitID == Traits.MerchFinMapping))
                            .SelectMany(hc => hc.HierarchyClassTrait)
                            .Where(hct => hct.traitID == Traits.NonAlignedSubteam);

                        context.HierarchyClassTrait.RemoveRange(disableEventGenerationTraits);
                    }
                }
            }
        }
    }
}
