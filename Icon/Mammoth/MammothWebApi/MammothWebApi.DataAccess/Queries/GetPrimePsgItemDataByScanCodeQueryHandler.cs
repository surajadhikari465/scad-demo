using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Models;
using MoreLinq;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetPrimePsgItemDataByScanCodeQueryHandler : IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>>
    {
        private IDbProvider db;
        private IPrimeAffinityPsgSettings settings;

        public GetPrimePsgItemDataByScanCodeQueryHandler(IDbProvider db, IPrimeAffinityPsgSettings settings)
        {
            this.db = db;
            this.settings = settings;
        }

        public IEnumerable<PrimePsgItemStoreDataModel> Search(GetPrimePsgItemDataByScanCodeQuery parameters)
        {
            var sql = @"SELECT ScanCode, BusinessUnitID
                        INTO #storeScanCodes
                        FROM @StoreScanCodes

                        SELECT
                            i.ItemID            AS ItemId,
                            i.ScanCode          AS ScanCode,
                            t.itemTypeCode      AS ItemTypeCode,
                            i.PSNumber          AS PsSubTeamNumber,
                            ssc.BusinessUnitID  AS BusinessUnitId,
                            l.StoreName         AS StoreName,
                            l.Region            AS Region
                        FROM #storeScanCodes ssc
                        INNER JOIN dbo.Items i on ssc.ScanCode = i.ScanCode
                        INNER JOIN dbo.ItemTypes t on i.ItemTypeID = t.itemTypeID
                        INNER JOIN dbo.Locale l on ssc.BusinessUnitID = l.BusinessUnitID
                        WHERE i.PSNumber NOT IN @ExcludedPsNumbers";

            var itemData = this.db.Connection.Query<PrimePsgItemStoreDataModel>(sql,
                new
                {
                    StoreScanCodes = parameters.StoreScanCodes.ToDataTable().AsTableValuedParameter("ScanCodeBusinessUnitIdType"),
                    ExcludedPsNumbers = settings.ExcludedPsNumbers
                },
                this.db.Transaction);
            return itemData;
        }
    }
}
