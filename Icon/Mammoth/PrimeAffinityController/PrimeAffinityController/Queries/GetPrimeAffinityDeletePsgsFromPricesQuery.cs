using Dapper;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Constants;
using PrimeAffinityController.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PrimeAffinityController.Queries
{
    public class GetPrimeAffinityDeletePsgsFromPricesQuery : IQueryHandler<GetPrimeAffinityDeletePsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>>
    {
        private IDbConnection connection;
        private ILogger<GetPrimeAffinityDeletePsgsFromPricesQuery> logger;

        public GetPrimeAffinityDeletePsgsFromPricesQuery(IDbConnection connection, ILogger<GetPrimeAffinityDeletePsgsFromPricesQuery> logger)
        {
            this.connection = connection;
            this.logger = logger;
        }

        public IEnumerable<PrimeAffinityPsgPriceModel> Search(GetPrimeAffinityDeletePsgsFromPricesParameters parameters)
        {
            return SearchWithTimeoutCheck(parameters, 0);
        }

        private IEnumerable<PrimeAffinityPsgPriceModel> SearchWithTimeoutCheck(GetPrimeAffinityDeletePsgsFromPricesParameters parameters, int timeoutOccurrences)
        {
            try
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
                        AND (EndDate = @Today OR EndDate = @EndOfYesterday)
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
                        EndOfYesterday = DateTime.Today.AddMilliseconds(-3),
                        Today = DateTime.Today
                    },
                    buffered: false,
                    commandTimeout: 60);
            }
            catch (SqlException ex)
            {
                if (ex.Number == ApplicationConstants.SqlTimeoutExceptionNumber && timeoutOccurrences < 3)
                {
                    logger.Error(
                        new
                        {
                            Message = "Timeout exception occurred. Retrying query for delete PSGs.",
                            Region = parameters.Region,
                            TimeoutOccurrences = timeoutOccurrences
                        }.ToJson());
                    return SearchWithTimeoutCheck(parameters, timeoutOccurrences + 1);
                }
                else
                {
                    logger.Error(
                        new
                        {
                            Message = "Timeout exception occurred. Timeout occurrences has exceeded max number of tries. Throwing error.",
                            Region = parameters.Region,
                            TimeoutOccurrences = timeoutOccurrences
                        }.ToJson());
                    throw;
                }
            }
        }
    }
}