using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public interface IEsbMessageBuilder
    {
        Task<ConsumerInformationType> BuildConsumerInformation(Nutrition nutrition, Action<string> processLogger);

        Task<List<HierarchyType>> BuildHierarchies(List<Hierarchy> hierarchies, Action<string> processLogger);

        Task<BuildMessageResult> BuildItem(List<MessageQueueItemModel> models);

        Task<ItemType> BuildItemType(MessageQueueItemModel message, Action<string> processLogger);

        Task<ScanCodeType> BuildScanCodeType(MessageQueueItemModel message, Action<string> processLogger);

        Task<List<Icon.Esb.Schemas.Wfm.Contracts.TraitType>> BuildTraitsFromNutrition(Nutrition nutrition, Action<string> processLogger);

        Task<List<Icon.Esb.Schemas.Wfm.Contracts.TraitType>> BuildTraitsFromAttributes(Dictionary<string, string> itemAttributes, Action<string> processLogger);

        Task<List<Icon.Esb.Schemas.Wfm.Contracts.TraitType>> BuildTraits(Dictionary<string, string> itemAttributes, Nutrition nutrition, Action<string> processLogger);

        Task<SelectionGroupsType> BuildProductSelectionGroupRootNode(MessageQueueItemModel message, Action<string> processLogger);

        Task<List<GroupTypeType>> BuildProductSelectionGroups(MessageQueueItemModel message, Action<string> processLogger);

        Task<GroupTypeType> CreateProductSelectionGroupElement(ProductSelectionGroup group, bool addOrUpdate, Action<string> processLogger);

        bool IsNutritionRemoved(Nutrition nutrition);
        Task RefreshCache();
        bool CacheLoaded { get; }
    }
}