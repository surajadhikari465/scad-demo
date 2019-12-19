using Icon.Framework;
using System;

namespace Icon.Web.Common.Utility
{
    public static class MerchToItemTypeCodeMapper
    {
        public static string GetItemTypeCode(string nonMerchandiseTraitValue)
        {
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

            return itemTypeCode;
        }

        public static int GetItemTypeId(string nonMerchandiseTraitValue)
        {
            int itemTypeId;

            switch (nonMerchandiseTraitValue)
            {
                case NonMerchandiseTraits.BottleDeposit:
                case NonMerchandiseTraits.Crv:
                    itemTypeId = ItemTypes.Deposit;
                    break;
                case NonMerchandiseTraits.BottleReturn:
                case NonMerchandiseTraits.CrvCredit:
                    itemTypeId = ItemTypes.Return;
                    break;
                case NonMerchandiseTraits.Coupon:
                    itemTypeId = ItemTypes.Coupon;
                    break;
                case NonMerchandiseTraits.LegacyPosOnly:
                case NonMerchandiseTraits.NonRetail:
                    itemTypeId = ItemTypes.NonRetail;
                    break;
                case NonMerchandiseTraits.BlackhawkFee:
                    itemTypeId = ItemTypes.Fee;
                    break;
                default:
                    itemTypeId = ItemTypes.RetailSale;
                    break;
            }

            return itemTypeId;
        }
    }
}