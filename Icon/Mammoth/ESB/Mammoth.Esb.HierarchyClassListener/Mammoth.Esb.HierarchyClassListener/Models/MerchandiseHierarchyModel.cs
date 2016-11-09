using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Models
{
    public class MerchandiseHierarchyModel
    {
        public int? SegmentHcid { get; set; }
        public int? FamilyHcid { get; set; }
        public int? ClassHcid { get; set; }
        public int? BrickHcid { get; set; }
        public int? SubBrickHcid { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
