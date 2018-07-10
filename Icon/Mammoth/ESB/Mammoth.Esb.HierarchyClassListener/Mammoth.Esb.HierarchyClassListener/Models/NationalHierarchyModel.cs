using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Models
{
    public class NationalHierarchyModel
    {
        public int? FamilyHcid { get; set; }
        public int? CategoryHcid { get; set; }
        public int? SubcategoryHcid { get; set; }
        public int? ClassHcid { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
