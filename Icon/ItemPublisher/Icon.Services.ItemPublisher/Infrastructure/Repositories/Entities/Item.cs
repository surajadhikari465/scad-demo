using System;

namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDescription { get; set; }
        public string ItemAttributesJson { get; set; }
        public int ScanCodeId { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDesc { get; set; }
        public DateTime SysStartTimeUtc { get; set; }
        public DateTime SysEndTimeUtc { get; set; }
    }
}