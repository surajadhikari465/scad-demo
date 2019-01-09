﻿using Icon.Common;
using Icon.Common.DataAccess;
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
    public class ItemAddOrUpdateCommandHandler : ICommandHandler<ItemAddOrUpdateCommand>
    {
        private readonly IRepository<Items> itemRpository;

        public ItemAddOrUpdateCommandHandler(IRepository<Items> itemRepository)
        {
            this.itemRpository = itemRepository;
        }

        public void Execute(ItemAddOrUpdateCommand data)
        {
            // get hospitality items.
            var itemsWithoutErrors = data.Items.Where(i => i.ErrorCode == null 
                    && (i.HospitalityItem.GetValueOrDefault(false) || i.KitchenItem.GetValueOrDefault(false))).ToList();
            try
            {
                AddOrUpdateItems(itemRpository, itemsWithoutErrors);
            }
            catch (Exception ex)
            {
                var errorDetails = ApplicationErrors.Messages.ItemAddOrUpdateError + " Exception: " + ex.Message;
                foreach (var item in itemsWithoutErrors)
                {
                    item.ErrorCode = ApplicationErrors.Codes.ItemAddOrUpdateError;
                    item.ErrorDetails = errorDetails;
                }
            }
        }

        private void AddOrUpdateItems(IRepository<Items> repo, IEnumerable<ItemModel> data)
        {
            
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
					FlexibleText = i.FlexibleText
				}).ToTvp("items", "dbo.ItemAddOrUpdateType");

            repo.UnitOfWork.Context.Database.ExecuteSqlCommand("exec dbo.ItemAddOrUpdate @items", items);
        }

    }
}
