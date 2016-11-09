﻿using Icon.Common.DataAccess;
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
    public class BulkImportItemCommandHandler : ICommandHandler<BulkImportCommand<BulkImportItemModel>>
    {
        private readonly IconContext context;
        private ILogger logger;

        public BulkImportItemCommandHandler(ILogger logger, IconContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportItemModel> data)
        {
            SqlParameter itemList = new SqlParameter("itemList", SqlDbType.Structured);
            SqlParameter userName = new SqlParameter("userName", SqlDbType.NVarChar);
            SqlParameter updateAllItemFields = new SqlParameter("updateAllItemFields", SqlDbType.Bit);
            itemList.TypeName = "app.ItemImportType";

            // The procedure doesn't need the "Lineage" properties, so convert to an anonymous object that contains only the properties needed to perform the update.
            var itemListData = data.BulkImportData.ConvertAll(item => new
            {
                ScanCode = item.ScanCode,
                ProductDescription = item.ProductDescription,
                PosDescription = item.PosDescription,
                PackageUnit = item.PackageUnit,
                FoodStampEligible = item.FoodStampEligible,
                PosScaleTare = item.PosScaleTare,
                RetailSize = item.RetailSize,
                RetailUom = item.RetailUom,
                DeliverySystem = item.DeliverySystem,
                BrandId = item.BrandId,
                BrowsingId = item.BrowsingId,
                MerchandiseId = item.MerchandiseId,
                TaxClassId = item.TaxId,
                IsValidated = item.IsValidated,
                DepartmentSale = item.DepartmentSale,
                HiddenItem = item.HiddenItem,
                NationalId = item.NationalId,
                Notes = item.Notes,
                AnimalWelfareRatingId = item.AnimalWelfareRatingId,
                Biodynamic = item.Biodynamic,
                CheeseMilkTypeId = item.CheeseAttributeMilkTypeId,
                CheeseRaw = item.CheeseAttributeRaw,
                EcoScaleRatingId = item.EcoScaleRatingId,
                GlutenFreeAgencyId = item.GlutenFreeAgencyId,
                KosherAgencyId = item.KosherAgencyId,
                Msc = item.Msc,
                NonGmoAgencyId = item.NonGmoAgencyId,
                OrganicAgencyId = item.OrganicAgencyId,
                PremiumBodyCare = item.PremiumBodyCare,
                SeafoodFreshOrFrozenId = item.SeafoodFreshOrFrozenId,
                SeafoodCatchTypeId = item.SeafoodWildOrFarmRaisedId,
                VeganAgencyId = item.VeganAgencyId,
                Vegetarian = item.Vegetarian,
                WholeTrade = item.WholeTrade,
                GrassFed = item.GrassFed,
                PastureRaised = item.PastureRaised,
                FreeRange = item.FreeRange,
                DryAged = item.DryAged,
                AirChilled = item.AirChilled,
                MadeInHouse = item.MadeInHouse,
                AlcoholByVolume = item.AlcoholByVolume,
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
            updateAllItemFields.Value = data.UpdateAllItemFields;

            string sql = "EXEC app.ItemImport @itemList, @userName, @updateAllItemFields";

            try
            {
                context.Database.ExecuteSqlCommand(sql, itemList, userName, updateAllItemFields);
                logger.Info(string.Format("User {0} imported {1} item(s) through bulk item import.", data.UserName, data.BulkImportData.Count));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Bulk Item Import Failed. User {0} imported {1} item(s) through bulk item import. Exception: {2}.", data.UserName, data.BulkImportData.Count, ex));
                throw;
            }
        }
    }
}