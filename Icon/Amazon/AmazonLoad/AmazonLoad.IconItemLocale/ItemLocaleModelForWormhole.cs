using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AmazonLoad.IconItemLocale
{
    public class ItemLocaleModelForWormhole
    {
        public ItemLocaleModelForWormhole() { }

        public ItemLocaleModelForWormhole(IconStoreModel store, ItemLocaleModelForIrma irmaData) : this()
        {
            // fill in icon store/locale data
            this.BusinessUnit = store.BusinessUnit;
            this.LocaleName = store.LocaleName;
            this.LocaleTypeCode = store.LocaleTypeCode;
            this.LocaleTypeDesc = store.LocaleTypeDesc;
            this.LocaleID = store.LocaleID;
            this.RegionCode = store.RegionCode;

            // fill in IRMA data
            this.InforItemId = irmaData.InforItemId;
            this.ScanCode = irmaData.Identifier;
            var scanCodeIdDesc = CalcTypeIdAndDescForScanCode(this.ScanCode);
            this.ScanCodeTypeId = scanCodeIdDesc.Item1;
            this.ScanCodeTypeDesc = scanCodeIdDesc.Item2;
            this.ItemTypeCode = irmaData.ItemTypeCode;
            this.ItemTypeDesc = GetItemTypeDescFromCode(ItemTypeCode);
            this.Authorized = irmaData.Authorized;
            this.LockedForSale = irmaData.LockedForSale;
            this.Recall = irmaData.Recall.GetValueOrDefault(false);
            this.Quantity_Required = irmaData.Quantity_Required;
            this.Price_Required = irmaData.Price_Required;
            this.QtyProhibit = irmaData.QtyProhibit.GetValueOrDefault(false);
            this.Case_Discount = irmaData.Case_Discount;
            this.Sold_By_Weight = irmaData.Sold_By_Weight;
            this.VisualVerify = irmaData.VisualVerify;
            // multiply PosScaleTare by .01 for the outgoing message
            this.PosScaleTare = irmaData.PosScaleTare.GetValueOrDefault(0) * .01m;
            this.TMDiscountEligible = irmaData.TMDiscountEligible;
            this.AgeCode = irmaData.AgeCode;
            this.Restricted_Hours = irmaData.Restricted_Hours;
            this.LinkedItemId = irmaData.LinkedItem_InforItemId;
            this.LinkedItemScanCode = irmaData.LinkedItem_Identifier;
            this.LinkedItemTypeCode = irmaData.LinkedItem_Type;
        }

        public int ItemId
        {
            get
            {
                return InforItemId;
            }
            set
            {
                InforItemId = value;
            }
        }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public int BusinessUnit { get; set; }
        public string LocaleName { get; set; }
        public string LocaleTypeCode { get; set; }
        public string LocaleTypeDesc { get; set; }
        public int LocaleID { get; set; }
        public string RegionCode { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDesc { get; set; }
        public int? LinkedItemId { get; set; }
        public string LinkedItemScanCode { get; set; }
        public string LinkedItemTypeCode { get; set; }
        public int InforItemId { get; set; }
        public bool Authorized { get; set; }
        public bool LockedForSale { get; set; }
        public bool Recall { get; set; }
        public bool Sold_By_Weight { get; set; }
        public bool Quantity_Required { get; set; }
        public bool Price_Required { get; set; }
        public bool QtyProhibit { get; set; }
        public bool VisualVerify { get; set; }
        public decimal? PosScaleTare { get; set; }
        public bool TMDiscountEligible { get; set; }
        public bool Case_Discount { get; set; }
        public int? AgeCode { get; set; }
        public bool Restricted_Hours { get; set; }        

        public string GetItemTypeDescFromCode(string itemTypeCode)
        {
            if (!string.IsNullOrWhiteSpace(itemTypeCode))
            {
                // get the iCON.dbo.ItemType desc based on the itemType code from IRMA Validated ScanCode
                //itemTypeID itemTypeCode itemTypeDesc
                //  1       RTL         Retail Sale
                //  2       DEP         Deposit
                //  3       TAR         Tare
                //  4       RTN         Return
                //  5       CPN         Coupon
                //  6       NRT         Non-Retail
                //  7       FEE         Fee
                
                switch (itemTypeCode)
                {
                    case ItemTypeCodes.RetailSale:
                        return ItemTypeDescriptions.RetailSale;
                    case ItemTypeCodes.Deposit:
                        return ItemTypeDescriptions.Deposit;
                    case ItemTypeCodes.Tare:
                        return ItemTypeDescriptions.Tare;
                    case ItemTypeCodes.Return:
                        return ItemTypeDescriptions.Return;
                    case ItemTypeCodes.Coupon:
                        return ItemTypeDescriptions.Coupon;
                    case ItemTypeCodes.NonRetail:
                        return ItemTypeDescriptions.NonRetail;
                    case ItemTypeCodes.Fee:
                        return ItemTypeDescriptions.Fee;
                    default:
                        break;
                }
            }
            return (string)null;
        }

        private Tuple<int, string> CalcTypeIdAndDescForScanCode(string scanCode)
        {
            // iCON.dbo.ScanCodeType
            // scanCodeTypeID scanCodeTypeDesc
            //      1           UPC
            //      2           POS PLU
            //      3           Scale PLU

            int typeId = 0;
            string typeDesc = "";

            const string RegEx_ScalePlu = "^2[0-9]{5}0{5}$";
            const string RegEx_IngredientPlu46 = "^460{4}[0-9]{5}$";
            const string RegEx_IngredientPlu48 = "^480{4}[0-9]{5}$";

            if (Regex.IsMatch(scanCode, RegEx_ScalePlu)
                || Regex.IsMatch(scanCode, RegEx_IngredientPlu46)
                || Regex.IsMatch(scanCode, RegEx_IngredientPlu48))
            {
                typeId = ScanCodeTypes.ScalePlu;
                typeDesc = ScanCodeTypes.Descriptions.ScalePlu;
            }
            else if (scanCode.Length < 7)
            {
                typeId = ScanCodeTypes.PosPlu;
                typeDesc = ScanCodeTypes.Descriptions.PosPlu;
            }
            else
            {
                typeId = ScanCodeTypes.Upc;
                typeDesc = ScanCodeTypes.Descriptions.Upc;
            }
            return new Tuple<int, string>(typeId, typeDesc);
        }
    }
}