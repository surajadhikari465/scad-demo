using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemsForUpdateQueryHandler : IQueryHandler<GetItemsForUpdateQuery, IEnumerable<UpdateItemModel>>
    {
        private IDbConnection dbConnection;

        public GetItemsForUpdateQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<UpdateItemModel> Search(GetItemsForUpdateQuery parameters)
        {
            return dbConnection.Query<UpdateItemModel>(
                @"
                SELECT ScanCode
                INTO #scanCodes
                FROM @ScanCodes

                SELECT 
                    i.ItemId, 
                    i.ScanCode, 
                    i.ItemAttributesJson 
                FROM ItemView i
                JOIN #scanCodes sc ON i.ScanCode = sc.ScanCode",
                new
                {
                    ScanCodes = parameters.ScanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType")
                });
        }
    }
}
