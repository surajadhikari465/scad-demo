using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Queries
{
    public class GetItemsByMerchandiseClassIdQueryHandler : IQueryHandler<GetItemsByMerchandiseHierarchyIdParameter, IEnumerable<Item>>
    {
        private IDbProvider db;

        public GetItemsByMerchandiseClassIdQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<Item> Search(GetItemsByMerchandiseHierarchyIdParameter parameters)
        {
            var dbParameters = new
            {
                HierarchyMerchandiseIDs = parameters.MerchandiseHierarchyIDs
            };
            string sql = @" SELECT 
                                ItemID,
                                ItemTypeID,
                                ScanCode,
                                HierarchyMerchandiseID,
                                HierarchyNationalClassID,
                                BrandHCID,
                                TaxClassHCID,
                                PSNumber,
                                Desc_Product,
                                Desc_POS,
                                PackageUnit,
                                RetailSize,
                                RetailUOM,
                                FoodStampEligible,
                                AddedDate,
                                ModifiedDate
                            FROM Items" +
                           $" WHERE HierarchyMerchandiseID IN @{nameof(dbParameters.HierarchyMerchandiseIDs)} ";
            var items = this.db.Connection.Query<Item>(sql, dbParameters, this.db.Transaction);
            return items;
        }
    }
}
