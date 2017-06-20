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
    public class GetItemsByBrandIdQueryHandler : IQueryHandler<GetItemsByBrandIdQuery, IEnumerable<Item>>
    {
        private IDbProvider db;

        public GetItemsByBrandIdQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<Item> Search(GetItemsByBrandIdQuery parameters)
        {
            string sql = @"
                            SELECT 
                                [ItemID],
                                [ItemTypeID],
                                [ScanCode],
                                [HierarchyMerchandiseID],
                                [HierarchyNationalClassID],
                                [BrandHCID],
                                [TaxClassHCID],
                                [PSNumber],
                                [Desc_Product],
                                [Desc_POS],
                                [PackageUnit],
                                [RetailSize],
                                [RetailUOM],
                                [FoodStampEligible],
                                [AddedDate],
                                [ModifiedDate]
                            FROM Items 
                            WHERE BrandHCID IN @BrandIDs";
            var items = this.db.Connection.Query<Item>(sql, new { BrandIDs = parameters.BrandIds }, this.db.Transaction);
            return items;
        }
    }
}
