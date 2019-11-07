using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Esb.ListenerApplication;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using KitBuilder.ESB.Listeners.Item.Service.Constants;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitBuilder.ESB.Listeners.Item.Service.Commands
{
    public class ItemAddOrUpdateOrRemoveCommandHandler : ICommandHandler<ItemAddOrUpdateOrRemoveCommand>
    {
        private readonly IRepository<Items> itemRepository;
        private readonly ItemListenerSettings itemListenerSettings;
        public ItemAddOrUpdateOrRemoveCommandHandler(IRepository<Items> itemRepository, ItemListenerSettings itemListenerSettings)
        {
            this.itemRepository = itemRepository;
            this.itemListenerSettings = itemListenerSettings;
        }

        public void Execute(ItemAddOrUpdateOrRemoveCommand data)
        {
            // get hospitality items.
            var hospitalityItemsWithoutErrors = data.Items.Where(i => i.ErrorCode == null && (i.HospitalityItem.GetValueOrDefault(false) || i.KitchenItem.GetValueOrDefault(false))).ToList();
            var nonHospitalityItemsWithoutErrors = data.Items.Where(i => i.ErrorCode == null && !(i.HospitalityItem.GetValueOrDefault(false) || i.KitchenItem.GetValueOrDefault(false))).ToList();

            try
            {
                AddOrUpdateItems(itemRepository, hospitalityItemsWithoutErrors);
                
            }
            catch (Exception ex)
            {
                var errorDetails = ApplicationErrors.Messages.ItemAddOrUpdateError + " Exception: " + ex.Message;
                foreach (var item in hospitalityItemsWithoutErrors)
                {
                    item.ErrorCode = ApplicationErrors.Codes.ItemAddOrUpdateError;
                    item.ErrorDetails = errorDetails;
                }
            }

            try
            {
                RemoveItems(itemRepository, nonHospitalityItemsWithoutErrors);

            }
            catch (Exception ex)
            {
                var errorDetails = ApplicationErrors.Messages.ItemRemoveError + " Exception: " + ex.Message;
                foreach (var item in hospitalityItemsWithoutErrors)
                {
                    item.ErrorCode = ApplicationErrors.Codes.ItemRemoveError;
                    item.ErrorDetails = errorDetails;
                }
            }
        }

        private void RemoveItems(IRepository<Items> repo, List<ItemModel> data)
        {
            if (!data.Any()) return;
            var items = data.Select(i => new { ItemId = i.ItemId}).ToTvp("items", "dbo.ItemRemoveType");
            repo.UnitOfWork.Context.Database.ExecuteSqlCommand("exec dbo.ItemRemove @items", items);
        }

        private void AddOrUpdateItems(IRepository<Items> repo, List<ItemModel> data)
        {

            if (!data.Any()) return;

            var items = data
                .Select(i => new
                {
                    ItemId = i.ItemId,
                    ScanCode = i.ScanCode,
                    ProductDesc = i.ProductDescription,
                    CustomerFriendlyDesc = i.CustomerFriendlyDescription,
                    KitchenDesc = i.KitchenDescription,
                    BrandName = i.BrandsHierarchyName,
                    ImageUrl = i.ImageUrl,
					FlexibleText = i.FlexibleText,
					PosDesc = i.PosDescription
				}).ToTvp("items", "dbo.ItemAddOrUpdateType");

            repo.UnitOfWork.Context.Database.ExecuteSqlCommand("exec dbo.ItemAddOrUpdate @items", items);
        }

    }
}
