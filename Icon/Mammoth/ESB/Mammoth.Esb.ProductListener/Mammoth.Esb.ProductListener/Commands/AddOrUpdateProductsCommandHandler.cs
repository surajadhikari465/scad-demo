using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MoreLinq;
using System.Data;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Commands
{
    public class AddOrUpdateProductsCommandHandler : ICommandHandler<AddOrUpdateProductsCommand>
    {
        IDbProvider db;

        public AddOrUpdateProductsCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateProductsCommand data)
        {
            AddOrUpdateItems(data);
            AddOrUpdateItemAttributesSign(data);
            AddOrUpdateItemAttributesNutrition(data);
            AddOrUpdateItemAttributesExtended(data);
            AddOrUpdateItemAttributesKit(data);
        }   

        private void AddOrUpdateItems(AddOrUpdateProductsCommand data)
        {
            string sql = @"dbo.AddOrUpdateItems";
            int rowCount = this.db.Connection.Execute(
                sql,
                new
                {
                    globalAttributes = data.Items.Select(
                        i => new
                        {
                            ItemID = i.GlobalAttributes.ItemID,
                            ItemTypeID = i.GlobalAttributes.ItemTypeID,
                            ScanCode = i.GlobalAttributes.ScanCode,
                            SubBrickID = i.GlobalAttributes.SubBrickID,
                            NationalClassID = i.GlobalAttributes.NationalClassID,
                            BrandHCID = i.GlobalAttributes.BrandHCID,
                            TaxClassHCID = i.GlobalAttributes.TaxClassHCID,
                            Desc_Product = i.GlobalAttributes.Desc_Product,
                            Desc_POS = i.GlobalAttributes.Desc_POS,
                            PackageUnit = i.GlobalAttributes.PackageUnit,
                            RetailSize = i.GlobalAttributes.RetailSize,
                            RetailUOM = i.GlobalAttributes.RetailUOM,
                            PSNumber = i.GlobalAttributes.PSNumber,
                            FoodStampEligible = i.GlobalAttributes.FoodStampEligible,
                            CustomerFriendlyDescription = i.GlobalAttributes.Desc_CustomerFriendly
                        })
                    .ToList()
                    .ToDataTable()
                },
                transaction: this.db.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        private void AddOrUpdateItemAttributesKit(AddOrUpdateProductsCommand data)
        {
            var itemKitAttributes = (from i in data.Items
                select new
                {
                    ItemID = i.GlobalAttributes.ItemID,
                    KitchenItem = i.KitItemAttributes.KitchenItem,
                    HospitalityItem = i.KitItemAttributes.HospitalityItem,
                    ImageUrl = i.KitItemAttributes.ImageUrl,
                    KitchenDescription = i.KitItemAttributes.KitchenDescription
                }).ToDataTable();


            if (itemKitAttributes.Rows.Count == 0) return;

            string sql = "dbo.AddOrUpdateItemAttributesKit";
            int rowCount = this.db.Connection.Execute(sql, new { kitAttributes = itemKitAttributes }, transaction: this.db.Transaction,commandType: CommandType.StoredProcedure);
        }

        private void AddOrUpdateItemAttributesSign(AddOrUpdateProductsCommand data)
        {
            string sql = @"dbo.AddOrUpdateItemAttributesSign";
            int rowCount = this.db.Connection.Execute(sql,
                new { signAttributes = data.Items.Select(i => i.SignAttributes).ToDataTable() },
                transaction: this.db.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        private void AddOrUpdateItemAttributesNutrition(AddOrUpdateProductsCommand data)
        {
            var nutritionAttributes = data.Items
                .Where(i => i.NutritionAttributes != null)
                .Select(i => i.NutritionAttributes);

            if (nutritionAttributes.Any())
            {
                int rowCount = this.db.Connection.Execute(
					sql: "dbo.AddOrUpdateItemAttributesNutrition",
                    param: new { nutritionAttributes = nutritionAttributes.ToDataTable() },
                    transaction: this.db.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
        }

        private void AddOrUpdateItemAttributesExtended(AddOrUpdateProductsCommand data)
        {
            string sql = @"dbo.AddOrUpdateItemAttributesExt";
            int rowCount = this.db.Connection.Execute(sql,
                new
                {
                    extAttributes = data.Items.SelectMany(
                        i => new[]
                        {
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.FairTradeCertified,
                                AttributeValue = i.ExtendedAttributes.FairTradeCertified },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.FlexibleText,
                                AttributeValue = i.ExtendedAttributes.FlexibleText },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.GlobalPricingProgram,
                                AttributeValue = i.ExtendedAttributes.GlobalPricingProgram },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.MadeWithBiodynamicGrapes,
                                AttributeValue = i.ExtendedAttributes.MadeWithBiodynamicGrapes },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.MadeWithOrganicGrapes,
                                AttributeValue = i.ExtendedAttributes.MadeWithOrganicGrapes },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.NutritionRequired,
                                AttributeValue = i.ExtendedAttributes.NutritionRequired },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.PrimeBeef,
                                AttributeValue = i.ExtendedAttributes.PrimeBeef },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.RainforestAlliance,
                                AttributeValue = i.ExtendedAttributes.RainforestAlliance },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.RefrigeratedOrShelfStable,
                                AttributeValue = i.ExtendedAttributes.RefrigeratedOrShelfStable },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.SmithsonianBirdFriendly,
                                AttributeValue = i.ExtendedAttributes.SmithsonianBirdFriendly },
                            new { ItemID = i.ExtendedAttributes.ItemId,
                                AttributeCode = Attributes.Codes.Wic,
                                AttributeValue = i.ExtendedAttributes.Wic }
                        }).ToDataTable()
                },
                transaction: this.db.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}