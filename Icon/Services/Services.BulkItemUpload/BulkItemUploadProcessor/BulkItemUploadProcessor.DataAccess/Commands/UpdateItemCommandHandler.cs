using System.Collections.Generic;
using System.Data;
using Dapper;
using Icon.Common.DataAccess;
using Newtonsoft.Json;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand>
    {
        private IDbConnection dbConnection;

        public UpdateItemCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(UpdateItemCommand data)
        {
            var itemAttributesJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                dbConnection.QueryFirst<string>(
                    "SELECT ItemAttributesJson FROM dbo.Item WHERE itemID = @ItemId",
                    data));

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
            dbConnection.Execute("dbo.UpdateItem",
                new
                {
                    data.ItemId,
                    data.BrandsHierarchyClassId,
                    data.FinancialHierarchyClassId,
                    data.MerchandiseHierarchyClassId,
                    data.NationalHierarchyClassId,
                    data.TaxHierarchyClassId,
                    data.ManufacturerHierarchyClassId,
                    ItemAttributesJson = JsonConvert.SerializeObject(itemAttributesJson),
                    itemTypeCode = data.ItemTypeCode
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}