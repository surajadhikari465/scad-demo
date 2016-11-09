using System;

namespace PushController.Common.Models
{
    public class ScanCodeModel
    {
        public string ScanCode { get; set; }
        public int ScanCodeId { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDesc { get; set; }
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public string ValidationDate { get; set; }
        public string DepartmentSaleTrait { get; set; }
        public string NonMerchandiseTrait { get; set; }
    }
}
