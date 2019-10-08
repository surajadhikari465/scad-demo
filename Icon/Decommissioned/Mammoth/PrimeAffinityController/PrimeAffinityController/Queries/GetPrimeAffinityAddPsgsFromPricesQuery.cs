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
    public class GetPrimeAffinityAddPsgsFromPricesQuery : IQueryHandler<GetPrimeAffinityAddPsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>>
    {
        private IDbConnection connection;
        private ILogger<GetPrimeAffinityAddPsgsFromPricesQuery> logger;

        public GetPrimeAffinityAddPsgsFromPricesQuery(IDbConnection connection, ILogger<GetPrimeAffinityAddPsgsFromPricesQuery> logger)
        {
            this.connection = connection;
            this.logger = logger;
        }

        public IEnumerable<PrimeAffinityPsgPriceModel> Search(GetPrimeAffinityAddPsgsFromPricesParameters parameters)
        {
            return SearchWithTimeoutCheck(parameters, 0);
        }

        private IEnumerable<PrimeAffinityPsgPriceModel> SearchWithTimeoutCheck(GetPrimeAffinityAddPsgsFromPricesParameters parameters, int timeoutOccurrences)
        {
            try
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
                            Message = "Timeout exception occurred. Retrying query for add PSGs.",
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