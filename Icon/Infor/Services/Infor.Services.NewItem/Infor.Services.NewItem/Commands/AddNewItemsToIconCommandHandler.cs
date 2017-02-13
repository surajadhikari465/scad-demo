using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Infor.Services.NewItem.Cache;
using Infor.Services.NewItem.Constants;
using Infor.Services.NewItem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Commands
{
    public class AddNewItemsToIconCommandHandler : ICommandHandler<AddNewItemsToIconCommand>
    {
        private IRenewableContext<IconContext> context;
        private IIconCache cache;

        public AddNewItemsToIconCommandHandler(IRenewableContext<IconContext> context, IIconCache cache)
        {
            this.context = context;
            this.cache = cache;
        }

        public void Execute(AddNewItemsToIconCommand data)
        {
            if (data.NewItems.Any())
            {
                var taxClassCodesToIdDictionary = cache.TaxClassCodesToIdDictionary;
                var nationalClassCodesToIdDictionary = cache.NationalClassCodesToIdDictionary;

                var newItemsParameter = data.NewItems
                    .Select(i => new
                    {
                        RegionCode = i.Region,
                        Identifier = i.ScanCode,
                        DefaultIdentifier = i.IsDefaultIdentifier,
                        BrandName = i.BrandName,
                        ItemDescription = i.ItemDescription,
                        PosDescription = i.PosDescription,
                        PackageUnit = i.PackageUnit,
                        RetailSize = i.RetailSize,
                        RetailUom = i.RetailUom,
                        FoodStamp = i.FoodStampEligible,
                        PosScaleTare = 0.0,
                        DepartmentSale = false,
                        GiftCard = false,
                        TaxClassID = GetTaxClassId(i, taxClassCodesToIdDictionary),
                        MerchandiseClassID = (int?)null,
                        IrmaSubTeamName = i.SubTeamName,
                        NationalClassID = GetNationalClassId(i, nationalClassCodesToIdDictionary),
                        OrganicAgencyId = (int?)null
                    })
                    .ToTvp("items", "app.IRMAItemType");

                try
                {
                    context.Context.Database.ExecuteSqlCommand("EXEC infor.AddNewItems @items", newItemsParameter);
                }
                catch (Exception ex)
                {
                    foreach (var item in data.NewItems.Where(i => i.ErrorCode == null))
                    {
                        item.ErrorCode = ApplicationErrors.Codes.FailedToAddItemsToIcon;
                        item.ErrorDetails = ex.ToString();
                    }
                    throw;
                }
            }
        }

        private static int? GetNationalClassId(Models.NewItemModel item, Dictionary<string, int> nationalClassCodesToIdDictionary)
        {
            if (!string.IsNullOrWhiteSpace(item.NationalClassCode) && nationalClassCodesToIdDictionary.ContainsKey(item.NationalClassCode))
            {
                return nationalClassCodesToIdDictionary[item.NationalClassCode];
            }
            else
            {
                return null;
            }
        }

        private static int? GetTaxClassId(Models.NewItemModel item, Dictionary<string, int> taxClassCodesToIdDictionary)
        {
            if (!string.IsNullOrWhiteSpace(item.TaxClassCode) && taxClassCodesToIdDictionary.ContainsKey(item.TaxClassCode))
            {
                return taxClassCodesToIdDictionary[item.TaxClassCode];
            }
            else
            {
                return null;
            }
        }
    }
}
