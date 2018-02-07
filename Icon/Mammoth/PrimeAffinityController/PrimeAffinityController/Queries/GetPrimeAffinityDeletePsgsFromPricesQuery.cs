using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace PrimeAffinityController.Queries
{
    public class GetPrimeAffinityDeletePsgsFromPricesQuery : IQueryHandler<GetPrimeAffinityDeletePsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>>
    {
        private IDbConnection connection;

        public GetPrimeAffinityDeletePsgsFromPricesQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<PrimeAffinityPsgPriceModel> Search(GetPrimeAffinityDeletePsgsFromPricesParameters parameters)
        {
            return connection.Query<PrimeAffinityPsgPriceModel>(
                $@" SELECT 
                        'Delete' AS MessageAction,
                        p.Region,
                        p.PriceID,
                        p.ItemID,
                        p.BusinessUnitID,
                        p.StartDate,
                        p.EndDate,
                        p.Price,
                        p.PriceType,
                        p.PriceUOM,
                        p.CurrencyID,
                        p.Multiple,
                        p.AddedDate,
                        p.ModifiedDate,
                        i.ScanCode,
                        it.ItemTypeCode,
                        l.StoreName
                    FROM dbo.Price_{parameters.Region} p
                    JOIN dbo.Items i ON p.ItemID = i.ItemID
                    JOIN dbo.ItemTypes it ON i.itemTypeID = it.itemTypeID
                    JOIN dbo.Locales_{parameters.Region} l ON p.BusinessUnitID = l.BusinessUnitID
                    WHERE PriceType IN @PriceTypes 
                        AND EndDate BETWEEN @Yesterday AND @Today
                        AND i.PSNumber NOT IN @ExcludedPSNumbers
                        AND NOT EXISTS 
                        (
                            SELECT 1
                            FROM dbo.Price_{parameters.Region} p2
                            WHERE p2.ItemID = p.ItemID
                                AND p2.BusinessUnitID = p.BusinessUnitID
                                AND p2.StartDate = @Today
                                AND p2.PriceType IN @PriceTypes
                        )
                    UNION
                    SELECT 
                        'Delete' AS MessageAction,
                        p.Region,
                        p.PriceID,
                        p.ItemID,
                        p.BusinessUnitID,
                        p.StartDate,
                        p.EndDate,
                        p.Price,
                        p.PriceType,
                        p.PriceUOM,
                        p.CurrencyID,
                        p.Multiple,
                        p.AddedDate,
                        p.ModifiedDate,
                        i.ScanCode,
                        it.ItemTypeCode,
                        l.StoreName
                    FROM dbo.Price_{parameters.Region} p
                    JOIN dbo.Items i ON p.ItemID = i.ItemID
                    JOIN dbo.ItemTypes it ON i.itemTypeID = it.itemTypeID
                    JOIN dbo.Locales_{parameters.Region} l ON p.BusinessUnitID = l.BusinessUnitID
                    WHERE PriceType <> 'REG'
                        AND PriceType NOT IN @PriceTypes 
                        AND StartDate = @Today
                        AND i.PSNumber NOT IN @ExcludedPSNumbers
                    ORDER BY p.BusinessUnitID",
                new
                {
                    ExcludedPSNumbers = parameters.ExcludedPSNumbers,
                    PriceTypes = parameters.PriceTypes,
                    Yesterday = DateTime.Today.AddDays(-1),
                    Today = DateTime.Today
                },
                buffered: false);
        }
    }
}
