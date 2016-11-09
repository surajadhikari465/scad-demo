using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Extensions;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkImportCertificationAgencyCommandHandler : ICommandHandler<BulkImportCommand<BulkImportCertificationAgencyModel>>
    {
        private IconContext context;
        private ILogger logger;

        public BulkImportCertificationAgencyCommandHandler(IconContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportCertificationAgencyModel> data)
        {
            SqlParameter agencies = new SqlParameter("certificationAgencies", SqlDbType.Structured);
            agencies.TypeName = "app.CertificationAgencyImportType";
            agencies.Value = data.BulkImportData.ConvertAll(ca => new
                {
                    AgencyId = ca.CertificationAgencyId,
                    AgencyName = ca.CertificationAgencyName,
                    GlutenFree = ca.GlutenFree,
                    Kosher = ca.Kosher,
                    NonGMO = ca.NonGmo,
                    Organic = ca.Organic,
                    Vegan = ca.Vegan
                }).ToDataTable();

            SqlParameter updateAgencyNames = new SqlParameter("updateAgencyNames", SqlDbType.Bit);
            updateAgencyNames.Value = 0;
            
            string sql = "EXEC app.AddOrUpdateCertificationAgencies @certificationAgencies, @updateAgencyNames";

            context.Database.ExecuteSqlCommand(sql, agencies, updateAgencyNames);
            logger.Info(String.Format("User {0} imported {1} certification agencies through bulk certification agency import.", data.UserName, data.BulkImportData.Count));
        }
    }
}
