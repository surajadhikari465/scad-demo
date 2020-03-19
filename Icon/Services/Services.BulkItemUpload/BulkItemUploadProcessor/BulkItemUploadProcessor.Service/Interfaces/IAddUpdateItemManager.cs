using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Interfaces
{
    public interface IAddUpdateItemManager
    {
        void CreateItems(List<AddItemModel> addItemModels,
           List<ErrorItem<AddItemModel>> invalidItems,
           List<ItemIdAndScanCode> addedItems);

        void UpdateItems(List<UpdateItemModel> updateItemModels,
             List<ErrorItem<UpdateItemModel>> invalidItems,
             List<ItemIdAndScanCode> updatedItems);

    }
}