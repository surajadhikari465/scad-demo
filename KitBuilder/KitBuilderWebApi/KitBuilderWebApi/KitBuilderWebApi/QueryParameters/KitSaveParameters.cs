using System.Collections.Generic;

namespace KitBuilderWebApi.QueryParameters
{

    public class KitSaveParameters
    {
        public int KitId { get; set; }
        public int KitItem { get; set; }
        public string KitDescription { get; set; }
        public List<int> InstructionListIds { get; set; }
        public List<KeyValuePair<int, int>> LinkGroupIds { get; set; }
        public List<KeyValuePair<int, int>> LinkGroupItemIds { get; set; }
    }
}
