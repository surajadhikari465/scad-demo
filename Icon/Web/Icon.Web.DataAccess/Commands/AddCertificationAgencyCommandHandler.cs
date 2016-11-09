using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Commands
{
    public class AddCertificationAgencyCommandHandler : ICommandHandler<AddCertificationAgencyCommand>
    {
        private IconContext context;

        public AddCertificationAgencyCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddCertificationAgencyCommand data)
        {
            bool duplicateAgencyNameExists = context.HierarchyClass.ContainsDuplicateAgencyName(data.Agency.hierarchyClassName);

            if (duplicateAgencyNameExists)
            {
                throw new DuplicateValueException(String.Format("The agency {0} already exists.", data.Agency.hierarchyClassName));
            }

            SqlParameter agencies = new SqlParameter("agencies", SqlDbType.Structured);
            agencies.TypeName = "app.CertificationAgencyImportType";

            List<AddCertificationAgencyCommand> dataList = new List<AddCertificationAgencyCommand>();
            dataList.Add(data);
            agencies.Value = dataList.ConvertAll(a => new
            {
                AgencyId = 0,
                AgencyName = a.Agency.hierarchyClassName,
                GlutenFree = a.GlutenFree,
                Kosher = a.Kosher,
                NonGMO = a.NonGMO,
                Organic = a.Organic,
                Vegan = a.Vegan
            }).ToDataTable();

            string sql = "EXEC app.AddOrUpdateCertificationAgencies @agencies";

            context.Database.ExecuteSqlCommand(sql, agencies);
        }
    }
}
