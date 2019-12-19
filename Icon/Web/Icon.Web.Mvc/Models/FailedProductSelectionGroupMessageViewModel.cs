using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedProductSelectionGroupMessageViewModel
    {
        public int Id { get; set; }
        public string ProductSelectionGroupName { get; set; }
        public string ProductSelectionGroupTypeName { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}