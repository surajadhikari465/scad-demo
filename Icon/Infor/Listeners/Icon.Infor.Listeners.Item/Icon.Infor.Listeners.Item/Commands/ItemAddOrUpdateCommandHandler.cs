using Icon.Common;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Extensions;
using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Item.Commands
{
    public class ItemAddOrUpdateCommandHandler : ICommandHandler<ItemAddOrUpdateCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public ItemAddOrUpdateCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Execute(ItemAddOrUpdateCommand data)
        {
            var itemsWithoutErrors = data.Items.Where(i => i.ErrorCode == null);
            try
            {
                using (var context = contextFactory.CreateContext())
                {
                    AddOrUpdateItems(context, itemsWithoutErrors);
                    AddOrUpdateItemTraits(context, itemsWithoutErrors);
                    AddOrUpdateItemHierarchyClasses(context, itemsWithoutErrors);
                    AddOrUpdateItemSignAttributes(context, itemsWithoutErrors);
                }
            }
            catch (Exception ex)
            {
                string errorDetails = ApplicationErrors.Messages.ItemAddOrUpdateError + " Exception: " + ex.ToString();
                foreach (var item in itemsWithoutErrors)
                {
                    item.ErrorCode = ApplicationErrors.Codes.ItemAddOrUpdateError;
                    item.ErrorDetails = errorDetails;
                }
            }
        }

        private void AddOrUpdateItems(IconContext context, IEnumerable<ItemModel> data)
        {
            var items = data
                            .Select(i => new
                            {
                                ItemId = i.ItemId,
                                ItemTypeId = ItemTypes.Ids[i.ItemTypeCode],
                                ScanCode = i.ScanCode,
                                ScanCodeTypeId = ScanCodeTypes.Ids[i.ScanCodeType],
                                InforMessageId = i.InforMessageId,
                                SequenceId = i.SequenceId
                            }).ToTvp("items", "infor.ItemAddOrUpdateType");

            context.Database.ExecuteSqlCommand("exec infor.ItemAddOrUpdate @items", items);
        }

        private void AddOrUpdateItemTraits(IconContext context, IEnumerable<ItemModel> data)
        {
            var itemTraits = data
                            .SelectMany(i => new[] {
                                new ItemTraitModel(i.ItemId, Traits.ProductDescription, i.ProductDescription, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.PosDescription, i.PosDescription, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.FoodStampEligible, i.FoodStampEligible, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.PosScaleTare, i.PosScaleTare, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.ProhibitDiscount, i.ProhibitDiscount, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.PackageUnit, i.PackageUnit, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.RetailSize, i.RetailSize, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.RetailUom, i.RetailUom, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.AlcoholByVolume, i.AlcoholByVolume, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.CaseinFree, i.CaseinFree, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.DrainedWeight, i.DrainedWeight, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.DrainedWeightUom, i.DrainedWeightUom, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.FairTradeCertified, i.FairTradeCertified, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.Hemp, i.Hemp, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.LocalLoanProducer, i.LocalLoanProducer, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.MainProductName, i.MainProductName, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.NutritionRequired, i.NutritionRequired, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.OrganicPersonalCare, i.OrganicPersonalCare, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.Paleo, i.Paleo, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.ProductFlavorType, i.ProductFlavorType, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.InsertDate, i.InsertDate, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.ModifiedDate, i.ModifiedDate, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.ModifiedUser, i.ModifiedUser, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.HiddenItem, i.HiddenItem, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.Notes, i.Notes, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.DeliverySystem, i.DeliverySystem, Locales.WholeFoods),
                                new ItemTraitModel(i.ItemId, Traits.CustomerFriendlyDescription, i.CustomerFriendlyDescription, Locales.WholeFoods),
                            }).ToTvp("itemTraits", "infor.ItemTraitAddOrUpdateType");

            context.Database.ExecuteSqlCommand("exec infor.ItemTraitAddOrUpdate @itemTraits", itemTraits);
        }

        private void AddOrUpdateItemHierarchyClasses(IconContext context, IEnumerable<ItemModel> data)
        {
            var itemHierarchies = data
                         .SelectMany(i => new[] {
                                new ItemHierarchyClassModel(i.ItemId, Hierarchies.Brands, i.BrandsHierarchyClassId),
                                new ItemHierarchyClassModel(i.ItemId, Hierarchies.Merchandise, i.MerchandiseHierarchyClassId),
                                new ItemHierarchyClassModel(i.ItemId, Hierarchies.Tax, i.TaxHierarchyClassId),
                                new ItemHierarchyClassModel(i.ItemId, Hierarchies.Financial, i.FinancialHierarchyClassId),
                                new ItemHierarchyClassModel(i.ItemId, Hierarchies.National, i.NationalHierarchyClassId)
                         }).ToTvp("itemHierarchyClasses", "infor.ItemHierarchyClassAddOrUpdateType");

            context.Database.ExecuteSqlCommand("exec infor.ItemHierarchyClassAddOrUpdate @itemHierarchyClasses", itemHierarchies);
        }

        private void AddOrUpdateItemSignAttributes(IconContext context, IEnumerable<ItemModel> data)
        {
            var itemSignAttributes = data
                     .Select(i => new ItemSignAttributeModel
                     (
                         i.ItemId, 
                         AnimalWelfareRatings.Ids.GetIdFromDescription(i.AnimalWelfareRating), 
                         i.Biodynamic.ToBool(), 
                         MilkTypes.Ids.GetIdFromDescription(i.CheeseMilkType), 
                         i.CheeseRaw.ToBool(),
                         EcoScaleRatings.Ids.GetIdFromDescription(i.EcoScaleRating),
                         i.GlutenFree,
                         i.Kosher,
                         i.Msc.ToBool(),
                         i.NonGmo,
                         i.Organic,
                         i.PremiumBodyCare.ToBool(),
                         SeafoodFreshOrFrozenTypes.Ids.GetIdFromDescription(i.FreshOrFrozen),
                         SeafoodCatchTypes.Ids.GetIdFromDescription(i.SeafoodCatchType),
                         i.Vegan,
                         i.Vegetarian.ToBool(),
                         i.WholeTrade.ToBool(),
                         i.GrassFed.ToBool(),
                         i.PastureRaised.ToBool(),
                         i.FreeRange.ToBool(),
                         i.DryAged.ToBool(),
                         i.AirChilled.ToBool(),
                         i.MadeInHouse.ToBool(),
                         i.CustomerFriendlyDescription
                     )).ToTvp("itemSignAttributes", "infor.ItemSignAttributeAddOrUpdateType");

            context.Database.ExecuteSqlCommand("exec infor.ItemSignAttributeAddOrUpdate @itemSignAttributes", itemSignAttributes);
        }
    }
}
