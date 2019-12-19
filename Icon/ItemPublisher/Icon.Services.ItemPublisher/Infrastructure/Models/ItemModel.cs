using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Models
{
    /// <summary>
    /// Special model specifically for Item because we need to have hospitality and kitchen properties and those
    /// are conceptually attributes
    /// </summary>
    public class ItemModel
    {
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDescription { get; set; }
        public Dictionary<string, string> ItemAttributes { get; set; }
        public int ScanCodeId { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDesc { get; set; }
        public DateTime SysStartTimeUtc { get; set; }
        public DateTime SysEndTimeUtc { get; set; }
        public bool IsKitchenItemSpecified { get; set; }
        public bool IsKitchenItem { get; set; }
        public bool IsHospitalityItemSpecified { get; set; }
        public bool IsHospitalityItem { get; set; }
        public string ImageUrl { get; set; }
        public string KitchenDescription { get; set; }
    }
}