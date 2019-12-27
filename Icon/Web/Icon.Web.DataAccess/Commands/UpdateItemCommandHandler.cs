using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Models;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand>
    {
        private IDbProvider db;

        public UpdateItemCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(UpdateItemCommand data)
        {
            var itemAttributesJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                db.Connection.QueryFirst<string>(
                    "SELECT ItemAttributesJson FROM dbo.Item WHERE itemID = @ItemId",
                    data,
                    transaction: this.db.Transaction));

            // remove any attributes that were not included in the request
            foreach (var itemAttribute in itemAttributesJson)
            {
                if (!data.ItemAttributes.ContainsKey(itemAttribute.Key))
                {
                    data.ItemAttributes[itemAttribute.Key] = null;
                }
            }

            foreach (var itemAttribute in data.ItemAttributes)
            {
                if (string.IsNullOrWhiteSpace(itemAttribute.Value?.ToString()))
                {
                    if (itemAttributesJson.ContainsKey(itemAttribute.Key))
                    {
                        itemAttributesJson.Remove(itemAttribute.Key);
                    }
                }
                else
                {
                    itemAttributesJson[itemAttribute.Key] = itemAttribute.Value;
                }
            }

            // Update Item
            DataTable items = new List<UpdateItemsType>
            {
                new UpdateItemsType
                {
                    ItemId = data.ItemId,
                    ScanCode = null,
                    BrandsHierarchyClassId = data.BrandsHierarchyClassId,
                    FinancialHierarchyClassId = data.FinancialHierarchyClassId,
                    MerchandiseHierarchyClassId  = data.MerchandiseHierarchyClassId,
                    NationalHierarchyClassId = data.NationalHierarchyClassId,
                    TaxHierarchyClassId = data.TaxHierarchyClassId,
                    ManufacturerHierarchyClassId = data.ManufacturerHierarchyClassId,
                    ItemAttributesJson = JsonConvert.SerializeObject(itemAttributesJson),
                    ItemTypeId = data.ItemTypeId
                }
            }.ToDataTable();

            db.Connection.Execute("dbo.UpdateItems", new { Items = items.AsTableValuedParameter("UpdateItemsType") },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);

            // Publish item events and messages
            var itemIds = new List<int> { data.ItemId };

            var intList = itemIds
                .Select(i => new { I = i })
                .ToDataTable();
            AddRegionalEvents(intList);

            var messageQueueItems = itemIds
                  .Select(i => new
                  {
                      ItemId = i,
                      EsbReadyDateTimeUtc = DateTime.UtcNow,
                      InsertDateUtc = DateTime.UtcNow
                  })
                  .ToDataTable();
            AddMessageQueueItems(messageQueueItems);
        }

        private void AddRegionalEvents(DataTable itemIds)
        {
            db.Connection.Execute(
                "app.GenerateItemUpdateEvents",
                new { ItemIds = itemIds.AsTableValuedParameter("app.IntList") },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }

        private void AddMessageQueueItems(DataTable messageQueueItems)
        {
            db.Connection.Execute(
                "esb.AddMessageQueueItem",
                new { @MessageQueueItems = messageQueueItems.AsTableValuedParameter("esb.MessageQueueItemIdsType") },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }
    }
}