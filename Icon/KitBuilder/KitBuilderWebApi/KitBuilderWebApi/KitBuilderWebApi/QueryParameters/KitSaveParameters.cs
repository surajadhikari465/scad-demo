using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitBuilderWebApi.QueryParameters
{

    public class KitSaveParameters
    {
        public int KitId { get; set; }
        public int KitItem { get; set; }
        public string KitDescription { get; set; }
        public List<int> InstructionListIds { get; set; }
        public List<int> LinkGroupIds { get; set; }
        public List<int> LinkGroupItemIds { get; set; }
    }
}
