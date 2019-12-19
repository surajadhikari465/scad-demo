using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MoreLinq;
using System;
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
                            CustomerFriendlyDescription = i.GlobalAttributes.Desc_CustomerFriendly,
                            ProhibitDiscount = i.GlobalAttributes.ProhibitDiscount
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
            var itemExtAttributeValues = data.Items.Where(x => x.ExtendedAttributes != null && x.ExtendedAttributes.Traits != null && x.ExtendedAttributes.Traits.Any())
                            .SelectMany(x => x.ExtendedAttributes.Traits.SelectMany(y => new[]
                                {
                                    new
                                    {
                                        ItemID = x.ExtendedAttributes.ItemId,
                                        AttributeCode = y.Key,
                                        AttributeValue = y.Value
                                    }
                                }
                            )).ToDataTable();
            
            if(itemExtAttributeValues.Rows.Count > 0)
            {
                var result = this.db.Connection.ExecuteScalar(
                    sql: "dbo.AddOrUpdateItemAttributesExt",
                    param: new { extAttributes = itemExtAttributeValues },
                    transaction: this.db.Transaction,
                    commandType: CommandType.StoredProcedure);

                data.TraitCodeWarning = (result == null || result == DBNull.Value) ? null : result.ToString();
            }
        }
    }
}