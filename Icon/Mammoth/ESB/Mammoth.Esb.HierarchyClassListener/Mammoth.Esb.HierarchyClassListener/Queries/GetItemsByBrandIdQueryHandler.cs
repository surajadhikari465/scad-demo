﻿using Dapper;
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
    public class GetItemsByBrandIdQueryHandler : IQueryHandler<GetItemsByBrandIdParameter, IEnumerable<Item>>
    {
        private IDbProvider db;

        public GetItemsByBrandIdQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<Item> Search(GetItemsByBrandIdParameter parameters)
        {
            var dbParameters = new
            {
                BrandIDs = parameters.BrandIds
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
                           $" WHERE BrandHCID IN @{nameof(dbParameters.BrandIDs)} ";
            var items = this.db.Connection.Query<Item>(sql, dbParameters, this.db.Transaction);
            return items;
        }
    }
}
