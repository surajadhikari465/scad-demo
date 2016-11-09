using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetMerchTaxMappingsQuery : IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>
    {
        private readonly IconContext context;

        public GetMerchTaxMappingsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MerchTaxMappingModel> Search(GetMerchTaxMappingsParameters parameters)
        {
            SqlParameter merchHierarchyClassID = new SqlParameter("@merchHierarchyClassID", SqlDbType.VarChar);
            merchHierarchyClassID.Value = parameters.MerchandiseHierarchyClassId;

            string sql = @"app.GetMerchTaxMapping @merchHierarchyClassID ";
            var dbResult = this.context.Database.SqlQuery<MerchTaxMappingModel>(sql, merchHierarchyClassID);

            return dbResult.ToList();
        }
    }
}
