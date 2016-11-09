using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAvailablePlusByCategoryQuery : IQueryHandler<GetAvailablePlusByCategoryParameters, List<IRMAItem>>
    {
        private readonly IconContext context;

        public GetAvailablePlusByCategoryQuery(IconContext context)
        {
            this.context = context;
        }

        public List<IRMAItem> Search(GetAvailablePlusByCategoryParameters parameters)
        {
            SqlParameter PluCategoryID = new SqlParameter("@PluCategoryID", SqlDbType.Int);
            PluCategoryID.Value = parameters.PluCategoryId;

            SqlParameter MaxPlus = new SqlParameter("@MaxPlus", SqlDbType.Int);
            MaxPlus.Value = parameters.MaxPlus;
            
            string sql = @"app.GetAvailablePlusForCategory @PluCategoryID, @MaxPlus";

            var dbResult = this.context.Database.SqlQuery<IRMAItem>(sql, PluCategoryID, MaxPlus);

            return dbResult.ToList();
        }
    }
}
