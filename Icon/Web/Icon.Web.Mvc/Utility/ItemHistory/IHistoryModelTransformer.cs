using System.Collections.Generic;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Mvc.Utility.ItemHistory
{
    public interface IHistoryModelTransformer
    {
        List<ItemHistoryModel> ToViewModels(IEnumerable<ItemHistoryDbModel> itemHistory);
        List<ItemHistoryModel> TransformInforHistory(IEnumerable<ItemInforHistoryDbModel> inforItemHistory, IEnumerable<AttributeDisplayModel> attributes);
    }
}