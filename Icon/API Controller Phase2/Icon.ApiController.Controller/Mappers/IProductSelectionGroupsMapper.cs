using Icon.Framework;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.Mappers
{
    public interface IProductSelectionGroupsMapper
    {
        void LoadProductSelectionGroups();
        Contracts.SelectionGroupsType GetProductSelectionGroups(MessageQueueProduct productMessage);
        Contracts.SelectionGroupsType GetProductSelectionGroups(MessageQueueItemLocale itemLocaleMessage);
    }
}
