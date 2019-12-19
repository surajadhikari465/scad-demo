using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.VIM
{
    public interface IVIMRepository
    {
        Dictionary<string, ItemMasterModel> GetItemMasterModel(IEnumerable<string> upcs);
        ItemMasterModel GetItemMasterModel(string upc);
        IEnumerable<ItemMasterModel> ItemMasterModel();
        IEnumerable<VIMOOSItemDataView> GetVIMOOSItemDataView(string upc, string ps_bu);
        Dictionary<string,List<VIMOOSItemDataView>> GetVIMOOSItemDataView(List<string> upcs, string ps_bu);
        IEnumerable<VimSubTeam> GetVimSubTeam(string region, List<string> storeNumbers, List<string> teams);
        IEnumerable<VimTeam> GetVimTeam();
        string GetVimUPC(string upc, string userRegion);
        HashSet<string> GetVimUPC(List<string> upcs, string userRegion);
    }
}
