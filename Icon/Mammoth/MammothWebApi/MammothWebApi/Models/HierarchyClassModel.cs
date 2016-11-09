using System.ComponentModel.DataAnnotations;

namespace MammothWebApi.Models
{
    public class HierarchyClassModel
    {
        public int HierarchyClassId { get; set; }
        public int HierarchyId { get; set; }
        [Required]
        public string HierarchyClassName { get; set; }
        public int? HierarchyParentClassId { get; set; }
    }
}