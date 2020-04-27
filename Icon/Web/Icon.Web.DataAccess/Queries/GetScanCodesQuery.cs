using Dapper;
using Icon.Common.DataAccess;
using MoreLinq;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetScanCodesQuery : IQueryHandler<GetScanCodesParameters, List<string>>
    {
        private IDbConnection connection;

        public GetScanCodesQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<string> Search(GetScanCodesParameters parameters)
        {
            var scanCodeExistsModels = connection.Query<string>($@"
                    SELECT ScanCode
                    INTO #searchScanCodes
                    FROM @ScanCodes

                    SELECT sc.scanCode
                    FROM dbo.ScanCode sc
                    JOIN dbo.item i ON i.itemid = sc.itemid
                    JOIN #searchScanCodes ssc ON ssc.scancode = sc.scancode",
                      new
                      {
                          ScanCodes = parameters.ListOfScanCodes.Select(sc => new { ScanCode = sc }).ToDataTable().AsTableValuedParameter("app.ScanCodeListType"),
                      })
                      .ToList();

            return scanCodeExistsModels;
        }
    }
}