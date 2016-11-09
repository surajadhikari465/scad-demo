using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetIrmaItemsQuery : IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>
    {
        private readonly IconContext context;

        public GetIrmaItemsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<IRMAItem> Search(GetIrmaItemsParameters parameters)
        {
            SqlParameter identifier = new SqlParameter("@identifier", SqlDbType.VarChar);
            identifier.Value = parameters.Identifier == null ? (Object)DBNull.Value : parameters.Identifier;

            SqlParameter itemDescription = new SqlParameter("@itemDescription", SqlDbType.VarChar);
            itemDescription.Value = parameters.ItemDescription == null ? (Object)DBNull.Value : parameters.ItemDescription;
            SqlParameter brandName = new SqlParameter("@brandName", SqlDbType.VarChar);
            brandName.Value = parameters.Brand == null ? (Object)DBNull.Value : parameters.Brand;
            SqlParameter regionCode = new SqlParameter("@regionCode", SqlDbType.VarChar);
            regionCode.Value = parameters.RegionCode == null ? (Object)DBNull.Value : parameters.RegionCode;
            SqlParameter parttialScanCodeSearch = new SqlParameter("@parttialScanCodeSearch", SqlDbType.Bit);
            parttialScanCodeSearch.Value = parameters.PartialScanCode;
            SqlParameter taxRomanceName = new SqlParameter("@taxRomanceName", SqlDbType.VarChar);
            taxRomanceName.Value = parameters.TaxRomanceName == null ? (Object)DBNull.Value : parameters.TaxRomanceName;

            string sql = @"app.GetIrmaItemsBySearchParams @identifier, @itemDescription, @brandName, @regionCode, @parttialScanCodeSearch, @taxRomanceName";
            var dbResult = this.context.Database.SqlQuery<IRMAItem>(sql, identifier, itemDescription, brandName, regionCode, parttialScanCodeSearch, taxRomanceName);

            return dbResult.ToList();
        }
    }
}

