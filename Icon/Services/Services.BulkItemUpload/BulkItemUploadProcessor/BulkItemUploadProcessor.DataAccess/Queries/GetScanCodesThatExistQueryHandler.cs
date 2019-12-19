using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetScanCodesThatExistQueryHandler : IQueryHandler<GetScanCodesThatExistParameters, HashSet<string>>
    {
        private readonly IDbConnection dbConnection;

        public GetScanCodesThatExistQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public HashSet<string> Search(GetScanCodesThatExistParameters parameters)
        {
            return dbConnection.Query<string>(
                @"
                SELECT ScanCode
                INTO #scanCodes
                FROM @ScanCodes

                SELECT 
                    sc.ScanCode
                FROM ScanCode sc
                JOIN #scanCodes temp ON sc.ScanCode = temp.ScanCode",
                new
                {
                    ScanCodes = parameters.ScanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType")
                })
                .ToHashSet();
        }
    }
}
