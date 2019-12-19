using System.Collections.Generic;
using System.Linq;
using OOSCommon.Import;
using OutOfStock.Messages;

namespace OOS.Model
{
    public class ItemDataMapper
    {
        public static IEnumerable<KnownUploadItem> ToKnownUploadItems(IEnumerable<OOSKnownItemData> itemDatas)
        {
            return itemDatas.Select(ToKnownUploadItem).ToList();
        }

        public static KnownUploadItem ToKnownUploadItem(OOSKnownItemData itemData)
        {
            var item = new KnownUploadItem
                             {
                                 Upc = itemData.name, 
                                 Vin = itemData.vin.ToString(),
                                 ReasonCode = itemData.reason_code.ToString(),
                                 StartDate = itemData.start_date,
                                 ProductStatus = itemData.ProductStatus,
                                 ExpirationDate = itemData.ExpirationDate,
                             };
            return item;
        }

        public static IEnumerable<OOSKnownItemData> ToOOSKnownItemDatas(IEnumerable<KnownUploadItem> items)
        {
            return items.Select(ToOOSKnownItemData).ToList();
        }

        public static OOSKnownItemData ToOOSKnownItemData(KnownUploadItem item)
        {
            return new OOSKnownItemData(item.Upc, item.ReasonCode, item.StartDate.ToString(), item.Vin, item.ProductStatus, item.ExpirationDate);
        }
    }
}
