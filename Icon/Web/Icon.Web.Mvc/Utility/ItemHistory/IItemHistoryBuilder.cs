using System.Collections.Generic;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Mvc.Utility
{
    public interface IItemHistoryBuilder
    {
        void AddHierarchyHistoryToResponse(ItemHistoryViewModel response, ItemHierarchyClassHistoryAllModel hierarchyHistory, ItemViewModel viewModel);
        void AddRevisionsByAttributeToResponse(ItemHistoryViewModel response);
        ItemHistoryViewModel BuildItemHistory(IEnumerable<ItemHistoryModel> itemHistory, ItemHierarchyClassHistoryAllModel hierarchyHistory, IEnumerable<AttributeDisplayModel> attributesItem, ItemViewModel viewModel);
        RevisionByDate Diff(ItemHistoryModel previous, ItemHistoryModel next);
        void OrderResponseRevisionHistory(ItemHistoryViewModel response);
        void RemoveEmptyRevisions(ItemHistoryViewModel response);

    }
}