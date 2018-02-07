using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace PrimeAffinityController.Queries
{
    public class GetPrimeAffinityAddPsgsFromPricesQuery : IQueryHandler<GetPrimeAffinityAddPsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>>
    {
        private IDbConnection connection;

        public GetPrimeAffinityAddPsgsFromPricesQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<PrimeAffinityPsgPriceModel> Search(GetPrimeAffinityAddPsgsFromPricesParameters parameters)
        {
            return connection.Query<PrimeAffinityPsgPriceModel>(
                $@" SELECT 
                        'AddOrUpdate' AS MessageAction,
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
                        AND StartDate = @Today
                        AND i.PSNumber NOT IN @ExcludedPSNumbers
                    ORDER BY p.BusinessUnitID",
                new
                {
                    ExcludedPSNumbers = parameters.ExcludedPSNumbers,
                    PriceTypes = parameters.PriceTypes,
                    Today = DateTime.Today
                },
                buffered: false);
        }
    }
}
