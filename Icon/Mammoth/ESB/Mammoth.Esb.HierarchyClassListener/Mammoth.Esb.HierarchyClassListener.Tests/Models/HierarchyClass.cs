using System;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Models
{
    public class HierarchyClass
    {
        public int HierarchyClassID { get; set; }
        public int HierarchyID { get; set; }
        public string HierarchyClassName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
