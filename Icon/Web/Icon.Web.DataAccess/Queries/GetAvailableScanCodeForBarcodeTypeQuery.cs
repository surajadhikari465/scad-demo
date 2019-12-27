using Dapper;
using Icon.Common.DataAccess;
using MoreLinq;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAvailableScanCodeForBarcodeTypeQuery : IQueryHandler<GetAvailableScanCodeForBarcodeTypeParameters, List<string>>
    {
        private IDbConnection connection;

        public GetAvailableScanCodeForBarcodeTypeQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<string> Search(GetAvailableScanCodeForBarcodeTypeParameters parameters)
        {
            return this.connection
                .Query<string>(
                    sql: "dbo.GetMultipleAvailableScanCodesForBarcodeTypeId",
                    param: new { BarcodeTypeId = parameters.BarCodeTypeId, Count = parameters.Count, ExcludedScanCodes = parameters.ExcludedScanCodes.ToDataTable() },
                    commandType: CommandType.StoredProcedure)
                .ToList();
        }
    }
}
