using BulkItemUploadProcessor.Common;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Mappers
{
    public class RowObjectToItemMapperResponse<TItem>
    {
        public List<TItem> Items { get; set; }
        public Dictionary<TItem, RowObject> ItemToRowDictionary { get; set; }

        public RowObject GetItemsRowObject(TItem item)
        {
            if(ItemToRowDictionary.ContainsKey(item))
            {
                return ItemToRowDictionary[item];
            }
            else
            {
                return null;
            }
        }
    }
}
