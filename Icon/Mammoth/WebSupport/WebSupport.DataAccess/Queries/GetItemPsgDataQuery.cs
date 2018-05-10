using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using MoreLinq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetItemPsgDataQuery : IQueryHandler<GetItemPsgDataParameters, IEnumerable<ItemPsgModel>>
    {
        private IDbConnection connection;

        public GetItemPsgDataQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<ItemPsgModel> Search(GetItemPsgDataParameters parameters)
        {
            List<ScanCodeBusinessUnitIdModel> scanCodeBusinessUnitIds = new List<ScanCodeBusinessUnitIdModel>();

            foreach (var scanCode in parameters.ScanCodes)
            {
                foreach (var businessUnitId in parameters.BusinessUnitIds)
                {
                    scanCodeBusinessUnitIds.Add(new ScanCodeBusinessUnitIdModel
                    {
                        ScanCode = scanCode,
                        BusinessUnitId = businessUnitId
                    });
                }
            }

            //Get item and price data in order to send PSG messages
            var psgModels = connection.Query<dynamic>($@"
                    DECLARE @Today DATETIME = CAST(GETDATE() AS DATE)

                    SELECT ScanCode
	                    ,BusinessUnitID
                    INTO #storeScanCodes
                    FROM @StoreScanCodes

                    SELECT l.BusinessUnitID
	                    ,i.ItemID
	                    ,it.ItemTypeCode
	                    ,l.Region
	                    ,i.ScanCode
	                    ,l.StoreName
	                    ,i.PSNumber
	                    ,p.PriceType
	                    ,p.StartDate
	                    ,p.EndDate
                        ,p.AddedDate
                    FROM #storeScanCodes ssc
                    JOIN Items i on i.ScanCode = ssc.ScanCode
                    JOIN ItemTypes it ON i.ItemTypeID = it.itemTypeID
                    JOIN Locale l ON ssc.BusinessUnitID = l.BusinessUnitID
                    LEFT JOIN Price p ON i.ItemID = p.ItemID
                        AND p.BusinessUnitID = ssc.BusinessUnitID
                        AND (p.PriceType = 'REG'
	                        OR (p.EndDate >= @Today
		                        OR p.StartDate <= @Today
		                    ))",
                      new
                      {
                          StoreScanCodes = scanCodeBusinessUnitIds.ToDataTable().AsTableValuedParameter("ScanCodeBusinessUnitIdType"),
                      })
                      .ToList();

            //Get item data for the reg price of the item or for when it does not have a price for a store
            var nonTprs = psgModels
                .Where(psgModel => psgModel.PriceType == null || psgModel.PriceType == "REG");
            //Get the most current sale for an item which has the most recent StartDate and AddedDate
            var currentTprs = psgModels
                .Where(psgModel => psgModel.PriceType != null && psgModel.PriceType != "REG")
                .GroupBy(psgModel => new { psgModel.ItemID, psgModel.BusinessUnitID })
                .Select(
                    psgGroup => psgGroup
                        .Where(p => p.StartDate == psgGroup.Max(p2 => p2.StartDate))
                        .MaxBy(psgModel => psgModel.AddedDate));

            //Union the nonTprs with the currentTprs and group them by ItemID and BusinessUnitID.
            //If the items have an active prime affinity eligible sale and is not an excluded sub team
            //then send an AddOrUpdate message. Otherwise send a Delete.
            List<ItemPsgModel> itemPsgModels = new List<ItemPsgModel>();
            foreach (var priceGroup in nonTprs.Union(currentTprs).GroupBy(p => new { p.ItemID, p.BusinessUnitID }))
            {
                string messageAction = null;
                if(priceGroup.Any(p => parameters.PriceTypes.Contains(p.PriceType) && !parameters.ExcludedPsNumbers.Contains(p.PSNumber) ))
                {
                    messageAction = "AddOrUpdate";
                }
                else
                {
                    messageAction = "Delete";
                }
                var firstPrice = priceGroup.First();
                itemPsgModels.Add(new ItemPsgModel
                {
                    BusinessUnitId = firstPrice.BusinessUnitID,
                    ItemId = firstPrice.ItemID,
                    ItemTypeCode = firstPrice.ItemTypeCode,
                    MessageAction = messageAction,
                    Region = firstPrice.Region,
                    ScanCode = firstPrice.ScanCode,
                    StoreName = firstPrice.StoreName,
                    SourceData = priceGroup.ToList()
                });
            }

            return itemPsgModels;
        }
    }
}