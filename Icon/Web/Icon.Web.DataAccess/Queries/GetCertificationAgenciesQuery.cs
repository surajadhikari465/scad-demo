using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCertificationAgenciesQuery : IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>
    {
        private readonly IconContext context;

        public GetCertificationAgenciesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<CertificationAgencyModel> Search(GetCertificationAgenciesParameters parameters)
        {
            string sql = "EXEC app.GetCertificationAgencies";
            DbRawSqlQuery<CertificationAgencyModel> agencies = this.context.Database.SqlQuery<CertificationAgencyModel>(sql);

            return agencies.ToList();
        }
    }
}