using System;

namespace Icon.Web.DataAccess.Models
{
    public class ItemHistoryDbModel
    {
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemAttributesJson { get; set; }
        public DateTime SysStartTimeUtc { get; set; }
        public DateTime SysEndTimeUtc { get; set; }
    }
}
