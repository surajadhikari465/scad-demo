using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Models
{
    public class SubTeamModel
    {
        public string FinancialHierarchyCode { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int PosDepartmentNumber { get; set; }
    }
}
