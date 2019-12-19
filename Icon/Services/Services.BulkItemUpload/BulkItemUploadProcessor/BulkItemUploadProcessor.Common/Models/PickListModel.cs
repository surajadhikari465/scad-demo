using System.Collections.Generic;

namespace BulkItemUploadProcessor.Common.Models
{
    public class PickListModel
    {
        public int PickListId { get; set; }
        public int? AttributeId { get; set; }
        public string PickListValue { get; set; }
        public bool isPickListSelectforDelete { get; set; }
    }
    public class ListWrapper<T>
    {
        public List<T> List { get; set; }
        public ListWrapper(List<T> list)
        {
            List = list;
        }
    }

}