using Icon.Framework;
using PushController.Common.Models;
using System;
using System.Collections.Generic;

namespace PushController.Common
{
    public static class Extensions
    {
        public static IrmaPushModel ToModel(this IRMAPush irmaPushEntity)
        {
            return new IrmaPushModel
            {
                IrmaPushId = irmaPushEntity.IRMAPushID,
                RegionCode = irmaPushEntity.RegionCode,
                BusinessUnitId = irmaPushEntity.BusinessUnit_ID,
                Identifier = irmaPushEntity.Identifier,
                ChangeType = irmaPushEntity.ChangeType,
                InsertDate = DateTime.Now,
                RetailSize = irmaPushEntity.RetailSize,
                RetailPackageUom = irmaPushEntity.RetailPackageUom,
                TmDiscountEligible = irmaPushEntity.TMDiscountEligible,
                CaseDiscount = irmaPushEntity.Case_Discount,
                AgeCode = irmaPushEntity.AgeCode,
                Recall = irmaPushEntity.Recall_Flag,
                RestrictedHours = irmaPushEntity.Restricted_Hours,
                SoldByWeight = irmaPushEntity.Sold_By_Weight,
                ScaleForcedTare = irmaPushEntity.ScaleForcedTare,
                QuantityRequired = irmaPushEntity.Quantity_Required,
                PriceRequired = irmaPushEntity.Price_Required,
                QuantityProhibit = irmaPushEntity.QtyProhibit,
                VisualVerify = irmaPushEntity.VisualVerify,
                RestrictSale = irmaPushEntity.RestrictSale,
                PosScaleTare = irmaPushEntity.PosScaleTare,
                LinkedIdentifier = irmaPushEntity.LinkedIdentifier,
                Price = irmaPushEntity.Price,
                RetailUom = irmaPushEntity.RetailUom,
                Multiple = irmaPushEntity.Multiple,
                SaleMultiple = irmaPushEntity.SaleMultiple,
                SalePrice = irmaPushEntity.Sale_Price,
                SaleStartDate = irmaPushEntity.Sale_Start_Date,
                SaleEndDate = irmaPushEntity.Sale_End_Date,
                InProcessBy = null,
                InUdmDate = null,
                EsbReadyDate = null,
                UdmFailedDate = null,
                EsbReadyFailedDate = null,
                MessageBuildError = null
            };
        }

        public static List<IRMAPush> ToEntities(this List<IrmaPushModel> irmaPushModels)
        {
            var irmaPushEntities = new List<IRMAPush>();

            foreach (var model in irmaPushModels)
            {
                irmaPushEntities.Add(new IRMAPush
                {
                    IRMAPushID = model.IrmaPushId,
                    RegionCode = model.RegionCode,
                    BusinessUnit_ID = model.BusinessUnitId,
                    Identifier = model.Identifier,
                    ChangeType = model.ChangeType,
                    InsertDate = model.InsertDate,
                    RetailSize = model.RetailSize,
                    RetailPackageUom = model.RetailPackageUom,
                    TMDiscountEligible = model.TmDiscountEligible,
                    Case_Discount = model.CaseDiscount,
                    AgeCode = model.AgeCode,
                    Recall_Flag = model.Recall,
                    Restricted_Hours = model.RestrictedHours,
                    Sold_By_Weight = model.SoldByWeight,
                    ScaleForcedTare = model.ScaleForcedTare,
                    Quantity_Required = model.QuantityRequired,
                    Price_Required = model.PriceRequired,
                    QtyProhibit = model.QuantityProhibit,
                    VisualVerify = model.VisualVerify,
                    RestrictSale = model.RestrictSale,
                    PosScaleTare = model.PosScaleTare,
                    LinkedIdentifier = model.LinkedIdentifier,
                    Price = model.Price,
                    RetailUom = model.RetailUom,
                    Multiple = model.Multiple,
                    SaleMultiple = model.SaleMultiple,
                    Sale_Price = model.SalePrice,
                    Sale_Start_Date = model.SaleStartDate,
                    Sale_End_Date = model.SaleEndDate,
                    InProcessBy = model.InProcessBy,
                    InUdmDate = model.InUdmDate,
                    EsbReadyDate = model.EsbReadyDate,
                    UdmFailedDate = model.UdmFailedDate,
                    EsbReadyFailedDate = model.EsbReadyFailedDate
                });
            }

            return irmaPushEntities;
        }

        public static bool ItemIsBottleDepositOrCrv(this ScanCodeModel scanCode)
        {
            if(StartupOptions.UseItemTypeInsteadOfNonMerchTrait)
            {
                return ItemIsBottleDepositOrCrvFromItemType(scanCode);
            }
            else
            {
                return ItemIsBottleDepositOrCrvFromNonMerchTrait(scanCode);
            }
        }

        private static bool ItemIsBottleDepositOrCrvFromItemType(ScanCodeModel scanCode)
        {
            if(scanCode.ItemTypeCode == ItemTypeCodes.Deposit || scanCode.ItemTypeCode == ItemTypeCodes.Fee)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ItemIsBottleDepositOrCrvFromNonMerchTrait(ScanCodeModel scanCode)
        {
            bool nonMerchandiseTraitExists = !String.IsNullOrEmpty(scanCode.NonMerchandiseTrait);

            if (nonMerchandiseTraitExists)
            {
                if (scanCode.NonMerchandiseTrait == NonMerchandiseTraits.BottleDeposit ||
                    scanCode.NonMerchandiseTrait == NonMerchandiseTraits.Crv ||
                    scanCode.NonMerchandiseTrait == NonMerchandiseTraits.BlackhawkFee)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
