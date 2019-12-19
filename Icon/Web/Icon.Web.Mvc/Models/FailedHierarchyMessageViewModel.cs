using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedHierarchyMessageViewModel
    {
        public int Id { get; set; }
        public string HierarchyName { get; set; }
        public string HierarchyClassName { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}