using Dapper;
using Icon.Common.DataAccess;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetScanCodeExistsQuery : IQueryHandler<GetScanCodeExistsParameters, List<ScanCodeExistsModel>>
    {
        private IDbConnection connection;

        public GetScanCodeExistsQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<ScanCodeExistsModel> Search(GetScanCodeExistsParameters parameters)
        {
            var scanCodeExistsModels = connection.Query<ScanCodeExistsModel>($@"
                    SELECT ScanCode
                    INTO #scanCodes
                    FROM @ScanCodes

                    SELECT ssc.ScanCode
	                    ,CASE WHEN i.ItemID IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS [Exists]
                    FROM #scanCodes ssc
                    LEFT JOIN Items i on i.ScanCode = ssc.ScanCode",
                      new
                      {
                          ScanCodes = parameters.ScanCodes.Select(sc => new { ScanCode = sc }).ToDataTable().AsTableValuedParameter("gpm.ScanCodesType"),
                      })
                      .ToList();

            return scanCodeExistsModels;
        }
    }
}