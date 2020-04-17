using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Builder;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using BulkItemUploadProcessor.Service.Interfaces;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulkItemUploadProcessor.Service.BulkUpload
{
    public class AddUpdateItemManager : IAddUpdateItemManager
    {
        private readonly ICommandHandler<UpdateItemsCommand> updateItemsCommandHandler;
        private readonly ICommandHandler<AddItemsCommand> addItemsCommandHandler;
        private IErrorMessageBuilder errorMessageBuilder;

        public AddUpdateItemManager(
              ICommandHandler<UpdateItemsCommand> updateItemsCommandHandler,
              ICommandHandler<AddItemsCommand> addItemsCommandHandler,
              IErrorMessageBuilder errorMessageBuilder
        )
        {
            this.addItemsCommandHandler = addItemsCommandHandler;
            this.updateItemsCommandHandler = updateItemsCommandHandler;
            this.errorMessageBuilder = errorMessageBuilder;
        }

        public void UpdateItems(List<UpdateItemModel> updateItemModels,
           List<ErrorItem<UpdateItemModel>> invalidItems,
           List<ItemIdAndScanCode> updatedItems
           )
        {
            var command = new UpdateItemsCommand();
            command.Items = updateItemModels;

            try
            {
                updateItemsCommandHandler.Execute(command);
                if (command.Items != null)
                {
                    updatedItems.AddRange(command.Items.Select(i => new ItemIdAndScanCode
                    {
                        ItemId = i.ItemId,
                        ScanCode = i.ScanCode
                    }));
                }
            }

            catch (Exception ex)
            {
                if (command.Items.Count > 1)
                {
                    UpdateItems(command.Items.Take(updateItemModels.Count / 2).ToList(), invalidItems, updatedItems);
                    UpdateItems(command.Items.Skip(updateItemModels.Count / 2).ToList(), invalidItems, updatedItems);
                }
                else
                {
                    var errorMessage = errorMessageBuilder.BuildErrorMessage(ex);
                    invalidItems.Add(new ErrorItem<UpdateItemModel>(command.Items[0], errorMessage));
                }
            }
        }

        public void CreateItems(List<AddItemModel> addItemModels,
          List<ErrorItem<AddItemModel>> invalidItems,
          List<ItemIdAndScanCode> addedItems)
        {
            var command = new AddItemsCommand();
            command.Items = addItemModels;

            try
            {
                addItemsCommandHandler.Execute(command);
                if(command.AddedItems != null)
                {
                    addedItems.AddRange(command.AddedItems);
                }

                if(command.InvalidItems != null)
                {
                    invalidItems.AddRange(command.InvalidItems);
                }
            }

            catch (Exception ex)
            {
                if (command.Items.Count > 1)
                {
                    CreateItems(command.Items.Take(addItemModels.Count / 2).ToList(), invalidItems, addedItems);
                    CreateItems(command.Items.Skip(addItemModels.Count / 2).ToList(), invalidItems, addedItems);
                }
                else
                {
                    var errorMessage = errorMessageBuilder.BuildErrorMessage(ex);
                    invalidItems.Add(new ErrorItem<AddItemModel>(command.Items[0], errorMessage));
                }
            }
        }
    }
}