using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateIrmaItemCommandHandler : ICommandHandler<UpdateIrmaItemCommand>
    {
        private IconContext context;

        public UpdateIrmaItemCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateIrmaItemCommand data)
        {
            IRMAItem updatedItem = context.IRMAItem.Find(data.IrmaItemId);
            updatedItem.itemDescription = data.ItemDescription;
            updatedItem.posDescription = data.PosDescription;
            updatedItem.packageUnit = data.PackageUnit;
            updatedItem.retailSize = data.RetailSize;
            updatedItem.retailUom = data.RetailUom;
            updatedItem.DeliverySystem = data.DeliverySystem;
            updatedItem.brandName = data.BrandName;
            updatedItem.foodStamp = data.FoodStampEligible;
            updatedItem.posScaleTare = data.PosScaleTare;
            updatedItem.merchandiseClassID = data.MerchandiseHierarchyClassId;
            updatedItem.taxClassID = data.TaxHierarchyClassId;
            updatedItem.nationalClassID = data.NationalHierarchyClassId;
            updatedItem.AnimalWelfareRating = data.AnimalWelfareRating;
            updatedItem.Biodynamic = data.Biodynamic;
            updatedItem.MilkType = data.CheeseMilkType;
            updatedItem.CheeseRaw = data.CheeseRaw;
            updatedItem.EcoScaleRating = data.EcoScaleRating;
            updatedItem.Msc = data.Msc;
            updatedItem.PremiumBodyCare = data.PremiumBodyCare;
            updatedItem.FreshOrFrozen = data.SeafoodFreshOrFrozen;
            updatedItem.SeafoodCatchType = data.SeafoodCatchType;
            updatedItem.Vegetarian = data.Vegetarian;
            updatedItem.WholeTrade = data.WholeTrade;
            updatedItem.GrassFed = data.GrassFed;
            updatedItem.PastureRaised = data.PastureRaised;
            updatedItem.FreeRange = data.FreeRange;
            updatedItem.DryAged = data.DryAged;
            updatedItem.AirChilled = data.AirChilled;
            updatedItem.MadeInHouse = data.MadeInHouse;
            updatedItem.AlcoholByVolume = data.AlcoholByVolume;

            var entry = context.Entry(updatedItem);
            entry.Property(p => p.itemDescription).IsModified = true;
            entry.Property(p => p.posDescription).IsModified = true;
            entry.Property(p => p.packageUnit).IsModified = true;
            entry.Property(p => p.retailSize).IsModified = true;
            entry.Property(p => p.retailUom).IsModified = true;
            entry.Property(p => p.brandName).IsModified = true;
            entry.Property(p => p.foodStamp).IsModified = true;
            entry.Property(p => p.posScaleTare).IsModified = true;
            entry.Property(p => p.merchandiseClassID).IsModified = true;
            entry.Property(p => p.taxClassID).IsModified = true;
            entry.Property(p => p.nationalClassID).IsModified = true;

            try
            {
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("Error updating IrmaItemId {0}", data.IrmaItemId), exception);
            }
            
        }
    }
}
