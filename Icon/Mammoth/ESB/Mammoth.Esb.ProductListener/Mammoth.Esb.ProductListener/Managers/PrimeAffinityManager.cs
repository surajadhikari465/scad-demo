using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Managers
{
    public class PrimeAffinityManager : IPrimeAffinityManager
    {
        private ProductListenerSettings settings;
        private IQueryHandler<GetItemsParameters, IEnumerable<ItemDataAccessModel>> getItemsQueryHandler;
        private IQueryHandler<GetPrimeAffinityItemStoreModelsParameters, IEnumerable<PrimeAffinityItemStoreModel>> getPrimeAffinityItemStoreModelsQueryHandler;
        private IQueryHandler<GetPrimeAffinityItemStoreModelsForActiveSalesParameters, List<PrimeAffinityItemStoreModel>> getPrimeAffinityItemStoreModelsForActiveSalesQueryHandler;
        private IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor;

        public PrimeAffinityManager(
            ProductListenerSettings settings,
            IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor,
            IQueryHandler<GetItemsParameters, IEnumerable<ItemDataAccessModel>> getItemsQueryHandler,
            IQueryHandler<GetPrimeAffinityItemStoreModelsParameters, IEnumerable<PrimeAffinityItemStoreModel>> getPrimeAffinityItemStoreModelsQueryHandler,
            IQueryHandler<GetPrimeAffinityItemStoreModelsForActiveSalesParameters, List<PrimeAffinityItemStoreModel>> getPrimeAffinityItemStoreModelsForActiveSalesQueryHandler
            )
        {
            this.settings = settings;
            this.primeAffinityPsgProcessor = primeAffinityPsgProcessor;
            this.getItemsQueryHandler = getItemsQueryHandler;
            this.getPrimeAffinityItemStoreModelsQueryHandler = getPrimeAffinityItemStoreModelsQueryHandler;
            this.getPrimeAffinityItemStoreModelsForActiveSalesQueryHandler = getPrimeAffinityItemStoreModelsForActiveSalesQueryHandler;
        }

        public void SendPrimeAffinityMessages(IEnumerable<ItemModel> items)
        {
            List<ItemDataAccessModel> itemDataAccessModels = GetItems(items);
            var itemsChangingSubTeams = items.Join(
                itemDataAccessModels,
                i => i.GlobalAttributes.ItemID,
                idam => idam.ItemID,
                (i, idam) => new
                {
                    NewItem = i,
                    ExistingItem = idam,
                })
                .Where(i => i.NewItem.GlobalAttributes.PSNumber != i.ExistingItem.PSNumber)
                .Select(i => new
                {
                    i.NewItem,
                    i.ExistingItem,
                    IsChangingToExcludedSubTeam = GetIsChangingToExcludedSubTeam(i.ExistingItem.PSNumber, i.NewItem.GlobalAttributes.PSNumber),
                    IsChangingToNonExcludedSubTeam = GetIsChangingToNonExcludedSubTeam(i.ExistingItem.PSNumber, i.NewItem.GlobalAttributes.PSNumber),
                }).ToList();
            var itemsChangingToExcludedSubTeam = itemsChangingSubTeams.Where(i => i.IsChangingToExcludedSubTeam).ToList();
            var itemsChangingToNonExcludedSubTeam = itemsChangingSubTeams.Where(i => i.IsChangingToNonExcludedSubTeam).ToList();

            if (itemsChangingToExcludedSubTeam.Count > 0)
            {
                var primeAffinityItemStoreModels = getPrimeAffinityItemStoreModelsQueryHandler
                    .Search(new GetPrimeAffinityItemStoreModelsParameters
                    {
                        ItemIds = itemsChangingToExcludedSubTeam
                            .Select(i => i.NewItem.GlobalAttributes.ItemID)
                    });
                IEnumerable<PrimeAffinityMessageModel> primeAffinityMessageModels = CreateMessageModels(primeAffinityItemStoreModels, ActionEnum.Delete);
                SendPsgs(primeAffinityMessageModels, ActionEnum.Delete);
            }

            if (itemsChangingToNonExcludedSubTeam.Count > 0)
            {
                var primeAffinityItemStoreModels = getPrimeAffinityItemStoreModelsForActiveSalesQueryHandler
                    .Search(new GetPrimeAffinityItemStoreModelsForActiveSalesParameters
                    {
                        Items = itemsChangingToNonExcludedSubTeam.Select(i => i.ExistingItem).ToList(),
                        EligiblePriceTypes = settings.EligiblePriceTypes
                    });
                if (primeAffinityItemStoreModels.Count > 0)
                {
                    IEnumerable<PrimeAffinityMessageModel> primeAffinityMessageModels = CreateMessageModels(primeAffinityItemStoreModels, ActionEnum.Delete);
                    SendPsgs(primeAffinityMessageModels, ActionEnum.AddOrUpdate);
                }
            }
        }

        private List<ItemDataAccessModel> GetItems(IEnumerable<ItemModel> items)
        {
            GetItemsParameters parameters = new GetItemsParameters();
            parameters.ItemIds = items.Select(i => i.GlobalAttributes.ItemID).ToList();
            return getItemsQueryHandler.Search(parameters).ToList();
        }

        private bool GetIsChangingToNonExcludedSubTeam(int existingSubTeam, int? newSubTeam)
        {
            return settings.ExcludedPSNumbers.Contains(existingSubTeam)
                && !settings.ExcludedPSNumbers.Contains(newSubTeam.Value);
        }

        private bool GetIsChangingToExcludedSubTeam(int existingSubTeam, int? newSubTeam)
        {
            return settings.ExcludedPSNumbers.Contains(newSubTeam.Value)
                && !settings.ExcludedPSNumbers.Contains(existingSubTeam);
        }

        private IEnumerable<PrimeAffinityMessageModel> CreateMessageModels(IEnumerable<PrimeAffinityItemStoreModel> primeAffinityItemStoreModels, ActionEnum action)
        {
            return primeAffinityItemStoreModels.Select(m => new PrimeAffinityMessageModel
            {
                BusinessUnitID = m.BusinessUnitId,
                ItemID = m.ItemId,
                ItemTypeCode = m.ItemTypeCode,
                MessageAction = action,
                Region = m.Region,
                ScanCode = m.ScanCode,
                StoreName = m.StoreName,
                InternalPriceObject = m
            });
        }

        private void SendPsgs(IEnumerable<PrimeAffinityMessageModel> primeAffinityMessageModels, ActionEnum messageAction)
        {
            foreach (var primeAffinityMessageModelGroup in primeAffinityMessageModels
                                .GroupBy(p => p.BusinessUnitID))
            {
                var region = primeAffinityMessageModelGroup.First().Region;
                foreach (var primeAffinityMessageModelBatch in primeAffinityMessageModelGroup.Batch(100))
                {
                    primeAffinityPsgProcessor.SendPsgs(new PrimeAffinityPsgProcessorParameters
                    {
                        MessageAction = messageAction,
                        PrimeAffinityMessageModels = primeAffinityMessageModelBatch,
                        Region = region
                    });
                }
            }
        }
    }
}