using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkImportNewItemCommandHandler : ICommandHandler<BulkImportCommand<BulkImportNewItemModel>>
    {
        private readonly IconContext context;
        private ILogger logger;

        public BulkImportNewItemCommandHandler(ILogger logger, IconContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportNewItemModel> data)
        {
            SqlParameter itemList = new SqlParameter("ItemList", SqlDbType.Structured);
            SqlParameter userName = new SqlParameter("UserName", SqlDbType.NVarChar);
            itemList.TypeName = "app.ItemAddType";

            // A BrandId of zero indicates a new brand, in which case we'll need to include the new brand name in the table type.
            var itemListData = data.BulkImportData.ConvertAll(item => new
            {
                ScanCode = item.ScanCode.TrimStart('0'),
                ProductDescription = item.ProductDescription,
                PosDescription = item.PosDescription,
                PackageUnit = item.PackageUnit,
                FoodStampEligible = item.FoodStampEligible,
                PosScaleTare = item.PosScaleTare,
                RetailSize = item.RetailSize,
                RetailUom = item.RetailUom,
                DeliverySystem = item.DeliverySystem,
                BrandId = item.BrandId,
                BrandName = item.BrandId == "0" ? item.BrandName : String.Empty,
                MerchandiseId = item.MerchandiseId,
                TaxId = item.TaxId,
                NationalClassId = item.NationalId,
                BrowsingId = item.BrowsingId,
                IsValidated = item.IsValidated,
                AnimalWelfareRating = GetNullableValue(item.AnimalWelfareRating),
                Biodynamic = GetNullableValue(item.Biodynamic),
                CheeseMilkType = GetNullableValue(item.CheeseAttributeMilkType),
                CheeseRaw = GetNullableValue(item.CheeseAttributeRaw),
                EcoScaleRating = GetNullableValue(item.EcoScaleRating),
                Msc = GetNullableValue(item.Msc),
                PremiumBodyCare = GetNullableValue(item.PremiumBodyCare),
                SeafoodFreshOrFrozen = GetNullableValue(item.SeafoodFreshOrFrozen),
                SeafoodCatchType = GetNullableValue(item.SeafoodWildOrFarmRaised),
                Vegetarian = GetNullableValue(item.Vegetarian),
                WholeTrade = GetNullableValue(item.WholeTrade),
                GrassFed = GetNullableValue(item.GrassFed),
                PastureRaised = GetNullableValue(item.PastureRaised),
                FreeRange = GetNullableValue(item.FreeRange),
                DryAged = GetNullableValue(item.DryAged),
                AirChilled = GetNullableValue(item.AirChilled),
                MadeInHouse = GetNullableValue(item.MadeInHouse),
                AlcoholByVolume = GetNullableValue(item.AlcoholByVolume),
                CaseinFree = item.CaseinFree,
                DrainedWeight = item.DrainedWeight,
                DrainedWeightUom = item.DrainedWeightUom,
                FairTradeCertified = item.FairTradeCertified,
                Hemp = item.Hemp,
                LocalLoanProducer = item.LocalLoanProducer,
                MainProductName = item.MainProductName,
                NutritionRequired = item.NutritionRequired,
                OrganicPersonalCare = item.OrganicPersonalCare,
                Paleo = item.Paleo,
                ProductFlavorType = item.ProductFlavorType
            });

            itemList.Value = itemListData.ToDataTable();
            userName.Value = data.UserName;

            string sql = "EXEC app.BulkItemAdd @ItemList, @UserName";

            try
            {
                context.Database.ExecuteSqlCommand(sql, itemList, userName);
                logger.Info(String.Format("User {0} added {1} item(s) through bulk item add.", data.UserName, data.BulkImportData.Count));
            }
            catch (Exception ex)
            {
                throw new CommandException("The CommandHandler threw an exception.", ex);
            }
        }

        private string GetNullableValue(string value)
        {
            return String.IsNullOrEmpty(value) ? null : value;
        }
    }
}
